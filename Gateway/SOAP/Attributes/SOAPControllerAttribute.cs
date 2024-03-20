using Microsoft.AspNetCore.Mvc;
using SOAP.Model;

namespace SOAP;
public class SOAPControllerAttribute : ProducesAttribute
{
    public SOAPVersion SOAPVersion { get; }
    public SOAPControllerAttribute(SOAPVersion soapVersion) : base(System.Net.Mime.MediaTypeNames.Application.Xml)
    {
        
        SOAPVersion = soapVersion;
    }

}