using GestorFinanceiroWeb.Domain.Core.Notifications;
using GestorFinanceiroWeb.Domain.Interfaces;
using GestorFinanceiroWeb.Domain.Organizadores.Commands;
using GestorFinanceiroWeb.Domain.Organizadores.Repository;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Authorization;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Models;
using GestorFinanceiroWeb.Infra.CrossCutting.Identity.Models.AccountViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GestorFinanceiroWeb.Services.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly ILogger _logger;
        private readonly IMediatorHandler _mediator;

        private readonly JwtTokenConfigurations _jwtTokenConfigurations;
        private readonly SigningCredentialsConfigurations _signingConfigurations;
        private readonly IOrganizadorRepository _organizadorRepository;



        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signManager,
                                 INotificationHandler<DomainNotification> notifications,
                                 ILoggerFactory loggerFactory,
                                 [FromServices]JwtTokenConfigurations jwtTokenConfigurations,
                                 [FromServices]SigningCredentialsConfigurations signingConfigurations,
                                 IMediatorHandler mediator,
                                 IOrganizadorRepository organizadorRepository,
                                 IUser user) : base(notifications, user, mediator)
        {
            _userManager = userManager;
            _signManager = signManager;
            _mediator = mediator;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _organizadorRepository = organizadorRepository;

            _jwtTokenConfigurations = jwtTokenConfigurations;
            _signingConfigurations = signingConfigurations;

        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


        [HttpPost]
        [AllowAnonymous]
        [Route("nova-conta")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model, int version)
        {
            if(version == 2) return Response(new { Message = "API V2 não disponível" });
            

            if (!ModelState.IsValid)
            {
                NotificarErroModelInvalida();
                return Response();
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("Eventos", "Ler"));
                await _userManager.AddClaimAsync(user, new Claim("Eventos", "Gravar"));

                var registroCommand = new RegistrarOrganizadorCommand(Guid.Parse(user.Id), model.Nome, model.CPF, user.Email);
                await _mediator.EnviarComando(registroCommand);

                if (!OperacaoValida())
                {
                    await _userManager.DeleteAsync(user);
                    return Response(model);
                }

                _logger.LogInformation(1, "Usuário criado com sucesso!");
                return Response(await GerarTokenUsuario(new LoginViewModel { Email = model.Email, Senha = model.Senha }));
            }

            AdicionarErrosIdentity(result);
            return Response(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("conta")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                NotificarErroModelInvalida();
                return Response(model);
            } 

            var result = await _signManager.PasswordSignInAsync(model.Email, model.Senha, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation(1, "Usuário logado com sucesso!");
                return Response(await GerarTokenUsuario(model));
            }

            NotificarErro(result.ToString(), "Falha ao realizar o login");
            return Response(model);
        }

        private async Task<object> GerarTokenUsuario(LoginViewModel login)
        {
            // Recupera os dados do usuário gravados no banco de dados
            var user = await _userManager.FindByEmailAsync(login.Email);

            // Recupera as claims do usuário gravadas no banco de dados
            var userClaims = await _userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Gera o objeto IdentityClaims (necessário)
            var identityClaims = new ClaimsIdentity(new GenericIdentity(user.Email, "Login"), userClaims);

            // Datas criação/expiração do token
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao.AddMinutes(_jwtTokenConfigurations.Minutes);


            // Gera o token
            var handler = new JwtSecurityTokenHandler();


            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtTokenConfigurations.Issuer,
                Audience = _jwtTokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identityClaims,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });


            // Transforma o token em string
            var encodedToken = handler.WriteToken(token);

            // Dados do Usuário
            var orgUser = _organizadorRepository.ObterPorId(Guid.Parse(user.Id));

            // Objeto de resposta
            return new Token
            {
                Authenticated = true,
                AccessToken = encodedToken,
                Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = "Token de validação",
                User = new
                {
                    id = user.Id,
                    nome = orgUser.Nome,
                    email = orgUser.Email,
                    claims = userClaims.Select(c => new { c.Type, c.Value })
                }
            };
        }
    }
}