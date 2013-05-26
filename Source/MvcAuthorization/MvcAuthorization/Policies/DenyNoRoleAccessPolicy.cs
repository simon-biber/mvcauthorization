using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Policies
{
    [PolicyMetadata(Name = "DenyNoRoleAccess")]
    public class DenyNoRoleAccessPolicy : AuthorizationPolicy
    {
        public override ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = args.RequiredRoles != null && args.RequiredRoles.Count() > 0 };
        }
    }
}
