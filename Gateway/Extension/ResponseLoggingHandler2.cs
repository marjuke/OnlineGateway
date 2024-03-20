using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.MiddleWare
{
    public class ResponseLoggingMiddleware2
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware2> _logger;

        public ResponseLoggingMiddleware2(RequestDelegate next, ILogger<ResponseLoggingMiddleware2> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            IPAddress remoteIpAddress = context.Connection.RemoteIpAddress;
            IPAddress remoteIpAddress2 = context.Connection.LocalIpAddress;
            string result1 = "";
            if (remoteIpAddress != null)
            {
                // If we got an IPV6 address, then we need to ask the network for the IPV4 address 
                // This usually only happens when the browser is on the same machine as the server.
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                result1 = remoteIpAddress.ToString();
            }
            var ipAddress = result1;
            _logger.LogInformation("Source IP Address is " + ipAddress + "");
            // Capture request information
            var requestInfo = $"{DateTime.Now} | {context.Request.Method} | {context.Request.Path}";

            // Log the request information
            _logger.LogInformation(requestInfo);
            var request = Convert.ToInt16(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Request"]);
            // Read the request body and save its content utf-16
            if (request == 1)
            {
                string requestBody = await ReadRequestBody(context.Request);
                LogXml(requestBody, "request");
            }
            if (request == 2)
            {

                var originalBodyStream = context.Response.Body;

                using (var responseBodyStream = new MemoryStream())
                {
                    // Replace the original response body stream with a MemoryStream
                    context.Response.Body = responseBodyStream;

                    // Call the next middleware in the pipeline
                    await _next(context);

                    // Rewind the MemoryStream to read the response
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
                    var cleanedContent = RemoveXmlNamespaces(responseBody);

                    // Log response XML
                    LogXml(cleanedContent, "response");

                    // Write the cleaned content back to the response body
                    context.Response.Body = originalBodyStream;
                    await context.Response.WriteAsync(cleanedContent);
                }
            }
            else
            {
                await _next(context);
            }
            
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            // Read the request body
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private void LogXml(string xmlData, string type)
        {
            // Generate a unique file name
            string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{type}.xml";

            // Combine the file name with the log directory to get the full file path
            string rootFolder = Directory.GetCurrentDirectory();
            string folderPath = Path.Combine(rootFolder, "ResponseFile");
            string filePath = Path.Combine(folderPath, fileName);
            //var filePath = Path.Combine(@"D:\Logs", fileName); // Specify your desired directory
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Write the XML data to the file
            File.WriteAllText(filePath, xmlData, Encoding.UTF8);
        }

        private string RemoveXmlNamespaces(string content)
        {
            // Remove xmlns:xsi and xmlns:xsd namespaces
            return content.Replace("x\0m\0l\0n\0s\0:\0x\0s\0i\0=\0\"\0h\0t\0t\0p\0:\0/\0/\0w\0w\0w\0.\0w\03\0.\0o\0r\0g\0/\02\00\00\01\0/\0X\0M\0L\0S\0c\0h\0e\0m\0a\0-\0i\0n\0s\0t\0a\0n\0c\0e\0\"\0 \0x\0m\0l\0n\0s\0:\0x\0s\0d\0=\0\"\0h\0t\0t\0p\0:\0/\0/\0w\0w\0w\0.\0w\03\0.\0o\0r\0g\0/\02\00\00\01\0/\0X\0M\0L\0S\0c\0h\0e\0m\0a\0\"\0", "");
                          
        }
    }
}
