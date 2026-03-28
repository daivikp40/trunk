using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyLogger;

namespace DuplicatePhotosFixer.Engine
{
   public class IgnoreRules
    {


        /// <summary>
        /// Regular Expressions input strings
        /// </summary>
        static string DirExclusionRulesHard = @".:\\"
           + @"("
           + @"RECYCLER|RECYCLED|\$RECYCLE\.BIN|System Volume Information|windows|windows\\temp|windows\.old|windows(.+?).old|Recovery|MSOCache|QUARANTINE|PerfLogs|winddk|" /*+ cGlobalSettings.TempFolderName + "|"*/
           + @"(Users|Documents and Settings)\\(.+?)\\((start menu|recent)|(appdata|application data|Local Settings)(\\temp|\\application data|\\local|\\locallow|\\roaming|\\local\\microsoft\\windows|\\roaming\\microsoft\\windows|Local Settings\\temp|\\Local\\Temp|\\local\\microsoft\\windows\\temporary internet files|\\local\\microsoft\\internet explorer|\\Local\\Microsoft\\Windows Mail\\Stationary|\\Start Menu|\\Roaming\\Microsoft\\Windows\\Start Menu|\\" + cGlobalSettings.GetCompanyName() + @"\\" + cGlobalSettings.GetProductNameFromDesc() + "))"
           + @")+($|\\|\\(.+?))".ToLower();
        static string DirExclusionRulesSoft = @".:\\"
            + @"("
            + @"Program Files|Program Files \(x86\)|temp1|rbtemp|temp|inetpub|ProgramData|help|i386|inf|idefense|totalcmd|local cloud|python27|"
            //+ @"(Users|Documents and Settings)\\(Default|Public|All Users|(.+?))\\(pictures\\theme.|syncfolder|google drive|dropbox|cloud drive|copy|Application Data|UserData|SendTo|Templates|PrintHood|NetHood|Cookies)"
            + @"(Users|Documents and Settings)\\(Default|Public|All Users|(.+?))\\(pictures\\theme.|Application Data|UserData|SendTo|Templates|PrintHood|NetHood|Cookies)"
            + @")"
            + @"+($|\\|\\(.+?))".ToLower();

        static string FilesExclusionRulesHard = @"kalimba\.wav|ding\.wav".ToLower();

