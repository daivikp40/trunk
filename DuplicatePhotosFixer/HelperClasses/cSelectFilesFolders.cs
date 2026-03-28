using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using DuplicatePhotosFixer.CustomExtendClass;

namespace DuplicatePhotosFixer.HelperClasses
{
    class cSelectFilesFolders
    {
        public class WpfWindowWrapper : System.Windows.Forms.IWin32Window
        {
            public WpfWindowWrapper(System.Windows.Window wpfWindow)
            {
                Handle = new System.Windows.Interop.WindowInteropHelper(wpfWindow).Handle;
            }

            public IntPtr Handle { get; private set; }
        }



        public static string[] ShowFolder_System(Window owner)
        {
            FolderSelectDialog dlgFolder = null;
            string[] selectedFolder = null;
            try
            {
                dlgFolder = new FolderSelectDialog();

                if (dlgFolder.ShowDialog(new WpfWindowWrapper(owner).Handle))
                {
                    selectedFolder = dlgFolder.FileName.Split('|');
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHome :btnAddFolder_Click ", ex);
            }
            finally
            {
                dlgFolder = null;
            }

            return selectedFolder;
        }


        public static string[] ShowFile_System(Window owner)
        {
            FileSelectDialog dlgFile = null;
            string[] selectedFile = null;
            try
            {
                dlgFile = new FileSelectDialog();
                

                if (dlgFile.ShowDialog())
                {
                    selectedFile = dlgFile.FileNames;
                }
            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("ucHome :btnAddPhotos_Click ", ex);
            }
            finally
            {
                dlgFile = null;
            }

            return selectedFile;
        }
    }
}
