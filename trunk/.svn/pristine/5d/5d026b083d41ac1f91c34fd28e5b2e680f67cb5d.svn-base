using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.ClassDictionary
{
    public static class EnumUtils
    {

        public static string stringValueOfDescAttr(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string stringValueOGenAttr(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            GeneralValueAttribute[] attributes = (GeneralValueAttribute[])fi.GetCustomAttributes(typeof(GeneralValueAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return value.ToString();
            }
        }

        public static object enumValueOf(string value, Type enumType)
        {
            string[] names = System.Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (stringValueOfDescAttr((System.Enum)System.Enum.Parse(enumType, name)).Equals(value))
                {
                    return System.Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }


        public static bool IsFlagSet<T>(this System.Enum type, T value)
        {
            return (((int)(object)type & (int)(object)value) == (int)(object)value);
        }




        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

    }

    class GeneralValueAttribute : Attribute
    {
        private string _value;

        public GeneralValueAttribute(string value)
        {
            _value = value;

        }
        public string Value
        {
            get { return _value; }
        }

    }
}
