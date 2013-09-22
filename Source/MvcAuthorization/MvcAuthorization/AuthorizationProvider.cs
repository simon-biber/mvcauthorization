using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Linq.Expressions;
using MvcAuthorization.AuthorizationDescriptors;
using System.Web.Routing;
using System.Web;

namespace MvcAuthorization
{
    public abstract class AuthorizationProvider : IAuthorizationProvider
    {
        #region Private Backing Variables

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected GlobalAuthorizationDescriptor _globalAuthorizationDescriptor = null;

        /// <summary>
        /// 
        /// </summary>
        [CLSCompliant(false)]
        protected object _globalAuthorizationDescriptorLock = new object();

        /// <summary>
        /// Cache for area authorizations
        /// </summary>
        [CLSCompliant(false)]
        protected ConcurrentDictionary<string, AreaAuthorizationDescriptor> _areaAuthorizationDescriptorCache = new ConcurrentDictionary<string, AreaAuthorizationDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        [CLSCompliant(false)]
        protected ConcurrentDictionary<string, ControllerAuthorizationDescriptor> _controllerAuthorizationDescriptorCache = new ConcurrentDictionary<string, ControllerAuthorizationDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        [CLSCompliant(false)]
        protected ConcurrentDictionary<string, ActionAuthorizationDescriptor> _actionAuthorizationDescriptorCache = new ConcurrentDictionary<string, ActionAuthorizationDescriptor>();

        /// <summary>
        /// Instantiate an object from a type using an IOC container
        /// </summary>
        [CLSCompliant(false)]
        protected static Func<Type, object> _typeResolver = null;

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _rootDirectory = "./";

        /// <summary>
        /// 
        /// </summary>
        private static readonly string _directorySeparator = "./";

        #endregion

        #region Abstract Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract GlobalAuthorizationDescriptor LoadGlobalAuthorizationDescriptor();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        protected abstract AreaAuthorizationDescriptor LoadAreaAuthorizationDescriptor(string areaName);

        /// <summary>
        /// Load a ControllerAuthorizationDescriptor from the backing store (must be implemented in the derived class)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected abstract ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName, string areaName);

