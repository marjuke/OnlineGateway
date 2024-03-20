using Domain.ResponseC;
using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model
{
    [XmlType(Namespace = SOAPConstants.SOAPNamespaceat)]
    public class GetbKashResponse
    {
        public int ResponseCode { get; set; } = 0;
        public string ResponseDesc { get; set; } = "Success";
        [XmlArrayItemAttribute(Namespace = SOAPConstants.SOAPNamespacegoa)]
        public Parameter[] Parameters { get; set; }
    }
}
