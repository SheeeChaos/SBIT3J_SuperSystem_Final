using SBIT3J_SuperSystem_Final.Models;
using SBIT3J_SuperSystem_Final.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Controllers
{
    //[Authorize]
    public class InventoryController : Controller
    {
        private DatabaseConnectionEntities objDatabaseConnectionEntities = new DatabaseConnectionEntities();


        // GET: Inventory
        public ActionResult Index()
        {
            return View(objDatabaseConnectionEntities.Sales_Transaction.ToList());

        }
        // GET: Product_Info/Details/5
        public ActionResult Sales_Transaction_Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Sales_Transaction_Details> Sales_Transaction_DetailsList = objDatabaseConnectionEntities.Sales_Transaction_Details
                                                                .Where(rd => rd.Transaction_ID == id)
                                                                .ToList();

            if (Sales_Transaction_DetailsList == null || Sales_Transaction_DetailsList.Count == 0)
            {
                return HttpNotFound();
            }
            ViewBag.transacID = id;

            return View(Sales_Transaction_DetailsList);
        }




        // **************************Products SECTION**************************



        public ActionResult Products()
        {
            var products = objDatabaseConnectionEntities.Product_Info
                              .Where(p => p.Is_Archived != true)
                              .ToList();


            return View(products);
        }


        // GET: Product_Info/Details/5
        public ActionResult ProductsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Info product_Info = objDatabaseConnectionEntities.Product_Info.Find(id);
            if (product_Info == null)
            {
                return HttpNotFound();
            }
            return View(product_Info);
        }

            // GET: Product_Info/Create
            public ActionResult ProductsCreate()
            {
                var hardcodedCategories = new List<string>
                {
                    "Top",
                    "Buttom",
                    "FootWear",
                    "HeadWear",
                    "Accessory"
                };
            var hardcodedSizes = new List<string>
                {
                    "XS",
                    "S",
                    "M",
                    "L",
                    "XL",
                    "XXL"
                };
            // Pass the products and categories to the view
            ViewBag.Categories = new SelectList(hardcodedCategories);
            ViewBag.Sizes = new SelectList(hardcodedSizes);
                return View();
            }

            // POST: Product_Info/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult ProductsCreate([Bind(Include = "Product_ID,Product_Code,Product_Name,Category,Size,Color,Sex,Is_Archived,Price,Stock_Level")] Product_Info product_Info)
            {
                if (ModelState.IsValid)
                {
                    objDatabaseConnectionEntities.Product_Info.Add(product_Info);
                    objDatabaseConnectionEntities.SaveChanges();
                    return RedirectToAction("Products");
                }

                return View(product_Info);
            }

        // GET: Product_Info/Edit/5
        public ActionResult ProductsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Info product_Info = objDatabaseConnectionEntities.Product_Info.Find(id);
            if (product_Info == null)
            {
                return HttpNotFound();
            }
            return View(product_Info);
        }

        // POST: Product_Info/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductsEdit([Bind(Include = "Product_ID,Product_Code,Product_Name,Category,Size,Color,Sex,Is_Archived,Price,Stock_Level")] Product_Info product_Info)
        {
            if (ModelState.IsValid)
            {
                objDatabaseConnectionEntities.Entry(product_Info).State = EntityState.Modified;
                objDatabaseConnectionEntities.SaveChanges();
                return RedirectToAction("Products");
            }
            return View(product_Info);
        }

        // GET: Product_Info/Delete/5
        public ActionResult ProductsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Info product_Info = objDatabaseConnectionEntities.Product_Info.Find(id);
            if (product_Info == null)
            {
                return HttpNotFound();
            }
            return View(product_Info);
        }

        // POST: Product_Info/Delete/5
        [HttpPost, ActionName("ProductsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Info product_Info = objDatabaseConnectionEntities.Product_Info.Find(id);
            objDatabaseConnectionEntities.Product_Info.Remove(product_Info);
            objDatabaseConnectionEntities.SaveChanges();
            return RedirectToAction("Products");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                objDatabaseConnectionEntities.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ProductsArchivedProduct()
        {
            var archivedProducts = objDatabaseConnectionEntities.Product_Info.Where(p => p.Is_Archived == true).ToList();

            return View(archivedProducts);
        }

        public ActionResult ArchiveProduct(int id)
        {
            var product = objDatabaseConnectionEntities.Product_Info.Find(id);

            if (product != null)
            {
                product.Is_Archived = true;

                objDatabaseConnectionEntities.SaveChanges();
            }
            return RedirectToAction("Products");
        }

        public ActionResult UnarchiveProduct(int id)
        {
            var product = objDatabaseConnectionEntities.Product_Info.Find(id);

            if (product != null)
            {
                product.Is_Archived = false;

                objDatabaseConnectionEntities.SaveChanges();
            }

            // Redirect to the action that displays the list of products
            return RedirectToAction("Products");
        }

        public JsonResult IsProductCodeExists(string productCode)
        {
            bool isExists = objDatabaseConnectionEntities.Product_Info.Any(p => p.Product_Code == productCode);

            return Json(!isExists, JsonRequestBehavior.AllowGet);
        }













        // **************************RESTOCK SECTION**************************





        public ActionResult Restock()
        {
            RestockRepository objProductRepository = new RestockRepository();
            var products = objProductRepository.GetAllProductforRestock();

            var restocks = objDatabaseConnectionEntities.Restocks.ToList();

            var objMultipleModel = new Tuple<IEnumerable<SelectListItem>, List<Restock>>(products, restocks);

            return View(objMultipleModel);
        }


        // GET: Product_Info/Details/5
        public ActionResult RestockDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Assuming that Restock_Detail has a property named restock_ID
            List<Restock_Detail> restockDetailsList = objDatabaseConnectionEntities.Restock_Detail
                                                                .Where(rd => rd.Restock_ID == id)
                                                                .ToList();

            if (restockDetailsList == null || restockDetailsList.Count == 0)
            {
                return HttpNotFound();
            }

            return View(restockDetailsList);
        }



        [HttpPost]
        public ActionResult AddRestock(Restock objRestock)
        {
            try
            {
                RestockRepository objTransactionRepository = new RestockRepository();
                objTransactionRepository.AddRestock(objRestock);
                return Json("Restock saved!");
            }
            catch (Exception ex)
            {
                return Json($"Error saving restock: {ex.Message}");
            }
        }


        [HttpGet]
        public JsonResult getItemUnitPrice(int ProductID)
        {
            decimal price = (decimal)objDatabaseConnectionEntities.Product_Info.Single(model => model.Product_ID == ProductID).Price;
            return Json(price, JsonRequestBehavior.AllowGet);

        }




















        // **************************ReturnsAndRefunds SECTION**************************




        // GET: Return_Item
        public ActionResult ReturnsAndRefunds()
        {
            var return_Item = objDatabaseConnectionEntities.Return_Item.Include(r => r.Product_Info).Include(r => r.Sales_Transaction);
            return View(return_Item.ToList());
        }

        // GET: Return_Item/Details/5
        public ActionResult ReturnsAndRefundsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Return_Item return_Item = objDatabaseConnectionEntities.Return_Item.Find(id);
            if (return_Item == null)
            {
                return HttpNotFound();
            }
            return View(return_Item);
        }

        // GET: Return_Item/Create
        public ActionResult ReturnsAndRefundsCreate()
        {
            ViewBag.Product_ID = new SelectList(objDatabaseConnectionEntities.Product_Info, "Product_ID", "Product_Code");
            ViewBag.Transaction_ID = new SelectList(objDatabaseConnectionEntities.Sales_Transaction, "Transaction_ID", "Transaction_ID");
            return View();
        }

        // POST: Return_Item/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnsAndRefundsCreate([Bind(Include = "Return_ID,Transaction_ID,Product_ID,Return_Date,Quantity_Returned,Reason,Resellable,Customer_Name")] Return_Item return_Item)
        {
            if (ModelState.IsValid)
            {
                objDatabaseConnectionEntities.Return_Item.Add(return_Item);
                objDatabaseConnectionEntities.SaveChanges();
                return RedirectToAction("ReturnsAndRefunds");
            }

            ViewBag.Product_ID = new SelectList(objDatabaseConnectionEntities.Product_Info, "Product_ID", "Product_Code", return_Item.Product_ID);
            ViewBag.Transaction_ID = new SelectList(objDatabaseConnectionEntities.Sales_Transaction, "Transaction_ID", "Transaction_ID", return_Item.Transaction_ID);
            return View(return_Item);
        }

        // GET: Return_Item/Edit/5
        public ActionResult ReturnsAndRefundsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Return_Item return_Item = objDatabaseConnectionEntities.Return_Item.Find(id);
            if (return_Item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Product_ID = new SelectList(objDatabaseConnectionEntities.Product_Info, "Product_ID", "Product_Code", return_Item.Product_ID);
            ViewBag.Transaction_ID = new SelectList(objDatabaseConnectionEntities.Sales_Transaction, "Transaction_ID", "Transaction_ID", return_Item.Transaction_ID);
            return View(return_Item);
        }

        // POST: Return_Item/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnsAndRefundsEdit([Bind(Include = "Return_ID,Transaction_ID,Product_ID,Return_Date,Quantity_Returned,Reason,Resellable,Customer_Name")] Return_Item return_Item)
        {
            if (ModelState.IsValid)
            {
                objDatabaseConnectionEntities.Entry(return_Item).State = EntityState.Modified;
                objDatabaseConnectionEntities.SaveChanges();
                return RedirectToAction("ReturnsAndRefunds");
            }
            ViewBag.Product_ID = new SelectList(objDatabaseConnectionEntities.Product_Info, "Product_ID", "Product_Code", return_Item.Product_ID);
            ViewBag.Transaction_ID = new SelectList(objDatabaseConnectionEntities.Sales_Transaction, "Transaction_ID", "Transaction_ID", return_Item.Transaction_ID);
            return View(return_Item);
        }

        // GET: Return_Item/Delete/5
        public ActionResult ReturnsAndRefundsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Return_Item return_Item = objDatabaseConnectionEntities.Return_Item.Find(id);
            if (return_Item == null)
            {
                return HttpNotFound();
            }
            return View(return_Item);
        }

        // POST: Return_Item/Delete/5
        [HttpPost, ActionName("ReturnsAndRefundsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ReturnsAndRefundsDeleteConfirmed(int id)
        {
            Return_Item return_Item = objDatabaseConnectionEntities.Return_Item.Find(id);
            objDatabaseConnectionEntities.Return_Item.Remove(return_Item);
            objDatabaseConnectionEntities.SaveChanges();
            return RedirectToAction("ReturnsAndRefunds");
        }









        // **************************ManageDiscount SECTION**************************

        public ActionResult ManageDiscount()
        {
            DateTime targetDate = new DateTime(2023, 1, 20);

            var discounts = objDatabaseConnectionEntities.Discounts
                .Where(p => p.End_Date > targetDate)
                .ToList();

            return View(discounts);
        }

        public ActionResult ManageDiscountCreate()
        {
            return View();
        }

        // POST: Discount/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageDiscountCreate([Bind(Include = "Discount_ID,Discount_Name,Description,Discount_Amount,Start_Date,End_Date")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                objDatabaseConnectionEntities.Discounts.Add(discount);
                objDatabaseConnectionEntities.SaveChanges();
                return RedirectToAction("ManageDiscount");
            }

            return View(discount);
        }

        // GET: Discount/Edit/5
        public ActionResult ManageDiscountEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discount discount = objDatabaseConnectionEntities.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }

            return View(discount);
        }

        // POST: Discount/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageDiscountEdit([Bind(Include = "Discount_ID,Discount_Name,Description,Discount_Amount,Start_Date,End_Date")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                objDatabaseConnectionEntities.Entry(discount).State = EntityState.Modified;
                objDatabaseConnectionEntities.SaveChanges();
                return RedirectToAction("ManageDiscount");
            }

            return View(discount);
        }

        // GET: Discount/Delete/5
        public ActionResult ManageDiscountDelete(int? id)
        {
            Discount discount = objDatabaseConnectionEntities.Discounts.Find(id);
            objDatabaseConnectionEntities.Discounts.Remove(discount);
            objDatabaseConnectionEntities.SaveChanges();
            return RedirectToAction("ManageDiscount");
        }
        public ActionResult DisabledDiscount()
        {
            DateTime targetDate = new DateTime(2023, 1, 20);

            var DisabledDiscount = objDatabaseConnectionEntities.Discounts
                .Where(p => p.End_Date < targetDate)
                .ToList();

            return View(DisabledDiscount);
        }
        public ActionResult ManageDiscountdisable(int? id)
        {

                Discount discount = objDatabaseConnectionEntities.Discounts.Find(id);

                if (discount == null)
                {
                    return HttpNotFound();
                }

                if (discount.End_Date.HasValue)
                {
                    discount.End_Date = new DateTime(2022, discount.End_Date.Value.Month, discount.End_Date.Value.Day);
                }

                objDatabaseConnectionEntities.SaveChanges();


            return RedirectToAction("ManageDiscount");
        }
        public ActionResult ManageDiscountUndisable(int? id)
        {

            Discount discount = objDatabaseConnectionEntities.Discounts.Find(id);

            if (discount == null)
            {
                return HttpNotFound();
            }

            if (discount.End_Date.HasValue)
            {
                discount.End_Date = new DateTime(2023, discount.End_Date.Value.Month, discount.End_Date.Value.Day);
            }

            objDatabaseConnectionEntities.SaveChanges();
            return RedirectToAction("DisabledDiscount");
        }



    }
}