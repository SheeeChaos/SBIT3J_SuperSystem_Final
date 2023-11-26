using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Controllers
{
    [Authorize]
    public class POSController : Controller
    {
        // GET: POS
        public ActionResult Index()
        {
            return View();
        }


    }
}