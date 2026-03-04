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
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using CFDI4._0.ToolsXML.Helpers;
using CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Comprobante_40.Encrypted;


namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Comprobante_40    
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

        //CREACIÓN DE LA CADENA ORIGINAL CON EL XSLT INCRUSTADO - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        private string CrearCadenaOriginal(string xml)
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
            using (StringReader stringReader = new StringReader(xml))
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
        public string CrearXmlSellado(Comprobante objComprobante)
        {
            //GENERAR NÚMERO DE CERTIFICADO - - - - - - - - - - - - - - - - - - - - - - - - 
            string numeroCertificado, aa, b, c;
            SelloDigital.leerCER(_RutaCerCSD, out aa, out b, out c, out numeroCertificado);
            objComprobante.NoCertificado = numeroCertificado;

            //SE CREA EL XML QUE REPRESENTA LA PREFACTURA
            string XML = ConvertirXML(objComprobante);
            string cadenaOriginal = CrearCadenaOriginal(XML);

            //EN ESTE PASO SE SELLA LA PREFACTURA
            SelloDigital oSelloDigital = new SelloDigital();

            //objComprobante.CadenaOriginal = cadenaOriginal; //SE ANEXA LA CADENA ORIGINAL PARA AGREGAR AL PDF ¡ANALIZAR!
            objComprobante.Certificado = oSelloDigital.Certificado(_RutaCerCSD);
            objComprobante.Sello = oSelloDigital.Sellar(cadenaOriginal, _RutaKeyCSD, _ClavePrivada);

            return ConvertirXML(objComprobante); //EL XML YA INCLUYE LA CADENAORIGINAL PERO EL XML LO IGNORA
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
            {
                ObtenerTimbreFiscalEnComplemento(oComprobante.Complemento, ref oComprobante.TimbreFiscalDigital);
                oComprobante.complementoPagos = DeserializarPagos(oComprobante);
            }

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

        //FUNCIÓN PARA OBTENER LOS NODOS DE LOS PAGOS DENTRO DEL NODO COMPLEMENTO EN EL XML 
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

        //CON ESTA FUNCIÓN PUEDO RECUPERAR LOS COMPLEMENTOS DE PAGO QUE ESTAN DENTRO DEL NODO COMPLEMENTOS PARA 
        //ASÍ PODER MANIPULAR EL OBJETO DE PAGOS
        public Pagos DeserializarPagos(Comprobante obj)
        {
            var nodoPagos = obj.Complemento.Any.FirstOrDefault(x => x.Name == "pago20:Pagos");

            if (nodoPagos == null) return null;

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


        //CADENA ORIGINAL A PARTIR DEL TIMBRE FISCAL UUID - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public string GenerarCadenaOriginalTimbrado(Comprobante comprobante)
        {
            try
            {

                if (comprobante.TimbreFiscalDigital == null)
                {
                    throw new InvalidOperationException("El Comprobante no contiene el TimbreFiscalDigital, revise que el CFDI haya sido timbrado y su XML deserializado correctamente.");
                }


                TimbreFiscalDigital tfd = comprobante.TimbreFiscalDigital;

                string version = tfd.Version ?? "";
                string uuid = tfd.UUID ?? "";

                string fechaTimbrado = tfd.FechaTimbrado.ToString("yyyy-MM-ddTHH:mm:ss");
                string rfcProvCertif = tfd.RfcProvCertif ?? "";
                string selloCFD = tfd.SelloCFD ?? "";
                string noCertSAT = tfd.NoCertificadoSAT ?? "";

                string cadenaOriginal = $"||{version}|{uuid}|{fechaTimbrado}|{rfcProvCertif}|{selloCFD}|{noCertSAT}||";

                return cadenaOriginal;
            }
            catch (Exception ex)
            {

                return $"Error al generar la cadena original del Timbre Fiscal Digital: {ex.Message}";

            }
        }
        //CADENA ORIGINAL A PARTIR DEL TIMBRE FISCAL UUID - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


        //PROCEDIMIENTOS PARA LA CANCELACIÓN DE CFDIs - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
        public SignatureType AgregarFirmaACancelacion(Cancelacion cancelacion)
        {
            // Registrar algoritmo de canonicalización
            CryptoConfig.AddAlgorithm(typeof(XmlDsigC14NTransform), "http://www.w3.org/TR/2001/REC-xml-c14n-20010315");

            // 1. Serializar el objeto Cancelacion a XmlDocument
            var xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "http://cancelacfd.sat.gob.mx");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");

            var serializer = new XmlSerializer(typeof(Cancelacion));
            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                serializer.Serialize(xmlWriter, cancelacion, ns);
                xmlDoc.LoadXml(stringWriter.ToString());
            }

            // 2. Obtener clave privada
            SecureString securePwd = new SecureString();
            foreach (char c in _ClavePrivada)
                securePwd.AppendChar(c);

            var privateKey = opensslkey.DecodeEncryptedPrivateKeyInfo(File.ReadAllBytes(_RutaKeyCSD), securePwd);

            // 3. Cargar certificado
            var cert = new X509Certificate2(_RutaCerCSD);

            // 4. Crear firma
            SignedXml signedXml = new SignedXml(xmlDoc)
            {
                SigningKey = privateKey
            };

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url; // SHA1

            Reference reference = new Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.DigestMethod = SignedXml.XmlDsigSHA1Url; // SHA1
            signedXml.AddReference(reference);

            // Crear X509Data con IssuerSerial
            var keyInfo = new KeyInfo();
            var x509Data = new KeyInfoX509Data(cert);
            x509Data.AddIssuerSerial(cert.Issuer, cert.SerialNumber); // IssuerSerial
            keyInfo.AddClause(x509Data);
            signedXml.KeyInfo = keyInfo;

            // 5. Generar la firma
            signedXml.ComputeSignature();
            XmlElement xmlSignature = signedXml.GetXml();

            // 6. Deserializar la firma a SignatureType
            using (var reader = new XmlNodeReader(xmlSignature))
            {
                XmlSerializer signatureSerializer = new XmlSerializer(typeof(SignatureType));
                return (SignatureType)signatureSerializer.Deserialize(reader);
            }
        }


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
