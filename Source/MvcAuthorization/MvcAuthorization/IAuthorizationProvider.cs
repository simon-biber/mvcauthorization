using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;

namespace MvcAuthorization
{
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// Validates that the current request for an action is authorized
        /// </summary>
        /// <returns></returns>
        bool IsAuthorizedAction(ActionExecutingContext actionExecutingContext);

        /// <summary>
        /// Validates that access to an action is authorized without the context of an ActionRequest
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        bool IsAuthorizedAction(string controllerName, string actionName, string areaName = null);

        /// <summary>
        /// Validates that the current request for an action is authorized given an HttpContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        bool IsAuthorizedAction(HttpContextBase httpContext);
    }
}
