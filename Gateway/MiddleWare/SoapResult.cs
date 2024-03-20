using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace Gateway.MiddleWare
{
    public class SoapResult : ActionResult
    {
        private readonly XmlDocument _soapXmlDocument;

        public SoapResult(XmlDocument soapXmlDocument)
        {
            _soapXmlDocument = soapXmlDocument ?? throw new ArgumentNullException(nameof(soapXmlDocument));
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/soap+xml";

            using (var stream = new MemoryStream())
            {
                _soapXmlDocument.Save(stream);
                stream.Position = 0;
                await stream.CopyToAsync(response.Body);
            }
        }
    }
}
