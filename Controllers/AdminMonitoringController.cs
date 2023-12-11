using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SBIT3J_SuperSystem_Final.Models.Sales_Transaction;

namespace SBIT3J_SuperSystem_Final.Controllers
{

    [Authorize]
    public class AdminMonitoringController : Controller
    {
        // GET: AdminMonitoring

        DatabaseConnectionEntities dbcon = new DatabaseConnectionEntities(); //Connection String
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            List<MonthlySalesData> monthlySales = new List<MonthlySalesData>();
            
            using (DatabaseConnectionEntities context = new DatabaseConnectionEntities())
            { // wala munang gagalaw nito -mark :>
                int[] salesPerMonth = new int[12];
                var query = context.Database.SqlQuery<MonthlySalesData>(@"
                    SELECT MONTH(Date) AS MonthNumber,
                           DATENAME(MONTH, Date) AS MonthName,
                           SUM(Total_Amount) AS TotalSales
                    FROM Sales_Transaction
                    WHERE YEAR(Date) = YEAR(GETDATE())
                    GROUP BY MONTH(Date), DATENAME(MONTH, Date)
                    ORDER BY MonthNumber;
                "); // sum all transactionshits each month of the current year

                monthlySales = query.ToList();
                for (int i = 0; i < monthlySales.Count; i++)
                {
                    salesPerMonth[monthlySales[i].MonthNumber - 1] = (int)(monthlySales[i].TotalSales ?? 0);
                }
            }

            return View(monthlySales);
        }


        public ActionResult SalesRevenue()
        {

            ViewBag.TotalCountEmployee = dbcon.EmployeeInformations.Count();
            return View();
        }

        public ActionResult ProductRevenue()
        {
            var employeeData = (from ei in dbcon.EmployeeInformations
                                join ea in dbcon.EmployeeAccounts on ei.Employee_ID equals ea.Employee_ID
                                select new EmployeeExampleInfo
                                {
                                    Employee_ID = ei.Employee_ID,
                                    First_Name = ei.First_Name,
                                    Username = ea.Username,
                                    Role = ea.Role
                                }).ToList();

            return View(employeeData);
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

            return View(dbcon.AuditTrails.ToList());
        }

        public ActionResult OverallActivities(string searchFilter)
        {
            var query = from ea in dbcon.EmployeeAccounts
                        join ei in dbcon.EmployeeInformations on ea.Employee_ID equals ei.Employee_ID
                        join at in dbcon.AuditTrails on ea.Account_ID equals at.Account_ID
                        select new OverallActivitiesModel
                        {
                            Employee_ID = ei.Employee_ID,
                            Name = ei.First_Name,
                            Username = ea.Username,
                            Role = ea.Role,
                            Activity = at.Activity,
                            Date = at.Date
                        };

            // Apply filter if roleFilter is not null or empty
            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(x =>
                                    x.Employee_ID.ToString().Contains(searchFilter) ||
                                    x.Name.ToLower().Contains(searchFilter) ||
                                    x.Username.ToLower().Contains(searchFilter) ||
                                    x.Role.ToLower().Contains(searchFilter)
        );
            }

            var auditTrailLogs = query.ToList();

            return View(auditTrailLogs);
        }


        public ActionResult Inventory()
        {
            return View();
        }
    }
}