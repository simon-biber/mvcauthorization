using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class AreaAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public AreaAuthorizationDescriptor(string areaName, List<string> roles, IEnumerable<string> policyHandlers)
        {
            Roles = roles;
            PolicyHandlerTypes = policyHandlers;
            AreaName = areaName;
        }
    }
}
