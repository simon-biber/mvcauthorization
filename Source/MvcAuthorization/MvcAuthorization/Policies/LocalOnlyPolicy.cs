using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Policies
{
    [PolicyMetadata(Name="LocalOnly")]
    public class LocalOnlyPolicy : AuthorizationPolicy
    {
        public override ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = HttpContext.Current.Request.IsLocal };
        }
    }
}
