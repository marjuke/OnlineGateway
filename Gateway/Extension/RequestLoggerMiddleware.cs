using Azure.Core;
using Gateway.SOAP.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;

public class RequestLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggerMiddleware> _logger;

    public RequestLoggerMiddleware(RequestDelegate next, ILogger<RequestLoggerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        //// Log request body if present
        ////if (context.Request.ContentLength > 0)
        ////{
        //    //string requestBody = await ReadRequestBody(context.Request);
        //    //string requestBody = context.Request.Headers.ContentType.ToString();
        //    //string requestBody1 = context.Request.Headers.Host.ToString();
        //    //var requestBody2 = context.Request.Body;
        //    //string requestBody3 = context.Request.;
            

        //var originalResponseBody = context.Response.Body;
        //using var newResponseBody = new MemoryStream();
        //context.Response.Body = newResponseBody;

        //// Call the next middleware in the pipeline  
        //await _next(context);

        //newResponseBody.Seek(0, SeekOrigin.Begin);
        //Char[] buffer;
        //var sr = new StreamReader(context.Response.Body);
       
        //buffer = new Char[(int)sr.BaseStream.Length];
        //await sr.ReadAsync(buffer, 0, (int)sr.BaseStream.Length);
        //string myString = new string(buffer);

        ////Console.WriteLine($"HTTP request information:\n" +
        ////    $"\tStatusCode: {context.Response.StatusCode}\n" +
        ////    $"\tContentType: {context.Response.ContentType}\n" +
        ////    $"\tHeaders: {FormatHeaders(context.Response.Headers)}\n" +
        ////    $"\tBody: {responseBodyText}");

        //_logger.LogInformation($"\tStatusCode: {context.Response.StatusCode}\n");
        //_logger.LogInformation($"\tContentType: {context.Response.ContentType}\n");
        //_logger.LogInformation($"\tHeaders: {FormatHeaders(context.Response.Headers)}\n");
        //_logger.LogInformation($"\tContentSize: {buffer.Length}\n");
        ////_logger.LogInformation($"\tBody: {myString}");
        //newResponseBody.Seek(0, SeekOrigin.Begin);
        //await newResponseBody.CopyToAsync(originalResponseBody);
        ////}

        //// Continue with the request pipeline
        await _next(context);
    }
    private static string FormatHeaders(IHeaderDictionary headers)
    {
        return string.Join(", ", headers.Select(kvp => $"{{<{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
    }

    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        // Ensure the request's body can be read multiple times (for the next middlewares in the pipeline).  
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        // Reset the request's body stream position for next middleware in the pipeline.  
        request.Body.Position = 0;
        return requestBody;
    }
    //private async Task<string> ReadRequestBody(HttpRequest request)
    //{
    //    using (var reader = new StreamReader(request.Body, Encoding.Unicode, true, 1024, true))
    //    {
    //        return await reader.ReadToEndAsync();
    //    }
    //}
}
