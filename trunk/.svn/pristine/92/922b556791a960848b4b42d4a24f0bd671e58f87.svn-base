using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DuplicatePhotosFixer.TrialProtector
{
    public static class KeyValidator
    {
#if _PRODUCT_KEY_NAME_
    internal const string StrProductName = "DPFXR"; //*/ string(_T("D")) + _T("F") + string(_T("F")) + _T("X") + string(_T("R"));
#else
        internal const string StrProductName = "DPFXR";
        //#error "define _PRODUCT_KEY_NAME_"
#endif

        static KeyValidator()
        {
            strKeysUsed = "";
            StoreSecrets = new Dictionary<USERSTRINGNUMBERS, string>();
            SetProgGUID();
            string guid = GetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED);
        }

        public static void ResetEverything()
        {
            try
            {
                string appDataFolderPath = KeyValidator.getAppDataApplicationPath();

                if (System.IO.Directory.Exists(appDataFolderPath))
                {
                    System.IO.Directory.Delete(appDataFolderPath, true);
                }

                try
                {
                    Registry.CurrentUser.DeleteSubKeyTree(KeyValidator.OurBaseKey);
                }
                catch (Exception ex)
                {
                    ;
                }

                try
                {
                    Registry.LocalMachine.DeleteSubKeyTree(KeyValidator.OurBaseKey);
                }
                catch (Exception ex)
                {
                    ;
                }
            }
            catch (Exception ex)
            {

                ;
            }

            try
            {
                strKeysUsed = "";
                if (StoreSecrets != null)
                {
                    StoreSecrets.Clear();
                    StoreSecrets = null;
                }
                StoreSecrets = new Dictionary<USERSTRINGNUMBERS, string>();
                string guid = GetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED);
                SetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED, Guid.NewGuid().ToString());
                string guid1 = GetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED);
            }
            catch (Exception ex)
            {
                ;
            }

        }

        /// <summary>
        /// Change these according to the product
        /// </summary>
        internal const string OurBaseKey = "Software\\" + StrProductName + "\\key"; //string(_T("S")) + _T("o") + string(_T("f")) + _T("t") + string(_T("w")) + _T("a") + string(_T("r")) + _T("e") + string(_T("\\")) + _T("D") + string(_T("F")) + _T("F") + string(_T("X")) + _T("R") + string(_T("\\")) + _T("k") + string(_T("e")) + _T("y");    
        const string strDefaultSecureKey = "Microsoft Visual C++ Library";

        const string DateInstalled = "DateInstalled";// */ string(_T("D")) + _T("a") + string(_T("t")) + _T("e") + string(_T("I")) + _T("n") + string(_T("s")) + _T("t") + string(_T("a")) + _T("l") + string(_T("l")) + _T("e") + string(_T("d"));
        const string ReadValue_PathPattern_Reg = "{0}\\{1}";// string(_T("%")) + _T("s") + string(_T("\\")) + _T("%") + string(_T("d"));
        const string strAppDataFileName = "backup{0}.bin";// */ string(_T("b")) + _T("a") + string(_T("c")) + _T("k") + string(_T("u")) + _T("p") + string(_T("%")) + _T("d") + string(_T(".")) + _T("b") + string(_T("i")) + _T("n");


        static readonly byte[] arrMain = new byte[30] { (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6', (byte)'7', (byte)'8', (byte)'9', (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H', (byte)'J', (byte)'K', (byte)'M', (byte)'N', (byte)'P', (byte)'R', (byte)'S', (byte)'T', (byte)'U', (byte)'V', (byte)'W', (byte)'X', (byte)'Y', (byte)'Z' };
        static readonly byte[] arrShuffle = new byte[30] { (byte)'9', (byte)'P', (byte)'H', (byte)'V', (byte)'Y', (byte)'B', (byte)'7', (byte)'E', (byte)'M', (byte)'6', (byte)'T', (byte)'W', (byte)'5', (byte)'S', (byte)'F', (byte)'Z', (byte)'8', (byte)'K', (byte)'U', (byte)'A', (byte)'3', (byte)'C', (byte)'N', (byte)'X', (byte)'4', (byte)'G', (byte)'2', (byte)'D', (byte)'R', (byte)'J' };
        static readonly byte[] arrTMStamp = new byte[30] { (byte)'V', (byte)'E', (byte)'9', (byte)'W', (byte)'M', (byte)'B', (byte)'C', (byte)'X', (byte)'8', (byte)'S', (byte)'P', (byte)'3', (byte)'D', (byte)'J', (byte)'T', (byte)'7', (byte)'A', (byte)'5', (byte)'G', (byte)'Z', (byte)'U', (byte)'N', (byte)'H', (byte)'F', (byte)'2', (byte)'R', (byte)'6', (byte)'K', (byte)'Y', (byte)'4' };

        static readonly string DateTimeStoreFormat = "yyyy-MM-dd";
        /// <summary>
        /// Data stored in these variable for further key status check
        /// </summary>
        static string strKeysUsed = "";
        static Dictionary<USERSTRINGNUMBERS, String> StoreSecrets = null;

        public enum USERSTRINGNUMBERS
        {
            USN_REG_ERR_COUNT = 0,
            USN_ICON_CREATED = 1,
            USN_REG_USER_KEY_INSTALL_DATE = 2,
            USN_REG_USER_KEY = 3,
            USN_BF_ICON_CREATED = 4,
            USN_TP_ICON_CREATED = 5,
            USN_GUID_GENERATED = 6, // Our Encryption Key stored in this number, we will generate GUID and store this.
            USN_NOTUSED_7,
        };

        public enum SYS_TRIAL_VERSION_STATUS
        {
            ///not for Dev. choose variable prime numbers to confuse cracker what these return values means
            //STATUS_IS_RETAIL_VERSION = 40507, // this may be retail version do not requiing any key or activation
            //TRIALPROTECTION::GetST_IsRetailKey() please use this function instead of above constant

            STATUS_EVAL_VERSION_EXPIRED = 181991, //the evaluation version has expired
            STATUS_SUBSCRIPTION_EXPIRED = 162997, //the subscription has been ended but version may be registred
            STATUS_SUBSCRIPTION_ACTIVATION_FAILED = 324931, //the subcscription is valid but activation has been failed
            STATUS_EVAL_PERIOD_REMAINING = 648973, //the evaluation period is still remaning
            STATUS_SUBSCRIPTION_REMAINING = 1896161,
        };

        public enum PCO_ERROR_CODES
        {
            OPERATION_RESULT_TRUE = 1, ////for places where we need like whether it is true or false
            OPERATION_RESULT_FALSE = 0, //for places where we need like whether it is true or false

            OPERATION_FAILED = 0, //basic error codes
            OPERATION_SUCCESS = 1, //basic error codes
                                   ////////////////for ERRORS All codes should be less than 0///////////
            OPERATION_FAIL_EXCEPTION = -1,
            OPERATION_FAIL_NO_CODE_YET = -2, //code for this has still to be written
            OPERATION_FAIL_FILE_NOT_EXISTS = -3,
            OPERATION_FAIL_STRING_LEN_ZERO = -4
            //////////////////////////////////////////////////////////////////////////

        };

        static UInt16[] crctab = new UInt16[256]{
            0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
            0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
            0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
            0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
            0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
            0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
            0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
            0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
            0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
            0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
            0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
            0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
            0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
            0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
            0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
            0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
            0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
            0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
            0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
            0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
            0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
            0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
            0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
            0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
            0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
            0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
            0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
            0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
            0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
            0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
            0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
            0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
        };


        public static UInt16 Crc16(byte[] data, int length)
        {
            int index = 0;
            uint crc = 0;
            for (crc = 0; length > 0; length--)
                crc = ((crc << 8) ^ crctab[((crc >> 8) ^ (data[index++]))]) & 0xFFFF;
            return (UInt16)crc;
        }





        static void DecryptCRC(byte[] strCRC, ref string strDecCRC)
        {

            for (int i = 0; i < 5; i++)
            {
                byte a = strCRC[i];

                int p = 0;
                for (p = 0; p < 10; p++)
                {
                    byte x = arrShuffle[p];
                    if (x == a)
                    {
                        break;
                    }
                }

                string sTemp = p.ToString();  //sTemp.Format(("%d"),p);            
                strDecCRC += sTemp;
            }


        }

        static bool MPDecrypt(byte[] strBlock1RND, byte[] strEncPN, ref byte[] strProductNameKey)
        {
            int i = 0;


            for (i = 0; i < 5; i++)
            {
                byte a = strEncPN[i];
                byte b = strBlock1RND[i];

                int firstcharpos = -1;
                int secondcharpos = -1;
                for (int j = 0; j < 30; j++)
                {
                    if (arrShuffle[j] == a)
                    {
                        firstcharpos = j;
                    }
                    if (arrMain[j] == b)
                    {
                        secondcharpos = j;
                    }
                }

                if (firstcharpos == -1 || secondcharpos == -1)
                {
                    Debug.Assert(false);
                }

                int nEncryptedCharPosInMainArray = -1;
                nEncryptedCharPosInMainArray = firstcharpos - secondcharpos;

                if (nEncryptedCharPosInMainArray < 0)
                {
                    nEncryptedCharPosInMainArray = nEncryptedCharPosInMainArray + 30;
                    strProductNameKey[i] = arrMain[nEncryptedCharPosInMainArray];
                }
                else
                {
                    strProductNameKey[i] = arrMain[nEncryptedCharPosInMainArray];
                }

            }

            //strProductNameKey[i] = '\0';

            string strProductNameKey2 = Encoding.Default.GetString(strProductNameKey);
            //if(!lstrcmpi(StrProductName,strProductNameKey))
            if (string.Compare(StrProductName, strProductNameKey2, true) != 0)
            {
                strProductNameKey = Encoding.Default.GetBytes("WRONG");
                return false;
            }
            return true;
        }

        static void DecryptedDate(ref byte[] strEncDate, ref long nY, ref long nM, ref long nD, ref long nForNumYears, ref long nForNumMonths)
        {
            //nForNumYears = 0,1,2,3,4,5,6,7,8,9,10
            byte szForNumYears = strEncDate[0];
            for (int i = 0; i < 11; i++)
            {
                if (arrTMStamp[i] == szForNumYears)
                {
                    nForNumYears = i;
                    break;
                }
            }
            //nForNumMonths = 11 (0),12,13,14,15,16,17,18,19,20,21,22(11)
            byte szForNumMonths = strEncDate[1];
            int nPosForMonths = 11;
            for (int i = 11; i < 23; i++)
            {
                if (arrTMStamp[i] == szForNumMonths)
                {
                    nPosForMonths = i;
                    break;
                }
            }

            nForNumMonths = nPosForMonths - 11;

            // Y = 14,15,16,17,18,19,20,21,22,23,24,25,26,27,28
            // M = 1,2,3,4,5,6,7,8,9,10,11,12
            // D = 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30
            //ll We witake Y from arrTMStamp - 14-15 = 0,1, 16-17=2,3 
            //So logic can be Y%14 + 1 (Since we are starting form year 14)
            //14%14  = 0*2 = 0 and + 1 = 1
            //15%14  = 1*2 = 2 and + 1 = 3
            //16%14  = 2*2 = 4 and + 1 = 5
            //17%14  = 3*2 = 6 and + 1 = 7
            //18%14  = 4*2 = 8 and + 1 = 9
            //..
            //so on

            //same for month
            //For date equal take exact till 30

            //char arrTMStamp[30] =	{'V','E','9','W','M','B','C','X','8','S','P','3','D','J','T','7','A','5','G','Z','U','N','H','F','2','R','6','K','Y','4'};

            //Remove first 2 characters as they are just random numbers
            int nLen = strEncDate.Length;
            Debug.Assert(nLen == 5);

            byte szY = strEncDate[2];
            byte szM = strEncDate[3];
            byte szD = strEncDate[4];

            int nYPos = -1;
            int nMPos = -1;
            int nDPos = -1;

            for (int i = 0; i < 30; i++)
            {
                if (arrTMStamp[i] == szY)
                {
                    nYPos = i;
                }
                if (arrTMStamp[i] == szM)
                {
                    nMPos = i;
                }
                if (arrTMStamp[i] == szD)
                {
                    nDPos = i;
                }

                if (nYPos >= 0 && nMPos >= 0 && nDPos >= 0)
                {
                    break;
                }
            }

            Debug.Assert(!(nYPos == -1));
            Debug.Assert(!(nMPos == -1));
            Debug.Assert(!(nDPos == -1));

            //if ypos = 0,1 - 14, 2,3 = 15, 4,5 = 16, 6,7 = 17

            nY = ((nYPos / 2) + 14);

            //if nMPos 1%12=1*2=2, +1 = 3 , 2%12=2*2=4, +1=5
            nM = ((nMPos / 2));

            if (nM == 0)
            {
                nM = 12; //12 month holds array pos 0,1
            }


            if (nDPos == 0)
            {
                nD = 30;
            }
            else
            {
                nD = nDPos;
            }

        }

        public static bool ValidateKey(string strInKey, ref long nDays, ref long nMonth, ref long nYear, ref long nDaysAllowed)
        {
            bool bRetVal = false;

            byte[] strBlock1RND = null;
            byte[] strEncPN = null;
            byte[] strEncTM = null;
            byte[] strEncCRC = null;
            byte[] strProductNameKey = null;

            try
            {
                string strKey = strInKey.Trim();

                int nkeyLen = strKey.Length;

                if (nkeyLen != 23)
                {
                    return bRetVal;
                }

                string cstrBlock1RND;
                string cstrEncPN;
                string cstrEncTM;
                string cstrEncCRC;
                int nPos = strKey.IndexOf("-");
                if (nPos != -1)
                {
                    string[] items = strKey.Split('-');

                    if (items.Length < 4)
                        return false;

                    cstrBlock1RND = items[0];
                    cstrEncPN = items[1];
                    cstrEncTM = items[2];
                    cstrEncCRC = items[3];
                }
                else
                {
                    return bRetVal;
                }

                int nLen = 5;

                strBlock1RND = Encoding.Default.GetBytes(cstrBlock1RND);
                strEncPN = Encoding.Default.GetBytes(cstrEncPN);
                strEncTM = Encoding.Default.GetBytes(cstrEncTM);
                strEncCRC = Encoding.Default.GetBytes(cstrEncCRC);

                strProductNameKey = new byte[(nLen /*+ 1*/)];
                //dono se orginal product name aa jayga..in strProductNameKey..
                //	MPDecrypt(strBlock1RND, strEncPN, strProductNameKey);
                if (!MPDecrypt(strBlock1RND, strEncPN, ref strProductNameKey))
                {

                    strBlock1RND = null;
                    strEncPN = null;
                    strProductNameKey = null;
                    strEncTM = null;
                    strEncCRC = null;

                    return false;

                }

                long nForNumYears = 0, nForNumMonths = 0;
                DecryptedDate(strEncTM, ref nYear, ref nMonth, ref nDays, ref nForNumYears, ref nForNumMonths);
                if (nYear < 100)
                {
                    nYear += 2000;
                }

                nDaysAllowed = (nForNumYears * 365) + (nForNumMonths * 30) + 4; //4 din extra de do, apan hamesha dete aaye hain


                string strDecCRC = "";
                DecryptCRC(strEncCRC, ref strDecCRC);
#if _DEBUG
	string strDate;

	strDate = string.Format("{0}-{1}-{2}", nYear, nMonth, nDays );

#endif
                int nDec_CRC = int.Parse(strDecCRC);


                int nLenOfWholeKey = 3 * nLen + 2; //i.e. = 17 /*3 blocks of nLen i.e. randnum, productname, timestamp  + 2 hyphens */ 
                string STR_FINAL_KEY = string.Format("{0}-{1}-{2}", Encoding.Default.GetString(strBlock1RND), Encoding.Default.GetString(strProductNameKey), Encoding.Default.GetString(strEncTM));


                byte[] szHex = Encoding.Default.GetBytes(STR_FINAL_KEY);
                //strcpy(szHex, CW2A(STR_FINAL_KEY));
                //str.ReleaseBuffer();

                UInt16 ss = Crc16(szHex, nLenOfWholeKey);

                szHex = null;

                if (nDec_CRC == ss)
                {
                    bRetVal = true;
                }
                else
                {
                    bRetVal = false;
                }


            }
            catch (Exception)
            {
                ;

            }
            finally
            {
                strBlock1RND = null;
                strEncPN = null;
                strProductNameKey = null;
                strEncTM = null;
                strEncCRC = null;
            }



            return bRetVal;
        }

        static void DecryptedDate(byte[] strEncDate, ref long nY, ref long nM, ref long nD, ref long nForNumYears, ref long nForNumMonths)
        {
            //nForNumYears = 0,1,2,3,4,5,6,7,8,9,10
            byte szForNumYears = strEncDate[0];
            for (int i = 0; i < 11; i++)
            {
                if (arrTMStamp[i] == szForNumYears)
                {
                    nForNumYears = i;
                    break;
                }
            }
            //nForNumMonths = 11 (0),12,13,14,15,16,17,18,19,20,21,22(11)
            byte szForNumMonths = strEncDate[1];
            int nPosForMonths = 11;
            for (int i = 11; i < 23; i++)
            {
                if (arrTMStamp[i] == szForNumMonths)
                {
                    nPosForMonths = i;
                    break;
                }
            }

            nForNumMonths = nPosForMonths - 11;

            // Y = 14,15,16,17,18,19,20,21,22,23,24,25,26,27,28
            // M = 1,2,3,4,5,6,7,8,9,10,11,12
            // D = 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30
            //ll We witake Y from arrTMStamp - 14-15 = 0,1, 16-17=2,3 
            //So logic can be Y%14 + 1 (Since we are starting form year 14)
            //14%14  = 0*2 = 0 and + 1 = 1
            //15%14  = 1*2 = 2 and + 1 = 3
            //16%14  = 2*2 = 4 and + 1 = 5
            //17%14  = 3*2 = 6 and + 1 = 7
            //18%14  = 4*2 = 8 and + 1 = 9
            //..
            //so on

            //same for month
            //For date equal take exact till 30

            //char arrTMStamp[30] =	{'V','E','9','W','M','B','C','X','8','S','P','3','D','J','T','7','A','5','G','Z','U','N','H','F','2','R','6','K','Y','4'};

            //Remove first 2 characters as they are just random numbers
            int nLen = strEncDate.Length;
            Debug.Assert(nLen == 5);

            byte szY = strEncDate[2];
            byte szM = strEncDate[3];
            byte szD = strEncDate[4];

            int nYPos = -1;
            int nMPos = -1;
            int nDPos = -1;

            for (int i = 0; i < 30; i++)
            {
                if (arrTMStamp[i] == szY)
                {
                    nYPos = i;
                }
                if (arrTMStamp[i] == szM)
                {
                    nMPos = i;
                }
                if (arrTMStamp[i] == szD)
                {
                    nDPos = i;
                }

                if (nYPos >= 0 && nMPos >= 0 && nDPos >= 0)
                {
                    break;
                }
            }

            Debug.Assert(!(nYPos == -1));
            Debug.Assert(!(nMPos == -1));
            Debug.Assert(!(nDPos == -1));

            //if ypos = 0,1 - 14, 2,3 = 15, 4,5 = 16, 6,7 = 17

            nY = ((nYPos / 2) + 14);

            //if nMPos 1%12=1*2=2, +1 = 3 , 2%12=2*2=4, +1=5
            nM = ((nMPos / 2));

            if (nM == 0)
            {
                nM = 12; //12 month holds array pos 0,1
            }


            if (nDPos == 0)
            {
                nD = 30;
            }
            else
            {
                nD = nDPos;
            }

        }

        public static void Crypt(ref byte[] inp, long inplen, string keyInput = "", long keylen = 0)
        {
            byte[] Sbox = null;
            byte[] Sbox2 = null;
            try
            {
                //we will consider size of sbox 256 bytes
                //(extra byte are only to prevent any mishep just in case)
                Sbox = new byte[256];
                Sbox2 = new byte[256];
                uint i = 0, j = 0, t = 0, x = 0;

                //UInt16 ConstLengthOfByte = 256;

                //this unsecured key is to be used only when there is no input key from user
                byte[] OurUnSecuredKey = Encoding.Default.GetBytes("Microsoft Visual C++ Library");
                int OurKeyLen = OurUnSecuredKey.Length;
                byte temp = 0, k = 0;


                //always initialize the arrays with zero


                //initialize sbox i
                for (i = 0; i < 256; i++)
                {
                    Sbox[i] = (byte)i;
                }

                j = 0;
                byte[] key = Encoding.Default.GetBytes(keyInput);
                keylen = key.Length;
                //whether user has sent any input key
                if (keylen > 0)
                {
                    //initialize the sbox2 with user key
                    for (i = 0; i < 256; i++)
                    {
                        if (j >= keylen)
                        {
                            j = 0;
                        }

                        Sbox2[i] = (byte)key[j++];
                    }
                }
                else
                {
                    //initialize the sbox2 with our key
                    for (i = 0; i < 256U; i++)
                    {
                        if (j >= OurKeyLen)
                        {
                            j = 0;
                        }
                        Sbox2[i] = (byte)OurUnSecuredKey[j++];
                    }
                }

                j = 0; //Initialize j
                       //scramble sbox1 with sbox2
                for (i = 0; i < 256; i++)
                {
                    j = (j + (UInt32)Sbox[i] + (UInt32)Sbox2[i]) % 256;
                    temp = Sbox[i];
                    Sbox[i] = Sbox[j];
                    Sbox[j] = temp;
                }

                i = j = 0;
                for (x = 0; x < inplen; x++)
                {
                    //increment i
                    i = (i + 1U) % 256U;
                    //increment j
                    j = (j + (UInt32)Sbox[i]) % 256;

                    //Scramble SBox #1 further so encryption routine will
                    //will repeat itself at great interval
                    temp = Sbox[i];
                    Sbox[i] = Sbox[j];
                    Sbox[j] = temp;

                    //Get ready to create pseudo random  byte for encryption key
                    t = ((UInt32)Sbox[i] + (UInt32)Sbox[j]) % 256;

                    //get the random byte
                    k = Sbox[t];

                    //xor with the data and done
                    inp[x] = (byte)(inp[x] ^ k);
                }
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                Sbox = null;
                Sbox2 = null;
            }

        }

        //const string strSecureKey = "Microsoft Visual C++ Library";


        public static bool SysSetUserString2(USERSTRINGNUMBERS which, string str)
        {
            byte[] buf = null;
            try
            {
                Debug.Assert(!(str.Length < 1));
                Debug.Assert(!(str.Length > 254));
                if (str.Length > 254)
                    str = str.Substring(0, 254);
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }

                string regpath;
                regpath = string.Format( /*"%s\\%d"*/  ReadValue_PathPattern_Reg, OurBaseKey, (int)which);
                int datalen = str.Length;
                string strValueName = string.Empty;
                byte[] t_buf = Encoding.Unicode.GetBytes(str);
                string strSecureKey = ""; //(StoreSecrets.ContainsKey(USERSTRINGNUMBERS.USN_GUID_GENERATED)) ? StoreSecrets[USERSTRINGNUMBERS.USN_GUID_GENERATED] : strDefaultSecureKey;

                if (which != USERSTRINGNUMBERS.USN_GUID_GENERATED)
                {
                    strSecureKey = (StoreSecrets.ContainsKey(USERSTRINGNUMBERS.USN_GUID_GENERATED)) ? StoreSecrets[USERSTRINGNUMBERS.USN_GUID_GENERATED] : strDefaultSecureKey;
                }
                else
                {
                    strSecureKey = strDefaultSecureKey;
                }

                if (string.IsNullOrEmpty(strSecureKey))
                {
                    Debug.Assert(false); //check the flow GUID should be set first
                    return false;
                }
                buf = new byte[UNICODE_ARRAY_LEN];
                Array.Copy(t_buf, buf, t_buf.Length);
                Crypt(ref buf, buf.Length, strSecureKey, strSecureKey.Length);

                string strPathName;

                strPathName = string.Format(strAppDataFileName, (int)which);
                WriteToAppDataFolder(strPathName, buf);

                //Debug.Assert(retval);

                //Write to Reg HKCU...


                if (Registry.CurrentUser.OpenSubKey(regpath) == null)
                {
                    Registry.CurrentUser.CreateSubKey(regpath);
                }

                Registry.SetValue("HKEY_CURRENT_USER\\" + regpath, strValueName, buf, RegistryValueKind.Binary);

                try
                {
                    if (Registry.LocalMachine.OpenSubKey(regpath, true) == null)
                    {
                        Registry.LocalMachine.CreateSubKey(regpath);
                    }

                    Registry.SetValue("HKEY_LOCAL_MACHINE\\" + regpath, strValueName, buf, RegistryValueKind.Binary);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                buf = null;
            }


            return true;
        }

        public static void SysGetUserString2(USERSTRINGNUMBERS which, ref string valStr)
        {
            byte[] val = null;
            byte[] buf = null;
            try
            {
                string strSecureKey = string.Empty;
                val = new byte[UNICODE_ARRAY_LEN];
                buf = new byte[UNICODE_ARRAY_LEN];

                Func<string> DecryptStringAndStore = () =>
                {
                    string valueString = "";
                    try
                    {
                        if (which != USERSTRINGNUMBERS.USN_GUID_GENERATED)
                        {
                            strSecureKey = (StoreSecrets.ContainsKey(USERSTRINGNUMBERS.USN_GUID_GENERATED)) ? StoreSecrets[USERSTRINGNUMBERS.USN_GUID_GENERATED] : strDefaultSecureKey;
                        }
                        else
                        {
                            strSecureKey = strDefaultSecureKey;
                        }

                        Debug.Assert(strSecureKey.Length > 0);

                        DecryptByteArray(buf, out valueString, strSecureKey);

                        if (!StoreSecrets.ContainsKey(which) && valueString.Length > 0)
                        {
                            StoreSecrets.Add(which, valueString);
                        }
                    }
                    catch (Exception)
                    {
                        ;
                    }

                    return valueString;
                };


                string strPathName;
                strPathName = string.Format(strAppDataFileName, (int)which);
                bool retval = ReadFromAppDataFolder(strPathName, ref buf);
                if (retval)
                {
                    valStr = DecryptStringAndStore();
                    return;
                }



                string regpath;
                regpath = string.Format(/*"%s\\%d"*/ ReadValue_PathPattern_Reg, OurBaseKey, (int)which);

                string strValueName = string.Empty;

                bool bIfDataReadFromRegistrySuccessfully = false;
                Func<string, bool> ReadReg = (hKey) =>
                {
                    try
                    {
                        object bufValue = Registry.GetValue(hKey + regpath, strValueName, null);
                        if (bufValue != null)
                        {
                            buf = (byte[])bufValue;
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        ;
                    }

                    return false;
                };

                if (Registry.CurrentUser.OpenSubKey(regpath) != null)
                {
                    bIfDataReadFromRegistrySuccessfully = ReadReg("HKEY_CURRENT_USER\\");
                }

                if (!bIfDataReadFromRegistrySuccessfully && Registry.LocalMachine.OpenSubKey(regpath) != null)
                {
                    bIfDataReadFromRegistrySuccessfully = ReadReg("HKEY_LOCAL_MACHINE\\");
                }

                if (bIfDataReadFromRegistrySuccessfully)
                {
                    valStr = DecryptStringAndStore();
                }
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                val = null;
                buf = null;
            }

        }

        public static string getAppDataApplicationPath()
        {
            string sPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), StrProductName);

            if (!Directory.Exists(sPath))
            {
                Directory.CreateDirectory(sPath);
            }
            return sPath;

        }

        const int UNICODE_ARRAY_LEN = 256 * 2;

        static bool WriteToAppDataFolder(string fileName, byte[] data) //str will always be 256 byte
        {
            string fileNameOnly = Path.GetFileName(fileName);
            string FilePath = Path.Combine(getAppDataApplicationPath(), fileNameOnly);

            try
            {
                using (FileStream fs = File.OpenWrite(FilePath))
                {
                    fs.Write(data, 0, UNICODE_ARRAY_LEN/*data.Length*/); //  we write only 512 bytes, actual 
                    int nval = (new Random().Next(100) * Environment.TickCount) % 255;
                    byte bVal;
                    byte.TryParse(nval + "", out bVal);
                    byte CONFUSE_VALUE = bVal;
                    //byte CONFUSE_VALUE = Convert.ToByte((new Random().Next(100) * Environment.TickCount) % 255);
                    fs.WriteByte(CONFUSE_VALUE);
                    fs.Close();
                }
            }
            catch (System.Exception /*ex*/)
            {

            }
            return false;
        }

        //string key = "16729006-4a4e-4411-920a-6875512cdd9f";
        public static void DecryptByteArray(byte[] buf, out string decryptedStr, string encryptionKey)
        {
            byte[] decryptedBytes = null;
            try
            {
                /// we are copying here, because it will change the original buf value.
                decryptedBytes = new byte[UNICODE_ARRAY_LEN];
                Array.Copy(buf, decryptedBytes, buf.Length);
                KeyValidator.Crypt(ref decryptedBytes, UNICODE_ARRAY_LEN, encryptionKey, encryptionKey.Length);
                decryptedBytes[510] = decryptedBytes[511] = 0;
                decryptedStr = Encoding.Unicode.GetString(decryptedBytes).Replace("\0", "").Trim();
                //Debug.Assert(decryptedStr.Length > 0);
            }
            finally
            {
                decryptedBytes = null;
            }

        }

        static bool ReadFromAppDataFolder(string fileName, ref byte[] buf) //str will always be 256 byte
        {
            string fileNameOnly = Path.GetFileName(fileName);
            string FilePath = Path.Combine(getAppDataApplicationPath(), fileNameOnly);


            try
            {
                if (!File.Exists(FilePath))
                {
                    buf = null;
                    return false;
                }
                using (FileStream fs = File.OpenRead(FilePath))
                {
                    fs.Read(buf, 0, UNICODE_ARRAY_LEN/*data.Length*/); //  we write only 512 bytes, actual 
                    fs.Close();
                }
            }
            catch (System.Exception /*ex*/)
            {

            }

            return (buf.Length == UNICODE_ARRAY_LEN);
        }

        public static bool SetUserString(USERSTRINGNUMBERS which, string strStringValue)
        {

            int i = (int)which;

            if (i < 8)
            {
                if (!StoreSecrets.ContainsKey(which)) StoreSecrets.Add(which, strStringValue);
                else StoreSecrets[which] = strStringValue;
            }
            else
            {
                Debug.Assert(false); //user strings are capped till 8
            }
            SysSetUserString2(which, strStringValue);

            return false;
        }

        //GetUserString
        public static string GetUserString(USERSTRINGNUMBERS which)
        {
            int i = (int)which;
            string Value = string.Empty;

            if (StoreSecrets.ContainsKey(which))
            {
                Value = StoreSecrets[which];
                // ASSERT(Value.GetLength());
                return Value.Trim();
            }
            else
            {
                //read from registry and write to map
                SysGetUserString2(which, ref Value);
                if (!StoreSecrets.ContainsKey(which) && Value.Length > 0) StoreSecrets.Add(which, Value);
            }

            return Value.Trim();
        }

        /*
    person enters key

    1. we will store the key in our array
    2. now array has one key -left(23)
    3. key is valid
    4. on exit on setuserstring array is converted to single line and saved
    5. on restart key is again sent to array
    6. left(23) in the string will always be the last used key
    7. after expiration another key is entered
    8. we will check whether we have that key
    9. if yes we will not enter that key and will always return empty string
    10. if new key we will follow step 1
    */

        //return true if key exists and should not be used
        public static bool CheckKeyUsed(string key)
        {

            key.Trim();
            key = key.ToUpper();
            if (string.IsNullOrEmpty(key))
            {
                strKeysUsed = GetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY).ToUpper();
            }
            if (strKeysUsed.Length > 0)
            {
                int retval = strKeysUsed.IndexOf(key);
                if (retval == -1)
                {
                    return false; //we are the first used key
                }
                return true; //we may be in the index
            }
            return false; //we are the fisrt used key
        }

        public static bool StoreRegKey(string strkey)
        {
            strkey.Trim();
            strkey = strkey.ToUpper();
            if (CheckKeyUsed(strkey))
            {
                return false; //we will not store key which is already there
            }
            strKeysUsed = strkey + strKeysUsed;

            //we are storing last 11 keys only ignore all other keys
            if (strKeysUsed.Length > 255)
            {
                strKeysUsed = strKeysUsed.Substring(0, 255);
            }

            SetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY, strKeysUsed);
            return true;
        }

        public static string GetLastRegKey()
        {
            string curKey;
            if (string.IsNullOrEmpty(strKeysUsed))
            {
                strKeysUsed = GetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY);
            }

            if (strKeysUsed.Length >= 23)
            {
                curKey = strKeysUsed.Substring(0, 23);
            }
            else
            {
                curKey = string.Empty;
            }
            return curKey;
        }

        public static int InstallArmadilloCode(string codestring)
        {
            try
            {
                long nYear = 0, nMonth = 0, nDays = 0, nDaysAllowed = 0;
                bool retval = ValidateKey(codestring, ref nDays, ref nMonth, ref nYear, ref nDaysAllowed);
                if (retval)
                {
                    DateTime timeInstalled = DateTime.Now;
                    DateTime timeKeyGen = new DateTime((int)nYear, (int)nMonth, (int)nDays, 0, 0, 0);
                    if (timeKeyGen > timeInstalled)
                    {
                        TimeSpan nSpan = timeKeyGen - timeInstalled;
                        if (nSpan.TotalDays > 3)
                        {
                            //how a key can be installed when gen time is more than 3 days 
                            return 0;
                        }
                    }

                    //7/30/2018 3:35:38 PM

                    //TCHAR userkey[256];
                    StoreRegKey(codestring);

                    string strTime = timeInstalled.ToShortDateString();//.ToString(DateTimeStoreFormat);//.ToShortDateString();//.ToString();
                    SetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE, strTime);

                    return 1;
                }
            }
            catch (Exception)
            {
                ;
            }

            return 0;
        }

        public static string GetFilePath(USERSTRINGNUMBERS which)
        {
            string strfileName = string.Format(strAppDataFileName, (int)which);
            string FilePath = Path.Combine(getAppDataApplicationPath(), strfileName);
            return FilePath;
        }

        public static bool SetDefaultArmadilloKey()
        {
            try
            {

                strKeysUsed = "";
                StoreSecrets.Clear();

                for (int i = 0; i < 8; i++)
                {
                    try
                    {
                        string FilePath = GetFilePath((USERSTRINGNUMBERS)i);
                        if (File.Exists(FilePath))
                        {
                            File.Delete(FilePath);
                        }
                    }
                    catch (Exception)
                    {
                        ;
                    }


                    string regpath = string.Format(/*"%s\\%d"*/ ReadValue_PathPattern_Reg, OurBaseKey, (int)i);

                    try
                    {
                        Registry.CurrentUser.DeleteSubKey(regpath);
                    }
                    catch (Exception)
                    {
                        ;
                    }

                    try
                    {
                        Registry.LocalMachine.DeleteSubKey(regpath);
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }

                return true;
            }
            catch (Exception)
            {

            }

            return false;
        }

        public static int VerifyArmadilloKey(string cscodestring, bool bIsCheckKeyUsed = false)
        {
            cscodestring = cscodestring.Trim().ToUpper();
            //string userkey;
            int retval = 0;
            long nYear = 0, nMonth = 0, nDays = 0, nDaysAllowed = 0;

            if (CheckKeyUsed(cscodestring) && bIsCheckKeyUsed)
            {
                //we have already used this key 
                return 0;
            }

            if (ValidateKey(cscodestring, ref nDays, ref nMonth, ref nYear, ref nDaysAllowed))
            {
                string strKey = GetLastRegKey();
                if (string.Compare(strKey, cscodestring, true) == 0) // if key is same 
                {
                    //ASSERT(0) ;//we should never be come here

                    string strInstallTime = GetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE);
                    try
                    {
                        DateTime timeInstalled;
                        if (DateTime.TryParse(strInstallTime, out timeInstalled))
                        {

                            {
                                DateTime curtime = DateTime.Now;
                                if (timeInstalled > curtime)
                                {
                                    TimeSpan nSpan = timeInstalled - curtime;
                                    if (nSpan.TotalDays < 3)
                                    {
                                        //how a key can be installed when gen time is more than 3 days 
                                        retval = 0;
                                        return retval; ;
                                    }
                                }
                                else if (curtime >= timeInstalled)
                                {
                                    TimeSpan nSpan = curtime - timeInstalled;

                                    if (nSpan.TotalDays > nDaysAllowed) //Key Expired
                                    {
                                        retval = 0;
                                        return retval;
                                    }
                                    retval = 1;
                                    return retval;
                                }
                            }//status is valid  ?			
                        } //parsedatetime ker paye ?
                    }
                    catch (System.Exception)
                    {
                        ;
                    }
                }//if(strKey.CompareNoCase(cscodestring) == 0) 
                else
                {


                    DateTime timeInstalled = DateTime.Now;
                    DateTime timeKeyGen = new DateTime((int)nYear, (int)nMonth, (int)nDays, 0, 0, 0);
                    if (timeKeyGen > timeInstalled)
                    {
                        TimeSpan nSpan = timeKeyGen - timeInstalled;
                        if (nSpan.TotalDays > 3)
                        {
                            //how a key can be installed when gen time is more than 3 days 
                            return 0;
                        }
                    }
                }
                return 1;
            }
            return retval;
        }

        static uint GetRemainingRegDays(ref bool bExpired) // from key
        {
            bExpired = false;
            string userkey;
            uint retval = 0;
            long nYear = 0, nMonth = 0, nDays = 0, nDaysAllowed = 0;

            string strKey = GetLastRegKey();

            if (strKey.Length > 0) // if key is same 
            {
                if (ValidateKey(strKey, ref nDays, ref nMonth, ref nYear, ref nDaysAllowed)) //Take out the number of days
                {
                    //string aa = string.Format(("GetRemainingRegDays Ka Validate bhi ho gaya {0} days"), nDaysAllowed);

                    string strInstallTime = GetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE);
                    try
                    {
                        DateTime timeInstalled;
                        if (DateTime.TryParse(strInstallTime, out timeInstalled))
                        {

                            {
                                DateTime curtime = DateTime.Now;
                                if (timeInstalled > curtime)
                                {
                                    TimeSpan nSpan = timeInstalled - curtime;
                                    if (nSpan.TotalDays < 3)
                                    {
                                        //how a key can be installed when gen time is more than 3 days 
                                        retval = 0;
                                        return retval; ;
                                    }
                                    else
                                    {
                                        retval = 0;
                                        bExpired = true;
                                    }
                                }
                                else if (curtime >= timeInstalled)
                                {
                                    TimeSpan nSpan = curtime - timeInstalled;
                                    long nDaysPassed = (long)nSpan.TotalDays;
                                    if (nDaysPassed >= nDaysAllowed) //Key Expired
                                    {
                                        retval = 0;
                                        bExpired = true;
                                        return retval;
                                    }

                                    //string aa = string.Format(("GetRemainingRegDays return kar raha hai - {0} days (nDaysAllowed={1} - nDaysPassed={2})"), nDaysAllowed - nDaysPassed, nDaysAllowed, nDaysPassed);
                                    long retvalue = nDaysAllowed - nDaysPassed;
                                    if (retval < 0)
                                    {
                                        return 0;
                                    }
                                    return (uint)(nDaysAllowed - nDaysPassed); //Yehi to remaining days honge
                                }
                            }//status is valid  ?			
                        } //parsedatetime ker paye ?
                        else
                        {
                            //CString aa = string.Format(_T("GetRemainingRegDays TimeInstalled parse nahi kar paya!"), nDaysAllowed);

                            //timeInstalled = DateTime.Now;
                            string str = DateTime.Now.ToShortDateString();//.ToString(DateTimeStoreFormat);//.ToShortDateString();//ToString();
                            SetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE, str);
                            return 1;
                        }
                    }
                    catch (System.Exception)
                    {
                        //string aa = string.Format(("GetRemainingRegDays CATCH!"), nDaysAllowed);
                    }
                }

            }
            return 0;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789&abcdefghijklmnopqrstuvwxyz!@#$%^&*()";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void SetProgGUID()
        {
            string StrUniqueGUID = GetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED);

            if (string.IsNullOrEmpty(StrUniqueGUID))
            {
                try
                {
                    SetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED, Guid.NewGuid().ToString());
                }
                catch (System.Exception)
                {
                    SetUserString(USERSTRINGNUMBERS.USN_GUID_GENERATED, RandomString(63));
                }


            }
        }

        public static SYS_TRIAL_VERSION_STATUS GetARMStatus(ref uint daysRemaining)
        {
            try
            {
                bool b_expired = false;
                ///will also return days remaining in trial or subscription
                daysRemaining = GetRemainingRegDays(ref b_expired);

                //////Trace.WriteLine(string.Format("{0} Days remaining\r\n", daysRemaining));

                //are we expired 
                if ((daysRemaining <= 0) && b_expired)
                {
                    b_expired = true;
                    //////Trace.WriteLine(string.Format("{0} Trial Expired\r\n", b_expired));
                    //return STATUS_EVAL_VERSION_EXPIRED;
                }


                //////Trace.WriteLine(string.Format("Trial Expired : {0} \r\n", b_expired));

                //are we reg. version 
                bool b_isRegistered = ((!b_expired) && (daysRemaining > 0));
                //////Trace.WriteLine(string.Format("Registered Version : {0} \r\n", b_isRegistered));

                ///when we are reg with valid key but days left is zero 	

                if (b_expired /*&& b_isRegistered*/)
                {
                    //////Trace.WriteLine("STATUS_SUBSCRIPTION_EXPIRED\r\n");
                    return SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_EXPIRED;
                }
                //valid key not expired and not retail
                else if (!b_expired && b_isRegistered)
                {
                    //////Trace.WriteLine("STATUS_SUBSCRIPTION_REMAINING\r\n");
                    return SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING;
                }
                //neither expired not registered means eval days remaining
                else if (!b_expired && !b_isRegistered)
                {
                    //////Trace.WriteLine("STATUS_EVAL_PERIOD_REMAINING\r\n");

                    return SYS_TRIAL_VERSION_STATUS.STATUS_EVAL_PERIOD_REMAINING; //our default trial state
                }

            }
            catch (Exception ex)
            {
                cGlobalSettings.oLogger.WriteLogException("GetARMStatus", ex);
            }

            //////Trace.WriteLine("STATUS_EVAL_VERSION_EXPIRED\r\n");
            return SYS_TRIAL_VERSION_STATUS.STATUS_EVAL_VERSION_EXPIRED; // we have expired our default status
        }



        public static void RefreshAllStatus()
        {
            uint nRemainingDays = 0;
            SYS_TRIAL_VERSION_STATUS status = GetARMStatus(ref nRemainingDays);
        }

        public static uint VerifyAndInstallCode(string code, bool b_InstallCode)
        {

            uint nRemainingDays = 0;
            SYS_TRIAL_VERSION_STATUS status = GetARMStatus(ref nRemainingDays);
            ////Trace.WriteLine(string.Format("before VerifyArmadilloKey {0}, {1}", (int)status, nRemainingDays));


            if (VerifyArmadilloKey(code) > 0)
            {
                nRemainingDays = 0;
                status = GetARMStatus(ref nRemainingDays);
                ////Trace.WriteLine(string.Format("After VerifyArmadilloKey {0}, {1}", (int)status, nRemainingDays));


                if (b_InstallCode)
                {
                    if (InstallArmadilloCode(code) > 0)
                    {
                        nRemainingDays = 0;
                        status = GetARMStatus(ref nRemainingDays);
                        ////Trace.WriteLine(string.Format("After InstallArmadilloCode {0}, {1}", (int)status, nRemainingDays));

                        RefreshAllStatus();
                        nRemainingDays = 0;
                        status = GetARMStatus(ref nRemainingDays);
                        ////Trace.WriteLine(string.Format("After RefreshAllStatus {0}, {1}", (int)status, nRemainingDays));

                        return nRemainingDays;
                    }
                    else
                    {

                        return 0;
                    }
                }
                return 1;
            }

            return 0;
        }

        public static bool bProductWillNeverExpire = false;
        public static void ExpireCurrentKey(bool bforceExpire = false)
        {
            if (bProductWillNeverExpire & !bforceExpire)
                return;

            SetProgGUID();
            RefreshAllStatus();

            string strInstallTime = GetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE);
            DateTime timeInstalled;
            if (DateTime.TryParse(strInstallTime, out timeInstalled))
            {
                bool b_expired = false;
                uint daysRemaining = GetRemainingRegDays(ref b_expired);

                TimeSpan timespan = TimeSpan.FromDays(daysRemaining + 9);
                timeInstalled += timespan;
                string str = timeInstalled.ToString();
                SetUserString(USERSTRINGNUMBERS.USN_REG_USER_KEY_INSTALL_DATE, str);
            }

            SetProgGUID();
            RefreshAllStatus();

            return;
        }



        public static bool ST_IsRegisteredVersion2()
        {
            RefreshAllStatus();
            uint nRemainingDays = 0;
            SYS_TRIAL_VERSION_STATUS status = GetARMStatus(ref nRemainingDays);

            int nReg = 0;
            if (status == SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_REMAINING || status == SYS_TRIAL_VERSION_STATUS.STATUS_SUBSCRIPTION_EXPIRED)
            {
                nReg = 1;
            }

            return (nReg == 1);
        }


        public static int ST_GetTotalAllowedTrialPeriod2() // Returns the total remaining trial days
        {
            RefreshAllStatus();
            bool b_expired = false;
            ///will also return days remaining in trial or subscription
            int nRet = (int)GetRemainingRegDays(ref b_expired);
            return nRet;
        }

        public static SYS_TRIAL_VERSION_STATUS ST_XGetRegStatus()
        {
            RefreshAllStatus();
            uint nRemainingDays = 0;
            SYS_TRIAL_VERSION_STATUS status = GetARMStatus(ref nRemainingDays);
            return status;
        }

        public static string ST_GetUserName()
        {
            string val = Environment.GetEnvironmentVariable("ALTUSERNAME");
            return (!string.IsNullOrEmpty(val)) ? val : "";
        }

        public static int ST_GetInstalledDays() // Returns the total remaining trial days
        {
            int val = 0;
            string daysInstalled = Environment.GetEnvironmentVariable("DAYSINSTALLED");
            if (!string.IsNullOrEmpty(daysInstalled))
            {

                ////Trace.WriteLine("ST_GetInstalledDays() {0}\r\n", daysInstalled);

                if (int.TryParse(daysInstalled, out val))
                {
                    return val;
                }

            }

            return 0;
        }

        public static int GetKeyValidDays() // Returns the total remaining trial days
        {
            int val = 1;
            string daysInstalled = Environment.GetEnvironmentVariable("STTOTALTRIALDAYS");
            if (!string.IsNullOrEmpty(daysInstalled))
            {

                ////Trace.WriteLine("GetKeyValidDays() {0}\r\n", daysInstalled);

                if (int.TryParse(daysInstalled, out val))
                {
                    return val;
                }

            }

            return 1;
        }

        public static bool ST_IsRetailKey()
        {
            string val = Environment.GetEnvironmentVariable("ASO_RETAIL_VERSION");
            if (!string.IsNullOrEmpty(val))
            {
                val = val.Trim();

                ////Trace.WriteLine("ST_IsRetailKey() {0}\r\n", val);

                return (string.Compare(val, ("true"), true) == 0);

            }

            return false;
        }


        public static int ST_InstallArmadilloCode2(string codestring)
        {


            int nReg = VerifyArmadilloKey(codestring);
            if (nReg > 0)
            {
                nReg = InstallArmadilloCode(codestring);
            }
            return nReg;
        }


        public static int ST_VerifyArmadilloKey2(string cscodestring)
        {

            RefreshAllStatus();
            return VerifyArmadilloKey(cscodestring);
        }
    }
}
