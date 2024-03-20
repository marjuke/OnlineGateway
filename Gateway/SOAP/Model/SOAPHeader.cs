using System.Xml.Serialization;

namespace SOAP.Model;
public partial class SOAPHeader
{
    [XmlElement(Namespace = SOAPConstants.SOAPSecurityNamespace)]
    public SOAPSecurity? Security { get; set; }
}