using CFDI4._0.ToolsXML.DTOs;
using CFDI4._0.ToolsXML.Helpers;
using CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40.Signature;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40
{
    public class OpCancelacion : OpXML
    {
        // ================================ METÓDOS PRIVADOS ================================
        private string CreateCancelationXML(List<CancelacionDTO> foliosFiscales, string rfcEmisor, bool isRetencion = false)
        {
            XNamespace satCancelacionXmlNamespace = isRetencion ? "http://www.sat.gob.mx/esquemas/retencionpago/1" : "http://cancelacfd.sat.gob.mx";
            var xmlSolicitud = new XElement(satCancelacionXmlNamespace + "Cancelacion",
                                            new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                            new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                                            new XAttribute("Fecha", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                                            new XAttribute("RfcEmisor", rfcEmisor),
                                            from uuid in foliosFiscales
                                            select new XElement(satCancelacionXmlNamespace + "Folios",
                                                new XElement(satCancelacionXmlNamespace + "Folio",
                                                new XAttribute("UUID", uuid.Folio.ToString()),
                                                new XAttribute("Motivo", Convert.ToInt16(uuid.Motivo).ToString().PadLeft(2, '0')),
                                                new XAttribute("FolioSustitucion", uuid.FolioSustitucion != null ? uuid.FolioSustitucion.ToString() : string.Empty)
                                                )));
            return xmlSolicitud.ToString();
        }

        private string Sign(List<CancelacionDTO> folios, string rfcEmisor, byte[] pfx, string password, bool isRetencion)
        {
            try
            {
                string xml;
                xml = CreateCancelationXML(folios, rfcEmisor, isRetencion);
                return SignCancelacion.SignXml(xml, pfx, password);
            }
            catch (CryptographicException e)
            {
                throw new CryptographicException("Error al sellar el XML.", e);
            }
            catch (Exception e)
            {
                throw new Exception("Los folios no tienen un formato valido.", e);
            }
        }

        // ================================ METÓDOS PUBLICOS ================================
        public SignResponse SellarCancelacion(List<CancelacionDTO> folios, string rfcEmisor, byte[] pfx, string password, bool isRetencion = false)
        {
            try
            {
                if (folios.Count > 0)
                {
                    Validation.ValidarCancelacion(folios);

                    return new SignResponse()
                    {
                        data = new SignDataResponse()
                        {
                            xml = Sign(folios, rfcEmisor, pfx, password, isRetencion)
                        }
                    };
                }

                throw new Exception("El listado de folios esta vacio.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocurrio un error {ex.Message}");
            }
        }

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
