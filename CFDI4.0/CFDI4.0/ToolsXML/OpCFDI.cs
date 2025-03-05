using CFDI4._0.CFDI;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CFDI4._0.ToolsXML
{
    public class OpCFDI : OpXML
    {
        private readonly string _RutaCerCSD;
        private readonly string _RutaKeyCSD;
        private readonly string _ClavePrivada;
        private readonly string _CadenaOriginalXSLT;

        // CONTRUCTOR PARA CREAR EL OBJ Comprobante --> Lado del cliente
        public OpCFDI(string RutaCerCSD, string RutaKeyCSD, string ClavePrivada)
        {
            _RutaCerCSD = RutaCerCSD; _RutaKeyCSD = RutaKeyCSD; _ClavePrivada = ClavePrivada;
            _CadenaOriginalXSLT = "CFDI4._0.Archivos.cadenaoriginal_4_0.xslt";
        }

        //CONTRUCTOR PARA OBTENER DATOS DEL XML --> LADO DEL SERVIDOR
        public OpCFDI()
        {
            _CadenaOriginalXSLT = "CFDI4._0.Archivos.cadenaoriginal_4_0.xslt";
        }

        //CREACIÓN DE LA CADENA ORIGINAL CON EL XSLT INCRUSTADO - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private string _CrearCadenaOriginal(string prefactura)
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

        //FUNCIÓN QUE AGREGA EL TIMBRE FISCAL AL NODO COMPLEMENTO EN EL XML DEL CFDI - - - - - - - - - - - - - - - - - - - - - - - - -
        private void ObtenerTimbreFiscalEnComplemento(ComprobanteComplemento complemento, ref TimbreFiscalDigital timbreFiscal)
        {
            //Procesar los complementos en el objeto Comprobante
            foreach (var oComplemento in complemento.Any)
            {
                if (!oComplemento.Name.Contains("TimbreFiscalDigital")) continue;

                // Crear el serializador para el complemento TimbreFiscalDigital
                XmlSerializer serializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));

                // Deserializar el complemento usando el OuterXml
                using (var readerComplemento = new StringReader(oComplemento.OuterXml))
                {
                    timbreFiscal =
                        (TimbreFiscalDigital)serializerComplemento.Deserialize(readerComplemento);
                }
            }
        }

        // FUNCIÓN QUE CREA Y RETORNA EL XML POR MEDIO DE UN OBJ DE TIPO Comprobante - - - - - - - - - - - - - - - - - - - - - - - - 
        // LA MISMA SERIALIZA EL OBJETO A XML - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public string ConvertirXML(Comprobante objComprobante)
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

        // FUNCIÓN QUE CREA Y RETORNA EL XML SELLADO POR MEDIO DE UN OBJ DE TIPO Comprobante - - - - - - - - - - - - - - - - - - - - - - - - 
        public (string, string) CrearXmlSellado(Comprobante objComprobante)
        {
            //GENERAR NÚMERO DE CERTIFICADO - - - - - - - - - - - - - - - - - - - - - - - - 
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(_RutaCerCSD, out aa, out b, out c, out numeroCertificado);
            objComprobante.NoCertificado = numeroCertificado;

            //SE CREA EL XML QUE REPRESENTA LA PREFACTURA
            string XML = ConvertirXML(objComprobante);
            string cadenaOriginal = _CrearCadenaOriginal(XML);

            //EN ESTE PASO SE SELLA LA PREFACTURA
            SelloDigital oSelloDigital = new SelloDigital();

            //objComprobante.CadenaOriginal = cadenaOriginal; //SE ANEXA LA CADENA ORIGINAL PARA AGREGAR AL PDF ¡ANALIZAR!
            objComprobante.Certificado = oSelloDigital.Certificado(_RutaCerCSD);
            objComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, _RutaKeyCSD, _ClavePrivada);

            return (ConvertirXML(objComprobante), cadenaOriginal); //EL XML YA INCLUYE LA CADENAORIGINAL PERO EL XML LO IGNORA
        }

        //CONVERSIÓN DE TODO EL XML A OBJETO DE C#
        public Comprobante DeserializarXMLCompleto(string xmlContent)
        {
            // Crear el serializador para el objeto Comprobante
            XmlSerializer serializer = new XmlSerializer(typeof(Comprobante), "http://www.sat.gob.mx/cfd/4");

            // Deserializar el XML contenido en la cadena
            Comprobante oComprobante;
            using (StringReader stringReader = new StringReader(xmlContent))
            {
                oComprobante = (Comprobante)serializer.Deserialize(stringReader);
            }

            if (oComprobante.Complemento != null)
                ObtenerTimbreFiscalEnComplemento(oComprobante.Complemento, ref oComprobante.TimbreFiscalDigital);

            return oComprobante;
        }
        
        //VALIDA Y SERIALIZA EL XML A OBJETO
        public bool DeserializarXMLCompleto(string xmlContent, out Comprobante objComprobante) 
        {
            objComprobante = new Comprobante();
            try
            {
                objComprobante = DeserializarXMLCompleto(xmlContent);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //CONVERSIÓN DEL XML DEL TIMBRE FISCAL A OBJETO DE C#
        public TimbreFiscalDigital DeserializarTimbreFiscal(string xmlContent)
        {
            // Cargar el documento XML
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlContent);

            // Seleccionar el nodo TimbreFiscalDigital
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

            XmlNode timbreNode = document.SelectSingleNode("//tfd:TimbreFiscalDigital", nsmgr);

            if (timbreNode == null)
            {
                throw new InvalidOperationException("No se encontró el nodo TimbreFiscalDigital en la respuesta SOAP.");
            }

            // Deserializar el nodo
            XmlSerializer serializer = new XmlSerializer(typeof(TimbreFiscalDigital));
            using (StringReader reader = new StringReader(timbreNode.OuterXml))
            {
                return (TimbreFiscalDigital)serializer.Deserialize(reader);
            }
        }

        public Comprobante AgregarTimbreFiscalAComplemento(Comprobante comprobante, TimbreFiscalDigital timbreFiscal)
        {
            // Crear un elemento XML para TimbreFiscalDigital
            XmlDocument doc = new XmlDocument();
            XmlElement timbreElement = doc.CreateElement("tfd", "TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital");

            timbreElement.SetAttribute("Version", timbreFiscal.Version);
            timbreElement.SetAttribute("UUID", timbreFiscal.UUID);
            timbreElement.SetAttribute("FechaTimbrado", timbreFiscal.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss"));
            timbreElement.SetAttribute("RfcProvCertif", timbreFiscal.RfcProvCertif);
            timbreElement.SetAttribute("SelloCFD", timbreFiscal.SelloCFD);
            timbreElement.SetAttribute("NoCertificadoSAT", timbreFiscal.NoCertificadoSAT);
            timbreElement.SetAttribute("SelloSAT", timbreFiscal.SelloSAT);

            // Verificar si el Complemento está inicializado
            if (comprobante.Complemento == null)
                comprobante.Complemento = new ComprobanteComplemento();

            // Convertir el arreglo Any a una lista temporal, agregar el timbre y volver a asignarlo como arreglo
            var elementos = comprobante.Complemento.Any?.ToList() ?? new List<XmlElement>();
            elementos.Add(timbreElement);
            comprobante.Complemento.Any = elementos.ToArray();

            return comprobante;
        }

        public string ConvertirTimbreFiscalAXml(TimbreFiscalDigital timbreFiscal)
        {
            // Crear un documento XML
            XmlDocument doc = new XmlDocument();

            // Crear el elemento "TimbreFiscalDigital" con el namespace "http://www.sat.gob.mx/TimbreFiscalDigital"
            XmlElement timbreElement = doc.CreateElement("tfd", "TimbreFiscalDigital", "http://www.sat.gob.mx/TimbreFiscalDigital");

            // Asignar atributos al elemento "TimbreFiscalDigital"
            timbreElement.SetAttribute("Version", timbreFiscal.Version);
            timbreElement.SetAttribute("UUID", timbreFiscal.UUID);
            timbreElement.SetAttribute("FechaTimbrado", timbreFiscal.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss"));
            timbreElement.SetAttribute("RfcProvCertif", timbreFiscal.RfcProvCertif);
            timbreElement.SetAttribute("SelloCFD", timbreFiscal.SelloCFD);
            timbreElement.SetAttribute("NoCertificadoSAT", timbreFiscal.NoCertificadoSAT);
            timbreElement.SetAttribute("SelloSAT", timbreFiscal.SelloSAT);

            // Convertir el elemento a una cadena XML y devolverlo
            using (StringWriter stringWriter = new StringWriter())
            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter))
            {
                timbreElement.WriteTo(xmlTextWriter);
                return stringWriter.ToString();
            }
        }

        public XmlElement ObtenerPagosEnComplemento(Pagos pago)
        {
            XmlDocument xmlDoc = new XmlDocument();

            // Serializar el objeto "Pagos" a XML asegurando el namespace correcto
            XmlSerializer serializer = new XmlSerializer(typeof(Pagos));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("pago20", "http://www.sat.gob.mx/Pagos20");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(memoryStream, new UTF8Encoding(false)))
                {
                    serializer.Serialize(writer, pago, namespaces);
                    writer.Flush();
                    memoryStream.Position = 0;
                    xmlDoc.Load(memoryStream);
                }
            }

            // Retornar el XmlElement generado
            return xmlDoc.DocumentElement;
        }

        public Pagos DeserializarPagos(Comprobante obj)
        {
            var nodoPagos = obj.Complemento.Any.FirstOrDefault(x => x.Name == "pago20:Pagos");

            var xmlStringPago = nodoPagos.OuterXml; // Obtener XML del nodo 
            Pagos complementoPagos = null;

            // Deserializar XML a objeto Pagos
            XmlSerializer serializerTwo = new XmlSerializer(typeof(Pagos));
            using (StringReader reader = new StringReader(xmlStringPago))
            {
                complementoPagos = (Pagos)serializerTwo.Deserialize(reader);
            }

            return complementoPagos;
        }

    }
}
