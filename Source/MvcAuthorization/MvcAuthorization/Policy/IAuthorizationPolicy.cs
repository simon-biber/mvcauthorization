﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public interface IAuthorizationPolicy
    {
        /// <summary>
        /// Globally applies a policy for all request types
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ApplyPolicyResult ApplyPolicy(ApplyPolicyArgs args);

        /// <summary>
        /// Applies the policy if the current item is a controller action
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ApplyPolicyResult ApplyActionPolicy(ApplyActionPolicyArgs args);

        /// <summary>
        /// Applies the policy if the current item is a SignalR hub or PersistentConnection
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ApplyPolicyResult ApplySignalRPolicy(ApplySignalRPolicyArgs args);
    }
}