        public static SerializableDictionary<string, List<string>> PathExclusionRulesHard = new SerializableDictionary<string, List<string>>
        {
            {
                ":\\documents and settings\\SYSTEM_USER_NAME\\",
                new List<string>
                {
                    @"documents\my music\music.asx",
                    @"documents\my music\music.bmp",
                    @"documents\my music\music.wma",
                    @"documents\my music\sample music\beethoven's symphony no. 9 (scherzo).wma",
                    @"documents\my music\sample music\new stories (highway blues).wma",
                    @"documents\my pictures\sample pictures\blue hills.jpg",
                    @"documents\my pictures\sample pictures\sunset.jpg",
                    @"documents\my pictures\sample pictures\water lilies.jpg",
                    @"documents\my pictures\sample pictures\winter.jpg",

                    @"contacts\\admin.contact", // exclusion items from other categories
                    @"favorites\\microsoft websites\\ie site on microsoft.com.url",
                    @"favorites\\microsoft websites\\ie add-on site.url",
                    @"favorites\\microsoft websites\\microsoft at home.url",
                    @"favorites\\microsoft websites\\microsoft at work.url",
                    @"favorites\\microsoft websites\\microsoft store.url",
                    @"favorites\\msn websites\\msn.url",
                    @"favorites\\msn websites\\msn sports.url",
                    @"favorites\\msn websites\\msnbc news.url",
                    @"favorites\\msn websites\\msn money.url",
                    @"favorites\\msn websites\\msn entertainment.url",
                    @"favorites\\msn websites\\msn autos.url",
                    @"favorites\\windows live\\windows live mail.url",
                    @"favorites\\windows live\\windows live spaces.url",
                    @"favorites\\windows live\\windows live gallery.url",
                    @"favorites\\windows live\\get windows live.url",
                    @"favorites\\links\\web slice gallery.url",
                    @"favorites\\links\\suggested sites.url",
                    @"favorites\\links for united states\\usa.gov.url",
                    @"favorites\\links for united states\\gobiernousa.gov.url",
                    @"favorites\\bing.url", // windows 8
                    @"recorded tv\\sample media\\win7_scenic-demoshort_raw.wtv",
                    @"searches\\winrt--{s-1-5-21-4251017031-2885081038-2634948052-1001}-.searchconnector-ms",

                    @"favorites\\msn.com.url",
                    @"favorites\\radio station guide.url",
                    @"favorites\\links\\customize links.url",
                    @"favorites\\links\\free hotmail.url",
                    @"favorites\\links\\windows.url",
                    @"favorites\\links\\windows media.url",
                    @"favorites\\links\\windows marketplace.url"
                    /* -- can include user specific list also
                    @"documents\\my music\\sample playlists\\000b1799\\plylst11.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst9.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst8.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst7.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst6.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst5.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst4.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst2.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst15.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst14.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst13.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst12.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst3.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst10.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst1.wpl",
                    //@"documents\\my music\\my playlists\\sample playlist.wpl" -- since users added music files are listed in it.*/



                }
            },
            {
                ":\\users\\SYSTEM_USER_NAME\\",
                new List<string>
                {
                    /*@"music\sample music\amanda.wma",
                    @"music\sample music\despertar.wma",
                    @"music\sample music\din din wo (little child).wma",
                    @"music\sample music\distance.wma",
                    @"music\sample music\i guess you're right.wma",
                    @"music\sample music\i ka barra (your work).wma",
                    @"music\sample music\kalimba.mp3",
                    @"music\sample music\love comes.wma",
                    @"music\sample music\maid with the flaxen hair.mp3",
                    @"music\sample music\muita bobeira.wma",
                    @"music\sample music\oam's blues.wma",
                    @"music\sample music\one step beyond.wma",
                    @"music\sample music\sleep away.mp3",
                    @"music\sample music\symphony_no_3.wma",*/
                    @"pictures\sample pictures\autumn leaves.jpg",
                    @"pictures\sample pictures\chrysanthemum.jpg",
                    @"pictures\sample pictures\creek.jpg",
                    @"pictures\sample pictures\desert landscape.jpg",
                    @"pictures\sample pictures\desert.jpg",
                    @"pictures\sample pictures\dock.jpg",
                    @"pictures\sample pictures\forest flowers.jpg",
                    @"pictures\sample pictures\forest.jpg",
                    @"pictures\sample pictures\frangipani flowers.jpg",
                    @"pictures\sample pictures\garden.jpg",
                    @"pictures\sample pictures\green sea turtle.jpg",
                    @"pictures\sample pictures\humpback whale.jpg",
                    @"pictures\sample pictures\hydrangeas.jpg",
                    @"pictures\sample pictures\jellyfish.jpg",
                    @"pictures\sample pictures\koala.jpg",
                    @"pictures\sample pictures\lighthouse.jpg",
                    @"pictures\sample pictures\oryx antelope.jpg",
                    @"pictures\sample pictures\penguins.jpg",
                    @"pictures\sample pictures\toco toucan.jpg",
                    @"pictures\sample pictures\tree.jpg",
                    @"pictures\sample pictures\tulips.jpg",
                    @"pictures\sample pictures\waterfall.jpg",
                    @"pictures\sample pictures\winter leaves.jpg",
                    @"pictures\sample pictures\blue hills.jpg",
                    @"pictures\sample pictures\sunset.jpg",
                    @"pictures\sample pictures\water lilies.jpg",
                    @"pictures\sample pictures\winter.jpg",
                    /*@"videos\sample videos\bear.wmv",
                    @"videos\sample videos\butterfly.wmv",
                    @"videos\sample videos\lake.wmv",
                    @"videos\sample videos\wildlife.wmv",*/
                    @"contacts\\admin.contact",                   
                    
                    /*@"contacts\\admin.contact", // exclusion items from other categories
                    @"favorites\\microsoft websites\\ie site on microsoft.com.url",
                    @"favorites\\microsoft websites\\ie add-on site.url",
                    @"favorites\\microsoft websites\\microsoft at home.url",
                    @"favorites\\microsoft websites\\microsoft at work.url",
                    @"favorites\\microsoft websites\\microsoft store.url",                    
                    @"favorites\\msn websites\\msn.url",
                    @"favorites\\msn websites\\msn sports.url",
                    @"favorites\\msn websites\\msnbc news.url",
                    @"favorites\\msn websites\\msn money.url",
                    @"favorites\\msn websites\\msn entertainment.url",
                    @"favorites\\msn websites\\msn autos.url",
                    @"favorites\\windows live\\windows live mail.url",
                    @"favorites\\windows live\\windows live spaces.url",
                    @"favorites\\windows live\\windows live gallery.url",
                    @"favorites\\windows live\\get windows live.url",
                    @"favorites\\links\\web slice gallery.url",
                    @"favorites\\links\\suggested sites.url",
                    @"favorites\\links for united states\\usa.gov.url",
                    @"favorites\\links for united states\\gobiernousa.gov.url",
                    @"favorites\\bing.url", // windows 8
                    @"recorded tv\\sample media\\win7_scenic-demoshort_raw.wtv",*/
                    @"searches\\winrt--{s-1-5-21-4251017031-2885081038-2634948052-1001}-.searchconnector-ms"//,

                   /* @"favorites\\msn.com.url",
                    @"favorites\\radio station guide.url",
                    @"favorites\\links\\customize links.url",
                    @"favorites\\links\\free hotmail.url",
                    @"favorites\\links\\windows.url",
                    @"favorites\\links\\windows media.url",
                    @"favorites\\links\\windows marketplace.url"*///,
                    /* -- can include user specific list also
                    @"documents\\my music\\sample playlists\\000b1799\\plylst11.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst9.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst8.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst7.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst6.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst5.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst4.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst2.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst15.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst14.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst13.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst12.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst3.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst10.wpl",
                    @"documents\\my music\\sample playlists\\000b1799\\plylst1.wpl",
                    //@"documents\\my music\\my playlists\\sample playlist.wpl" -- since users added music files are listed in it.*/

                }
            }
        };


