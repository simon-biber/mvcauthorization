﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.AuthorizationDescriptors
{
    public class AreaAuthorizationDescriptor : BaseAuthorizationDescriptor
    {
        public AreaAuthorizationDescriptor(string areaName, List<string> roles, IEnumerable<PolicyAuthorizationDescriptor> policyAuthorizationDescriptors)
        {
            if (roles != null)
            {
                Roles = roles.AsReadOnly();
            }

            if (policyAuthorizationDescriptors != null)
            {
                PolicyAuthorizationDescriptors = policyAuthorizationDescriptors.ToList().AsReadOnly();
            }

            AreaName = areaName;
        }

        public string AreaName { get; private set; }
    }
}
