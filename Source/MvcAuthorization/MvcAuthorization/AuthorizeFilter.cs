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
             IAuthorizationProvider authorizationProvider = DependencyResolver.Current.GetService<IAuthorizationProvider>();
             
             // If there's none defined through dependency resolver use the configuration provider as the default
             if (authorizationProvider == null)
             {
                 authorizationProvider = ConfigurationAuthorizationProvider.Instance;
             }

            string area = filterContext.RouteData.DataTokens["area"] as string;

            if (!authorizationProvider.IsAuthorizedController(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, area) || !authorizationProvider.IsAuthorizedAction(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName, area))
             {
                 filterContext.Result = new HttpUnauthorizedResult();
             }

            base.OnActionExecuting(filterContext);
        }
    }
}
