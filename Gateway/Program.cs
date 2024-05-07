using Gateway.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Serilog;
using SOAP.Mvc.DependencyInjection;
using System.Text;
using CustomFormattersSample.Formatters;
using Microsoft.AspNetCore.HttpOverrides;
using Gateway.MiddleWare;



var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(builder.Configuration)
             .CreateBootstrapLogger();
builder.Host.UseSerilog();

try
{
    Log.Information("Starting Web Host");


    builder.Host.UseSerilog((hostContext, services, configuration) =>
    {
        configuration.ReadFrom.Configuration(builder.Configuration);
        
    });
    builder.Services.AddControllers();
    builder.Services.AddControllers(options =>
    {
        //options.ModelBinderProviders.Insert(0, new SOAPRequestBodyModelBinderProvider());
        //options.ModelBinderProviders.Insert(0, new SOAPRequestEnvelopeModelBinderProvider());
        //options.ModelBinderProviders.Insert(
        //        0, new QueryStringNullOrEmptyModelBinderProvider());
        
        var tmp = new Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter();
        tmp.SupportedEncodings.Remove(Encoding.UTF8);
        //tmp.SupportedEncodings.Remove(Encoding.ASCII);
        //tmp.SupportedEncodings.Remove(Encoding.Latin1);
        options.OutputFormatters.Add(tmp);
        options.InputFormatters.Insert(0, new VcardInputFormatter(options));
        //options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
        //options.InputFormatters.Add(new XmlSerializerInputFormatter(new MvcOptions(),Encoding.Unicode));


    });

    //.AddXmlSerializerFormatters();
    //builder.Services.AddControllers(options =>
    //{
    //    // Configure XML formatter
    //    options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
    //    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter(new XmlWriterSettings
    //    {
    //        Encoding = Encoding.Unicode,// Set the desired encoding (UTF-16 Little Endian)
    //    }));

    //}).AddXmlDataContractSerializerFormatters();

    //builder.Services.AddControllers(options =>
    //{
    //    //options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>(); // Remove default JSON formatter
    //    options.OutputFormatters.Add(new XmlSerializerOutputFormatterNamespace(new XmlWriterSettings { OmitXmlDeclaration = false }));
    //});

    builder.Services.Configure<MvcOptions>(options =>
    {
        
        var xmlWriterSettings = options.OutputFormatters
            .OfType<Microsoft.AspNetCore.Mvc.Formatters.XmlSerializerOutputFormatter>()
            .Single()
            .WriterSettings;
        xmlWriterSettings.Encoding = Encoding.UTF8;
        xmlWriterSettings.OmitXmlDeclaration = false;
        //xmlWriterSettings.Indent = true;
        //xmlWriterSettings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
        
        
    });
    //builder.Services.AddControllers(options =>
    //{
    //    options.OutputFormatters.Insert(0, new NoNamespaceXmlOutputFormatter());
    //});
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddIdentityService(builder.Configuration);
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    //builder.Services.AddSoapCore();
    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.InvalidModelStateResponseFactory = ApiBehaviorOptionsExtensions.InvalidModelStateResponseFactory;
    });
    //builder.Services.AddAuthentication().AddSOAP_FileDataStore();
    var app = builder.Build();
    //app.UseMiddleware<RequestLoggerMiddleware>();
    app.UseMiddleware<ResponseLoggingMiddleware>();
    app.UseMiddleware<AdminSafeListMiddleware>();
    //app.UseMiddleware<ResponseLoggingMiddleware2>();
    
    app.UseSerilogRequestLogging();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseSwagger();
        //app.UseSwaggerUI();
    }
    
    app.UseCors("CorsPolicy");
    app.UseRouting();
    //app.UseEndpoints(endpoints =>
    //{
    //    _ = endpoints.UseSoapEndpoint<ChannelController>("/Channel/Check.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
    //});

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();

    }
    catch (Exception ex)
    {

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }

    //app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    //app.UseCustomMiddleware();

    app.MapControllers();
    

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

