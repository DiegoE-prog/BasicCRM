using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using BasicCRM.Data.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace BasicCRM.Data
{
    public static class DataServiceRegistration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Address>, AddressRepository>();
            services.AddScoped<IRepository<Client>, ClientRepository>();

            return services;
        }
    }
}
