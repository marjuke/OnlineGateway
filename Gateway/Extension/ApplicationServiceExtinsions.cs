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
using Gateway.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TransactionList.Handler).GetTypeInfo().Assembly));
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:4200");
                });
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true; // Ensure synchronous IO is enabled
            });
            services.AddScoped<TokenService>();
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4Zs6r0AEnoLgN0GaRirrdL8RLUvwume4xQGBLjl3DKBTYwYnum1etLV2kjE7"));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("4Zs6r0AEnoLgN0GaRirrdL8RLUvwume4xQGBLjl3DKBTYwYnum1etLV2kjE7dsfdsfdfdsfdsfewrfdfDFSDFASDDdsfdsfewr23432adadsdsadfdsfdsfdsfdsfdsfadsfdsfdsafafafasdfsfewrewrdsfadsfadsfads"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            return services;
        }
    }
}
