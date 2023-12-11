using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Geom;
using Rotativa;
using Rotativa.Options;
using System.Drawing;
using System.Net.NetworkInformation;

namespace SBIT3J_SuperSystem_Final.Controllers
{

    //[Authorize]
    public class AdminMonitoringController : Controller
    {
        // GET: AdminMonitoring

        DatabaseConnectionEntities dbcon = new DatabaseConnectionEntities(); //Connection String
        public ActionResult Index()
        {
            return View();
        }

        ////////////////           THIS PART IS FOR DASHBOARD                  //////////////////////////

        public ActionResult Dashboard()
        {
            using (dbcon)
            {
                var monthlySalesData = dbcon.Sales_Transaction
                    .Where(s => s.Date != null) // Exclude null dates
                    .GroupBy(s => new { Year = s.Date.Value.Year, Month = s.Date.Value.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalSales = g.Sum(s => s.Total_Amount) ?? 0 // Use 0 as the default value if Total_Amount is null
                    })
                    .AsEnumerable() // Switch to LINQ to Objects
                    .Select(g => new MonthlySalesViewModel
                    {
                        Month = $"{g.Year}-{g.Month}",
                        TotalSales = g.TotalSales
                    })
                    .OrderBy(g => g.Month)
                    .ToList();

                var salesGraphData = new SalesGraphViewModel
                {
                    MonthlySales = monthlySalesData
                };

                return View(salesGraphData);
            }
           
        }


        ////////////////           THIS PART IS FOR SALES REVENUE                   //////////////////////////

        public ActionResult SalesRevenue(string searchFilter, DateTime? startDate, DateTime? endDate, string filterType)
        {

            var query = from std in dbcon.Sales_Transaction_Details
                        join st in dbcon.Sales_Transaction on std.Transaction_ID equals st.Transaction_ID
                        join d in dbcon.Discounts on std.Discount_ID equals d.Discount_ID into discountGroup
                        from discount in discountGroup.DefaultIfEmpty()
                        select new SalesRevenueModel
                        {
                            TransactionDetailID = std.Transaction_Detail_ID,
                            TransactionID = st.Transaction_ID,
                            TotalAmount = st.Total_Amount,
                            DiscountAmount = discount != null ? discount.Discount_Amount : 0,
                            TotalSales = st.Total_Amount - (discount != null ? discount.Discount_Amount : 0),
                            Date = st.Date
                        };

            if (!string.IsNullOrEmpty(searchFilter) && filterType == "search")
            {
                query = query.Where(x =>
                    x.TransactionID.ToString().Contains(searchFilter) ||
                    x.TotalAmount.ToString().Contains(searchFilter)
                );
            }

            if (startDate != null && endDate != null && filterType == "date")
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            decimal? totalAmountSum = query.Sum(x => x.TotalAmount);
            ViewBag.totalSales = totalAmountSum;

            int totalCount = query.Count();
            ViewBag.TotalCount = totalCount;

            decimal? averageDailySales = totalCount > 0 ? totalAmountSum / totalCount : 0;
            ViewBag.AverageDailySales = averageDailySales;

            return View(query.ToList());
        }


        public ActionResult GenerateSalesTransactionList(string searchFilter, DateTime? startDate, DateTime? endDate)
        {
            var query = GetSalesTransactionFiltered(searchFilter, startDate, endDate);


            var pdfResult = new ViewAsPdf("SalesTransactionListPrintPdf", query)
            {
                FileName = "SalesTransactions.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                CustomSwitches = "--no-images", // Optional: Use this switch if you want to exclude images
            };

            return pdfResult;
        }

        private List<Sales_Transaction>GetSalesTransactionFiltered(string searchFilter, DateTime? startDate, DateTime? endDate)
        {
            var query = dbcon.Sales_Transaction.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(x =>
                    x.Transaction_ID.ToString().Contains(searchFilter) ||
                    x.Total_Amount.ToString().Contains(searchFilter)
                );
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            decimal? totalAmountSum = query.Sum(x => x.Total_Amount);
            ViewBag.totalSales = totalAmountSum;

            int totalCount = query.Count();
            ViewBag.TotalCount = totalCount;

            decimal? averageDailySales = totalCount > 0 ? totalAmountSum / totalCount : 0;
            ViewBag.AverageDailySales = averageDailySales;

            return query.ToList();
        }

