using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    public class Parameter
    {
        [XmlElement("Key", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string Key { get; set; }
        [XmlElement("Value", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string Value { get; set; }
    }
}
