using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MvcAuthorization.Policies
{
    [PolicyMetadata(Name="LocalOnly")]
    public class LocalOnlyPolicy : IAuthorizationPolicy
    {
        public ApplyPolicyResult Apply(ApplyPolicyArgs args)
        {
            return new ApplyPolicyResult() { IsAuthorized = HttpContext.Current.Request.IsLocal };
        }
    }
}