        ////////////////           THIS PART IS FOR PRODUCT REVENUE                   //////////////////////////
        public ActionResult ProductRevenue()
        {
            var query = (from pi in dbcon.Product_Info
                                    join std in dbcon.Sales_Transaction_Details on pi.Product_ID equals std.Product_ID
                                    group new { pi, std } by new { pi.Product_Code, pi.Product_Name, pi.Price }
                                    into g
                                    select new ProductRevenueModel
                                    {
                                        Product_Code = g.Key.Product_Code,
                                        Product_Name = g.Key.Product_Name,
                                        Price = g.Key.Price,
                                        Total_Quantity = g.Sum(x => x.std.Total_Quantity),
                                        Total_Amount = g.Sum(x => x.std.Total_Quantity * x.pi.Price)
                                    }).ToList();

            return View(query);

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


        ////////////////           THIS PART IS FOR OVER ALL ACTIVITES               //////////////////////////
        public ActionResult OverallActivities(string searchFilter, DateTime? startDate, DateTime? endDate, string filterType)
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

            if (!string.IsNullOrEmpty(searchFilter) && filterType == "search")
            {
                query = query.Where(x =>
                    x.Employee_ID.ToString().Contains(searchFilter) ||
                    x.Name.ToLower().Contains(searchFilter) ||
                    x.Username.ToLower().Contains(searchFilter) ||
                    x.Role.ToLower().Contains(searchFilter)
                );
            }

            if (startDate != null && endDate != null && filterType == "date")
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            var auditTrailLogs = query.ToList();

            return View(auditTrailLogs);
        }

        public ActionResult GeneratePdf(string searchFilter, DateTime? startDate, DateTime? endDate)
        {
            var query = GetFilteredData(searchFilter, startDate, endDate);

            var pdfResult = new ViewAsPdf("PrintPdf", query)
            {
                FileName = "OverallActivities.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                CustomSwitches = "--no-images", // Optional: Use this switch if you want to exclude images
            };

            return pdfResult;
        }

        private List<OverallActivitiesModel> GetFilteredData(string searchFilter, DateTime? startDate, DateTime? endDate)
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

            // Apply filters
            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(x =>
                    x.Employee_ID.ToString().Contains(searchFilter) ||
                    x.Name.ToLower().Contains(searchFilter) ||
                    x.Username.ToLower().Contains(searchFilter) ||
                    x.Role.ToLower().Contains(searchFilter)
                );
            }

            if (startDate != null && endDate != null)
            {
                query = query.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            return query.ToList();
        }

        ////////////////           THIS PART IS FOR INVENTORY                  //////////////////////////
        public ActionResult Inventory(string searchFilter)
        {
            var query = dbcon.Product_Info.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                // Apply search filter
                query = query.Where(p =>
                    p.Product_Code.Contains(searchFilter) ||
                    p.Product_Name.Contains(searchFilter) ||
                    p.Category.Contains(searchFilter) ||
                    p.Price.ToString().Contains(searchFilter)
                );
            }
            return View(query.ToList());
        }

        private List<Product_Info> GetProductsFiltered(string searchFilter)
        {
            var query = dbcon.Product_Info.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                // Apply search filter
                query = query.Where(p =>
                    p.Product_Code.Contains(searchFilter) ||
                    p.Product_Name.Contains(searchFilter) ||
                    p.Category.Contains(searchFilter) ||
                    p.Price.ToString().Contains(searchFilter)
                );
            }
            return query.ToList();
        }

        public ActionResult GenerateInventoryList(string searchFilter)
        {
            var query = GetProductsFiltered(searchFilter);

            var pdfResult = new ViewAsPdf("InventoryListPrintPdf", query)
            {
                FileName = "InventoryList.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                CustomSwitches = "--no-images", // Optional: Use this switch if you want to exclude images
            };

            return pdfResult;
        }
    }
}