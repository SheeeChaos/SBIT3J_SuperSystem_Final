using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Controllers
{
    [Authorize]
    public class EmployeeManagementController : Controller
    {
        // GET: EmployeeManagement
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SampleView()
        {
            return View();
        }
    }
}