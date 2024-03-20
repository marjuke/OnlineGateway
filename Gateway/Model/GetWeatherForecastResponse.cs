using System.Xml.Serialization;
using Gateway;
using SOAP;

// [XmlType(Namespace = "")]
[XmlType(Namespace = SOAPConstants.SOAPNamespaceat)]
public class GetWeatherForecastResponse
{
    // [XmlNamespaceDeclarations]
    // public XmlSerializerNamespaces? ns = new XmlSerializerNamespaces();
    // public GetWeatherForecastResponse()
    // {
    //     ns.Add(SOAPResponseBody.DefaultNamespacePrefix, SOAPResponseBody.DefaultNamespace);
    // }
    public int ResponseCode { get; set; } = 0;
    public string ResponseDesc { get; set; } = "Success";

    [XmlArrayItemAttribute(Namespace = SOAPConstants.SOAPNamespacegoa)]
    public WeatherForecast[] WeatherForecasts { get; set; }
}