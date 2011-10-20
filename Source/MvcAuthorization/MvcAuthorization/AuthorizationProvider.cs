using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace MvcAuthorization
{
    public abstract class AuthorizationProvider : IAuthorizationProvider
    {
        #region Private Backing Variables

        /// <summary>
        /// Cache for controller descriptors
        /// </summary>
        private static ConcurrentDictionary<Type, ReflectedControllerDescriptor> _controllerDescriptorCache = new ConcurrentDictionary<Type, ReflectedControllerDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        private static ConcurrentDictionary<string, ControllerAuthorizationDescriptor> _controllerAuthorizationDescriptorCache = new ConcurrentDictionary<string, ControllerAuthorizationDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        private static ConcurrentDictionary<string, ActionAuthorizationDescriptor> _actionAuthorizationDescriptorCache = new ConcurrentDictionary<string, ActionAuthorizationDescriptor>();

        #endregion

        #region Abstract Members

        /// <summary>
        /// Load a ControllerAuthorizationDescriptor from the backing store (must be implemented in the derived class)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected abstract ControllerAuthorizationDescriptor LoadControllerAuthorizationDescriptor(string controllerName);

        /// <summary>
        /// Load an ActionAuthorizationDescriptor from the backing store (must be implemented in the derived class)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected abstract ActionAuthorizationDescriptor LoadActionAuthorizationDescriptor(string controllerName, string actionName);

        #endregion

        #region Protected Helpers

        /// <summary>
        /// Get a ControllerAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        protected ControllerAuthorizationDescriptor GetControllerAuthorizationDescriptor(string controllerName)
        {
            return _controllerAuthorizationDescriptorCache.GetOrAdd(controllerName,
                    (name) =>
                    {
                        return LoadControllerAuthorizationDescriptor(controllerName);
                    });
        }

        /// <summary>
        /// Load an ActionAuthorizationDescriptor from cache (or backing store if not cached yet)
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        protected ActionAuthorizationDescriptor GetActionAuthorizationDescriptor(string controllerName, string actionName)
        {
            return _actionAuthorizationDescriptorCache.GetOrAdd(actionName,
                    (name) =>
                    {
                        return LoadActionAuthorizationDescriptor(controllerName, actionName);
                    });
        }

        #endregion

        #region IAuthorizationProvider Members

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="action"></param>
        /// <param name="authorizationCheck"></param>
        /// <returns></returns>
        public bool IsAuthorizedAction<TController>(Expression<Action<TController>> action)
        {
            string actionName = string.Empty;
            MethodCallExpression expr = action.Body as MethodCallExpression;

            if (expr == null)
            {
                return false;
            }
            actionName = expr.Method.Name;
            ReflectedControllerDescriptor controllerDescriptor = _controllerDescriptorCache.GetOrAdd(typeof(TController), (type) => new ReflectedControllerDescriptor(type));
            return IsAuthorizedAction(controllerDescriptor.ControllerName, actionName);
        }


        public bool IsAuthorizedAction(string controllerName, string actionName)
        {
            // Get the controller item
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName);

            // Get the action item
            ActionAuthorizationDescriptor action = GetActionAuthorizationDescriptor(controllerName, actionName);

            // Return auth
            return IsAuthorized(controller) && IsAuthorized(action);
        }

        public bool IsAuthorizedController<TController>()
        {
            ReflectedControllerDescriptor controllerDescriptor = _controllerDescriptorCache.GetOrAdd(typeof(TController), (type) => new ReflectedControllerDescriptor(type));
            return IsAuthorizedController(controllerDescriptor.ControllerName);
        }

        public bool IsAuthorizedController(string controllerName)
        {
            // Get the controller item
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName);

            // Return auth
            return IsAuthorized(controller);
        }

        public IEnumerable<string> GetRolesAction<TController>(Expression<Action<TController>> action)
        {
            string actionName = string.Empty;
            MethodCallExpression expr = action.Body as MethodCallExpression;

            if (expr == null)
            {
                return null;
            }
            actionName = expr.Method.Name;
            ReflectedControllerDescriptor controllerDescriptor = _controllerDescriptorCache.GetOrAdd(typeof(TController), (type) => new ReflectedControllerDescriptor(type));
            return GetRolesAction(controllerDescriptor.ControllerName, actionName);
        }

        public IEnumerable<string> GetRolesController<TController>()
        {
            ReflectedControllerDescriptor controllerDescriptor = _controllerDescriptorCache.GetOrAdd(typeof(TController), (type) => new ReflectedControllerDescriptor(type));
            return GetRolesController(controllerDescriptor.ControllerName);
        }

        public IEnumerable<string> GetRolesAction(string controllerName, string actionName)
        {
            ActionAuthorizationDescriptor action = GetActionAuthorizationDescriptor(controllerName, actionName);
            return action != null && action.Roles != null ? action.Roles : null;
        }

        public IEnumerable<string> GetRolesController(string controllerName)
        {
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName);
            return controller != null && controller.Roles != null ? controller.Roles : null;
        }

        #endregion

        #region Check authorization against controller/action descriptors

        protected virtual bool IsAuthorized(ControllerAuthorizationDescriptor descriptor)
        {
            if (descriptor == null || descriptor.Roles == null || descriptor.Roles.Count == 0)
            {
                return true;
            }

            foreach (var role in descriptor.Roles)
            {
                if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                {
                    // True if one role matches
                    return true;
                }
            }

            // No roles match, false
            return false;
        }

        protected virtual bool IsAuthorized(ActionAuthorizationDescriptor descriptor)
        {
            if (descriptor == null || descriptor.Roles == null || descriptor.Roles.Count == 0)
            {
                return true;
            }

            foreach (var role in descriptor.Roles)
            {
                if (System.Threading.Thread.CurrentPrincipal.IsInRole(role))
                {
                    // True if one role matches
                    return true;
                }
            }

            // No roles match, false
            return false;
        }

        #endregion
    }
}