        public Regex IgnoreFolderHardPattern = null;
        public Regex IgnoreFolderSoftPattern = null;
        public Regex IgnoreFileHardPattern = null;
        public Regex IgnoreFileSoftPattern = null;
        public Regex IgnoreExtensionHardPattern = null;
        public Regex IgnoreExtensionSoftPattern = null;
        public Regex IgnorePathHardPattern = null;
        public Regex IgnoreExtensionForSmartOnlyPattern = null;
        public void GenerateRegEx()
        {
            List<string> FullPaths = null;
            try
            {
                IgnoreFolderHardPattern = CreateRegexExp(DirExclusionRulesHard.ToLower(), false);
                IgnoreFolderSoftPattern = CreateRegexExp(DirExclusionRulesSoft.ToLower(), false);
                IgnoreFileHardPattern = CreateRegexExp(FilesExclusionRulesHard.ToLower(), false);
                //IgnoreFileSoftPattern = CreateRegexExp(FilesExclusionRulesSoft.ToLower(), false);
                //IgnoreExtensionHardPattern = CreateRegexExp(ExtensionExclusionRulesHard.ToLower(), true);
                //IgnoreExtensionSoftPattern = CreateRegexExp(ExtensionExclusionRulesSoft.ToLower(), true);
                //IgnoreExtensionForSmartOnlyPattern = CreateRegexExp(ExtensionExclusionRulesForSmartOnly.ToLower(), true);

                FullPaths = PathExclusionRulesHard.SelectMany(x => x.Value.Select(y => PathEx.Combine(x.Key, y))).ToList(); ///have to add this from cLogger custom class ;;;for now using myLogger
                IgnorePathHardPattern = CreateRegexfromList(FullPaths);

                // GetExclusionFolderList(ref LsOtherUserPaths);
            }
            finally
            {
                if (FullPaths != null)
                {
                    FullPaths.Clear();
                    FullPaths = null;
                }
            }

        }
        /// <summary>
        /// Verify is directory is excluded or not based on file attributes
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsDirectoryIgnored(string FullName/*, FileAttributes Attributes*/)
        {
            return (
                FullName.ToLower().StartsWith("$rmmetadata")
                || (FullName.Length > 248)
                || (IgnoreFolderHardPattern != null && IgnoreFolderHardPattern.IsMatch(FullName))
                || (IgnoreFolderSoftPattern != null && IgnoreFolderSoftPattern.IsMatch(FullName))
                // || (ignoreRules.LsOtherUserPaths!=null && ignoreRules.LsOtherUserPaths.BinarySearch(FullName) > -1)
                || IsIgnoredSpecialFolder(FullName)
                );

        }

        /// <summary>
        /// To skip folders like created with symbol #255.
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool IsIgnoredSpecialFolder(string FullName)
        {
            return string.IsNullOrEmpty(new DirectoryInfo(FullName.AddBackSlash()).Name.Trim());
        }

        public static Regex CreateRegexfromList(List<string> LsItems)
        {
            string sRegex = string.Join("|", LsItems.Where(x => !string.IsNullOrEmpty(x)).Select(x => Regex.Escape(x.RemoveEndBackSlash().ToLower())).ToArray());
            sRegex = sRegex.Replace("SYSTEM_USER_NAME".ToLower(), "(.+?)");
            sRegex = string.Format(@"{0}+($|\\(.+?)|\\)", sRegex).ToLower();
            return new Regex(sRegex, RegexOptions.Compiled);
        }

        public Regex CreateRegexExp(string input, bool b_isExtension)
        {
            Regex newRex = null;
            try
            {
                //input = input.RegExAddEscapeSequenceToSpecialCharacters();
                input = b_isExtension ? @"\.(" + input + ")$" : input;
                //cGlobalSettings.oLogger.WriteLogVerbose("RegEx::input-"+input);
                newRex = new Regex(input, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            }
            catch (System.Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("IgnoreRulesUser::CreateRegexExp : ", ex);
            }
            return newRex;
        }
    }
}
