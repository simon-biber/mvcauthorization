using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAuthorization.Web.Test.Areas.Admin.Controllers
{
    public class TopSecretController : Controller
    {
        //
        // GET: /Admin/TopSecret/

        public ActionResult Index()
        {
            return View();
        }

    }
}
