using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;



namespace DuplicatePhotosFixer
{
    //LANG_FILE_SETTINGS(_T("en"),          _T("English"), _T("eng_rcp.ini") , _T("english"), _T("Arial"), -12, DEFAULT_CHARSET, 33, TRUE ) 


    public class cLanguageSettings
    {
        // http://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx 

        /// <summary>
        /// Language name Abbreviation (en,du)
        /// </summary>
        public string NameAbbv { get; set; }

        /// <summary>
        /// Full DisplayName (Chinese (Traditional))
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of INI file to be loaded
        /// </summary>
        public string IniName { get; set; }

        /// <summary>
        /// Only Name of Language like (English,chinese)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Font to be used if other than default
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Reduce size of font for adjustment
        /// </summary>
        public int ReduceFontSize { get; set; }

        /// <summary>
        /// Charset to be used
        /// </summary>
        public int CharSet { get; set; }

        /// <summary>
        /// Language Identifier from enum
        /// </summary>
        public cClientEnum.LanguageIdentifier LangIdentifier { get; set; }

        /// <summary>
        /// Other settings
        /// </summary>
        public bool ChangeSize { get; set; }

        public Image ImageFlag { get; set; }
        public string HTMLImageName { get; set; }

        /// <summary>
        /// Reduce size of font for adjustment
        /// </summary>
        public int ImagePosition { get; set; }

        public cLanguageSettings()
        {

        }

        public cLanguageSettings(string oNameAbbv, string oDisplayName, string oIniName, string oName, string oFontName, int oReduceFontSize, cClientEnum.LanguageIdentifier oLangIdentifier, Image oImgFlag, string oHtmlImgName, int nImagePosition)
        {
            NameAbbv = oNameAbbv;
            DisplayName = oDisplayName;
            IniName = oIniName;
            Name = oName;
            FontName = oFontName;
            ReduceFontSize = oReduceFontSize;
            LangIdentifier = oLangIdentifier;
            ImageFlag = oImgFlag;
            HTMLImageName = oHtmlImgName;
            ImagePosition = nImagePosition;
        }
    }
}
