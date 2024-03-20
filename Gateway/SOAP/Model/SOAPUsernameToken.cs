using System.Xml.Serialization;
namespace SOAP.Model;
public partial class SOAPUsernameToken
{
    [XmlAttribute(Namespace = SOAPConstants.SOAPSecurityUtilityNamespace)]
    public string? Id { get; set; }
    public string? Username { get; set; }
    public Password? Password { get; set; }
    public string? Nonce { get; set; }
    [XmlElement(Namespace = SOAPConstants.SOAPSecurityUtilityNamespace)]
    public DateTime? Created { get; set; }
}

public partial class Password
{
    [XmlAttribute]
    public string? Type { get; set; }
    [XmlText]
    public string? Value { get; set; }
}