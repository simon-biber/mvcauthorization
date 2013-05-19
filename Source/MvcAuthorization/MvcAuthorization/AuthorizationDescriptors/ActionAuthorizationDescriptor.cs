using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class ActionAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public ActionAuthorizationDescriptor(string actionName, string controllerName, string areaName, List<string> roles, IEnumerable<IPolicyHandler> policyHandlers)
        {
            Roles = roles;
            PolicyHandlers = policyHandlers;
            ControllerName = controllerName;
            ActionName = actionName;
            AreaName = areaName;
        }

    }
}
