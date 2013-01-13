﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Linq.Expressions;
using MvcAuthorization.AuthorizationDescriptors;

namespace MvcAuthorization
{
    public abstract class AuthorizationProvider : IAuthorizationProvider
    {
        #region Private Backing Variables

        /// <summary>
        /// Cache for area authorizations
        /// </summary>
        private static ConcurrentDictionary<string, AreaAuthorizationDescriptor> _areaAuthorizationDescriptorCache = new ConcurrentDictionary<string, AreaAuthorizationDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        private static ConcurrentDictionary<string, ControllerAuthorizationDescriptor> _controllerAuthorizationDescriptorCache = new ConcurrentDictionary<string, ControllerAuthorizationDescriptor>();

        /// <summary>
        /// Cache for controller authorizations
        /// </summary>
        private static ConcurrentDictionary<string, ActionAuthorizationDescriptor> _actionAuthorizationDescriptorCache = new ConcurrentDictionary<string, ActionAuthorizationDescriptor>();

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

        #endregion

        #region IAuthorizationProvider Members

        public bool IsAuthorizedAction(string controllerName, string actionName, string areaName = null)
        {
            // Get the area item
            AreaAuthorizationDescriptor area = GetAreaAuthorizationDescriptor(areaName);

            // Get the controller item
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName, areaName);

            // Get the action item
            ActionAuthorizationDescriptor action = GetActionAuthorizationDescriptor(controllerName, actionName, areaName);

            // Return auth
            return area.IsAuthorized() && controller.IsAuthorized() && action.IsAuthorized();
        }

        public bool IsAuthorizedController(string controllerName, string areaName = null)
        {
            // Get the area item
            AreaAuthorizationDescriptor area = GetAreaAuthorizationDescriptor(areaName);

            // Get the controller item
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName, areaName);

            // Return auth
            return area.IsAuthorized() && controller.IsAuthorized();
        }

        public IEnumerable<string> GetRolesAction(string controllerName, string actionName, string areaName = null)
        {
            ActionAuthorizationDescriptor action = GetActionAuthorizationDescriptor(controllerName, actionName, areaName);
            return action != null && action.Roles != null ? action.Roles : null;
        }

        public IEnumerable<string> GetRolesController(string controllerName, string areaName = null)
        {
            ControllerAuthorizationDescriptor controller = GetControllerAuthorizationDescriptor(controllerName, areaName);
            return controller != null && controller.Roles != null ? controller.Roles : null;
        }

        #endregion
    }
}
