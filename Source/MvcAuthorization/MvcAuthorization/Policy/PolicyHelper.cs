using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Concurrent;
using MvcAuthorization.Policy;
using MvcAuthorization.AuthorizationDescriptors;

namespace MvcAuthorization.Policy
{
    internal static class PolicyHelper
    {
        /// <summary>
        /// Maps a policy descriptor to a type
        /// </summary>
        private static ConcurrentDictionary<PolicyAuthorizationDescriptor, Type> _policyHandlerTypeCache = new ConcurrentDictionary<PolicyAuthorizationDescriptor, Type>();

        /// <summary>
        /// Given a policy handler type, stores a cached instance for this thread (for cases where we are not using an IOC container for lifetime management)
        /// </summary>
        private static ConcurrentDictionary<Type, IAuthorizationPolicy> _policyHandlerCache = new ConcurrentDictionary<Type, IAuthorizationPolicy>();

        /// <summary>
        /// Holds a list of policy handler types and their metadata in the current AppDomain
        /// </summary>
        private static Lazy<List<Tuple<Type, PolicyMetadataAttribute>>> _policyHandlerTypeListCache = new Lazy<List<Tuple<Type, PolicyMetadataAttribute>>>(() => LoadAndCachePolicyHandlers());
        
        /// <summary>
        /// Gets the policy handler from the policy descriptor
        /// </summary>
        /// <param name="policyHandlerType"></param>
        /// <returns></returns>
        internal static IAuthorizationPolicy GetPolicyHandlerInstance(PolicyAuthorizationDescriptor policyAuthorizationDescriptor)
        {
            if (policyAuthorizationDescriptor == null)
            {
                return null;
            }

            Func<Type, object> typeResolver = AuthorizationProvider.GetTypeResolver();

            // Get the handler type from the cache
            Type handlerType = _policyHandlerTypeCache.GetOrAdd(policyAuthorizationDescriptor, (descriptor) =>
                {
                    var policyAttribute = _policyHandlerTypeListCache.Value.Where(x =>
                    {
                        return x.Item2 != null && string.Equals(x.Item2.Name, descriptor.Name, StringComparison.OrdinalIgnoreCase);
                    }).FirstOrDefault();

                    return policyAttribute != null ? policyAttribute.Item1 : null;
                });

            // Resolve the instance using the IOC container if specified
            if (typeResolver != null && handlerType != null)
            {
                IAuthorizationPolicy policyHandler = typeResolver.Invoke(handlerType) as IAuthorizationPolicy;

                if (policyHandler != null)
                {
                    return policyHandler;
                }
            }

            // Get the policy handler from the cache, default per-thread lifetime
            return handlerType != null ? _policyHandlerCache.GetOrAdd(handlerType, (type) =>
                                                {
                                                    return (IAuthorizationPolicy)Activator.CreateInstance(type);
                                                })
                                        : null;
        }

        /// <summary>
        /// Builds a list of policy handler types in the current AppDomain and retrieves their metadata.
        /// </summary>
        /// <returns></returns>
        internal static List<Tuple<Type, PolicyMetadataAttribute>> LoadAndCachePolicyHandlers()
        {
            List<Tuple<Type, PolicyMetadataAttribute>> result = new List<Tuple<Type, PolicyMetadataAttribute>>();

            var policyHandlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                                                    .Where(t => typeof(IAuthorizationPolicy).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass && t.IsPublic && !t.IsGenericType);

            result = policyHandlerTypes
                .Select(x => new Tuple<Type, PolicyMetadataAttribute>(x, (PolicyMetadataAttribute)Attribute.GetCustomAttribute(x, typeof(PolicyMetadataAttribute)))).ToList();


            // Find empty handlers
            var emptyPolicyHandlers = result.Where(x => x.Item2 == null || string.IsNullOrWhiteSpace(x.Item2.Name));

            if (emptyPolicyHandlers.Count() > 0)
            {
                throw new ApplicationException(string.Format("Policy names are required. The following policy type(s) inherit from IAuthorizationPolicy but do not have a policy name set through the PolicyMetadataAttribute: {0}", string.Join(", ", emptyPolicyHandlers.Select(g => g.Item1.FullName))));
            }

            // Find duplicates and throw an error if there is more than one policy handler with the same name
            var duplicateHandlers = result.Where(x => x.Item2 != null).Select(x => x.Item2).GroupBy(x => x.Name).Where(x => x.Count() > 1);

            if (duplicateHandlers.Count() > 0)
            {
                throw new ApplicationException(string.Format("Policy names must be unique. The following policy name(s) were found more than once: {0}", string.Join(", ", duplicateHandlers.Select(g => g.Key))));
            }

            return result;
        }

    }
}
