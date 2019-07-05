using GestorFinanceiroWeb.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace GestorFinanceiroWeb.Services.Api.Middlewares
{
    public class SwaggerMiddleare
    {
        private readonly RequestDelegate _next;
        private readonly IUser _user;

        public SwaggerMiddleare(RequestDelegate next, IUser user)
        {
            _next = next;
            _user = user;
        }

        public async Task Invoke(HttpContext context)
        {
            // Validações
            if (context.Request.Path.StartsWithSegments("/swagger")
                && !_user.IsAuthenticated())
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            // Invoca o próximo middleare registrado no pipeline
            await _next.Invoke(context);
        }
    }

    public static class SwaggerMiddleareExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerMiddleare>();
        }
    }
}
