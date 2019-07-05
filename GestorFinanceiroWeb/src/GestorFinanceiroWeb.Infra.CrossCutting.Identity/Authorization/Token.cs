using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Infra.CrossCutting.Identity.Authorization
{
    public class Token
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
        public object User { get; set; }
    }
}
