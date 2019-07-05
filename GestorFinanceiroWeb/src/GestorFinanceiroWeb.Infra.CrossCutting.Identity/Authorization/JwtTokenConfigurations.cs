using System;
using System.Collections.Generic;
using System.Text;

namespace GestorFinanceiroWeb.Infra.CrossCutting.Identity.Authorization
{
    // aula19 - 2h06min JwtTokenOptions
    public class JwtTokenConfigurations
    {
        /// <summary>
        /// Emissor
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Pra qual site é válido
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Tempo de duração do token
        /// </summary>
        public int Minutes { get; set; }
    }
}
