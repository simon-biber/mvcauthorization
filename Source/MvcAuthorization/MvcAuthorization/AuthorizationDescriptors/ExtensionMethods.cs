using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public static class ExtensionMethods
    {
        public static bool IsAuthorized(this BaseAuthorizationDescriptor descriptor, ActionExecutingContext actionExecutingContext)
        {
            if (descriptor == null)
            {
                return true;
            }

            // No roles means not secured by role
            bool isAuthorized = descriptor.Roles == null || descriptor.Roles.Count() == 0;

            if (descriptor.Roles != null)
            {
                foreach (var role in descriptor.Roles)
                {
                    if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                    {
                        // True if one role matches
                        isAuthorized = true;
                        break;
                    }
                }
            }

            // Only invoke the policy handler if role is valid
            if (isAuthorized && descriptor.PolicyHandlers != null)
            {
                foreach (var policyHandler in descriptor.PolicyHandlers)
                {
                    // Handle via policy
                    isAuthorized = policyHandler.Handle(new PolicyHandlerArgs()
                    {
                        ActionName = descriptor.ActionName,
                        AreaName = descriptor.AreaName,
                        ControllerName = descriptor.ControllerName,
                        ActionExecutingContext = actionExecutingContext
                    });

                    if (!isAuthorized)
                    {
                        // Stop checking if one fails
                        break;
                    }
                }
                
            }
            return isAuthorized;
        }
    }
}
