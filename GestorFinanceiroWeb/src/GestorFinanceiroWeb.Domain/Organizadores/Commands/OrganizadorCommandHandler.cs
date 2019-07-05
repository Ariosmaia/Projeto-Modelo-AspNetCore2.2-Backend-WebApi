using GestorFinanceiroWeb.Domain.Core.Events;
using GestorFinanceiroWeb.Domain.Core.Notifications;
using GestorFinanceiroWeb.Domain.Handlers;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Domain.Organizadores.Events;
using GestorFinanceiroWeb.Domain.Organizadores.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Domain.Organizadores.Commands
{
    public class OrganizadorCommandHandler : CommandHandler,
        IRequestHandler<RegistrarOrganizadorCommand, bool>
    {
        private readonly IMediatorHandler _mediator;
        private readonly IOrganizadorRepository _organizadorRepository;

        public OrganizadorCommandHandler(
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications,
            IOrganizadorRepository organizadorRepository, IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _organizadorRepository = organizadorRepository;
            _mediator = mediator;
        }

        public Task<bool> Handle(RegistrarOrganizadorCommand message, CancellationToken cancellationToken)
        {
            var organizador = new Organizador(message.Id, message.Nome, message.CPF, message.Email);

            if (!organizador.EhValido())
            {
                NotificarValidacoesErro(organizador.ValidationResult);
                return Task.FromResult(false);
            }

            var organizadorExistente = _organizadorRepository.Buscar(o => o.CPF == organizador.CPF || o.Email == organizador.Email);

            if (organizadorExistente.Any())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "CPF ou e-mail já utilizados"));
            }

            _organizadorRepository.Adicionar(organizador);

            if (Commit())
            {
                _mediator.PublicarEvento(new OrganizadorRegistradoEvent(organizador.Id, organizador.Nome, organizador.CPF, organizador.Email));
            }

            return Task.FromResult(true);
        }
    }
}
