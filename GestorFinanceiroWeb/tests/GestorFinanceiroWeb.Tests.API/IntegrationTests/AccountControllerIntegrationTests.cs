using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Models.AccountViewModels;
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
    public class AccountControllerIntegrationTests
    {
        public AccountControllerIntegrationTests()
        {
            Environment.CriarServidor();
        }

        [Fact]
        public async Task AccountController_RegistrarNovoOrganizador_RetornarComSucesso()
        {
            // Arrange
            var user = new RegisterViewModel
            {
                Nome = "Allan Rios",
                CPF = "17385989950",
                Email = "arios@maia10.com.br",
                Senha = "Abc123@.",
                SenhaConfirmacao = "Abc123@."
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await Environment.Client.PostAsync("api/v1/nova-conta", postContent);

            var userResult = JsonConvert.DeserializeObject<UsuarioJsonDTO>(await response.Content.ReadAsStringAsync());

            // Assert
            response.EnsureSuccessStatusCode();
            var token = userResult.data.accessToken;
            Assert.NotEmpty(token);
        }
    }
}
