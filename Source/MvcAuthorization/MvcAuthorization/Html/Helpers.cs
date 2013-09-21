using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcAuthorization;
using MvcAuthorization.Configuration;

// TODO: Remove this in a new major version. The helpers extension was moved to the System.Web.Mvc namespace.
namespace MvcAuthorization.Html
{
    internal class DummyClass
    {

    }
}

namespace System.Web.Mvc
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
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName = null, string areaName = null, object htmlAttributes = null)
        {
            IAuthorizationProvider authorizationProvider = DependencyResolver.Current.GetService<IAuthorizationProvider>();

            // If there's none defined through dependency resolver use the configuration provider as the default
            if (authorizationProvider == null)
            {
                authorizationProvider = ConfigurationAuthorizationProvider.Instance;
            }

            MvcHtmlString html = MvcHtmlString.Empty;
            string controller = controllerName ?? helper.ViewContext.RouteData.GetRequiredString("controller");

            if (authorizationProvider.IsAuthorizedAction(controller, actionName, areaName))
            {
                html = System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, linkText, actionName, controller, new { area = areaName }, htmlAttributes);
            }

            return html;
        }
    }
}
