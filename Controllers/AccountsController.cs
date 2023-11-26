using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SBIT3J_SuperSystem_Final.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts

        DatabaseEntitiesConnection dbcon = new DatabaseEntitiesConnection();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        public ActionResult Login(EmployeeAccount employeeAccount)
        {
            if(ModelState.IsValid==true)
            {
                var credentials = dbcon.EmployeeAccounts.Where(model => model.Username == employeeAccount.Username
                                                   && model.Password == employeeAccount.Password).FirstOrDefault();

                if (credentials == null)
                {
                    ViewBag.ErrorMessage = "Incorrect Username or Password";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(employeeAccount.Username, false);
                    Session["Username"] = employeeAccount.Username;

                    if (credentials.Role == "Admin")  // <-- Move this line inside the if block
                    {
                        return RedirectToAction("Dashboard", "AdminMonitoring");
                    }
                    else if(credentials.Role == "Cashier")
                    {
                        return RedirectToAction("Index", "POS");
                    }
                    else if (credentials.Role == "StockManager")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    else if( credentials.Role == "Hr")
                    {
                        return RedirectToAction("Index", "EmployeeManagement");
                    }
                    else if ( credentials.Role == "Accountant")
                    {
                        return RedirectToAction("Index", "Payroll");
                    }
                    else
                    {
                        return RedirectToAction("Index","SuperAdmin");
                    }

                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}