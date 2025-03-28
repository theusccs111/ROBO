using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ROBO.Dominio.Exceptions;
using ROBO.Dominio.Extensions;
using ROBO.Dominio.Resource.Base;
using System.Net;

namespace ROBO.Web.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var resultado = new Resultado<object>();
            if (context.Exception is ValidationException)
            {
                var validationException = (ValidationException)context.Exception;
                string[] failures = validationException.Failures.ConvertDictionaryToArray();

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                resultado = new Resultado<object>
                {
                    Success = false,
                    Message = validationException.Message,
                    MessageDetail = string.Join(";", failures),
                    Data = null
                };

                context.Result = new JsonResult(resultado);
                return;
            }

            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;

            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;

            resultado = new Resultado<object>
            {
                Success = false,
                Message = context.Exception.Message,
                //MessageDetail = exception.InnerException?.Message,
                Data = null
            };

            context.Result = new JsonResult(resultado);
        }
    }
}
