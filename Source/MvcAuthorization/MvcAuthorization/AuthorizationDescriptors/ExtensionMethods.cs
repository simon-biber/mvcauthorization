using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public static class ExtensionMethods
    {
        public static bool IsAuthorized(this BaseAuthorizationDescriptor descriptor)
        {
            if (descriptor == null || descriptor.Roles == null || descriptor.Roles.Count == 0)
            {
                return true;
            }

            foreach (var role in descriptor.Roles)
            {
                if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                {
                    // True if one role matches
                    return true;
                }
            }

            // No roles match, false
            return false;
        }
    }
}
