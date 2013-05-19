using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class AreaAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public AreaAuthorizationDescriptor(string areaName, List<string> roles, IEnumerable<IPolicyHandler> policyHandlers)
        {
            Roles = roles;
            PolicyHandlers = policyHandlers;
            AreaName = areaName;
        }
    }
}
