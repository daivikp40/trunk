using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicatePhotosFixer.App_code
{
    public class StringWriterWithEncoding : StringWriter
    {
        private Encoding m_encoding;

        public StringWriterWithEncoding(StringBuilder sb, Encoding encoding) : base(sb)
        {
            m_encoding = encoding;
        }

        public override Encoding Encoding
        {
            get
            {
                return m_encoding;
            }
        }
    }
}
