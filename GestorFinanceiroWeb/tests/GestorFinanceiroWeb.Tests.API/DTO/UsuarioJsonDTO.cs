using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Tests.API.DTO
{
    public class UsuarioJsonDTO
    {
        public bool success { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public bool authenticated { get; set; }
        public string created { get; set; }
        public string expiration { get; set; }
        public string accessToken { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public Claim[] claims { get; set; }
    }

    public class Claim
    {
        public string type { get; set; }
        public string value { get; set; }
    }

}
