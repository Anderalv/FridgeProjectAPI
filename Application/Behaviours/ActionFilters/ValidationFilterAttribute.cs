using Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Behaviours.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter {
        private readonly ILoggerManager _logger;
        public ValidationFilterAttribute(ILoggerManager logger) {
            _logger = logger; 
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}