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
            if (roles != null)
            {
                Roles = roles.AsReadOnly();
            }

            if (policyAuthorizationDescriptors != null)
            {
                PolicyAuthorizationDescriptors = policyAuthorizationDescriptors.ToList().AsReadOnly();
            }

            ControllerName = controllerName;
            ActionName = actionName;
            AreaName = areaName;
        }

        public string AreaName { get; private set; }
        public string ActionName { get; private set; }
        public string ControllerName { get; private set; }

    }
}
