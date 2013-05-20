using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class AreaAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public AreaAuthorizationDescriptor(string areaName, List<string> roles, IEnumerable<PolicyAuthorizationDescriptor> policyAuthorizationDescriptors)
        {
            Roles = roles;
            PolicyAuthorizationDescriptors = policyAuthorizationDescriptors;
            AreaName = areaName;
        }
    }
}
