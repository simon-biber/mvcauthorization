using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization
{
    public class ActionAuthorizationDescriptor
    {
        public ActionAuthorizationDescriptor(string actionName, string controllerName, List<string> roles)
        {
            Roles = roles;
            ControllerName = controllerName;
            ActionName = actionName;
        }

        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public List<string> Roles { get; set; }
    }
}
