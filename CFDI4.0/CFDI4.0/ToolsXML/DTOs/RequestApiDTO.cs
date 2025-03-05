using System;
using System.Collections.Generic;
using System.Text;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace CFDI4._0.ToolsXML.DTOs
{
    public class RequestApiDTO
    {
        public bool status { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public Content data { get; set; }
        public credentialsDTO credentials { get; set; }
    }

    public class Content
    {
        public string timbrexml { get; set; }
        public string xml { get; set; }
        public string jsonComprobante { get; set; }
        public string base64pdf { get; set; }
    }   

    public class credentialsDTO
    {
        public string ApiKey { get; set; }
        public string JWT { get; set; }
    }
}
