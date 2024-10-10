using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestWeb.Filters
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SessionTimeoutAttribute));
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];

            log.Debug("Session ID: " + session.Id);

            if (controller.Equals("Account") && action.Equals("Signin"))
            {
                return;
            }

            if (session.IsAvailable)
            {
                if(session.GetInt32("User_session") == null)
                {
                    // signout
                    context.HttpContext.SignOutAsync();
                    // Redirect To Signin page
                    context.Result = new RedirectToActionResult("Signin", "Account", null);
                }
                else
                {
                    // Update session time
                    session.SetInt32("User_session", session.GetInt32("User_session").Value);
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
