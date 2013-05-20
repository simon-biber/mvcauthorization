using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class ControllerAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public ControllerAuthorizationDescriptor(string controllerName, string areaName, List<string> roles, IEnumerable<PolicyAuthorizationDescriptor> policyAuthorizationDescriptors)
        {
            Roles = roles;
            PolicyAuthorizationDescriptors = policyAuthorizationDescriptors;
            ControllerName = controllerName;
            AreaName = areaName;
        }

    }
}
