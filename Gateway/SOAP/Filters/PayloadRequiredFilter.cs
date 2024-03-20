using Domain.Identity;
using Gateway.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using SOAP.Controllers;
using SOAP.Model;

namespace SOAP.Mvc.Filters;
public class PayloadRequiredAttribute : ActionFilterAttribute
{
    
   
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!typeof(SOAPControllerBase).IsAssignableFrom(context.Controller.GetType()))
            throw new Exception("PayloadRequiredAttribute can only be used on classes deriving from SOAPControllerBase");
        SOAPControllerBase controller = (SOAPControllerBase)context.Controller;
        //Log.Information("PayloadInfoContentType: "+controller.Request.ContentType);
        //Log.Information("PayloadInfoPath: "+controller.Request.Path);
        var parametersMetaData = context.ActionDescriptor.Parameters.Where(
            x =>
            typeof(SOAPRequestEnvelope).IsAssignableTo(x.ParameterType) ||
            typeof(SOAPRequestBody).IsAssignableTo(x.ParameterType) ||
            typeof(SOAP1_1RequestEnvelope).IsAssignableTo(x.ParameterType) ||
            typeof(SOAP1_2RequestEnvelope).IsAssignableTo(x.ParameterType)
            );
        if (parametersMetaData.Count() < 1)
        {
            if (context.HttpContext.Request.ContentLength == 0)
                context.Result = controller.SOAPPayloadMissing();
            return;
        }

        if (parametersMetaData.Count() > 1)
            throw new Exception($"PayloadRequiredAttribute cannot determine the target model type. Too many parameters of type {nameof(SOAPRequestEnvelope)}, {nameof(SOAPRequestBody)}, {nameof(SOAP1_1RequestEnvelope)} or {nameof(SOAP1_2RequestEnvelope)}");
        var parameterMetaData = parametersMetaData.First();
        if (parameterMetaData is not null && typeof(SOAPRequestEnvelope).IsAssignableTo(parameterMetaData.ParameterType))
        {
            var payload = context.ActionArguments.Where(x => x.Value is SOAPRequestEnvelope).Select(v => (SOAPRequestEnvelope?)(v.Value)).FirstOrDefault();
            if (payload is null || payload.Body is null)
                context.Result = controller.SOAPPayloadMissing();
        }
        else
        {
            if (parameterMetaData is not null && typeof(SOAPRequestBody).IsAssignableTo(parameterMetaData.ParameterType))
            {
                var payload = context.ActionArguments.Where(x => x.Value is SOAPRequestBody).Select(v => (SOAPRequestBody?)(v.Value)).FirstOrDefault();
                if (payload is null)
                    context.Result = controller.SOAPPayloadMissing();
            }
        }
    }
}