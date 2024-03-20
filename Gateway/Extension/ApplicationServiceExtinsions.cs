using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Application.Core;
using Persistence;
using Application.ThirdPartyChannel;
using System.Reflection;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using SoapCore;
using System.ServiceModel;
namespace Gateway.Extension
{
    public static class ApplicationServiceExtinsions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture("en-US");
            //    // Other globalization settings...
            //});
            //services.Configure<RequestLocalizationOptions>(options =>
            //{
            //    options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
            //    // Other globalization settings...
            //});
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // Add service for List Handler
            //services.AddMediatR(typeof(List.Handler));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TransactionList.Handler).GetTypeInfo().Assembly));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
                });
            });
            //services.AddMvc(options =>
            //{
            //    options.OutputFormatters.Add(new XmlSerializerOutputFormatterNamespace());
            //}).AddXmlSerializerFormatters();
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true; // Ensure synchronous IO is enabled
            });
            // Inside Configure method

            return services;
        }
    }
}
