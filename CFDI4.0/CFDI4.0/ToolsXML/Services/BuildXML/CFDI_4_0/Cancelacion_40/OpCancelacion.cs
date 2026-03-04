using CFDI4._0.ToolsXML.DTOs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using static CFDI4._0.ToolsXML.Helpers.OpXML;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40
{
    public class OpCancelacion
    {
        //ANALIZAR EL CASO DE LA URL QUE SE COLOCA EN EL CASO DE UNA RETENCIÓN
        public string ConvertirXML(Cancelacion cancelacion)
        {
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, "http://cancelacfd.sat.gob.mx");

            XmlSerializer serializer = new XmlSerializer(typeof(Cancelacion));
            using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                serializer.Serialize(stringWriter, cancelacion, ns);
                return stringWriter.ToString();
            }
        }

        public Cancelacion DeserializarXMLCancelacion(string xmlContent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Cancelacion));

            using (StringReader reader = new StringReader(xmlContent))
            {
                return (Cancelacion)serializer.Deserialize(reader);
            }
        }
    }
}
