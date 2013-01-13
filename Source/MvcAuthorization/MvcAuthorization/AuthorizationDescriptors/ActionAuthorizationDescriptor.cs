using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class ActionAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public ActionAuthorizationDescriptor(string actionName, string controllerName, string areaName, List<string> roles)
        {
            Roles = roles;
            ControllerName = controllerName;
            ActionName = actionName;
            AreaName = areaName;
        }

        public string AreaName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
