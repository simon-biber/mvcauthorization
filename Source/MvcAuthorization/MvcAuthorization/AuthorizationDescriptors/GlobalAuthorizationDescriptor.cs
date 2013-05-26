using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class GlobalAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public GlobalAuthorizationDescriptor(List<string> roles, IEnumerable<PolicyAuthorizationDescriptor> policyAuthorizationDescriptors)
        {
            if (roles != null)
            {
                Roles = roles.AsReadOnly();
            }

            if (policyAuthorizationDescriptors != null)
            {
                PolicyAuthorizationDescriptors = policyAuthorizationDescriptors.ToList().AsReadOnly();
            }
        }
    }
}
