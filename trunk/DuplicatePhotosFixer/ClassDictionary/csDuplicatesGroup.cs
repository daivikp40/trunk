using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public class csDuplicatesGroup
    {
        //here children will have nodes with one element also
        //It has all the matching children with match score >= 0.85. This is FOR the MAIN Duplicate source

        public List<int> childrenKey = new List<int>();

        //public List<csImageFileInfo> children = new List<csImageFileInfo>();
        //this array is updated as and when user changes the matching level.
        //This array content is shown to the user. It provides the filtered array of the above array, as per the matching level selected
        //Say if user selects Matching level as 0.95.... This array filters above array and shows the children with match score >= 0.95 ONLY
        public IEnumerable<csImageFileInfo> filteredChildren = null;
    }
}
