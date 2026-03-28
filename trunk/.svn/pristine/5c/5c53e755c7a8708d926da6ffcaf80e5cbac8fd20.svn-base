using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.Track
{
    public class WebClientEx
    {
        /// <summary>
        /// "Authorization": "bearer 36b2a50a940458361b3c012dc1ea65d8"
        /// </summary>
        public string DownloadString(string url)
        {
            string response = "";
            try
            {
                using (WebDownload oProxy = new WebDownload())
                {
                    ServicePointManager.ServerCertificateValidationCallback += customXertificateValidation;
                    HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                    oProxy.CachePolicy = noCachePolicy;

                    //System.Net.IWebProxy iwpxy = WebRequest.GetSystemWebProxy();
                    oProxy.Proxy = WebRequest.DefaultWebProxy; //iwpxy; //WebRequest.GetSystemWebProxy();

                    //oProxy.Proxy = WebProxy.GetDefaultProxy();
                    response = oProxy.DownloadString(url);
                    //cGlobalSettings.oLogger.WriteLogVerbose(string.Format("\t{0}\tResponse:\t{1}\t{2}", RandomNumber, email, response));
                    noCachePolicy = null;
                }
            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("DownloadString", ex);
            }

            return response;

        }

        private static bool customXertificateValidation(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            var certificate = (X509Certificate2)cert;

            // Inspect the server certficiate here to validate 
            // that you are dealing with the correct server.
            // If so return true, if not return false.
            return true;
        }
    }

    public class WebDownload : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebDownload() : this(1000 * 60 * 5) { }

        public WebDownload(int timeout)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }



    }
}
