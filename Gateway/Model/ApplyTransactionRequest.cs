using Gateway.Model.SOAP;
using SOAP.Model;
using System.Xml.Serialization;

namespace Gateway.Model
{
    //[XmlType(Namespace = ApplyTransactionRequest.DefaultNamespace)]
    public partial class ApplyTransactionRequest
    {
        //public const string DefaultNamespacePrefix = "tem";
        //public const string DefaultNamespace = "http://some.com/service/";

        [XmlElement("Header")]
        public SOAPHeader Header { get; set; }
        [XmlElement("Body")]
        public SOAPRequestInnerBody Body { get; set; }
    }
    
}
