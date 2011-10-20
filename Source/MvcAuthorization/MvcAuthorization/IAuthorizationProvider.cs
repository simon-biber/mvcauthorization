using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace MvcAuthorization
{
    public interface IAuthorizationProvider
    {
        bool IsAuthorizedAction<TController>(Expression<Action<TController>> action);
        bool IsAuthorizedController<TController>();
        bool IsAuthorizedAction(string controllerName, string actionName);
        bool IsAuthorizedController(string controllerName);

        IEnumerable<string> GetRolesAction<TController>(Expression<Action<TController>> action);
        IEnumerable<string> GetRolesController<TController>();
        IEnumerable<string> GetRolesAction(string controllerName, string actionName);
        IEnumerable<string> GetRolesController(string controllerName);
    }
}
