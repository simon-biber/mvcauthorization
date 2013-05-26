using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcAuthorization.Policy
{
    public class ApplyPolicyArgs
    {
        public IEnumerable<string> RequiredRoles { get; set; }
    }
}
