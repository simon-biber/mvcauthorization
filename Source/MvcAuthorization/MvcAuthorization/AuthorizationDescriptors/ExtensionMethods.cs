using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcAuthorization.Policy;
using System.Collections.Concurrent;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Maps a policy typeName to a type
        /// </summary>
        private static ConcurrentDictionary<PolicyAuthorizationDescriptor, Type> _policyHandlerTypeCache = new ConcurrentDictionary<PolicyAuthorizationDescriptor, Type>();

        /// <summary>
        /// Given a policy handler type, stores a cached instance for this thread (for cases where we are not using an IOC container for lifetime management)
        /// </summary>
        private static ConcurrentDictionary<Type, IPolicyHandler> _policyHandlerCache = new ConcurrentDictionary<Type, IPolicyHandler>();

        /// <summary>
        /// Determines whether or not the user is authorized based on the descriptor
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="actionExecutingContext"></param>
        /// <returns></returns>
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
            if (isAuthorized && descriptor.PolicyAuthorizationDescriptors != null)
            {
                foreach (var policyAuthorizationDescriptor in descriptor.PolicyAuthorizationDescriptors)
                {
                    IPolicyHandler policyHandler = GetPolicyHandlerInstance(policyAuthorizationDescriptor);

                    if (policyHandler != null)
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
                
            }
            return isAuthorized;
        }
        
        /// <summary>
        /// Instantiate the policy handler from the typeName
        /// </summary>
        /// <param name="policyHandlerType"></param>
        /// <returns></returns>
        private static IPolicyHandler GetPolicyHandlerInstance(PolicyAuthorizationDescriptor policyAuthorizationDescriptor)
        {
            Func<Type, object> typeResolver = AuthorizationProvider.GetTypeResolver();
                           
            // Get the handler type from the cache
            Type handlerType = _policyHandlerTypeCache.GetOrAdd(policyAuthorizationDescriptor, (descriptor) =>
                {
                    if (string.Equals(descriptor.Type, "TypeName", StringComparison.OrdinalIgnoreCase))
                    {
                        return Type.GetType(descriptor.Value);
                    }
                    else if(string.Equals(descriptor.Type, "Name", StringComparison.OrdinalIgnoreCase))
                    {
                        var policyHandlerInterface = typeof(IPolicyHandler);
                        var policyHandlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                                                        .Where(t => policyHandlerInterface.IsAssignableFrom(t) && !t.IsAbstract && t.IsClass && t.IsPublic && !t.IsGenericType);

                        var policyAttributes = policyHandlers.Where(t => {
                                var attribute = (AuthorizationPolicyAttribute)Attribute.GetCustomAttribute(t, typeof(AuthorizationPolicyAttribute));
                                return attribute != null && string.Equals(attribute.Name, descriptor.Value);                          
                        });
                        return policyAttributes.FirstOrDefault();
                    }
                    else
                    {
                        throw new Exception("Error");
                    }
                });

            // Resolve the instance using the IOC container if specified
            if (typeResolver != null && handlerType != null)
            {
                IPolicyHandler policyHandler = typeResolver.Invoke(handlerType) as IPolicyHandler;

                if (policyHandler != null)
                {
                    return policyHandler;
                }
            }

            // Get the policy handler from the cache, default per-thread lifetime
            return handlerType != null ? _policyHandlerCache.GetOrAdd(handlerType, (type) =>
                                                {
                                                    return (IPolicyHandler)Activator.CreateInstance(type);
                                                })
                                        : null;
        }
    }
}
