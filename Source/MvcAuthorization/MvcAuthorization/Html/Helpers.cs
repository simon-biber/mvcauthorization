using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcAuthorization.Configuration;

namespace MvcAuthorization.Html
{
    public static class Helpers
    {
        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName)
        {
            IAuthorizationProvider authorizationProvider = DependencyResolver.Current.GetService<IAuthorizationProvider>();
            MvcHtmlString html = MvcHtmlString.Empty;
            string controllerName = helper.ViewContext.RouteData.GetRequiredString("controller");

            if (authorizationProvider.IsAuthorizedController(controllerName) && authorizationProvider.IsAuthorizedAction(controllerName, actionName))
            {
                html = System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, linkText, actionName);
            }

            return html;
        }
    }
}
