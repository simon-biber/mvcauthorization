using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class ControllerAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public ControllerAuthorizationDescriptor(string controllerName, string areaName, List<string> roles)
        {
            Roles = roles;
            ControllerName = controllerName;
            AreaName = areaName;
        }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
    }
}
