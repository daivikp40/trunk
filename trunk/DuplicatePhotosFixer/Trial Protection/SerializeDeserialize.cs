/************************************************************************/
/* 

 * Other Methods
 * ------------------------------------------------------
    XmlSerializer serializer = new XmlSerializer( typeof( MyXmlClass ) );

    using( XmlReader reader = XmlReader.Create( file ) )
    {
        MyXmlClass myXmlClass = ( MyXmlClass )serializer.Deserialize( reader );

    }
 * ------------------------------------------------------
 * 
 */
/************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;


    

    /// <summary>
    /// Reference: 
    /// http://www.dotnetjohn.com/articles.aspx?articleid=173
    /// 
    /// </summary>
    public class SerializeDeserialize
    {
        /// <summary>
        /// Encoding method used for Serialization and De-serialization
        /// 
        /// don't use this directory Encoding.UTF8
        /// as this adds 3 bytes preamble to XML
        /// and is then not load correctly
        /// always use new keyword with encoding type
        /// 
        /// allowed types
        /// new UTF8Encoding();
        /// new UnicodeEncoding(false, false); 
        /// </summary>
        public static Encoding encoding = new UTF8Encoding();//UnicodeEncoding(false, false); 


        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static String SerializeObject<T>(Object pObject)
        {
            MemoryStream    memoryStream = null;
            XmlSerializer   xs = null;
            XmlTextWriter   xmlTextWriter = null;
            try
            {   
                memoryStream = new MemoryStream();
                xs = new XmlSerializer(typeof(T));
                xmlTextWriter = new XmlTextWriter(memoryStream, encoding/*.UTF8*/);

                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                String XmlizedString = ConvertionFunctions.UnicodeByteArrayToString(memoryStream.ToArray(), encoding);                
                return XmlizedString;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }
            finally
            {
                memoryStream = null;
                xs = null;
                xmlTextWriter = null;                
            }
        }


        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static Object DeserializeObject<T>(String pXmlizedString)
        {
            XmlSerializer   xs = null;
            MemoryStream    memoryStream = null;
            XmlTextWriter   xmlTextWriter = null;
            try
            {
                xs = new XmlSerializer(typeof(T));
                memoryStream = new MemoryStream(ConvertionFunctions.StringToUnicodeByteArray(pXmlizedString, encoding));
                xmlTextWriter = new XmlTextWriter(memoryStream, encoding/*Encoding.Unicode/ *.UTF8* /*/);

                return xs.Deserialize(memoryStream);
            }
            catch (System.Exception ex)
            {
                Logger.WriteException("DeserializeObject", ex);
            }
            finally
            {
                xs = null;
                memoryStream = null;
                xmlTextWriter = null;

            }

            return string.Empty;
            
        }
    }





    /// <summary>
    /// Conversion function from one type to another
    /// </summary>
    public static class ConvertionFunctions
    {
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        public static String UnicodeByteArrayToString(Byte[] characters, Encoding encoding)
        {


            //UnicodeEncoding/*UTF8Encoding*/ encoding = new UnicodeEncoding/*UTF8Encoding*/();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        public static Byte[] StringToUnicodeByteArray(String pXmlString, Encoding encoding)
        {
            //UnicodeEncoding/*UTF8Encoding*/ encoding = new UnicodeEncoding/*UTF8Encoding*/();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
