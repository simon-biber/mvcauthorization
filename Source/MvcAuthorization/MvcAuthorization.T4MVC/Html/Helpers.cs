using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcAuthorization;

namespace System.Web.Mvc
{

    public static class Helpers
    {
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, ActionResult result, object htmlAttributes = null)
        {
            IAuthorizationProvider authorizationProvider = DependencyResolver.Current.GetService<IAuthorizationProvider>();

            // If there's none defined through dependency resolver use the configuration provider as the default
            if (authorizationProvider == null)
            {
                authorizationProvider = ConfigurationAuthorizationProvider.DefaultInstance;
            }

            var routeDataDictionary = result.GetRouteValueDictionary();
            string actionName = routeDataDictionary["action"].ToString();
            string controllerName = routeDataDictionary["controller"].ToString();
            string areaName = routeDataDictionary["area"].ToString();

            MvcHtmlString html = MvcHtmlString.Empty;

            if (authorizationProvider.IsAuthorizedAction(controllerName, actionName, areaName))
            {
                html = System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, new { area = areaName }, htmlAttributes);
            }

            return html;
        }
    }
}
