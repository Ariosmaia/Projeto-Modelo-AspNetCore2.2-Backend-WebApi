using GestorFinanceiroWeb.Domain.Core.Events;
using GestorFinanceiroWeb.Domain.Core.Notifications;
using GestorFinanceiroWeb.Domain.Eventos.Events;
using GestorFinanceiroWeb.Domain.Eventos.Repository;
using GestorFinanceiroWeb.Domain.Handlers;
using GestorFinanceiroWeb.Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Domain.Eventos.Commands
{
    public class EventoCommandHandler : CommandHandler,
        IRequestHandler<RegistrarEventoCommand, bool>,
        IRequestHandler<AtualizarEventoCommand, bool>,
        IRequestHandler<ExcluirEventoCommand, bool>,
        IRequestHandler<IncluirEnderecoEventoCommand, bool>,
        IRequestHandler<AtualizarEnderecoEventoCommand, bool>

    {
        private readonly IEventoRepository _eventoRepository;
        private readonly IUser _user;
        private readonly IMediatorHandler _mediator;

        public EventoCommandHandler(IEventoRepository eventoRepository,
                                    IUnitOfWork uow,
                                    INotificationHandler<DomainNotification> notifications,
                                    IUser user,
                                    IMediatorHandler mediator) : base(uow, mediator, notifications)
        {
            _eventoRepository = eventoRepository;
            _user = user;
            _mediator = mediator;
        }

        public Task<bool> Handle(RegistrarEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Endereco.Id, message.Endereco.Logradouro, message.Endereco.Numero, message.Endereco.Complemento, message.Endereco.Bairro, message.Endereco.CEP, message.Endereco.Cidade, message.Endereco.Estado, message.Endereco.EventoId.Value);
            var evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
                message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, endereco, message.CategoriaId);

            if (!EventoValido(evento)) return Task.FromResult(false);

            // TODO:
            // Validacoes de negocio!
            // Organizador pode registrar evento?

            _eventoRepository.Adicionar(evento);

            if (Commit())
            {
                _mediator.PublicarEvento(new EventoRegistradoEvent(evento.Id, evento.Nome, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(AtualizarEventoCommand message, CancellationToken cancellationToken)
        {
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (!EventoExistente(message.Id, message.MessageType)) return Task.FromResult(false);

            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Evento não pertencente ao Organizador"));
                return Task.FromResult(false);
            }

            var evento = Evento.EventoFactory.NovoEventoCompleto(message.Id, message.Nome, message.DescricaoCurta,
                message.DescricaoLonga, message.DataInicio, message.DataFim, message.Gratuito, message.Valor,
                message.Online, message.NomeEmpresa, message.OrganizadorId, eventoAtual.Endereco, message.CategoriaId);

            if (!evento.Online && evento.Endereco == null)
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Não é possivel atualizar um evento sem informar o endereço"));
                return Task.FromResult(false);
            }

            if (!EventoValido(evento)) return Task.FromResult(false);

            _eventoRepository.Atualizar(evento);

            if (Commit())
            {
                _mediator.PublicarEvento(new EventoAtualizadoEvent(evento.Id, evento.Nome, evento.DescricaoCurta, evento.DescricaoLonga, evento.DataInicio, evento.DataFim, evento.Gratuito, evento.Valor, evento.Online, evento.NomeEmpresa));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(ExcluirEventoCommand message, CancellationToken cancellationToken)
        {
            if (!EventoExistente(message.Id, message.MessageType)) return Task.FromResult(false);
            var eventoAtual = _eventoRepository.ObterPorId(message.Id);

            if (eventoAtual.OrganizadorId != _user.GetUserId())
            {
                _mediator.PublicarEvento(new DomainNotification(message.MessageType, "Evento não pertencente ao Organizador"));
                return Task.FromResult(false);
            }

            // Validacoes de negocio
            eventoAtual.ExcluirEvento();

            _eventoRepository.Atualizar(eventoAtual);

            if (Commit())
            {
                _mediator.PublicarEvento(new EventoExcluidoEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(IncluirEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.EventoId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return Task.FromResult(false);
            }

            var evento = _eventoRepository.ObterPorId(message.EventoId.Value);
            evento.TornarPresencial();

            _eventoRepository.Atualizar(evento);
            _eventoRepository.AdicionarEndereco(endereco);

            if (Commit())
            {
                _mediator.PublicarEvento(new EnderecoEventoAdicionadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(AtualizarEnderecoEventoCommand message, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(message.Id, message.Logradouro, message.Numero, message.Complemento, message.Bairro, message.CEP, message.Cidade, message.Estado, message.EventoId.Value);
            if (!endereco.EhValido())
            {
                NotificarValidacoesErro(endereco.ValidationResult);
                return Task.FromResult(false);
            }

            _eventoRepository.AtualizarEndereco(endereco);

            if (Commit())
            {
                _mediator.PublicarEvento(new EnderecoEventoAtualizadoEvent(endereco.Id, endereco.Logradouro, endereco.Numero, endereco.Complemento, endereco.Bairro, endereco.CEP, endereco.Cidade, endereco.Estado, endereco.EventoId.Value));
            }

            return Task.FromResult(true);
        }

        private bool EventoValido(Evento evento)
        {
            if (evento.EhValido()) return true;

            NotificarValidacoesErro(evento.ValidationResult);
            return false;
        }

        private bool EventoExistente(Guid id, string messageType)
        {
            var evento = _eventoRepository.ObterPorId(id);

            if (evento != null) return true;

            _mediator.PublicarEvento(new DomainNotification(messageType, "Evento não encontrado."));
            return false;
        }

    }
}
