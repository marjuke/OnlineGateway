using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml.Serialization;
using System.Xml;

namespace Gateway.Extension
{
    public class XmlSerializerOutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter
    {
        protected override void Serialize(XmlSerializer xmlSerializer, XmlWriter xmlWriter, object? value)
        {

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            xmlSerializer.Serialize(xmlWriter, value, ns);
        }
    }
}
