using CFDI4._0.CFDI;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Reflection;
using System;

namespace CFDI4._0.ToolsXML
{
    public class OpXML
    {  
        private readonly string _RutaCerCSD;
        private readonly string _RutaKeyCSD; 
        private readonly string _ClavePrivada;
        private readonly string _CadenaOriginalXSLT;

        private string XML; 

        public OpXML(string RutaCerCSD, string RutaKeyCSD, string ClavePrivada)
        {
            _RutaCerCSD   = RutaCerCSD;
            _RutaKeyCSD   = RutaKeyCSD;
            _ClavePrivada = ClavePrivada;

            _CadenaOriginalXSLT = "CFDI4._0.Archivos.cadenaoriginal_4_0.xslt";
        }

        //CLASE PRIVADA QUE CONFIGURA LA CODIFICACIÓN DEL XML
        private class StringWriterWithEncoding : StringWriter
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

        public string CrearXmlSellado(Comprobante objComprobante)   
        {
            //GENERAR NÚMERO DE CERTIFICADO - - - - - - - - - - - - - - - - - - - - - - - - 
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(_RutaCerCSD, out aa, out b, out c, out numeroCertificado);
            objComprobante.NoCertificado = numeroCertificado;

            //SE CREA EL XML QUE REPRESENTA LA PREFACTURA
            XML = CrearXml(objComprobante);
            string cademaOriginal = CrearCadenaOriginal(XML);

            //EN ESTE PASO SE SELLA LA PREFACTURA
            SelloDigital oSelloDigital = new SelloDigital();
            objComprobante.Certificado = oSelloDigital.Certificado(_RutaCerCSD);
            objComprobante.Sello = oSelloDigital.Sellar(cademaOriginal, _RutaKeyCSD, _ClavePrivada);

            return CrearXml(objComprobante);
        }

        public string ConvertXmlToBase64(string xmlContent)
        {
            if (string.IsNullOrEmpty(xmlContent))
                throw new ArgumentException("El contenido del XML no puede estar vacío.");

            // Convertir el contenido XML en bytes usando UTF-8
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlContent);

            // Convertir a Base64
            return Convert.ToBase64String(xmlBytes);
        }


        // FUNCIÓN QUE CREA Y RETORNA EL XML - - - - - - - - - - - - - - - - - - - - - - - - 
        private string CrearXml(Comprobante objComprobante)          
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
                    OXmlSerializer.Serialize(writter, objComprobante, xmlNamespaces);
                    sXml = sww.ToString();
                }
            }

            return sXml;
        }

        //HOLA MUNDO SIN ORIGINALSTRING - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private string CrearCadenaOriginal(string prefactura)
        {
            string cadenaOriginal = string.Empty;

            // Crear el transformador XSLT
            XslCompiledTransform transformador = new XslCompiledTransform(true);

            // Configurar XSLT para que no intente resolver URIs externos
            XsltSettings settings = XsltSettings.Default;
            XmlUrlResolver resolver = new XmlUrlResolver(); // Resolver vacío

            // Cargar el recurso incrustado
            using (Stream xsltStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_CadenaOriginalXSLT))
            {
                if (xsltStream == null)
                {
                    throw new FileNotFoundException("El recurso XSLT no se encontró. Verifique el nombre y la ubicación del archivo.");
                }

                // Cargar el transformador con el recurso incrustado, configuraciones y resolver
                using (XmlReader reader = XmlReader.Create(xsltStream))
                {
                    transformador.Load(reader, settings, resolver);
                }
            }

            // Convertir la cadena XML a un XmlReader
            using (StringReader stringReader = new StringReader(prefactura))
            using (XmlReader xmlInputReader = XmlReader.Create(stringReader))
            using (StringWriter sw = new StringWriter())
            using (XmlWriter xwo = XmlWriter.Create(sw, transformador.OutputSettings))
            {
                // Transformar el XML de entrada
                transformador.Transform(xmlInputReader, xwo);
                cadenaOriginal = sw.ToString();
            }

            return cadenaOriginal;
        }

        

        

    }
}
