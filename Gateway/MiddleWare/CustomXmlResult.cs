using Microsoft.AspNetCore.Mvc;
using System.Xml.Serialization;
using System.Xml;

namespace Gateway.MiddleWare
{
    public class CustomXmlResult : IActionResult
    {
        private readonly object _data;

        public CustomXmlResult(object data)
        {
            _data = data;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = context.HttpContext.Response.ContentType;

            var xmlSerializer = new XmlSerializer(_data.GetType());
            using (var writer = XmlWriter.Create(response.Body, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", ""); // Omit default namespaces
                xmlSerializer.Serialize(writer, _data, ns);
                await writer.FlushAsync();
            }
        }
    }
}
