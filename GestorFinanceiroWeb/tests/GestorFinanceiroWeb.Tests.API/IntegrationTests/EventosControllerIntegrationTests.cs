using GestorFinanceiroWeb.Application.ViewModels;
using GestorFinanceiroWeb.Tests.API.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GestorFinanceiroWeb.Tests.API.IntegrationTests
{
    public class EventosControllerIntegrationTests
    {
        public EventosControllerIntegrationTests()
        {
            Environment.CriarServidor();
        }

        [Fact]
        public async Task EventosController_ObterListaEventos_RetornarComSucesso()
        {
            // Arrange & Act
            var response = await Environment.Client.GetAsync("api/v1/eventos");
            var responseEvento = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(responseEvento);
        }

        [Fact]
        public async Task EventosController_ObterMeusEventos_RetornarComSucesso()
        {
            // Arrange 
            var user = await UserUtils.RealizarLoginOrganizador(Environment.Client);

            // Act
            var response = await Environment.Server
                .CreateRequest("api/v1/eventos/meus-eventos")
                .AddHeader("Content-Type", "application/json")
                .AddHeader("Authorization", "Bearer " + user.accessToken)
                .GetAsync();

            var responseEvento = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(responseEvento);
        }

        [Fact(DisplayName = "Evento registrado com sucesso")]
        //[Trait("Category", "Testes de integração API")]
        public async Task EventosController_RegistrarNovoEvento_RetornarComSucesso()
        {
            // Arrange
            var login = await UserUtils.RealizarLoginOrganizador(Environment.Client);

            var evento = new EventoViewModel
            {
                Nome = "DevXperience",
                DescricaoCurta = "Um evento de tecnologia",
                DescricaoLonga = "Um evento de tecnologia",
                CategoriaId = new Guid("ac381ba8-c187-482c-a5cb-899ad7176137"),
                DataInicio = DateTime.Now.AddDays(1),
                DataFim = DateTime.Now.AddDays(2),
                Gratuito = false,
                Valor = 500,
                NomeEmpresa = "DevX",
                Online = true,
                Endereco = new EnderecoViewModel(),
                OrganizadorId = new Guid(login.user.id)
            };

            // Act
            var response = await Environment.Server
                .CreateRequest("api/v1/eventos")
                .AddHeader("Authorization", "Bearer " + login.accessToken)
                .And(
                    request =>
                        request.Content =
                            new StringContent(JsonConvert.SerializeObject(evento), Encoding.UTF8, "application/json"))
                //.And(request => request.Method = HttpMethod.Put)
                .PostAsync();

            //var conteudo = response.Content.ReadAsStringAsync().Result;

            var eventoResult = JsonConvert.DeserializeObject<EventoJsonDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.IsType<EventoDTO>(eventoResult.data);
        }
    }
}
