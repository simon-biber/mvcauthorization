using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.Policy;
using System.Web;

namespace MvcAuthorization.Policies
{
    [AuthorizationPolicy(Name="SecureConnectionOnly")]
    public class SecureConnectionOnlyPolicy : IPolicyHandler
    {
        public bool Handle(PolicyHandlerArgs args)
        {
            return HttpContext.Current.Request.IsSecureConnection;
        }
    }
}
