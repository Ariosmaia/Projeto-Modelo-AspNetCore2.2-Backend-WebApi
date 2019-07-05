using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GestorFinanceiroWeb.Services.Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                // Configura detalhes como a versão da API
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Gestor Web API",
                    Description = "API do Gestor Financeiro",
                    TermsOfService = "",
                    Contact = new Contact { Name = "Allan", Email = "allan@propackages.com.br", Url = "http://propackages.com.br" },
                    License = new License { Name = "MIT", Url = "http://propackages.com.br/licensa" }

                });

                //s.AddSecurityDefinition(
                //    "bearer",
                //    new ApiKeyScheme
                //    {
                //        In = "header",
                //        Description = "Autenticação baseada em Json Web Token (JWT)",
                //        Name = "Authorization",
                //        Type = "apiKey"
                //    });

                //s.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
            });
        }
    }
}


