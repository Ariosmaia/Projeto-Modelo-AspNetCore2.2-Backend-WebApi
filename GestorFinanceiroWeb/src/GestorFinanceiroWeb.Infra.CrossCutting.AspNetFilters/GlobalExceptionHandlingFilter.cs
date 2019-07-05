using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Hosting;

namespace GestorFinanceiroWeb.Infra.CrossCutting.AspNetFilters
{
    // Gestão dos erros e redirecionar e fazer o logg. 
    public class GlobalExceptionHandlingFilter : IExceptionFilter // Filtro tem o papel de capturar o erro e fazer o logg
    {
        private readonly ILogger<GlobalExceptionHandlingFilter> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalExceptionHandlingFilter(ILogger<GlobalExceptionHandlingFilter> logger, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {

            if (_hostingEnvironment.IsProduction())
            {
                _logger.LogError(1, context.Exception, context.Exception.Message);
            }

            var result = new ViewResult { ViewName = "error" };
            var modelData = new EmptyModelMetadataProvider(); // Criar uma nova instancia do viewdata
            result.ViewData = new ViewDataDictionary(modelData, context.ModelState)
            {
                {"MensagemErro", context.Exception.Message }
            };

            context.ExceptionHandled = true;
            context.Result = result;
        }
    }
}
