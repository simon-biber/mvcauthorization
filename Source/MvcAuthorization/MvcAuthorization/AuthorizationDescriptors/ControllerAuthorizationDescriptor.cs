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
            if (roles != null)
            {
                Roles = roles.AsReadOnly();
            }

            if (policyAuthorizationDescriptors != null)
            {
                PolicyAuthorizationDescriptors = policyAuthorizationDescriptors.ToList().AsReadOnly();
            }

            ControllerName = controllerName;
            AreaName = areaName;
        }

        public string AreaName { get; private set; }
        public string ControllerName { get; private set; }

    }
}
