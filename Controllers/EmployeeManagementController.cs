using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SBIT3J_SuperSystem_Final.Controllers
{
    //[Authorize]
    
    public class EmployeeManagementController : Controller

    {
        DatabaseConnectionEntities dt = new DatabaseConnectionEntities();
        // GET: EmployeeManagement
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Time_In()
        {
            DatabaseConnectionEntities db = new DatabaseConnectionEntities();
            List<EmployeeInformation> emplist = new List<EmployeeInformation>();
            return View();
        }
        public ActionResult Time_Out()
        {
            return View();
        }
        public ActionResult Hr_Employee_L()
        {
            DatabaseConnectionEntities db = new DatabaseConnectionEntities();
            List<EmployeeInformation> emplist = db.EmployeeInformations.ToList();
            return View(db.EmployeeInformations.ToList());
        }
        public ActionResult Hr_Attendance_L()
        {
            DatabaseConnectionEntities db = new DatabaseConnectionEntities();
            List<Employee_Attendance> emplist = db.Employee_Attendance.ToList();
            return View(db.Employee_Attendance.ToList());
        }
        public ActionResult Hr_RequestLeave()
        {
            return View();
        }
        public ActionResult Create(EmployeeInformation dbe)
        {
            try
            {

                using (DatabaseConnectionEntities dbModel = new
                DatabaseConnectionEntities())

                {

                    dbModel.EmployeeInformations.Add(dbe);

                    dbModel.SaveChanges();

                }

                return RedirectToAction("Hr_Employee_L");

            }

            catch


            {

                return View(dbe);
            }
        }
        
        public ActionResult Details(int id)

        {

            using (DatabaseConnectionEntities dbModel = new
            DatabaseConnectionEntities())
            {
                return
              View(dbModel.EmployeeInformations.Where(x =>
              x.Employee_ID == id).FirstOrDefault());

            }

        }
        public ActionResult Edit(int id)
        {

            using (DatabaseConnectionEntities dbModel =
            new DatabaseConnectionEntities())

            {

                return
                View(dbModel.EmployeeInformations.Where(x
                => x.Employee_ID == id).FirstOrDefault());

            }

        }

        [HttpPost]

        public ActionResult Edit(int id, EmployeeInformation Empl)
        {

            try

            {

                using (DatabaseConnectionEntities dbModel = new
                DatabaseConnectionEntities())

                {

                    dbModel.Entry(Empl).State =
                    EntityState.Modified;

                    dbModel.SaveChanges();

                }

                return RedirectToAction("Hr_Employee_L");

            }

            catch

            {

                return View(Empl);

            }
        }
        public ActionResult Delete(int id)

        {

            using (DatabaseConnectionEntities dbModel = new
            DatabaseConnectionEntities())

            {

                return
                View(dbModel.EmployeeInformations.Where(x =>
                x.Employee_ID == id).FirstOrDefault());

            }

        }

        [HttpPost]

        public ActionResult Delete(int id, EmployeeInformation collection)

        {
            try
            {

                using (DatabaseConnectionEntities dbModel = new
                DatabaseConnectionEntities())

                {

                    EmployeeInformation Empl =
                    dbModel.EmployeeInformations.Where(x => x.Employee_ID ==
                    id).FirstOrDefault();

                    dbModel.EmployeeInformations.Remove(Empl);

                    dbModel.SaveChanges();

                }

                return RedirectToAction("Hr_Employee_L");

            }

            catch
            {

                return View();

            }
        }
    }
}