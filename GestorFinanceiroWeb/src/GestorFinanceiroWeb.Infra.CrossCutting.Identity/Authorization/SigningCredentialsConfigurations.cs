using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Infra.CrossCutting.Identity.Authorization
{
    public class SigningCredentialsConfigurations
    {
        private const string SecretKey = "eventosio@meuambienteToken";

        public SymmetricSecurityKey Key { get; private set; }

        public SigningCredentials SigningCredentials { get; private set; }

        public SigningCredentialsConfigurations()
        {
            Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
