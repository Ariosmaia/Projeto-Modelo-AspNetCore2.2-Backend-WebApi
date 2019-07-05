using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Models.AccountViewModels;
using GestorFinanceiroWeb.Tests.API.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Tests.API.IntegrationTests
{
    public class UserUtils
    {
        public static async Task<Data> RealizarLoginOrganizador(HttpClient client)
        {
            var user = new LoginViewModel
            {
                Email = "arios@maia8.com.br",
                Senha = "Abc123@.",
                RememberMe = false
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/v1/conta", postContent);

            var postResult = await response.Content.ReadAsStringAsync();
            var userResult = JsonConvert.DeserializeObject<UsuarioJsonDTO>(postResult);

            return userResult.data;
        }
    }
}
