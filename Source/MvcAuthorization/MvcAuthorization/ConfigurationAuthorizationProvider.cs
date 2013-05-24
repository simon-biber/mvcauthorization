using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Configuration;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Collections.Concurrent;
using MvcAuthorization.AuthorizationDescriptors;

namespace MvcAuthorization
{
    public class ConfigurationAuthorizationProvider : AuthorizationProvider
    {
        protected override AreaAuthorizationDescriptor LoadAreaAuthorizationDescriptor(string areaName)
        {
            AreaAuthorizationConfigurationElement element = AuthorizationConfiguration.AreaMappings.Where(a => a.Area == (areaName ?? "")).FirstOrDefault();
            return new AreaAuthorizationDescriptor(areaName, element != null && !string.IsNullOrEmpty(element.Roles) ? element.Roles.Split(',').ToList() : null, LoadPolicyHandlerFromConfig(element != null ? element.Policies : null));
        }

        protected override ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName, string areaName)
        {
            ControllerAuthorizationConfigurationElement element = AuthorizationConfiguration.AreaMappings.Where(a => a.Area == (areaName ?? "")).SelectMany(cm => cm.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName)).FirstOrDefault();
            return new ControllerAuthorizationDescriptor(controllerName, areaName, element != null && !string.IsNullOrEmpty(element.Roles) ? element.Roles.Split(',').ToList() : null, LoadPolicyHandlerFromConfig(element != null ? element.Policies : null));
        }

        protected override ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName, string areaName)
        {
            ControllerAuthorizationConfigurationElement controllerElement = AuthorizationConfiguration.AreaMappings.Where(a => a.Area == (areaName ?? "")).SelectMany(cm => cm.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName)).FirstOrDefault();

            if (controllerElement != null)
            {
                ActionAuthorizationConfigurationElement actionElement = controllerElement.ActionAuthorizationMappings.Where(e => e.Action == actionName).FirstOrDefault();

                if (actionElement != null)
                {
                    return new ActionAuthorizationDescriptor(actionName, controllerName, areaName, 
                                                    !string.IsNullOrEmpty(actionElement.Roles) ? actionElement.Roles.Split(',').ToList() : null, 
                                                    LoadPolicyHandlerFromConfig(actionElement.Policies));
                }
            }

            // Null for all roles if no controller/action element found
            return new ActionAuthorizationDescriptor(actionName, controllerName, areaName, null, null);
        }

        protected IEnumerable<PolicyAuthorizationDescriptor> LoadPolicyHandlerFromConfig(PolicyAuthorizationConfigurationCollection policyHandlerCollection)
        {
            if (policyHandlerCollection != null && policyHandlerCollection.Count > 0)
            {
                List<PolicyAuthorizationDescriptor> policyDescriptors = new List<PolicyAuthorizationDescriptor>();

                foreach (PolicyAuthorizationConfigurationElement policyHandlerElement in policyHandlerCollection)
                {
                    policyDescriptors.Add(new PolicyAuthorizationDescriptor(policyHandlerElement.LoadByTypeName, policyHandlerElement.Name));
                }
                return policyDescriptors;
            }

            return null;
        }

        #region Singleton instance for when there is no dependency resolver

        private static readonly Lazy<ConfigurationAuthorizationProvider> _instance
             = new Lazy<ConfigurationAuthorizationProvider>(() => new ConfigurationAuthorizationProvider());

        internal static ConfigurationAuthorizationProvider Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion
    }
}
