using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    [XmlRoot("ApplyTransactionRequest", Namespace = SOAPConstants.SOAPNamespacetem)]
    public class ApplyTransactionRequest
    {
        [XmlElement("Header", Namespace = SOAPConstants.SOAPNamespaceat)]
        public Header Header { get; set; }

        [XmlElement("Body", Namespace = SOAPConstants.SOAPNamespaceat)]
        public Body Body { get; set; }
    }
}
