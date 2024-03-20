using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    public class SOAPBody
    {
        [XmlElement("ApplyTransactionRequest", Namespace = SOAPConstants.SOAPNamespacetem)]
        public ApplyTransactionRequest ApplyTransactionRequest { get; set; }
    }
}
