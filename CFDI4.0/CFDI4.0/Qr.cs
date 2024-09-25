using QRCoder;
using System;
using System.IO;

namespace CFDI4._0
{
    public class Qr
    {
        public static string GenerateQRCode(string text)
        {
            // Crear el generador de códigos QR
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            // Usar un nivel de corrección de errores más bajo si es aceptable
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M, true, true, QRCodeGenerator.EciMode.Utf8);
            BitmapByteQRCode bitmapByteCode = new BitmapByteQRCode(qrCodeData);

            // Obtener el gráfico del código QR como un array de bytes, reducir el tamaño del gráfico si es posible
            byte[] bitMap = bitmapByteCode.GetGraphic(10);

            // Escribir el gráfico en un MemoryStream y convertirlo a una cadena Base64
            using (var ms = new MemoryStream())
            {
                ms.Write(bitMap, 0, bitMap.Length);
                byte[] byteImage = ms.ToArray();
                string base64 = Convert.ToBase64String(byteImage);
                return $"data:image/png;base64,{base64}";
            }
        }

    }

}
