using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections.Concurrent;
using MvcAuthorization.Policy;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class CheckAuthorizationResult
    {
        public bool IsAuthorized { get; set; }
        public List<PolicyAuthorizationDescriptor> PoliciesToSkip { get; set; }
    }

    public static class ExtensionMethods
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
        /// Determines whether or not the user is authorized based on the descriptor
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="actionExecutingContext"></param>
        /// <returns></returns>
        public static CheckAuthorizationResult IsAuthorized(this BaseAuthorizationDescriptor descriptor, ActionExecutingContext actionExecutingContext, List<PolicyAuthorizationDescriptor> policiesToIgnore)
        {
            Func<IAuthorizationPolicy, ApplyPolicyResult> policyChecker = (policyHandler) =>
            {
                return policyHandler.ApplyPolicy(new ApplyPolicyArgs()
                {
                    RequiredRoles = descriptor.Roles
                });
            };

            return IsAuthorizedCore(descriptor, policyChecker, policiesToIgnore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descriptor"></param>
        /// <param name="policyChecker"></param>
        /// <returns></returns>
        private static CheckAuthorizationResult IsAuthorizedCore(BaseAuthorizationDescriptor descriptor, Func<IAuthorizationPolicy, ApplyPolicyResult> policyChecker, List<PolicyAuthorizationDescriptor> policiesToIgnore)
        {
            CheckAuthorizationResult result = new CheckAuthorizationResult();
            result.PoliciesToSkip = policiesToIgnore;                               // Modify the original list if passed in

            if (descriptor == null)
            {
                result.IsAuthorized = true;
                return result;
            }

            // No roles means not secured by role
            result.IsAuthorized = descriptor.Roles == null || descriptor.Roles.Count() == 0;

            // Validate policy first
            if (descriptor.PolicyAuthorizationDescriptors != null && descriptor.PolicyAuthorizationDescriptors.Count() > 0)
            {
                // Add the policies to ignore list
                var notExists = descriptor.PolicyAuthorizationDescriptors.Where(pd => pd.IgnoreInherited 
                                    && (result.PoliciesToSkip == null || !result.PoliciesToSkip.Any(s => string.Equals(pd.Name, s.Name, StringComparison.OrdinalIgnoreCase))));

                if (result.PoliciesToSkip == null && notExists.Count() > 0)
                {
                    result.PoliciesToSkip = new List<PolicyAuthorizationDescriptor>();
                    result.PoliciesToSkip.AddRange(notExists);
                }

                foreach (var policyAuthorizationDescriptor in descriptor.PolicyAuthorizationDescriptors)
                {
                    if (result.PoliciesToSkip != null && result.PoliciesToSkip.Any(pd => string.Equals(pd.Name, policyAuthorizationDescriptor.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    IAuthorizationPolicy policyHandler = GetPolicyHandlerInstance(policyAuthorizationDescriptor);

                    if (policyHandler != null)
                    {
                        // Handle via policy
                        var policyResult = policyChecker.Invoke(policyHandler);

                        // Short circuit if the policy must be satsified and the user is not authorized
                        if (!policyResult.IsAuthorized)
                        {
                            result.IsAuthorized = false;
                            return result;
                        }
                    }
                }

            }

            // If we got here that means all policies were met.
            // If we have roles check them to see if the user is allowed.
            if (descriptor.Roles != null)
            {
                foreach (var role in descriptor.Roles)
                {
                    if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                    {
                        // True if one role matches
                        result.IsAuthorized = true;
                        break;
                    }
                }
            }

            
            return result;
        }
        
        /// <summary>
        /// Gets the policy handler from the policy descriptor
        /// </summary>
        /// <param name="policyHandlerType"></param>
        /// <returns></returns>
        private static IAuthorizationPolicy GetPolicyHandlerInstance(PolicyAuthorizationDescriptor policyAuthorizationDescriptor)
        {
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
        private static List<Tuple<Type, PolicyMetadataAttribute>> LoadAndCachePolicyHandlers()
        {
            List<Tuple<Type, PolicyMetadataAttribute>> result = new List<Tuple<Type, PolicyMetadataAttribute>>();

            var policyHandlerTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                                                    .Where(t => typeof(IAuthorizationPolicy).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass && t.IsPublic && !t.IsGenericType);

            result = policyHandlerTypes
                .Select(x => new Tuple<Type, PolicyMetadataAttribute>(x, (PolicyMetadataAttribute)Attribute.GetCustomAttribute(x, typeof(PolicyMetadataAttribute)))).ToList();

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
