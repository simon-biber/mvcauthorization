using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuthorization.Web.Test
{
    public class MvcDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IAuthorizationProvider))
            {
                return new ConfigurationAuthorizationProvider();
            }
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == typeof(IAuthorizationProvider))
            {
                return new ConfigurationAuthorizationProvider[] { new ConfigurationAuthorizationProvider() };
            }
            return new object[] { };
        }
    }
}
