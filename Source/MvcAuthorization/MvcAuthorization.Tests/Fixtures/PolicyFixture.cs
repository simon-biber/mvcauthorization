using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Tests.Fixtures
{
    [PolicyMetadata(Name="TestPolicyFixture")]
    public class PolicyFixture : AuthorizationPolicy
    {
        public bool IsAuthorizedResult { get; set; }
        public bool IsPolicyApplied { get; set; }

        public PolicyFixture()
        {
            IsPolicyApplied = false;
        }

        public override ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            IsPolicyApplied = true;
            return new ApplyPolicyResult() { IsAuthorized = IsAuthorizedResult };
        }
    }
}
