using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System.IO;
using System.Security.Cryptography;

namespace CFDI4._0.ToolsXML.Helpers
{
    public static class PFX  
    {
        public static byte[] Create(byte[] bytesCER, byte[] bytesKEY, string password)
        {
            var certParser = new X509CertificateParser();
            X509Certificate bcCert = certParser.ReadCertificate(bytesCER);
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
    }
}
