using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SBIT3J_SuperSystem_Final.Controllers
{

    public class AccountsController : Controller
    {
        // GET: Accounts

        DatabaseConnectionEntities dbcon = new DatabaseConnectionEntities();
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
            if (ModelState.IsValid)
            {
                var credentials = dbcon.EmployeeAccounts
                    .Where(model => model.Username == employeeAccount.Username && model.Password == employeeAccount.Password)
                    .FirstOrDefault();

                if (credentials == null)
                {
                    ViewBag.ErrorMessage = "Incorrect Username or Password";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(employeeAccount.Username, false);
                    Session["Username"] = employeeAccount.Username;

                    int accountId = credentials.Account_ID; // Assuming Account_ID is the primary key of the Account table
                    DateTime loginDateTime = DateTime.Now;
                    string activity = "Login"; // Specify the activity as "Login"

                    AddAuditTrail(accountId, activity, loginDateTime);

                    if (credentials.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "AdminMonitoring");
                    }
                    else if (credentials.Role == "Cashier")
                    {
                        return RedirectToAction("Index", "POS");
                    }
                    else if (credentials.Role == "StockManager")
                    {
                        return RedirectToAction("Index", "Inventory");
                    }
                    else if (credentials.Role == "Hr")
                    {
                        return RedirectToAction("Index", "EmployeeManagement");
                    }
                    else if (credentials.Role == "Accountant")
                    {
                        return RedirectToAction("Index", "Payroll");
                    }
                    else
                    {
                        return RedirectToAction("Index", "SuperAdmin");
                    }
                }
            }
            return View();
        }

        public void AddAuditTrail(int accountId, string activity, DateTime loginDateTime)  
        {
            try
            {
                AuditTrail auditTrail = new AuditTrail
                {
                    Account_ID = accountId,
                    Activity = activity,
                    Date = loginDateTime
                };

                dbcon.AuditTrails.Add(auditTrail);
                dbcon.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }

                throw;
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}