using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class AreaAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public AreaAuthorizationDescriptor(string areaName, List<string> roles)
        {
            Roles = roles;
            AreaName = areaName;
        }
        public string AreaName { get; set; }
    }
}
