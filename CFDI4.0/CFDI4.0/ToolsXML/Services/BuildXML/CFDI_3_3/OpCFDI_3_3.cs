using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CFDI4._0.CFDI;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using CFDI4._0.CFDI.EsquemaCFDI3_3;
using ComplementoPagos10;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_3_3
{
    public class OpCFDI_3_3
    {
        //CONVERSIÓN DE TODO EL XML A OBJETO DE C#
        public Comprobante3_3 DeserializarXMLCompleto(string xmlContent)
        {
            // Crear el serializador para el objeto Comprobante 
            XmlSerializer serializer = new XmlSerializer(typeof(Comprobante3_3), "http://www.sat.gob.mx/cfd/3");

            // Deserializar el XML contenido en la cadena
            Comprobante3_3 oComprobante;    
            using (StringReader stringReader = new StringReader(xmlContent))
            {
                oComprobante = (Comprobante3_3)serializer.Deserialize(stringReader);
            }

            if (oComprobante.Complemento != null)
            {
                ObtenerTimbreFiscalEnComplemento(oComprobante.Complemento, ref oComprobante.TimbreFiscalDigital);
                oComprobante.complementoPagos = DeserializarPagos(oComprobante);
            }

            return oComprobante;
        }

        //FUNCIÓN QUE AGREGA EL TIMBRE FISCAL AL NODO COMPLEMENTO EN EL XML DEL CFDI - - - - - - - - - - - - - - - - - - - - - - - - -
        // FUNCIÓN QUE OBTIENE EL TIMBRE FISCAL DIGITAL
        private void ObtenerTimbreFiscalEnComplemento(ComprobanteComplemento3_3[] complemento, ref TimbreFiscalDigital timbreFiscal)
        {
            foreach (var oComplemento in complemento)
            {
                if (oComplemento?.Any == null) continue;

                var nodoTimbre = oComplemento.Any.FirstOrDefault(x =>
                    x.LocalName == "TimbreFiscalDigital" &&
                    x.NamespaceURI == "http://www.sat.gob.mx/TimbreFiscalDigital");

                if (nodoTimbre != null)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TimbreFiscalDigital));
                    using (StringReader reader = new StringReader(nodoTimbre.OuterXml))
                    {
                        timbreFiscal = (TimbreFiscalDigital)serializer.Deserialize(reader);
                    }
                }
            }
        }

        // FUNCIÓN QUE OBTIENE COMPLEMENTO DE PAGOS 1.0
        public Pagos1_0 DeserializarPagos(Comprobante3_3 obj)
        {
            if (obj?.Complemento == null)
                return null;

            foreach (var comp in obj.Complemento)
            {
                if (comp?.Any == null) continue;

                var nodoPagos = comp.Any.FirstOrDefault(x =>
                    x.LocalName == "Pagos" &&
                    x.NamespaceURI == "http://www.sat.gob.mx/Pagos");

                if (nodoPagos != null)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Pagos1_0));
                        using (StringReader reader = new StringReader(nodoPagos.OuterXml))
                        {
                            return (Pagos1_0)serializer.Deserialize(reader);
                        }
                    }
                    catch
                    {
                        return null; // puedes loguear el error si es necesario
                    }
                }
            }

            return null;
        }



    }
}
