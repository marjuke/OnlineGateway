using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class AdminSafeListMiddleware
{
    private readonly RequestDelegate _next;

    public AdminSafeListMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        //_allowedIPs = config.GetSection("AdminSafeList").Get<string[]>();
    }

    public async Task Invoke(HttpContext context)
    {
        var clientIP = context.Connection.RemoteIpAddress;
        var isAllowed = false;
        var allowedIPs = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AdminSafeList").Get<List<safeList>>();
        //what is the json format for the allowedIPs in appsettings.json

        if (allowedIPs.Any(s=>s.IP == "All"))
        {
            isAllowed = true;
        }
        else
        {
            foreach (var ip in allowedIPs)
            {
                var parsedIP = IPAddress.Parse(ip.IP);
                if (parsedIP.Equals(clientIP))
                {
                    isAllowed = true;
                    break;
                }
            }
        }

        

        if (!isAllowed)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }
}
public class safeList
{
    public string Name { get; set; }
    public string IP { get; set; }
}
