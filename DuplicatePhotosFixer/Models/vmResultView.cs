using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DuplicatePhotosFixer.ClassDictionary;

namespace DuplicatePhotosFixer.Models
{
    public class vmResultView : INotifyPropertyChanged
    {
        #region Properties

        private ObservableCollection<DuplicateGroup> _duplicateGroups;
        public ObservableCollection<DuplicateGroup> DuplicateGroups
        {
            get { return _duplicateGroups; }
            set
            {
                _duplicateGroups = value;
                OnPropertyChanged("DuplicateGroups");
            }
        }

        private int _totalDuplicates;
        public int TotalDuplicates
        {
            get { return _totalDuplicates; }
            set
            {
                _totalDuplicates = value;
                OnPropertyChanged("TotalDuplicates");
            }
        }

        private string _spaceToSave;
        public string SpaceToSave
        {
            get { return _spaceToSave; }
            set
            {
                _spaceToSave = value;
                OnPropertyChanged("SpaceToSave");
            }
        }

        private bool _isImageView = true;
        public bool IsImageView
        {
            get { return _isImageView; }
            set
            {
                _isImageView = value;
                OnPropertyChanged("IsImageView");
            }
        }

        #endregion

        private Dispatcher _uiDispatcher;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                if (_uiDispatcher != null && !_uiDispatcher.CheckAccess())
                {
                    _uiDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        handler(this, new PropertyChangedEventArgs(propertyName));
                    }));
                }
                else
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public vmResultView()
        {
            try
            {
                _uiDispatcher = System.Windows.Application.Current?.Dispatcher;
            }
            catch { }

            DuplicateGroups = new ObservableCollection<DuplicateGroup>();
            TotalDuplicates = 0;
            SpaceToSave = "0 KB";
        }

        /// <summary>
        /// Load duplicate groups from the backend data
        /// </summary>
        public void LoadDuplicateGroups()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("Loading duplicate groups...");

                DuplicateGroups.Clear();
                TotalDuplicates = 0;
                long totalSize = 0;

                // Get all image file info from backend
                if (cGlobalSettings.listImageFileInfo != null && cGlobalSettings.listImageFileInfo.Count > 0)
                {
                    // Group duplicates by hash
                    var groupedByHash = cGlobalSettings.listImageFileInfo
                        .Values
                        .Where(x => x.byteHash != null)
                        .GroupBy(x => Convert.ToBase64String(x.byteHash))
                        .Where(g => g.Count() > 1); // Only groups with more than 1 file

                    foreach (var group in groupedByHash)
                    {
                        var files = group.ToList();
                        var dupGroup = new DuplicateGroup
                        {
                            GroupId = Guid.NewGuid().ToString(),
                            Files = new ObservableCollection<DuplicateFile>(),
                            TotalSize = files.Sum(f => f.fileSize),
                            FileCount = files.Count
                        };

                        foreach (var file in files)
                        {
                            dupGroup.Files.Add(new DuplicateFile
                            {
                                FileId = file.key.ToString(),
                                FileName = file.fileName,
                                FilePath = file.filePath,
                                FileSize = file.fileSize,
                                FileSizeFormatted = file.fileSizeWithUnit,
                                CreatedDate = file.createDate,
                                ModifiedDate = file.modDate,
                                ThumbnailPath = file.ThumbnailPath,
                                IsSelected = false
                            });

                            TotalDuplicates++;
                            totalSize += file.fileSize;
                        }

                        DuplicateGroups.Add(dupGroup);
                    }

                    // Format total space
                    SpaceToSave = FormatFileSize(totalSize);
                    cGlobalSettings.oLogger?.WriteLogVerbose($"Loaded {DuplicateGroups.Count} duplicate groups, Total duplicates: {TotalDuplicates}, Total size: {SpaceToSave}");
                }
                else
                {
                    cGlobalSettings.oLogger?.WriteLogVerbose("No duplicate data found");
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("LoadDuplicateGroups", ex);
            }
        }

        /// <summary>
        /// Auto mark duplicates (keep oldest, mark rest for deletion)
        /// </summary>
        public void AutoMark()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("Auto Mark started");

                foreach (var group in DuplicateGroups)
                {
                    if (group.Files.Count > 1)
                    {
                        // Sort by modified date, keep the oldest
                        var sorted = group.Files.OrderBy(f => f.ModifiedDate).ToList();

                        // Mark all except the first (oldest) one
                        for (int i = 1; i < sorted.Count; i++)
                        {
                            sorted[i].IsSelected = true;
                        }
                    }
                }

                cGlobalSettings.oLogger?.WriteLogVerbose("Auto Mark completed");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("AutoMark", ex);
            }
        }

        /// <summary>
        /// Unmark all duplicates
        /// </summary>
        public void UnmarkAll()
        {
            try
            {
                cGlobalSettings.oLogger?.WriteLogVerbose("Unmark All started");

                foreach (var group in DuplicateGroups)
                {
                    foreach (var file in group.Files)
                    {
                        file.IsSelected = false;
                    }
                }

                cGlobalSettings.oLogger?.WriteLogVerbose("Unmark All completed");
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger?.WriteLogException("UnmarkAll", ex);
            }
        }

        /// <summary>
        /// Format file size to human-readable format
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }

    /// <summary>
    /// Represents a group of duplicate files
    /// </summary>
    public class DuplicateGroup : INotifyPropertyChanged
    {
        private string _groupId;
        public string GroupId
        {
            get { return _groupId; }
            set
            {
                _groupId = value;
                OnPropertyChanged("GroupId");
            }
        }

        private ObservableCollection<DuplicateFile> _files;
        public ObservableCollection<DuplicateFile> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged("Files");
            }
        }

        private long _totalSize;
        public long TotalSize
        {
            get { return _totalSize; }
            set
            {
                _totalSize = value;
                OnPropertyChanged("TotalSize");
            }
        }

        private int _fileCount;
        public int FileCount
        {
            get { return _fileCount; }
            set
            {
                _fileCount = value;
                OnPropertyChanged("FileCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>
    /// Represents a single duplicate file
    /// </summary>
    public class DuplicateFile : INotifyPropertyChanged
    {
        private string _fileId;
        public string FileId
        {
            get { return _fileId; }
            set
            {
                _fileId = value;
                OnPropertyChanged("FileId");
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private long _fileSize;
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                _fileSize = value;
                OnPropertyChanged("FileSize");
            }
        }

        private string _fileSizeFormatted;
        public string FileSizeFormatted
        {
            get { return _fileSizeFormatted; }
            set
            {
                _fileSizeFormatted = value;
                OnPropertyChanged("FileSizeFormatted");
            }
        }

        private DateTime _createdDate;
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set
            {
                _createdDate = value;
                OnPropertyChanged("CreatedDate");
            }
        }

        private DateTime _modifiedDate;
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
            set
            {
                _modifiedDate = value;
                OnPropertyChanged("ModifiedDate");
            }
        }

        private string _thumbnailPath;
        public string ThumbnailPath
        {
            get { return _thumbnailPath; }
            set
            {
                _thumbnailPath = value;
                OnPropertyChanged("ThumbnailPath");
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}