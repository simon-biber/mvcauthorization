using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Web.Test.Policy
{
    public class MyCustomPolicy : AuthorizationPolicy
    {
        public override ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }
    }
}