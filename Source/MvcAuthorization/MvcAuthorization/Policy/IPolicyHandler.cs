using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAuthorization.Policy
{
    public interface IPolicyHandler
    {
        bool Handle(PolicyHandlerArgs args);
    }
}
