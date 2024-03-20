using Gateway.Model;
using System.Xml.Serialization;

namespace SOAP.Model;

[XmlType(Namespace = SOAPResponseBody.DefaultNamespace)]
public partial class SOAPResponseBody
{
    //public const string DefaultNamespacePrefix = "ser";
    //public const string DefaultNamespace = "http://some.com/service/";

    public const string DefaultNamespacePrefix = "tem";
    public const string DefaultNamespace = "http://tempuri.org/";
    public const string DefaultNamespaceatPrefix = "at";
    public const string DefaultNamespaceat = "http://cps.huawei.com/cpsinterface/goa/at";

    public const string DefaultNamespacegoaPrefix = "goa";
    public const string DefaultNamespacegoa = "http://cps.huawei.com/cpsinterface/goa";
    public GetWeatherForecastResponse GetWeatherForecastResponse { get; set; }
    public ApplyTransactionResponse ApplyTransactionResponse { get; set; }
}
