using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public class AuthorizationPolicyAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
