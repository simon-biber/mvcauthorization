using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Configuration;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Collections.Concurrent;

namespace MvcAuthorization
{
    public class ConfigurationAuthorizationProvider : AuthorizationProvider
    {
        protected override ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName)
        {
            ControllerAuthorizationConfigurationElement element = AuthorizationConfiguration.ControllerMappings.Where(e => e.Controller == controllerName).FirstOrDefault();
            return new ControllerAuthorizationDescriptor(controllerName, element != null && !string.IsNullOrEmpty(element.Roles) ? element.Roles.Split(',').ToList() : null);
        }

        protected override ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName)
        {
            ControllerAuthorizationConfigurationElement controllerElement = AuthorizationConfiguration.ControllerMappings.Where(e => e.Controller == controllerName).FirstOrDefault();

            if (controllerElement != null)
            {
                ActionAuthorizationConfigurationElement actionElement = controllerElement.ActionAuthorizationMappings.Where(e => e.Action == actionName).FirstOrDefault();

                if (actionElement != null && !string.IsNullOrEmpty(actionElement.Roles))
                {
                    return new ActionAuthorizationDescriptor(actionName, controllerName, actionElement.Roles.Split(',').ToList());
                }
            }

            // Null for all roles
            return new ActionAuthorizationDescriptor(actionName, controllerName, null);
        }
    }
}
