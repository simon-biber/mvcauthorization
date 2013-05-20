﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public interface IAuthorizationPolicy
    {
        ApplyPolicyResult Apply(ApplyPolicyArgs args);
    }
}