using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcAuthorization
{
    public class AuthorizeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IAuthorizationProvider authorizationProvider = AuthorizationProvider.ResolveType<IAuthorizationProvider>();

            // If there's none defined through the type resolver use the configuration provider as the default
            if (authorizationProvider == null)
            {
                authorizationProvider = ConfigurationAuthorizationProvider.Instance;
            }

            if (!authorizationProvider.IsCurrentRequestAuthorized(filterContext))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
