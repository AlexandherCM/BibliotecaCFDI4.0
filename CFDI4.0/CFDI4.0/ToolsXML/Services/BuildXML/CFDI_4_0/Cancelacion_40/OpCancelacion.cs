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
using System.Xml.Linq;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40
{
    public class OpCancelacion
    {
        public static byte[] CreatePFX(byte[] bytesCER, byte[] bytesKEY, string password)
        {
            var certParser = new X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate bcCert = certParser.ReadCertificate(bytesCER);
            if (bcCert == null)
                throw new CryptographicException("No se pudo leer el certificado (.cer).");

            // Desencriptar llave privada
            AsymmetricKeyParameter bcPrivKey = PrivateKeyFactory.DecryptKey(password.ToCharArray(), bytesKEY);
            if (bcPrivKey == null || !bcPrivKey.IsPrivate)
                throw new CryptographicException("No se pudo desencriptar la llave privada (.key). Verifica contraseña.");

            // Crear PKCS12 (PFX)
            var store = new Pkcs12StoreBuilder().Build();
            string alias = "csd";

            var certEntry = new X509CertificateEntry(bcCert);
            store.SetKeyEntry(alias, new AsymmetricKeyEntry(bcPrivKey), new[] { certEntry });

            using (var ms = new MemoryStream())
            {
                store.Save(ms, password.ToCharArray(), new SecureRandom());
                return ms.ToArray();
            }
        }

        //public static string SignCancelacion(List<CancelacionDTO> folios, string rfcEmisor, byte[] pfx, string password, bool isRetencion)
        //{
        //    try
        //    {
        //        string xml;
        //        xml = CreateCancelationXML(folios, rfcEmisor, isRetencion);
        //        return SignUtils.SignXml(xml, pfx, password);
        //    }
        //    catch (CryptographicException e)
        //    {
        //        throw new CryptographicException("Error al sellar el XML.", e);
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Los folios no tienen un formato valido.", e);
        //    }
        //}

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
