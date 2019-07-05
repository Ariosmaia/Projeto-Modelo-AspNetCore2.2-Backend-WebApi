using GestorFinanceiroWeb.Domain.Core.Events;
using GestorFinanceiroWeb.Domain.Eventos.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Domain.Eventos.Event
{
    public class EventoEventHandler :
        INotificationHandler<EventoRegistradoEvent>,
        INotificationHandler<EventoAtualizadoEvent>,
        INotificationHandler<EventoExcluidoEvent>,
        INotificationHandler<EnderecoEventoAdicionadoEvent>,
        INotificationHandler<EnderecoEventoAtualizadoEvent>
    {
        public Task Handle(EventoRegistradoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação
            return Task.CompletedTask;
        }

        public Task Handle(EventoAtualizadoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação
            return Task.CompletedTask;
        }

        public Task Handle(EventoExcluidoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação
            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoAdicionadoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação
            return Task.CompletedTask;
        }

        public Task Handle(EnderecoEventoAtualizadoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Disparar alguma ação
            return Task.CompletedTask;
        }
    }
}
