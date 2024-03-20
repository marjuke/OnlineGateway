using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SOAP.Model;

namespace SOAP.Mvc.DependencyInjection;
public static class ApiBehaviorOptionsExtensions
{
    public static IActionResult InvalidModelStateResponseFactory(ActionContext actionContext)
    {
        /***
        * Here we can customise the default error response e.g. SOAP Version by Controller, along with SOAP:Fault detail element
        ***/
        string? faultstring = actionContext.ModelState.First(e => e.Value?.Errors.Count > 0).Value?.Errors.First().ErrorMessage;
        if (faultstring is null) faultstring = "An unexpected error occurred.";
        SOAPControllerAttribute? soapAttribute = actionContext.ActionDescriptor.EndpointMetadata.OfType<SOAPControllerAttribute>().FirstOrDefault();
        // Must return a SOAP Fault
        if (soapAttribute is not null)
        {
            return soapAttribute.SOAPVersion == SOAPVersion.v1_1 ?
                SOAPEnvelopeResponses.SOAPFault(new SOAP.Model.SOAP1_1ResponseEnvelope(), faultstring) :
                SOAPEnvelopeResponses.SOAPFault(new SOAP.Model.SOAP1_2ResponseEnvelope(), new Reason(faultstring));
        }
        else
        {
            // Must be REST so return the default WebAPI ProblemDetails object response
            var handler = actionContext.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            if (actionContext.ModelState.ValidationState == ModelValidationState.Invalid)
            {
                var problem = handler.CreateValidationProblemDetails(actionContext.HttpContext, actionContext.ModelState);
                return new BadRequestObjectResult(problem);
            }
            else
            {
                var problem = handler.CreateProblemDetails(actionContext.HttpContext, 500, faultstring, null, null, null);
                return new ObjectResult(problem) { StatusCode = 500 };
            }
        }
    }
}