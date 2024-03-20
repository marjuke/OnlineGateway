using Persistence;
using Serilog;
using System.Net;
using System.Text;

namespace Gateway.MiddleWare
{
    public class ResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ResponseLoggingMiddleware> _logger;
        //private readonly DataContext _context;

        public ResponseLoggingMiddleware(RequestDelegate next, ILogger<ResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            //_context = context;
        }

        public async Task Invoke(HttpContext context)
        {
            IPAddress remoteIpAddress = context.Connection.RemoteIpAddress;
            IPAddress remoteIpAddress2 = context.Connection.LocalIpAddress;
            string result1 = "";
            if (remoteIpAddress != null)
            {
                if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
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
            if (request==1)
            {
                //await _next(context);
                //string requestBody = await ReadRequestBody(context.Request);
                //LogXml(requestBody, "request");
                //await _next(context);



                context.Request.EnableBuffering();

                // Read the request body
                string requestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    context.Request.Body.Seek(0, SeekOrigin.Begin); // Reset the request body position
                }

                // Log the request data to a file
                //LogRequestData(requestBody);
                LogXml(requestBody, "request");

                // Call the next middleware in the pipeline
                await _next(context);

            }
            else if(request == 2)
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


                    // Log response XML
                    LogXml(responseBody, "response");

                    // Copy the response from the MemoryStream to the original response body stream
                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(originalBodyStream);

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
    }
}
