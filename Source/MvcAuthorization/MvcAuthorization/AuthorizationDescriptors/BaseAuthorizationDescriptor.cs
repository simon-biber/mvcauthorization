using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public abstract class BaseAuthorizationDescriptor
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<PolicyAuthorizationDescriptor> PolicyAuthorizationDescriptors { get; set; }
        public string AreaName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
