using System.Xml.Serialization;
namespace SOAP.Model;
[XmlType(Namespace = "")]
public partial class SOAP1_1Fault : SOAPFault
{
    public enum SOAP1_1FaultCodes
    {
        Client,
        Server
    }
    [XmlIgnore]
    public SOAP1_1FaultCodes faultcode { get; set; }
    [XmlElement("faultcode")]
    public string faultcodestring
    {
        get
        {
            return $"{SOAPConstants.DefaultSOAPEnvelopeNamespacePrefix}:{faultcode.ToString()}";
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public string faultstring { get; set; }
    public SOAPFaultDetail? detail { get; set; }
    // Needed for Serialization
    protected SOAP1_1Fault() { faultstring = ""; }
    public SOAP1_1Fault(SOAP1_1FaultCodes faultcode, String faultstring)
    {
        this.faultcode = faultcode;
        this.faultstring = faultstring;
    }

}