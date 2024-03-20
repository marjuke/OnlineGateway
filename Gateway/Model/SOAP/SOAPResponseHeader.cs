using SOAP.Model;
using System.Xml.Serialization;

namespace SOAP.Model;

[XmlType(Namespace = SOAPResponseHeader.DefaultNamespacegoa)]
public class SOAPResponseHeader
{
    public const string DefaultNamespacePrefix = "tem";
    public const string DefaultNamespace = "http://tempuri.org/";
    public const string DefaultNamespaceatPrefix = "at";
    public const string DefaultNamespaceat = "http://cps.huawei.com/cpsinterface/goa/at";

    //public const string DefaultNamespacegoaPrefix = "goa";
    //public const string DefaultNamespacegoa = "http://cps.huawei.com/cpsinterface/goa";
    public const string DefaultNamespacegoaPrefix = "";
    public const string DefaultNamespacegoa = "";
    public string Version { get; set; }
    public string CommandID { get; set; }
    public string ConversationID { get; set; }
    public string OriginatorConversationID { get; set; }
    public string Timestamp { get; set; }

}

