using Microsoft.AspNetCore.Authentication;
using SOAP.Authentication.DataStores.File;
using SOAP.Authorization;
namespace SOAP.Authentication.Handlers.SOAP;
public static class SOAPAuthenticationBuilderExtensions
{
    public static AuthenticationBuilder AddSOAP_FileDataStore(this AuthenticationBuilder builder, Action<SOAPAuthenticationHandlerOptions>? configureFileAuthenticationHandlerOptions = null, Action<PasswordEncoderOptions>? configurePasswordEncoderOptions = null)
        => builder.AddSOAP_FileDataStore(SOAPAuthenticationDefaults.AuthenticationScheme, configureFileAuthenticationHandlerOptions, configurePasswordEncoderOptions);
    public static AuthenticationBuilder AddSOAP_FileDataStore(this AuthenticationBuilder builder, string authenticationScheme, Action<SOAPAuthenticationHandlerOptions>? configureFileAuthenticationHandlerOptions = null, Action<PasswordEncoderOptions>? configurePasswordEncoderOptions = null)
        => builder.AddSOAP_FileDataStore(authenticationScheme, null, configureFileAuthenticationHandlerOptions, configurePasswordEncoderOptions);
    public static AuthenticationBuilder AddSOAP_FileDataStore(this AuthenticationBuilder builder, string authenticationScheme, string? displayName, Action<SOAPAuthenticationHandlerOptions>? configureFileAuthenticationHandlerOptions = null, Action<PasswordEncoderOptions>? configurePasswordEncoderOptions = null)
    {
        builder.AddScheme<SOAPAuthenticationHandlerOptions, SOAPAuthenticationHandler>(authenticationScheme, displayName, configureFileAuthenticationHandlerOptions);
        // Add a Password Encoder
        var encoderOptionsBuilder = builder.Services.AddOptions<PasswordEncoderOptions>();
        if (configurePasswordEncoderOptions is not null)
            encoderOptionsBuilder.Configure(configurePasswordEncoderOptions);
        builder.Services.AddSingleton<IPasswordEncoder, PasswordEncoder>();
        // Add a DataStore (File)
        builder.Services.AddSingleton<IAuthenticationRepository, FileAuthDataStore>();
        builder.Services.AddSingleton<IAuthorizationRepository, FileAuthDataStore>();
        return builder;
    }
}
