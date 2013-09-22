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
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName)
        {
            return SecureActionLink(helper, linkText, actionName, null, null, null);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return SecureActionLink(helper, linkText, actionName, controllerName, null, null);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, string areaName)
        {
            return SecureActionLink(helper, linkText, actionName, controllerName, areaName, null);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, object htmlAttributes)
        {
            return SecureActionLink(helper, linkText, actionName, null, null, htmlAttributes);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object htmlAttributes)
        {
            return SecureActionLink(helper, linkText, actionName, controllerName, null, htmlAttributes);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="areaName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString SecureActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, string areaName, object htmlAttributes)
        {
            IAuthorizationProvider authorizationProvider = DependencyResolver.Current.GetService<IAuthorizationProvider>();

            // If there's none defined through dependency resolver use the configuration provider as the default
            if (authorizationProvider == null)
            {
                authorizationProvider = ConfigurationAuthorizationProvider.DefaultInstance;
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
