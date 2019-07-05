using AutoMapper;
using GestorFinanceiroWeb.Application.ViewModels;
using GestorFinanceiroWeb.Domain.Eventos.Commands;
using GestorFinanceiroWeb.Domain.Organizadores.Commands;
using System;

namespace GestorFinanceiroWeb.Services.Api.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region Evento
            CreateMap<EventoViewModel, RegistrarEventoCommand>()
                .ConstructUsing(c => new RegistrarEventoCommand(c.Nome, c.DescricaoCurta, c.DescricaoLonga, c.DataInicio, c.DataFim, c.Gratuito, c.Valor, c.Online, c.NomeEmpresa, c.OrganizadorId, c.CategoriaId,
                     new IncluirEnderecoEventoCommand(c.Endereco.Id, c.Endereco.Logradouro, c.Endereco.Numero, c.Endereco.Complemento, c.Endereco.Bairro, c.Endereco.CEP, c.Endereco.Cidade, c.Endereco.Estado, c.Id)));
            CreateMap<EventoViewModel, AtualizarEventoCommand>()
                .ConstructUsing(c => new AtualizarEventoCommand(c.Id, c.Nome, c.DescricaoCurta, c.DescricaoLonga, c.DataInicio, c.DataFim, c.Gratuito, c.Valor, c.Online, c.NomeEmpresa, c.OrganizadorId, c.CategoriaId));

            CreateMap<EventoViewModel, ExcluirEventoCommand>()
                .ConstructUsing(c => new ExcluirEventoCommand(c.Id));

            #region Endereco do Evento
            CreateMap<EnderecoViewModel, IncluirEnderecoEventoCommand>()
                .ConstructUsing(c => new IncluirEnderecoEventoCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.CEP, c.Cidade, c.Estado, c.EventoId));

            CreateMap<EnderecoViewModel, AtualizarEnderecoEventoCommand>()
                .ConstructUsing(c => new AtualizarEnderecoEventoCommand(Guid.NewGuid(), c.Logradouro, c.Numero, c.Complemento, c.Bairro, c.CEP, c.Cidade, c.Estado, c.EventoId));

            #endregion


            #endregion

            #region Organizador
            CreateMap<OrganizadorViewModel, RegistrarOrganizadorCommand>()
                .ConstructUsing(c => new RegistrarOrganizadorCommand(c.Id, c.Nome, c.CPF, c.Email));
            #endregion
        }
    }
}
