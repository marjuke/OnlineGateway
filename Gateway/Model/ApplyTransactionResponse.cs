using Domain.ResponseC;
using SOAP;
using System.Text;
using System.Xml.Serialization;

namespace Gateway.Model
{
    [XmlType(Namespace = SOAPConstants.SOAPNamespaceat)]
    public class ApplyTransactionResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; } = "Success";
        [XmlArrayItemAttribute(Namespace = SOAPConstants.SOAPNamespacegoa)]
        public Parameter[] Parameters { get; set; }
    }
}
