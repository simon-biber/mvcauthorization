using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public interface IAuthorizationPolicy
    {
        /// <summary>
        /// Applies an authorization policy
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args);
    }
}
