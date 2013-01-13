using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace MvcAuthorization
{
    public interface IAuthorizationProvider
    {
        bool IsAuthorizedAction(string controllerName, string actionName, string areaName = null);
        bool IsAuthorizedController(string controllerName, string areaName = null);
        IEnumerable<string> GetRolesAction(string controllerName, string actionName, string areaName = null);
        IEnumerable<string> GetRolesController(string controllerName, string areaName = null);
    }
}
