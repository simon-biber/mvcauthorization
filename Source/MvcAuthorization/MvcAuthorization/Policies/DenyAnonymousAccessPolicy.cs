using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policies
{
    [PolicyMetadata(Name="DenyAnonymousAccess")]
    public class DenyAnonymousAccessPolicy : IAuthorizationPolicy
    {
        public ApplyPolicyResult Apply(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated };
        }
    }
}
