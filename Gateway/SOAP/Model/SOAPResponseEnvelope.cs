using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
namespace SOAP.Model;

[XmlRoot("Envelope", Namespace = SOAPConstants.SOAP1_1Namespace)]
public partial class SOAP1_1ResponseEnvelope : SOAPResponseEnvelope
{
    public SOAP1_1ResponseEnvelope()
    {
        ns.Add(SOAPConstants.DefaultSOAPEnvelopeNamespacePrefix, SOAPConstants.SOAP1_1Namespace);
    }
    [NotNull]
    [XmlElement("Body")]
    public SOAP1_1ResponseBody? Body_Typed
    {
        get
        {
            if (_body is null)
                _body = (SOAP1_1ResponseBody)CreateBody();
            return (SOAP1_1ResponseBody)_body;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

}

[XmlRoot("Envelope", Namespace = SOAPConstants.SOAP1_2Namespace)]
public partial class SOAP1_2ResponseEnvelope : SOAPResponseEnvelope
{
    public SOAP1_2ResponseEnvelope()
    {
        ns.Add(SOAPConstants.DefaultSOAPEnvelopeNamespacePrefix, SOAPConstants.SOAP1_2Namespace);
        
        
    }

    [NotNull]
    [XmlElement("Header")]
    public SOAP1_2ResponseHeader? Header_Typed
    {
        get
        {
            if (_head is null)
                _head = (SOAP1_2ResponseHeader)CreateHeader();
            return (SOAP1_2ResponseHeader)_head;
        }
        set
        {
            throw new NotImplementedException();
        }
    }
    
    [NotNull]
    [XmlElement("Body")]
    public SOAP1_2ResponseBody? Body_Typed
    {
        get
        {
            if (_body is null)
                _body = (SOAP1_2ResponseBody)CreateBody();
            return (SOAP1_2ResponseBody)_body;
        }
        set
        {
            throw new NotImplementedException();
        }
    }

}

public abstract partial class SOAPResponseEnvelope
{
    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
    
    protected SOAPResponseBody? _body;
    protected SOAPResponseHeader? _head;

    [NotNull]
    [XmlIgnore]
    public SOAPResponseHeader? Header
    {
        get
        {
            if (_head is null)
                _head = CreateHeader();
            return _head;
        }
        set { _head = value; }
    }
    [NotNull]
    [XmlIgnore]
    public SOAPResponseBody? Body
    {
        get
        {
            if (_body is null)
                _body = CreateBody();
            return _body;
        }
        set { _body = value; }
    }
    protected virtual SOAPResponseBody CreateBody() => this is SOAP1_1ResponseEnvelope ? new SOAP1_1ResponseBody() : new SOAP1_2ResponseBody();
    protected virtual SOAPResponseHeader CreateHeader() => this is SOAP1_1ResponseEnvelope ? new SOAP1_1ResponseHeader() : new SOAP1_2ResponseHeader();

}
