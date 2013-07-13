using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MvcAuthorization.Policy;
using System.Web.Mvc;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public abstract class BaseAuthorizationDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<PolicyAuthorizationDescriptor> PolicyAuthorizationDescriptors { get; set; }

        /// <summary>
        /// Determines whether or not the user is authorized based on the descriptor
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="actionExecutingContext"></param>
        /// <returns></returns>
        public CheckAuthorizationResult IsAuthorized(ActionExecutingContext actionExecutingContext, List<PolicyAuthorizationDescriptor> policiesToIgnore)
        {
            Func<IAuthorizationPolicy, ApplyPolicyResult> policyChecker = (policyHandler) =>
            {
                return policyHandler.ApplyPolicy(new ApplyPolicyArgs()
                {
                    RequiredRoles = Roles
                });
            };

            return IsAuthorizedCore(policyChecker, policiesToIgnore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="policyChecker"></param>
        /// <returns></returns>
        private CheckAuthorizationResult IsAuthorizedCore(Func<IAuthorizationPolicy, ApplyPolicyResult> policyChecker, List<PolicyAuthorizationDescriptor> policiesToIgnore)
        {
            CheckAuthorizationResult result = new CheckAuthorizationResult();
            result.PoliciesToSkip = policiesToIgnore;                               // Modify the original list if passed in

            // No roles means not secured by role
            result.IsAuthorized = Roles == null || Roles.Count() == 0 || Roles.All(r => r == null);

            // Validate policy first
            if (PolicyAuthorizationDescriptors != null && PolicyAuthorizationDescriptors.Count() > 0)
            {
                // Add the policies to ignore list
                var notExists = PolicyAuthorizationDescriptors.Where(pd => pd != null && pd.IgnoreInherited
                                    && (result.PoliciesToSkip == null || !result.PoliciesToSkip.Any(s => string.Equals(pd.Name, s.Name, StringComparison.OrdinalIgnoreCase))));

                if (result.PoliciesToSkip == null && notExists.Count() > 0)
                {
                    result.PoliciesToSkip = new List<PolicyAuthorizationDescriptor>();
                    result.PoliciesToSkip.AddRange(notExists);
                }

                foreach (var policyAuthorizationDescriptor in PolicyAuthorizationDescriptors)
                {
                    if (result.PoliciesToSkip != null && result.PoliciesToSkip.Any(pd => string.Equals(pd.Name, policyAuthorizationDescriptor.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    IAuthorizationPolicy policyHandler = PolicyHelper.GetPolicyHandlerInstance(policyAuthorizationDescriptor);

                    if (policyHandler != null)
                    {
                        // Handle via policy
                        var policyResult = policyChecker.Invoke(policyHandler);

                        // Short circuit if the policy must be satsified and the user is not authorized
                        if (!policyResult.IsAuthorized)
                        {
                            result.IsAuthorized = false;
                            return result;
                        }
                    }
                }

            }

            // If we got here that means all policies were met.
            // If we have roles check them to see if the user is allowed.
            if (Roles != null)
            {
                foreach (var role in Roles)
                {
                    if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                    {
                        // True if one role matches
                        result.IsAuthorized = true;
                        break;
                    }
                }
            }


            return result;
        }

    }
}
