using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcAuthorization.Tests.Fixtures
{
    public class AuthorizationProviderFixture : AuthorizationProvider
    {
        List<string> _roles;
        List<AuthorizationDescriptors.PolicyAuthorizationDescriptor> _policyAuthorizationDescriptors;

        public AuthorizationProviderFixture(string role, AuthorizationDescriptors.PolicyAuthorizationDescriptor policyAuthorizationDescriptor)
        {
            _roles = new List<string>() { role };
            _policyAuthorizationDescriptors = new List<AuthorizationDescriptors.PolicyAuthorizationDescriptor> { policyAuthorizationDescriptor };
        }

        /// <summary>
        /// Default, no roles or policy
        /// </summary>
        public AuthorizationProviderFixture()
        {
            _roles = null;
            _policyAuthorizationDescriptors = null;
        }

        protected override AuthorizationDescriptors.GlobalAuthorizationDescriptor LoadGlobalAuthorizationDescriptor()
        {
            return new AuthorizationDescriptors.GlobalAuthorizationDescriptor(_roles, _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.AreaAuthorizationDescriptor LoadAreaAuthorizationDescriptor(string areaName)
        {
            return new AuthorizationDescriptors.AreaAuthorizationDescriptor(areaName, _roles, _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName, string areaName)
        {
            return new AuthorizationDescriptors.ControllerAuthorizationDescriptor(controllerName, areaName, _roles, _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName, string areaName)
        {
            return new AuthorizationDescriptors.ActionAuthorizationDescriptor(actionName, controllerName, areaName, _roles, _policyAuthorizationDescriptors);
        }
    }
}
