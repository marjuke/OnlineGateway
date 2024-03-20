using System.Xml.Serialization;
namespace SOAP.Model;
[XmlType(Namespace = "")]
public partial class SOAPFaultDetail
{
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces? ns;
}