using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    public class Header
    {
        [XmlElement("CommandID", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string CommandID { get; set; }
        [XmlElement("Version", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public double Version { get; set; }
        [XmlElement("LoginID", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string LoginID { get; set; }
        [XmlElement("Password", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string Password { get; set; }
        [XmlElement("Timestamp", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string Timestamp { get; set; }
        [XmlElement("ConversationID", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public string ConversationID { get; set; }
        [XmlElement("HeaderExtension", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public HeaderExtension HeaderExtension { get; set; }


    }
}
