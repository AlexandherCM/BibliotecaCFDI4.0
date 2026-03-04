using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Xml;

namespace CFDI4._0.ToolsXML.Services.BuildXML.CFDI_4_0.Cancelacion_40.Signature
{
    public class SignUtils
    {
        public static string SignXml(string xml, byte[] pfx, string password)
        {
            // Importante: para PACs, firma sobre XML sin "pretty print"
            var doc = new XmlDocument { PreserveWhitespace = false };
            doc.LoadXml(xml);

            XmlElement signature = BuildSatCancelSignature(doc, pfx, password);

            // Evita saltos/espacios en Base64 (algunos PACs son estrictos)
            NormalizeBase64(signature, "SignatureValue");
            NormalizeBase64(signature, "X509Certificate");

            // Normaliza el issuer a una sola línea
            NormalizeSpaces(signature, "X509IssuerName");

            if (doc.DocumentElement == null)
                throw new CryptographicException("XML de cancelación inválido: no existe nodo raíz.");

            doc.DocumentElement.AppendChild(doc.ImportNode(signature, true));

            // IMPORTANTÍSIMO: no serializar con indentación después
            return doc.OuterXml;
        }

        public static XmlElement BuildSatCancelSignature(XmlDocument doc, byte[] pfx, string pfxPassword)
        {
            // NETSTANDARD2.0: no existe EphemeralKeySet.
            // Strategy: intentar MachineKeySet (Windows services) y si falla, fallback a Exportable.
            X509Certificate2 cert;
            try
            {
                cert = new X509Certificate2(
                    pfx,
                    pfxPassword,
                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet
                );
            }
            catch
            {
                cert = new X509Certificate2(
                    pfx,
                    pfxPassword,
                    X509KeyStorageFlags.Exportable
                );
            }

            if (!cert.HasPrivateKey)
                throw new CryptographicException("El PFX no contiene llave privada (HasPrivateKey=false).");

            // Disponible en netstandard2.0 vía RSACertificateExtensions
            RSA rsa = cert.GetRSAPrivateKey();
            if (rsa == null)
                throw new CryptographicException("No se pudo obtener la llave RSA del PFX (GetRSAPrivateKey()=null).");

            var signedXml = new SignedXml(doc);
            signedXml.SigningKey = rsa;

            // Cancelación SAT / PAC (como el ejemplo que te dio tu proveedor):
            // C14N 1.0 + RSA-SHA1 + SHA1
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigCanonicalizationUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            var reference = new Reference("");
            reference.DigestMethod = SignedXml.XmlDsigSHA1Url;

            // Sólo enveloped-signature
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            // KeyInfo con certificado
            var keyInfo = new KeyInfo();
            var x509Data = new KeyInfoX509Data(cert);
            x509Data.AddIssuerSerial(cert.Issuer, cert.SerialNumber);
            keyInfo.AddClause(x509Data);
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();
            return signedXml.GetXml();
        }

        private static void NormalizeBase64(XmlElement sig, string localName)
        {
            var el = sig.GetElementsByTagName(localName, SignedXml.XmlDsigNamespaceUrl)
                        .Cast<XmlNode>()
                        .FirstOrDefault() as XmlElement;

            if (el != null)
                el.InnerText = Regex.Replace(el.InnerText, @"\s+", "");
        }

        private static void NormalizeSpaces(XmlElement sig, string localName)
        {
            var el = sig.GetElementsByTagName(localName, SignedXml.XmlDsigNamespaceUrl)
                        .Cast<XmlNode>()
                        .FirstOrDefault() as XmlElement;

            if (el != null)
                el.InnerText = Regex.Replace(el.InnerText, @"\s+", " ").Trim();
        }
    }
}
