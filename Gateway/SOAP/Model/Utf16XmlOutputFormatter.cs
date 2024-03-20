using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Gateway.SOAP.Model
{
    public class Utf16XmlOutputFormatter : XmlDataContractSerializerOutputFormatter
    {
        public Utf16XmlOutputFormatter(MvcOptions options)
            : base((ILoggerFactory)options)
        {
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            
            using (var writer = new XmlTextWriter(response.Body, Encoding.Unicode))
            {
                // Serialize your data to the writer
                // ...
            }

            return Task.CompletedTask;
        }
    }
}
