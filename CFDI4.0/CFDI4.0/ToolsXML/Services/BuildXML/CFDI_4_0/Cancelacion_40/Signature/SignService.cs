using CFDI4._0.ToolsXML.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40.Signature
{
    public class SignService
    {
        public static SignResponse SellarCancelacion(List<CancelacionDTO> folios, string rfcEmisor, byte[] pfx, string password, bool isRetencion = false)
        {
            try
            {
                if (folios.Count > 0)
                {
                    Validation.ValidarCancelacion(folios);
                    return new SignResponse() { data = new SignDataResponse() { xml = SignCancelacion(folios, rfcEmisor, pfx, password, isRetencion) } };
                }
                throw new Exception("El listado de folios esta vacio.");
            }
            catch (Exception ex)
            {
                throw new Exception("Fallo.");
            }
        }   

        public static string SignCancelacion(List<CancelacionDTO> folios, string rfcEmisor, byte[] pfx, string password, bool isRetencion)
        {
            try
            {
                string xml;
                xml = CreateCancelationXML(folios, rfcEmisor, isRetencion);
                return SignUtils.SignXml(xml, pfx, password);
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

        public static string CreateCancelationXML(List<CancelacionDTO> foliosFiscales, string rfcEmisor, bool isRetencion = false)
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
    }
}
