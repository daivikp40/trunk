using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
  public  class cGeneralSettings
    {

        #region general settings

        private bool m_OpenDialogOnDeleteChecked = false;
        public bool OpenDialogOnDeleteChecked 
        {
            get { return m_OpenDialogOnDeleteChecked; }
            set { m_OpenDialogOnDeleteChecked = value; }
        }

        private bool m_OpenAssistantChecked = false;
        public bool OpenAssistantChecked
        {
            get
            {
               return m_OpenAssistantChecked;
            }
            set
            {
                m_OpenAssistantChecked= value;
            }
        }

        private bool m_BackbtnChecked = false;
        public bool BackbtnChecked
        {
            get
            {
                return m_BackbtnChecked;
            }
            set
            {
                m_BackbtnChecked = value;
            }
        }

        private bool m_SysStartupChecked = false;
        public bool SysStartupChecked
        {
            get
            {
                return m_SysStartupChecked;
            }
            set
            {
                m_SysStartupChecked = value;
            }
        }

        private bool m_ProductMesChecked = true;

        public bool ProductMesChecked
        {
            get
            {
                return m_ProductMesChecked;
            }

            set
            {
                m_ProductMesChecked = value;
            }
        }

        private bool m_OtherProductMesChecked = true;
        public bool OtherProductMesChecked
        {
            get { return m_OtherProductMesChecked; }
            set { m_OtherProductMesChecked = value; }
        }

        private bool m_PeriodicMesChecked = true;
        public bool PeriodicMesChecked
        {
            get { return m_PeriodicMesChecked; }
            set { m_PeriodicMesChecked = value; }
        }



        #endregion

        // public  bool bNotShowDeleteConfirmation = false;
        // public  bool bNotOpenAssistantConfirmation = false;
        // public  bool bNotShowBackConfirmation = false;
        // public  bool StartScanSystemStartup = true;
        // public  bool bShowProductMessages = true;
        // public  bool bShowOtherProductMessages = true;
        //// public  bool bCheckUpdates = true;
        // public bool bShowPeriodicMessages = true;
    }
}
