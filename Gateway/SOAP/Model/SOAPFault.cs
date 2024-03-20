using System.Xml.Serialization;
namespace SOAP.Model;
[XmlType(Namespace = "")]
public abstract partial class SOAPFault
{
    public enum PartyAtFault
    {
        Client,
        Server
    }
}