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

             if (!authorizationProvider.IsAuthorizedController(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName) || !authorizationProvider.IsAuthorizedAction(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName))
             {
                 filterContext.Result = new HttpUnauthorizedResult();
             }

            base.OnActionExecuting(filterContext);
        }
    }
}
