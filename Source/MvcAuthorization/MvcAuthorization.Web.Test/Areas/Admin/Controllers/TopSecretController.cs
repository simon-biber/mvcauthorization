using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuthorization.Web.Test.Areas.Admin.Controllers
{
    public partial class TopSecretController : Controller
    {
        //
        // GET: /Admin/TopSecret/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
