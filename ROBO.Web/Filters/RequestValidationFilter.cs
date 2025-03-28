using Microsoft.AspNetCore.Mvc.Filters;
using ROBO.Dominio.Exceptions;
using ROBO.Dominio.Extensions;

namespace ROBO.Web.Filters
{
    public class RequestValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                throw new ValidationException("Erro", filterContext.ModelState.ToExceptionMessage());
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
