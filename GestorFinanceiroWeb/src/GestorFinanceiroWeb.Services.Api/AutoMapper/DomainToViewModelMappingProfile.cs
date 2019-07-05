using AutoMapper;
using GestorFinanceiroWeb.Application.ViewModels;
using GestorFinanceiroWeb.Domain.Eventos;
using GestorFinanceiroWeb.Domain.Organizadores;

namespace GestorFinanceiroWeb.Services.Api.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {

            CreateMap<Evento, EventoViewModel>();
            CreateMap<Endereco, EnderecoViewModel>();
            CreateMap<Categoria, CategoriaViewModel>();
            CreateMap<Organizador, OrganizadorViewModel>();
        }
    }
}
