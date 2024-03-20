using Gateway.Model;
using System.Xml.Serialization;
namespace SOAP.Model;

//[XmlType(Namespace = SOAPRequestBody.DefaultNamespace)]
public partial class SOAPRequestBody
{
    
    //public GetWeatherForecastRequest? GetWeatherForecast { get; set; }
    [XmlElement("ApplyTransactionRequest")]
    public ApplyTransactionRequest ApplyTransactionRequest { get; set; }

}