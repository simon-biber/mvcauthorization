using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcAuthorization.AuthorizationDescriptors;

namespace MvcAuthorization
{
    public class CheckAuthorizationResult
    {
        public bool IsAuthorized { get; set; }
        public List<PolicyAuthorizationDescriptor> PoliciesToSkip { get; set; }
    }
}
