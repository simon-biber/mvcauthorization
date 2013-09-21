using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcAuthorization.Configuration;

namespace MvcAuthorization
{
    public class AuthorizeFilter : AuthorizeAttribute
    {
        public string AccessDeniedController { get; set; }
        public string AccessDeniedAction { get; set; }
        public string AccessDeniedArea { get; set; }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            IAuthorizationProvider authorizationProvider = AuthorizationProvider.ResolveType<IAuthorizationProvider>();

            // If there's none defined through the type resolver use the configuration provider as the default
            if (authorizationProvider == null)
            {
                authorizationProvider = ConfigurationAuthorizationProvider.DefaultInstance;
            }
            return authorizationProvider.IsAuthorizedAction(httpContext);
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(AccessDeniedAction) && !string.IsNullOrWhiteSpace(AccessDeniedController))
            {
                filterContext.Result = new RedirectToRouteResult(
                            new System.Web.Routing.RouteValueDictionary(
                                new
                                {
                                    Controller = AccessDeniedController,
                                    Action = AccessDeniedAction,
                                    Area = AccessDeniedArea
                                }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
}
