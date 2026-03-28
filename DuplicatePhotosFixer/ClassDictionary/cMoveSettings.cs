using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
   public  class cMoveSettings
    {
        private bool m_DbMoveToRecycle = true;
        public bool DbMoveToRecycle
        {
            get { return m_DbMoveToRecycle; }
            set { m_DbMoveToRecycle = value; }
        }

        private bool m_DbMoveToLocation = false;
        public bool DbMoveToLocation
        {
            get { return m_DbMoveToLocation; }
            set { m_DbMoveToLocation = value; }
        }

        private string m_DbLocationPath = string.Empty;
        public string DbLocationPath
        {
            get
            {
                if (string.IsNullOrEmpty(m_DbLocationPath))
                {
                    /*m_FilePathToMoveThisLocation = cGlobalSettings.FilePathToMoveThisLocation();*/
                }
                return m_DbLocationPath;
            }
            set { m_DbLocationPath = value; }
        }
    }
}
