using Domain.Identity;
using Persistence;

namespace Gateway.Extension
{
    public static class IdentityServiceExtensios
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection services,IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false ;
                
                
            }).AddEntityFrameworkStores<DataContext>();
            services.AddAuthentication();
            return services;
        }
    }
}
