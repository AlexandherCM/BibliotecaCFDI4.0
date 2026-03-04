using System.Runtime.Serialization;

namespace CFDI4._0.ToolsXML.DTOs
{
    public class Response
    {
        public string status { get; set; } = "success";
        public string message { get; set; }
        public string messageDetail { get; set; }
    }

    public class SignResponse : Response
    {
        [DataMember]
        public SignDataResponse data { get; set; }

    }

    public partial class SignDataResponse
    {
        [DataMember]
        public string xml { get; set; }
    }

    public class SignResponseV2 : Response
    {
        [DataMember]
        public SignDataResponseV2 data { get; set; }
    }

    public partial class SignDataResponseV2
    {
        [DataMember]
        public string cadenaOriginal { get; set; }
    }
}
