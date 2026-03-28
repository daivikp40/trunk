using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Engine.WinApiScan
{
    public static class FileFunctions
    {
        /// <summary>
        /// Will retrieve all the drives
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalDrives()
        {

            List<string> lsDrives = new List<string>();

            DriveInfo[] sDrives = DriveInfo.GetDrives();


            foreach (DriveInfo oDr in sDrives)
            {
                try
                {
                    switch (oDr.DriveType)
                    {
                        case DriveType.CDRom:
                        case DriveType.Network:
                        case DriveType.NoRootDirectory:
                        case DriveType.Ram:
                        case DriveType.Unknown:
                            continue;
                        case DriveType.Fixed:
                        case DriveType.Removable:
                            {
                                try
                                {
                                    if (ShouldSkipDriveWithRealPath(oDr.Name))
                                    {
                                        continue;
                                    }

                                    if (/*!IsFloppyDrive(oDr.Name) &&*/ (oDr.DriveFormat == "NTFS" || oDr.DriveFormat.Contains("FAT")) && oDr.IsReady)
                                    {
                                        lsDrives.Add(oDr.Name);
                                        //cGlobalSettings.oLogger.WriteLogVerbose("{0} | type : {1} : readystate : {2}", oDr.Name, oDr.IsReady, oDr.DriveFormat);
                                    }
                                }
                                catch (System.Exception)
                                {
                                    continue;
                                }
                            }
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }

                    //if (oDr.DriveType == DriveType.Fixed || /*oDr.DriveType == DriveType.Network ||*/ oDr.DriveType == DriveType.Removable)
                    //{

                    //}

                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("======== GetLocalDrives listing ========", ex);
                }
            }

#if DEBUG
            // lsDrives = new List<string> { "d:\\", "f:\\", /*"h:\\", "i:\\", "E:\\"*/ };
            //lsDrives = new List<string> { "C:\\", "D:\\", "E:\\"/*, "z:\\"*/ };
            //lsDrives = new List<string> { "C:\\", "D:\\", "F:\\" };
#endif

            return lsDrives;

            //return System.IO.Directory.GetLogicalDrives().ToList();
        }

        public static List<string> GetDefaultLocalDrives()
        {

            List<string> lsDrives = new List<string>();

            DriveInfo[] sDrives = DriveInfo.GetDrives();

            int i = 0;
            foreach (DriveInfo oDr in sDrives)
            {
                try
                {
                    switch (oDr.DriveType)
                    {
                        case DriveType.CDRom:
                        case DriveType.Network:
                        case DriveType.NoRootDirectory:
                        case DriveType.Ram:
                        case DriveType.Unknown:
                            continue;
                        case DriveType.Fixed:
                        case DriveType.Removable:
                            {
                                try
                                {
                                    if (ShouldSkipDriveWithRealPath(oDr.Name))
                                    {
                                        continue;
                                    }

                                    if (/*!IsFloppyDrive(oDr.Name) &&*/ (oDr.DriveFormat == "NTFS" || oDr.DriveFormat.Contains("FAT")) && oDr.IsReady)
                                    {
                                        lsDrives.Add(oDr.Name);
                                        i++;
                                        //cGlobalSettings.oLogger.WriteLogVerbose("{0} | type : {1} : readystate : {2}", oDr.Name, oDr.IsReady, oDr.DriveFormat);
                                    }
                                }
                                catch (System.Exception)
                                {
                                    continue;
                                }
                            }
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                    if (i == 1) break;

                    //if (oDr.DriveType == DriveType.Fixed || /*oDr.DriveType == DriveType.Network ||*/ oDr.DriveType == DriveType.Removable)
                    //{

                    //}

                }
                catch (System.Exception ex)
                {
                    cGlobalSettings.oLogger.WriteLogException("======== GetLocalDrives listing ========", ex);
                }
            }

#if DEBUG
            // lsDrives = new List<string> { "d:\\", "f:\\", /*"h:\\", "i:\\", "E:\\"*/ };
            //lsDrives = new List<string> { "C:\\", "D:\\", "E:\\"/*, "z:\\"*/ };
            //lsDrives = new List<string> { "C:\\", "D:\\", "F:\\" };
#endif

            return lsDrives;

            //return System.IO.Directory.GetLogicalDrives().ToList();
        }

        public static bool IsDirectoryInValid(string sDirPath)
        {
            return IsDirectoryInValid(sDirPath, false);
        }

        public static bool IsDirectoryInValid(string sDirPath, bool IsRootDrive)
        {
            try
            {
                if (sDirPath.Length < 4 && !IsRootDrive)
                {
                    return false;
                }

                if (!IsDirPathLengthAllowed(sDirPath))
                {
                    cGlobalSettings.oLogger.WriteLogVerbose("Directory skipped. Length is more then allowed-Length:{0}, Directory:{1}", sDirPath.Length, sDirPath);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("CustomScanViewModel::IsDirectoryInValid()", ex);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Add backlash before extension if not exist.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string AddSearchPattern(string path, string searchPattern)
        {
            return Path.Combine(path.AddBackSlash(), searchPattern);
        }


        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AddBackSlash(this string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path += Path.DirectorySeparatorChar.ToString();

            return path;
        }

        /// <summary>
        /// Add backupSlash in end
        /// 
        /// Path.AltDirectorySeparatorChar=/ 
        /// Path.DirectorySeparatorChar=\ 
        /// Path.PathSeparator=; 
        /// Path.VolumeSeparatorChar=:
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveEndBackSlashIfIsFirstChar(this string path)
        {
            if (path.StartsWith(Path.DirectorySeparatorChar.ToString()))
                path = path.Substring(Path.DirectorySeparatorChar.ToString().Length);

            return path;
        }


        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveForwardBackSlashIfIsFirstChar(this string path)
        {
            if (path.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                path = path.Substring(Path.AltDirectorySeparatorChar.ToString().Length);

            return path;
        }

        public static bool IsDirMatchedOrStartsWith(this string inputDirectory, string DirectoryToMatchWith)
        {
            string input = inputDirectory.AddBackSlash().ToLower();
            string compareDir = DirectoryToMatchWith.AddBackSlash().ToLower();
            return ((input == compareDir) || input.StartsWith(compareDir));
        }

        public static bool IsDirExactMatchedCaseInsensitive(this string inputDirectory, string DirectoryToMatchWith)
        {
            string input = inputDirectory.AddBackSlash().ToLower();
            string compareDir = DirectoryToMatchWith.AddBackSlash().ToLower();
            return ((input == compareDir));
        }


        /// <summary>
        /// split path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> SplitPath(this string path)
        {
            return path.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).ToList();
        }


        /// <summary>
        /// get base path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetBaseDirectory(this string path)
        {
            return path.SplitPath().DefaultIfEmpty(string.Empty).First();
        }

        /// <summary>
        /// Does path has invalid characters
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool PathHasInValidCharacters(this string path)
        {
            return (path.IndexOfAny(Path.GetInvalidFileNameChars()) > -1 || path.IndexOfAny(Path.GetInvalidPathChars()) > -1);
        }


        /// <summary>
        /// Remove invalid characters from path if exists
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        /// <summary>
        /// Add backupSlash in end
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string RemoveEndBackSlash(this string path)
        {
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path = path.Remove(path.Length - 1);

            return path;
        }

        public static string RemoveEndVolumeSeparatorChar(this string path)
        {
            path = path.RemoveEndBackSlash();
            if (path.EndsWith(Path.VolumeSeparatorChar.ToString()))
                path = path.Remove(path.Length - 1);

            return path;
        }

        public static void TraceTime(string traceMsg)
        {
            System.Diagnostics.Trace.WriteLine(string.Concat(traceMsg, ":", DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss fff")));
        }


        public static string GetDriveLetter(string drive)
        {
            string driveLetter = drive;
            driveLetter = driveLetter.Replace(":", "");
            driveLetter = driveLetter.Replace("/", "");
            driveLetter = driveLetter.Replace("\\", "");
            return driveLetter;
        }


        /// <summary>
        /// File verify file or directory exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool FileOrDirectoryExists(string name)
        {
            return (Directory.Exists(name) || File.Exists(name));
        }

        /// <summary>
        /// Verify it is directory or file
        /// </summary>
        /// <param name="FileorFolderPath"></param>
        /// <returns></returns>
        public static bool IsDirectory(string FileorFolderPath)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(FileorFolderPath);

                //detect whether its a directory or file
                bool b_isDir = ((attr & FileAttributes.Directory) == FileAttributes.Directory);

                return b_isDir;
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                //cGlobalSettings.oLogger.WriteLogException("FileFunctions::IsDirectory", ex);
                return false;
            }

        }


        /// <summary>
        /// Preallocate with creating a dump file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool PreallocateSpaceUsingDumpFile(string path, long size)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


                // preallocate space
                using (FileStream outFile = System.IO.File.Create(path))
                {
                    outFile.SetLength(size);
                    if (size > 0)
                    {
                        outFile.WriteByte(0);
                    }
                    outFile.Close();
                    outFile.Dispose();
                }


                return true;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FileFunctions::PreallocateSpaceUsingDumpFile", ex);
                //Trace.WriteLine(ex.ToString());
            }

            return false;

        }



        /// <summary>
        /// This is used while restoring the 0 bytes file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateZeroByteFile(string path)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path))) Directory.CreateDirectory(Path.GetDirectoryName(path));
                // Create the file. 
                using (FileStream fs = File.Create(path))
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FileFunctions::CreateZeroByteFile", ex);
            }

            return false;
        }





        const int MAX_PATH = 255;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern int GetLongPathName([MarshalAs(UnmanagedType.LPTStr)] string lpszShortPath, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszLongPath, [MarshalAs(UnmanagedType.U4)] int cchBuffer);



        /// <summary>
        /// GetShortPath
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetShortPath(string path)
        {
            var sbShortPath = new StringBuilder(MAX_PATH);
            GetShortPathName(path, sbShortPath, MAX_PATH);
            string shortPath = sbShortPath.ToString();
            sbShortPath = null;
            return shortPath;

        }


        /// <summary>
        /// Converts a short path to a long path.
        /// </summary>
        /// <param name="shortPath">A path that may contain short path elements (~1).</param>
        /// <returns>The long path.  Null or empty if the input is null or empty.</returns>
        internal static string GetLongPathName(string shortPath)
        {
            //string sShortPath = GetShortPath(shortPath);
            if (String.IsNullOrEmpty(shortPath))
            {
                return shortPath;
            }
            StringBuilder builder = new StringBuilder(cWin32APIs.BUFSIZE);
            int result = GetLongPathName(shortPath, builder, builder.Capacity);
            if (result > builder.Capacity)
            {
                builder = new StringBuilder(result);
                result = GetLongPathName(shortPath, builder, builder.Capacity);
            }

            if (result > 0 && result < builder.Capacity)
            {
                return builder.ToString(0, result);
            }
            else
            {
                if (result > 0)
                {
                    builder = new StringBuilder(result);
                    result = GetLongPathName(shortPath, builder, builder.Capacity);
                    return builder.ToString(0, result);
                }
                else
                {

                    // We are not able the long path name information of file
                    // Ex.  - c:\pagefile.sys - ERROR_SHARING_VIOLATION
                    //

                    System.ComponentModel.Win32Exception exception = null;
                    try
                    {
                        exception = new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                        cGlobalSettings.oLogger.WriteLogVerbose(string.Format("======== Error in getting GetLongPathName for file={0}, Win32Error={1}, Message={2} ========",
                            shortPath, exception.NativeErrorCode, exception.Message));
                    }
                    finally
                    {
                        exception = null;
                    }


                    return shortPath;

                    /*throw new FileNotFoundException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        "File Not found",
                        shortPath),
                        shortPath);*/
                }
            }
        }


        #region Get logical drives

        [DllImport("kernel32.dll")]
        static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);


        /// <summary>
        /// Reference:: http://www.pinvoke.net/default.aspx/kernel32.querydosdevice
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa366789(v=vs.85).aspx
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetRealPath(string path)
        {
            try
            {

                StringBuilder pathInformation = new StringBuilder(cWin32APIs.MAX_PATH);

                // Get the drive letter of the 
                string driveLetter = Path.GetPathRoot(path).Replace("\\", "");
                QueryDosDevice(driveLetter, pathInformation, cWin32APIs.MAX_PATH);

                return pathInformation.ToString();

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("======== GetRealPath ========", ex);
            }
            return string.Empty;
        }


        /// <summary>
        /// Detect if it floppy drive
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsCDROMDrive(string path)
        {
            return GetRealPath(path).ToLower().Contains(@"\device\cdrom".ToLower());
        }

        /// <summary>
        /// Detect if it floppy drive
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFloppyDrive(string path)
        {
            return GetRealPath(path).ToLower().Contains(@"\Device\Floppy".ToLower());
        }


        public static bool ShouldSkipDriveWithRealPath(string path)
        {
            return (IsFloppyDrive(path) || IsCDROMDrive(path));
        }



        /// <summary>
        /// Os Drive
        /// </summary>
        /// <returns></returns>
        public static string GetOsDrive()
        {
            return Path.GetPathRoot(Environment.SystemDirectory);
        }

        #endregion // Get logical drives


        /// <summary>
        /// gets the smart selection list for personal smart selection from flag value 
        /// </summary>
        /// <param name="nOptions"></param>
        /// <returns></returns>
        public static List<string> GetSmartBackupPersonalItems()
        {
            // set backup options
            List<string> LsIncludedDirectoryList = new List<string>();
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            //LsIncludedDirectoryList.Add(cWin32APIs.GetShFolderPath(cClientEnum.CSIDL.CSIDL_MYVIDEO));
            return LsIncludedDirectoryList;
        }

        /// <summary>
        /// gets the default selection directory list for custom selection 
        /// </summary>
        /// <param name="nOptions"></param>
        /// <returns></returns>
        public static List<string> GetCustomBackupDefaultItems()
        {
            // set backup options
            List<string> LsIncludedDirectoryList = new List<string>();
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            LsIncludedDirectoryList.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            return LsIncludedDirectoryList;
        }



        /// <summary>
        /// Path names are limited to 248 characters.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsDirPathLengthAllowed(string path)
        {
            return !(path.Length > 248);
        }

        /// <summary>
        /// File names are limited to 260 characters
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFilePathLengthAllowed(string path)
        {
            return !(path.Length > 260);
        }


        public static bool DirectoryCanListFiles(string folder)
        {
            try
            {
                bool hasAccess = false;
                //Step 1. Get the userName for which, this app domain code has been executing
                string executingUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                NTAccount acc = new NTAccount(executingUser);
                SecurityIdentifier secId = acc.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;

                DirectorySecurity dirSec = Directory.GetAccessControl(folder);

                //Step 2. Get directory permission details for each user/group
                System.Security.AccessControl.AuthorizationRuleCollection authRules = dirSec.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule ar in authRules)
                {
                    if (secId.CompareTo(ar.IdentityReference as SecurityIdentifier) == 0)
                    {
                        var fileSystemRights = ar.FileSystemRights;
                        Console.WriteLine(fileSystemRights);

                        //Step 3. Check file system rights here, read / write as required
                        if (fileSystemRights == FileSystemRights.Read ||
                            fileSystemRights == FileSystemRights.ReadAndExecute ||
                            fileSystemRights == FileSystemRights.ReadData ||
                            fileSystemRights == FileSystemRights.ListDirectory)
                        {
                            hasAccess = true;
                        }
                    }
                }
                return hasAccess;
            }
            catch (System.UnauthorizedAccessException ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DirectoryCanListFiles::UnauthorizedAccessException", ex);
                return false;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DirectoryCanListFiles::DirectoryNotFoundException", ex);
                return false;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DirectoryCanListFiles::Exception", ex);
                return false;
            }

        }



        /// <summary>
        /// Reads the file permissions and verifies the file exists or not.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool DoesFileHasReadPermissions(string file)
        {
            try
            {
                bool hasAccess = false;
                //Step 1. Get the userName for which, this app domain code has been executing
                string executingUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                NTAccount acc = new NTAccount(executingUser);
                SecurityIdentifier secId = acc.Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;

                FileSecurity sec = File.GetAccessControl(file);

                //Step 2. Get directory permission details for each user/group
                System.Security.AccessControl.AuthorizationRuleCollection authRules = sec.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule ar in authRules)
                {
                    if (secId.CompareTo(ar.IdentityReference as SecurityIdentifier) == 0)
                    {
                        var fileSystemRights = ar.FileSystemRights;
                        Console.WriteLine(fileSystemRights);

                        //Step 3. Check file system rights here, read / write as required
                        if (fileSystemRights == FileSystemRights.Read ||
                            fileSystemRights == FileSystemRights.ReadAndExecute ||
                            fileSystemRights == FileSystemRights.ReadData ||
                            fileSystemRights == FileSystemRights.ListDirectory)
                        {
                            hasAccess = true;
                        }
                    }
                }
                return hasAccess;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DoesFileHasReadPermissions", ex);
                return false;
            }

        }

        public static void SafeDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("FileFunctions::SafeDelete", ex);
            }

        }


        public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            bool bSuccess = false;
            try
            {
                // Get the subdirectories for the specified directory.
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                DirectoryInfo[] dirs = dir.GetDirectories();

                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                // If the destination directory doesn't exist, create it. 
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, false);
                }

                // If copying subdirectories, copy them and their contents to new location. 
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }

                bSuccess = true;
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogVerbose("DirectoryCopy", ex);
                bSuccess = false;
            }

            return bSuccess;

        }


    }
}
