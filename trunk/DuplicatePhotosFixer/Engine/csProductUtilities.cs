using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Engine
{
    public class csProductsUtilities
    {
        public int productID { get; set; }
        public int Priority { get; set; }
        public string product_name { get; set; }
        public string product_description { get; set; }
        public string product_summary { get; set; }
        public string strEULALink { get; set; }
        public string strPPLink { get; set; }
        public string installurl { get; set; }
        public string product_registry_key_name { get; set; }
        public string product_exe { get; set; }
        public string product_folder_name { get; set; }
        public string product_registry_path { get; set; }
        public string productInstallPath { get; set; }
        public cClientEnum.PBInstalledState productInstallStatus { get; set; }
        public string strSummary { get; set; }
        public List<string> lstFeatures { get; set; }
        public Image logo { get; set; }
        public string strInstallParams { get; set; }

   
}
}

