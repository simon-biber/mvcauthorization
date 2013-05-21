using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public abstract class AuthorizationPolicy : IAuthorizationPolicy
    {
        /// <summary>
        /// Globally applies a policy for all request types
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }

        /// <summary>
        /// Applies the policy if the current item is a controller action
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ApplyPolicyResult ApplyActionPolicy(ApplyActionPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }

        /// <summary>
        /// Applies the policy if the current item is a SignalR hub or PersistentConnection
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ApplyPolicyResult ApplySignalRPolicy(ApplySignalRPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }
    }
}
