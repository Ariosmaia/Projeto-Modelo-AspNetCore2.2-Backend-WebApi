using AutoMapper;
using GestorFinanceiroWeb.Services.Api.AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GestorFinanceiroWeb.Services.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper();

            AutoMapperConfiguration.RegisterMappings();
        }
    }
}
