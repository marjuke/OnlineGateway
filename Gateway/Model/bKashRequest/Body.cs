using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    public class Body
    {
        [XmlElement("Parameters", Namespace = SOAPConstants.SOAPNamespaceat)]
        public Parameters Parameters { get; set; }
    }
}
