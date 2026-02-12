using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TeatroUH.Web.Security
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            var email = session.GetString("USER_EMAIL");
            var role = session.GetString("USER_ROLE");

            // No logueado -> Login
            if (string.IsNullOrWhiteSpace(email))
            {
                context.Result = new RedirectToActionResult("Login", "Cuenta", null);
                return;
            }

            // No admin -> AccessDenied (vista)
            if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ViewResult { ViewName = "AccessDenied" };
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
