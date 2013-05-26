using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public abstract class BaseAuthorizationDescriptor
    {
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<string> Roles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<PolicyAuthorizationDescriptor> PolicyAuthorizationDescriptors { get; set; }
    }
}
