using Serilog;
using System.Text;

namespace Gateway.Extension
{
    public class ResponseLoggingHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Intercept response and log XML content to a file
            await LogResponseToTextFile(response);

            return response;
        }

        private async Task LogResponseToTextFile(HttpResponseMessage response)
        {
            // Ensure the response has content and is XML
            if (response.Content != null && response.Content.Headers.ContentType?.MediaType == "text/xml")
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                // Generate a unique file name (you can adjust this as needed)
                var fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}_response.xml";

                // Write XML content to a text file
                var filePath = Path.Combine(@"C:\Logs", fileName); // Specify your desired directory
                await File.WriteAllTextAsync(filePath, responseBody, Encoding.UTF8);

                // Log file path for reference
                Log.Information($"Response XML stored in file: {filePath}");
            }
        }
    }
}
