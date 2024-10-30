using CFDI4._0.CFDI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace CFDI4._0.ToolsXML
{
    public class OpXML
    {
        private class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding(Encoding encoding)
                : base()
            {
                this.m_Encoding = encoding;
            }
            private readonly Encoding m_Encoding;
            public override Encoding Encoding
            {
                get
                {
                    return this.m_Encoding;
                }
            }
        }

        public string CreateXML(Comprobante oComprobante, string pathXML)
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();

            // Agregar los namespaces necesarios
            xmlNamespaces.Add("cfdi", "http://www.sat.gob.mx/cfd/4");
            xmlNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            // Serialización del Xml - - - - - - - - - - - - - - - - - - - - - - - - - 
            XmlSerializer OXmlSerializer = new XmlSerializer(typeof(Comprobante));
            // Serialización del Xml - - - - - - - - - - - - - - - - - - - - - - - - - 

            string sXml = string.Empty;

            using (var sww = new StringWriterWithEncoding(Encoding.UTF8))
            {
                using (XmlWriter writter = new XmlTextWriter(sww))
                {
                    OXmlSerializer.Serialize(writter, oComprobante, xmlNamespaces);
                    sXml = sww.ToString();
                }
            }

            return sXml;
        }
    }
}
