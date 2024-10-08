﻿using System.Text;
using System.Xml.Serialization;
using Gateway.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CustomFormattersSample.Formatters;

// <snippet_Class>
public class VcardInputFormatter : XmlSerializerInputFormatter
{
    public VcardInputFormatter(MvcOptions options) : base(options)
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/xml;charset=utf-16"));

        //SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }
    public override async Task<InputFormatterResult> ReadRequestBodyAsync(
        InputFormatterContext context, Encoding encoding)
    {
        var type = context.ModelType;
        var request = context.HttpContext.Request;

        using (var reader = new StreamReader(request.Body, encoding))
        {
            var content = await reader.ReadToEndAsync();
            var serializer = new XmlSerializer(type);
            using (var stringReader = new StringReader(content))
            {
                var result = serializer.Deserialize(stringReader);
                return await InputFormatterResult.SuccessAsync(result);
            }
        }
    }
    protected override bool CanReadType(Type type)
    {
        // Add any additional checks for supported types
        return true;
    }
    // <snippet_ctor>
    //public VcardInputFormatter()
    //{
    //    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

    //    SupportedEncodings.Add(Encoding.UTF8);
    //    SupportedEncodings.Add(Encoding.Unicode);
    //}
    // </snippet_ctor>

    //protected override bool CanReadType(Type type)
    //    => type == typeof(Contact);

    //public override async Task<InputFormatterResult> ReadRequestBodyAsync(
    //    InputFormatterContext context, Encoding effectiveEncoding)
    //{
    //    var httpContext = context.HttpContext;
    //    var serviceProvider = httpContext.RequestServices;

    //    var logger = serviceProvider.GetRequiredService<ILogger<VcardInputFormatter>>();

    //    using var reader = new StreamReader(httpContext.Request.Body, effectiveEncoding);
    //    string? nameLine = null;

    //    try
    //    {
    //        await ReadLineAsync("BEGIN:VCARD", reader, context, logger);
    //        await ReadLineAsync("VERSION:", reader, context, logger);

    //        nameLine = await ReadLineAsync("N:", reader, context, logger);

    //        var split = nameLine.Split(";".ToCharArray());
    //        var contact = new Contact(FirstName: split[1], LastName: split[0].Substring(2));

    //        await ReadLineAsync("FN:", reader, context, logger);
    //        await ReadLineAsync("END:VCARD", reader, context, logger);

    //        logger.LogInformation("nameLine = {nameLine}", nameLine);

    //        return await InputFormatterResult.SuccessAsync(contact);
    //    }
    //    catch
    //    {
    //        logger.LogError("Read failed: nameLine = {nameLine}", nameLine);
    //        return await InputFormatterResult.FailureAsync();
    //    }
    //}

    //private static async Task<string> ReadLineAsync(
    //    string expectedText, StreamReader reader, InputFormatterContext context,
    //    ILogger logger)
    //{
    //    var line = await reader.ReadLineAsync();

    //    if (line is null || !line.StartsWith(expectedText))
    //    {
    //        var errorMessage = $"Looked for '{expectedText}' and got '{line}'";

    //        context.ModelState.TryAddModelError(context.ModelName, errorMessage);
    //        logger.LogError(errorMessage);

    //        throw new Exception(errorMessage);
    //    }

    //    return line;
    //}
}
// </snippet_Class>