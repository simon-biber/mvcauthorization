using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcAuthorization
{
    public interface IAuthorizationProvider
    {
        /// <summary>
        /// Validates that the current request is authorized
        /// </summary>
        /// <returns></returns>
        bool IsCurrentRequestAuthorized(ActionExecutingContext actionExecutingContext);

        /// <summary>
        /// Validates that the specified action is authorized
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <param name="areaName"></param>
        /// <returns></returns>
        bool IsAuthorizedAction(string controllerName, string actionName, string areaName = null, RouteValueDictionary routeValueDictionary = null);
    }
}
