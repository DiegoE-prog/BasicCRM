using BasicCRM.Business.Services.Interfaces;
using BasicCRM.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BasicCRM.Business
{
    public static class BussinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAddressService, AddressService>();

            return services;
        }
    }
}
