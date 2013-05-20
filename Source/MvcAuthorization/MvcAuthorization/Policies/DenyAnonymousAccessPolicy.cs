using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Policies
{
    [AuthorizationPolicy(Name="DenyAnonymousAccess")]
    public class DenyAnonymousAccessPolicy : IPolicyHandler
    {
        public bool Handle(PolicyHandlerArgs args)
        {
            return System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }
    }
}
