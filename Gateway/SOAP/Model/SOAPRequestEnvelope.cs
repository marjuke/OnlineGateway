
using Gateway.Model.bKashRequest;
using SOAP;
using System.Xml.Serialization;


[XmlRoot("Envelope", Namespace = SOAPConstants.SOAP1_1Namespace)]
public partial class SOAP1_1RequestEnvelope : SOAPRequestEnvelope { }

[XmlRoot("Envelope", Namespace = SOAPConstants.SOAP1_2Namespace)]
public partial class SOAP1_2RequestEnvelope : SOAPRequestEnvelope { }

public partial class SOAPRequestEnvelope
{
    [XmlElement("Body", Namespace =SOAPConstants.SOAP1_2Namespace)]
    public SOAPBody Body { get; set; }
}

