﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcAuthorization.Web.Test.Policy
{
    public class MyCustomPolicy : IAuthorizationPolicy
    {
        public ApplyPolicyResult Apply(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = true };
        }
    }
}