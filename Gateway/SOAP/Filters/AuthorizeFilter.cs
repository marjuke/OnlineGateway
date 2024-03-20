using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SOAP.Authorization;
using SOAP.Model;
namespace SOAP.Mvc.Filters;
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAuthorizationFilter
{
    public string? Departments { get; set; }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        Task.Run(() => OnAuthorizationAsync(context)).Wait();
        return;
    }
    public virtual async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var User = context.HttpContext.User;
        SOAPControllerAttribute? soapAttribute = context.ActionDescriptor.EndpointMetadata.OfType<SOAPControllerAttribute>().FirstOrDefault();
        if (soapAttribute is null)
            throw new Exception("AuthorizeAttribute requires a SOAPControllerAttribute on the controller");
        if (context.HttpContext.Items[SOAPAuthData.RequestKey_SOAPAuthData] is not SOAPAuthData authDataObject)
            throw new Exception("AuthorizeAttribute cannot find SOAPAuthData in the request. The AuthenticationHandler should supply this.");
        authDataObject.TargetServiceName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
        authDataObject.RequestedDepartments = Departments;
        var authZRepository = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationRepository>();
        if (!await authZRepository.AuthorizeUserAsync(User, authDataObject))
        {
            await context.HttpContext.ForbidAsync();
            context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
            return;
        }
        return;
    }

}