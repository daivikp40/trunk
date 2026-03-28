using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
   public  class cExtraSettings
    {
        private bool _DbConsiderTimeInterval = true;
        public bool DbConsiderTimeInterval
        {
            get
            {
                return _DbConsiderTimeInterval;
            }
            set
            {
                _DbConsiderTimeInterval = value;
            }

        }

        private bool _DbConsiderGPS = true;
        public bool DbConsiderGPS {
            get { return _DbConsiderGPS; }
            
            set   { _DbConsiderGPS = value; }
        
        }

        private bool _DbConsiderTimeNoCaptureDt = false;
        public bool DbConsiderTimeNoCaptureDt {
            get { 
                return _DbConsiderTimeNoCaptureDt; 
            }
            set { 
                _DbConsiderTimeNoCaptureDt = value; }
            
            
                }
        private bool _DbIncludeRotatedImages = true;
        public bool DbIncludeRotatedImages {
            get
            {
                return _DbIncludeRotatedImages;
            }

            set
            {
                _DbIncludeRotatedImages = value;
            } 
        }

        private bool  _DbIncludeFlippedImages = true; 
        public bool DbIncludeFlippedImages {

            get { return _DbIncludeFlippedImages; }

            set
            {

                _DbIncludeFlippedImages = value;
            }
        }


    
    }
}
