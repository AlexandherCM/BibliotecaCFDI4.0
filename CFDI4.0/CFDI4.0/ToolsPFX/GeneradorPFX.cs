using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.IO;

namespace CFDI4._0.ToolsPFX
{
    public class GeneradorPFX
    {
        // Genera un archivo PFX (PKCS#12) a partir de un certificado .cer y una clave privada .key (formato DER)
        public void Generar(string rutaCertificado, string rutaLlave, string clavePrivada, string rutaPfx)
        {
            // Leer el certificado .cer usando BouncyCastle
            var certParser = new X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate certBC = certParser.ReadCertificate(File.ReadAllBytes(rutaCertificado));

            // Convertir .key DER a clave privada
            AsymmetricKeyParameter key = ConvertirClaveDerAPem(rutaLlave, clavePrivada);

            // Crear contenedor PKCS12
            var store = new Pkcs12StoreBuilder().Build();
            var certEntry = new X509CertificateEntry(certBC);
            store.SetCertificateEntry("cert", certEntry);
            store.SetKeyEntry("key", new AsymmetricKeyEntry(key), new[] { certEntry });

            // Guardar como .pfx
            using (var fs = new FileStream(rutaPfx, FileMode.Create, FileAccess.Write))
            {
                store.Save(fs, clavePrivada.ToCharArray(), new SecureRandom());
            }
        }

        // Convierte una clave privada en formato DER a AsymmetricKeyParameter.
        private AsymmetricKeyParameter ConvertirClaveDerAPem(string rutaKeyDer, string clavePrivada)
        {
            try
            {
                byte[] keyBytes = File.ReadAllBytes(rutaKeyDer);
                return PrivateKeyFactory.DecryptKey(clavePrivada.ToCharArray(), keyBytes);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al convertir la clave .key DER a PEM: " + ex.Message, ex);
            }
        }
    }
}
