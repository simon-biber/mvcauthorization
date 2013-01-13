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
            return new AreaAuthorizationDescriptor(areaName, element != null && !string.IsNullOrEmpty(element.Roles) ? element.Roles.Split(',').ToList() : null);
        }

        protected override ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName, string areaName)
        {
            ControllerAuthorizationConfigurationElement element = AuthorizationConfiguration.AreaMappings.Where(a => a.Area == (areaName ?? "")).SelectMany(cm => cm.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName)).FirstOrDefault();
            //ControllerAuthorizationConfigurationElement element = AuthorizationConfiguration.ControllerMappings.Where(e => e.Controller == controllerName && e.Area == (areaName ?? "")).FirstOrDefault();
            return new ControllerAuthorizationDescriptor(controllerName, areaName, element != null && !string.IsNullOrEmpty(element.Roles) ? element.Roles.Split(',').ToList() : null);
        }

        protected override ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName, string areaName)
        {
            ControllerAuthorizationConfigurationElement controllerElement = AuthorizationConfiguration.AreaMappings.Where(a => a.Area == (areaName ?? "")).SelectMany(cm => cm.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName)).FirstOrDefault();

            if (controllerElement != null)
            {
                ActionAuthorizationConfigurationElement actionElement = controllerElement.ActionAuthorizationMappings.Where(e => e.Action == actionName).FirstOrDefault();

                if (actionElement != null && !string.IsNullOrEmpty(actionElement.Roles))
                {
                    return new ActionAuthorizationDescriptor(actionName, controllerName, areaName, actionElement.Roles.Split(',').ToList());
                }
            }

            // Null for all roles
            return new ActionAuthorizationDescriptor(actionName, controllerName, areaName, null);
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
