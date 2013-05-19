using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcAuthorization.Policy;

namespace MvcAuthorization.Web.Test.Policy
{
    public class MyCustomPolicy : IPolicyHandler
    {
        public bool Handle(PolicyHandlerArgs args)
        {
            return true;
        }
    }
}