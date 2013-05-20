using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class ActionAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public ActionAuthorizationDescriptor(string actionName, string controllerName, string areaName, List<string> roles, IEnumerable<PolicyAuthorizationDescriptor> policyAuthorizationDescriptors)
        {
            Roles = roles;
            PolicyAuthorizationDescriptors = policyAuthorizationDescriptors;
            ControllerName = controllerName;
            ActionName = actionName;
            AreaName = areaName;
        }

    }
}
