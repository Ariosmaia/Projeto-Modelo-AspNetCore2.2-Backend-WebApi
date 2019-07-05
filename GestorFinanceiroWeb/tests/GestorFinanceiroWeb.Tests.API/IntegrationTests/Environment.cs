using GestorFinanceiroWeb.Services.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

namespace GestorFinanceiroWeb.Tests.API.IntegrationTests
{
    public class Environment
    {
        public static TestServer Server;
        public static HttpClient Client;

        public static void CriarServidor()
        {

            Server = new TestServer(
                new WebHostBuilder()
                    .UseEnvironment("Testing")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseUrls("http://localhost:8285")
                    .UseStartup<StartupTests>());

            Client = Server.CreateClient();
        }
    }
}
