using AutoMapper;
using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Infra.CrossCutting.AspNetFilters;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Data;
using GestorFinanceiroWeb.Services.Api.Configurations;
using GestorFinanceiroWeb.Services.Api.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using static GestorFinanceiroWeb.Services.Api.Configurations.MvcConfiguration;

namespace GestorFinanceiroWeb.Services.Api
{
    public class StartupTests
    {
        public IConfiguration Configuration { get; }

        public StartupTests(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
           
            // Contexto do EF para o Identity
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionTeste")));

            // Configurações de Autenticação, Autorização e JWT.
            services.AddMvcSecurity(Configuration);

            // Options para configurações customizadas
            services.AddOptions();

            // Configurações Elmah
            services.AddElmahIo(o =>
            {
                o.ApiKey = "67352fb2dbd047e488354d7d74a10a1b";
                o.LogId = new Guid("2fabadd6-1d95-43a0-a584-eb8dcb324c36");
            });

            // MVC com restrição de XML e adição de filtro de ações.
            services.AddMvc(options => 
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
                options.Filters.Add(new ServiceFilterAttribute(typeof(GlobalActionLogger)));

            }).AddJsonOptions(options =>
            {
                // remove valores nulos do retorno da API
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Versionamento do WebApi
            services.AddApiVersioning("api/v{version}");

            // AutoMapper
            var assembly = typeof(Program).GetTypeInfo().Assembly;
            services.AddAutoMapper(assembly);

            // Configuração Swagger
            services.AddSwaggerConfig();

            services.AddHttpContextAccessor();

            // MediatR
            services.AddMediatR(typeof(Startup));

            // Registrar todos DI
            services.AddDIConfig();
        }

        public void Configure(IApplicationBuilder app, 
                              IHostingEnvironment env,
                              IHttpContextAccessor accessor,
                              ILoggerFactory loggerFactory)
        {


            #region Configurações MVC

            var elmahSts = new ElmahIoProviderOptions
            {
                OnMessage = message =>
                {
                    message.Version = "v1.0";
                    message.Application = "Eventos.IO";
                    message.Hostname = "http://localhost";
                },
            };
            loggerFactory.AddElmahIo("67352fb2dbd047e488354d7d74a10a1b", new Guid("2fabadd6-1d95-43a0-a584-eb8dcb324c36"), elmahSts);
            app.UseElmahIo();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

            #endregion

            #region Swagger
            if (env.IsProduction())
            {
                // bloqueia o acesso a usuários não logados
                app.UseSwaggerAuthorized();
            }
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "GestorWeb API v1.0");
            });
            #endregion

        }
    }
}
