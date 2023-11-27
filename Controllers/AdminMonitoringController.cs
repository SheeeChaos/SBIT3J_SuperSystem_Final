using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Controllers
{
  
    public class AdminMonitoringController : Controller
    {
        // GET: AdminMonitoring
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();  
        }

        public ActionResult Revenue()
        {
            return View();
        }
    }
}