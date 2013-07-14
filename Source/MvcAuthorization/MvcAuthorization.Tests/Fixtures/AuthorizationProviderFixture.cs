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
        List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> _actionConfig;
        List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> _controllerConfig;
        List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> _areaConfig;
        List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> _globalConfig;

        public AuthorizationProviderFixture(List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> actionConfig, 
                List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> controllerConfig, 
                List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> areaConfig, 
                List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> globalConfig)
        {
            _roles = null;
            _policyAuthorizationDescriptors = null;

            _actionConfig = actionConfig;
            _controllerConfig = controllerConfig;
            _areaConfig = areaConfig;
            _globalConfig = globalConfig;
        }

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
            return new AuthorizationDescriptors.GlobalAuthorizationDescriptor(GetTestRoleDataByKey(null, _globalConfig) ?? _roles, GetTestPolicyDataByKey(null, _globalConfig) ?? _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.AreaAuthorizationDescriptor LoadAreaAuthorizationDescriptor(string areaName)
        {
            return new AuthorizationDescriptors.AreaAuthorizationDescriptor(areaName, GetTestRoleDataByKey(areaName, _areaConfig) ?? _roles, GetTestPolicyDataByKey(areaName, _areaConfig) ?? _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName, string areaName)
        {
            return new AuthorizationDescriptors.ControllerAuthorizationDescriptor(controllerName, areaName, GetTestRoleDataByKey(controllerName, _controllerConfig) ?? _roles, GetTestPolicyDataByKey(controllerName, _controllerConfig) ?? _policyAuthorizationDescriptors);
        }

        protected override AuthorizationDescriptors.ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName, string areaName)
        {
            return new AuthorizationDescriptors.ActionAuthorizationDescriptor(actionName, controllerName, areaName, GetTestRoleDataByKey(actionName, _actionConfig) ?? _roles, GetTestPolicyDataByKey(actionName, _actionConfig) ?? _policyAuthorizationDescriptors);
        }

        protected List<string> GetTestRoleDataByKey(string key, List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> source)
        {
            if (source == null)
            {
                return null;
            }

            var item = source.Where(x => x.Item1 == key && x.Item2 != null && x.Item2.Count() > 0).FirstOrDefault();
            if(item != null)
            {
                return item.Item2.ToList();
            }
            else
            {
                return null;
            }
        }

        protected List<AuthorizationDescriptors.PolicyAuthorizationDescriptor> GetTestPolicyDataByKey(string key, List<Tuple<string, string[], List<AuthorizationDescriptors.PolicyAuthorizationDescriptor>>> source)
        {
            if (source == null)
            {
                return null;
            }
            return source.Where(x => x.Item1 == key && x.Item3 != null && x.Item3.Count() > 0).Select(x => x.Item3).FirstOrDefault();
        }
    }
}
