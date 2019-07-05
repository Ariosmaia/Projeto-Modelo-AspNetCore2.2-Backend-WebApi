using GestorFinanceiroWeb.Domain.Core.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Domain.Organizadores.Events
{
    public class OrganizadorEventHandler :
        INotificationHandler<OrganizadorRegistradoEvent>
    {
        public Task Handle(OrganizadorRegistradoEvent message, CancellationToken cancellationToken)
        {
            // TODO: Enviar um email?
            return Task.CompletedTask;
        }
    }
}
