using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public abstract class AuthorizationPolicy : IAuthorizationPolicy
    {
        /// <summary>
        /// Applies an authorization policy
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }
    }
}
