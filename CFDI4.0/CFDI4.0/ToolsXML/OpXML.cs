using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CFDI4._0.ToolsXML
{
    public class OpXML
    {
        //CLASE PROTEGIDA QUE CONFIGURA LA CODIFICACIÓN DEL XML
        protected class StringWriterWithEncoding : StringWriter
        {
            private readonly Encoding m_Encoding;
            public StringWriterWithEncoding(Encoding encoding) : base()
            {
                this.m_Encoding = encoding;
            }

            public override Encoding Encoding
            {
                get { return this.m_Encoding; }
            }
        }

        //FUNCIÓN PARA CONVERTIR XML A BASE 64
        public string ConvertXmlToBase64(string xmlContent)
        {
            if (string.IsNullOrEmpty(xmlContent))
                throw new ArgumentException("El contenido del XML no puede estar vacío.");

            // Convertir el contenido XML en bytes usando UTF-8
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlContent);

            // Convertir a Base64
            return Convert.ToBase64String(xmlBytes);
        }

        //FUNCIÓN PARA CONVERTIR BASE 64 A XML
        public string ConvertBase64ToXml(string base64Content)
        {
            if (string.IsNullOrEmpty(base64Content))
                throw new ArgumentException("El contenido en Base64 no puede estar vacío.");

            // Convertir de Base64 a bytes
            byte[] xmlBytes = Convert.FromBase64String(base64Content);

            // Convertir los bytes a una cadena usando UTF-8
            return Encoding.UTF8.GetString(xmlBytes);
        }

    }
}
