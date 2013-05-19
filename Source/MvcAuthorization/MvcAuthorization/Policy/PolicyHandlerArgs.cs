using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcAuthorization
{
    public class PolicyHandlerArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Will be present if this validation is for an Action request that is currently processing.
        /// Will be null otherwise (for example, when validating an action link's visibility).
        /// </summary>
        public ActionExecutingContext ActionExecutingContext { get; set; }
    }
}
