using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Policies
{
    [PolicyMetadata(Name="DenyAnonymousAccess")]
    public class DenyAnonymousAccessPolicy : AuthorizationPolicy
    {
        public override ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated };
        }
    }
}
