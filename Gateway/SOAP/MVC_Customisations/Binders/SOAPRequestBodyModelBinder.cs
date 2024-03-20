using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SOAP;
using SOAP.Model;
using SOAP.Mvc.Filters;

// This allows a controller action to receive a SOAPRequestBody rather than a 
// SOAP version specific SOAP?_?RequestEnvelope
public class SOAPRequestBodyModelBinder : IModelBinder
{
    protected static FieldInfo isRequiredFieldInfo = typeof(DefaultModelMetadata).GetField("_isRequired", BindingFlags.Instance | BindingFlags.NonPublic);
    protected Dictionary<Type, (ModelMetadata metaData, IModelBinder binder)> _binders;
    protected (ModelMetadata metaData, IModelBinder binder) _target;
    public SOAPRequestBodyModelBinder(Dictionary<Type, (ModelMetadata metaData, IModelBinder binder)> binders, (ModelMetadata metaData, IModelBinder binder) target)
    {
        _binders = binders;
        _target = target;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        SOAPControllerAttribute soapAttribute = bindingContext.ActionContext.ActionDescriptor.EndpointMetadata.OfType<SOAPControllerAttribute>().FirstOrDefault();
        if (soapAttribute is null)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            return;
        }
        Type requestModelType = soapAttribute.SOAPVersion == SOAPVersion.v1_1 ? typeof(SOAP1_1RequestEnvelope) : typeof(SOAP1_2RequestEnvelope);
        (ModelMetadata metaData, IModelBinder binder) modelClasses = _binders[requestModelType];
        ModelMetadata requestModelMetaData = modelClasses.metaData;
        IModelBinder requestModelBinder = modelClasses.binder;
        var requestBindingContext = DefaultModelBindingContext.CreateBindingContext(
            bindingContext.ActionContext,
            bindingContext.ValueProvider,
            requestModelMetaData,
            bindingInfo: null,
            bindingContext.ModelName);
        await requestModelBinder.BindModelAsync(requestBindingContext);
        if (!requestBindingContext.Result.IsModelSet || requestBindingContext.Result.Model is null)
        {
            if (bindingContext.ModelMetadata.IsRequired)
            {
                // if the method is protected by a PayloadRequiredAttribute
                // Then we can set IsRequired to false to bypass 'Required' model Validation as the PayloadRequiredAttribute will capture the missing payload
                PayloadRequiredAttribute payloadRequiredAttribute = bindingContext.ActionContext.ActionDescriptor.EndpointMetadata.OfType<PayloadRequiredAttribute>().FirstOrDefault();
                if (payloadRequiredAttribute is not null)
                    isRequiredFieldInfo?.SetValue(bindingContext.ModelMetadata, false);
            }
        }
        bindingContext.Result = requestBindingContext.Result;
        if (requestBindingContext.Result.IsModelSet && requestBindingContext.Result.Model is not null)
        {
            var model = ((SOAPRequestEnvelope)requestBindingContext.Result.Model).Body;
            bindingContext.Result = ModelBindingResult.Success(model);
            if (model is not null)
            {
                // Setting the ValidationState ensures properties on derived types are correctly 
                bindingContext.ValidationState[model] = new ValidationStateEntry
                {
                    Metadata = _target.metaData,
                };
            }
        }
        return;
    }
}

class SOAPRequestBodyModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType != typeof(SOAPRequestBody))
            return null;
        var subclasses = new[] { typeof(SOAP1_1RequestEnvelope), typeof(SOAP1_2RequestEnvelope) };
        var binders = new Dictionary<Type, (ModelMetadata, IModelBinder)>();
        foreach (var type in subclasses)
        {
            var modelMetadata = context.MetadataProvider.GetMetadataForType(type);
            binders[type] = (modelMetadata, context.CreateBinder(modelMetadata, new BindingInfo() { BindingSource = BindingSource.Body, EmptyBodyBehavior = EmptyBodyBehavior.Allow }));
        }
        var targetmodelMetadata = context.MetadataProvider.GetMetadataForType(typeof(SOAPRequestBody));
        (ModelMetadata, IModelBinder) target = (targetmodelMetadata, context.CreateBinder(targetmodelMetadata, new BindingInfo() { BindingSource = BindingSource.Body, EmptyBodyBehavior = EmptyBodyBehavior.Allow }));
        return new SOAPRequestBodyModelBinder(binders, target);

    }
}