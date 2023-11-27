using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Controllers
{

    [Authorize]
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

        public ActionResult SalesRevenue()
        {
            return View();
        }

        public ActionResult ProductRevenue()
        {
            return View();
        }

        public ActionResult Profit()
        {
            return View();
        }

        public ActionResult Losses()
        {
            return View();
        }

        public ActionResult CashierLogs()
        {
            return View();
        }

        public ActionResult OverallActivities ()
        {
            return View();
        }
        public ActionResult Inventory()
        {
            return View();
        }
    }
}