using GestorFinanceiroWeb.Domain.Core.Notifications;
using GestorFinanceiroWeb.Domain.Eventos.Commands;
using GestorFinanceiroWeb.Domain.Eventos.Event;
using GestorFinanceiroWeb.Domain.Eventos.Events;
using GestorFinanceiroWeb.Domain.Eventos.Repository;
using GestorFinanceiroWeb.Domain.Handlers;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Domain.Organizadores.Commands;
using GestorFinanceiroWeb.Domain.Organizadores.Events;
using GestorFinanceiroWeb.Domain.Organizadores.Repository;
using GestorFinanceiroWeb.Infra.CrossCutting.AspNetFilters;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Models;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Services;
using GestorFinanceiroWeb.Infra.Data.Context;
using GestorFinanceiroWeb.Infra.Data.EventSourcing;
using GestorFinanceiroWeb.Infra.Data.Repository;
using GestorFinanceiroWeb.Infra.Data.Repository.EventSourcing;
using GestorFinanceiroWeb.Infra.Data.UoW;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace GestorFinanceiroWeb.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {

            // ASPNET
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            // Domain - Commands
            services.AddScoped<IRequestHandler<RegistrarEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<ExcluirEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarEnderecoEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<IncluirEnderecoEventoCommand, bool>, EventoCommandHandler>();
            services.AddScoped<IRequestHandler<RegistrarOrganizadorCommand, bool>, OrganizadorCommandHandler>();

            // Domain - Eventos
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<EventoRegistradoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EventoExcluidoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EnderecoEventoAtualizadoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<EnderecoEventoAdicionadoEvent>, EventoEventHandler>();
            services.AddScoped<INotificationHandler<OrganizadorRegistradoEvent>, OrganizadorEventHandler>();

            // Infra - Data
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IOrganizadorRepository, OrganizadorRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<EventosContext>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSQLContext>();

            // Infra - Identity
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddScoped<IUser, AspNetUser>();

            //Infra - Filtros
            services.AddScoped<ILogger<GlobalExceptionHandlingFilter>, Logger<GlobalExceptionHandlingFilter>>();
            services.AddScoped<ILogger<GlobalActionLogger>, Logger<GlobalActionLogger>>();
            services.AddScoped<GlobalExceptionHandlingFilter>();
            services.AddScoped<GlobalActionLogger>();


        }
    }
}
