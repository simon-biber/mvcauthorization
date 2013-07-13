using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public static class AuthorizationDescriptorExtensions
    {
        public static CheckAuthorizationResult IsAuthorizedOrDefault(this BaseAuthorizationDescriptor descriptor, ActionExecutingContext actionExecutingContext, List<PolicyAuthorizationDescriptor> policiesToIgnore)
        {
            if (descriptor == null)
            {
                return new CheckAuthorizationResult() 
                {
                    IsAuthorized = true
                };
            }
            return descriptor.IsAuthorized(actionExecutingContext, policiesToIgnore);
        }
    }
}
