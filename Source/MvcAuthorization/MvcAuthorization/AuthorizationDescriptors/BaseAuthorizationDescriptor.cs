using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class BaseAuthorizationDescriptor
    {
        public List<string> Roles { get; set; }
    }
}
