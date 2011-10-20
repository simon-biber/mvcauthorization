using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization
{
    public class ControllerAuthorizationDescriptor
    {
        public ControllerAuthorizationDescriptor(string controllerName, List<string> roles)
        {
            Roles = roles;
            ControllerName = controllerName;
        }
        public List<string> Roles { get; set; }
        public string ControllerName { get; set; }
    }
}