        /// <summary>
        /// Load an ActionAuthorizationDescriptor from the backing store (must be implemented in the derived class)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected abstract ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName, string areaName);

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Gets the cache key for area authorization
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        protected string GetAreaAuthorizationDescriptorCacheKey(string areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName))
            {
                return _rootDirectory;
            }
            return string.Concat(_rootDirectory, areaName);
        }

        /// <summary>
        /// Gets the cache key for controller authorization
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        protected string GetControllerAuthorizationDescriptorCacheKey(string controllerName, string areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName))
            {
                return string.Concat(_rootDirectory, controllerName);
            }
            return string.Concat(_rootDirectory, areaName, _directorySeparator, controllerName);
        }

        /// <summary>
        /// Gets the cache key for area action
        /// </summary>
        /// <param name="areaName"></param>
        /// <returns></returns>
        protected string GetActionAuthorizationDescriptorCacheKey(string controllerName, string actionName, string areaName)
        {
            if (string.IsNullOrWhiteSpace(areaName))
            {
                return string.Concat(_rootDirectory, controllerName, _directorySeparator, actionName);
            }
            return string.Concat(_rootDirectory, areaName, _directorySeparator, controllerName, _directorySeparator, actionName);
        }

        /// <summary>
        /// Get a GlobalAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected GlobalAuthorizationDescriptor GetGlobalAuthorizationDescriptor()
        {
            if (_globalAuthorizationDescriptor == null)
            {
                lock (_globalAuthorizationDescriptorLock)
                {
                    if (_globalAuthorizationDescriptor == null)
                    {
                        _globalAuthorizationDescriptor = LoadGlobalAuthorizationDescriptor();
                    }
                }
            }
            return _globalAuthorizationDescriptor;
        }

        /// <summary>
        /// Get a AreaAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected AreaAuthorizationDescriptor GetAreaAuthorizationDescriptor(string areaName)
        {
            return _areaAuthorizationDescriptorCache.GetOrAdd(GetAreaAuthorizationDescriptorCacheKey(areaName),
                    (name) =>
                    {
                        return LoadAreaAuthorizationDescriptor(areaName);
                    });
        }

        /// <summary>
        /// Get a ControllerAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected ControllerAuthorizationDescriptor GetControllerAuthorizationDescriptor(string controllerName, string areaName)
        {
            return _controllerAuthorizationDescriptorCache.GetOrAdd(GetControllerAuthorizationDescriptorCacheKey(controllerName, areaName),
                    (name) =>
                    {
                        return LoadControllerAuthorizationDescriptor(controllerName, areaName);
                    });
        }

        /// <summary>
        /// Load an ActionAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected ActionAuthorizationDescriptor GetActionAuthorizationDescriptor(string controllerName, string actionName, string areaName)
        {
            return _actionAuthorizationDescriptorCache.GetOrAdd(GetActionAuthorizationDescriptorCacheKey(controllerName, actionName, areaName),
                    (name) =>
                    {
                        return LoadActionAuthorizationDescriptor(controllerName, actionName, areaName);
                    });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutingContext"></param>
        /// <param name="global"></param>
        /// <param name="area"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        protected bool CheckIsAuthorized(GlobalAuthorizationDescriptor global, AreaAuthorizationDescriptor area, ControllerAuthorizationDescriptor controller, ActionAuthorizationDescriptor action)
        {
            var actionResult = action.IsAuthorizedOrDefault(null);
            var controllerResult = controller.IsAuthorizedOrDefault(actionResult.PoliciesToSkip);
            var areaResult = area.IsAuthorizedOrDefault(controllerResult.PoliciesToSkip);
            var globalResult = global.IsAuthorizedOrDefault(areaResult.PoliciesToSkip);

            return actionResult.IsAuthorized && controllerResult.IsAuthorized && areaResult.IsAuthorized && globalResult.IsAuthorized;
        }

        #endregion

        #region IAuthorizationProvider Members

        public bool IsAuthorizedAction(string controllerName, string actionName, string areaName = null)
        {
            // Get the global item
            GlobalAuthorizationDescriptor global = GetGlobalAuthorizationDescriptor();

            // Get the area item
            AreaAuthorizationDescriptor area = GetAreaAuthorizationDescriptor(areaName);

            // Get the controller item
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName, areaName);

            // Get the action item
            ActionAuthorizationDescriptor action = GetActionAuthorizationDescriptor(controllerName, actionName, areaName);

            // Return auth
            return CheckIsAuthorized(global, area, controller, action);
        }

        public bool IsAuthorizedAction(ActionExecutingContext actionExecutingContext)
        {
            string areaName = actionExecutingContext.RouteData.DataTokens["area"] as string;
            string actionName = actionExecutingContext.ActionDescriptor.ActionName;
            string controllerName = actionExecutingContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            return IsAuthorizedAction(controllerName, actionName, areaName);
        }

        public bool IsAuthorizedAction(HttpContextBase httpContext)
        {
            RouteData routeData = httpContext.Request.RequestContext.RouteData;
            string actionName = routeData.GetRequiredString("action");
            string controllerName = routeData.GetRequiredString("controller");
            string areaName = routeData.Values["area"] as string;

            return IsAuthorizedAction(controllerName, actionName, areaName);
        }

        #endregion

        #region IOC Container Hooks

        /// <summary>
        /// Initializes a type resolver, which returns a concrete instance of an object based on type.
        /// </summary>
        /// <param name="typeResolver"></param>
        public static void ResolveDependenciesUsing(Func<Type, object> typeResolver)
        {
            _typeResolver = typeResolver;
        }

        /// <summary>
        /// Resolves a given type using the type resolver (if specified)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ResolveType<T>() where T : class
        {
            if (_typeResolver != null)
            {
                return _typeResolver.Invoke(typeof(T)) as T;
            }

            return null;
        }

        /// <summary>
        /// Resolves a given type using the type resolver (if specified)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ResolveType(Type type)
        {
            if (_typeResolver != null)
            {
                return _typeResolver.Invoke(type);
            }

            return null;
        }

        /// <summary>
        /// Returns the current type resolver
        /// </summary>
        /// <returns></returns>
        public static Func<Type,object> GetTypeResolver()
        {
            return _typeResolver;
        }

        #endregion
    }
}
