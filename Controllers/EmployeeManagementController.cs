using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace SBIT3J_SuperSystem_Final.Controllers
{
 [Authorize]
 public class EmployeeManagementController : Controller

    {
        private DatabaseConnectionEntities dt = new DatabaseConnectionEntities();
        // GET: EmployeeManagement
        public ActionResult Index()
        {

            return View();
        }

        /* ONGOING TIME IN AND TIME OUT */
        [HttpPost]
        public ActionResult TimeInn(int employeeId)
        {
            var timeLog = new Employee_Attendance
            {
                Account_ID = employeeId,
                Time_In = DateTime.Now
            };
            dt.Employee_Attendance.Add(timeLog);
            dt.SaveChanges();
            return Json(new { message = "Time In recorded successfully!" });
        }
        [HttpPost]
        public ActionResult TimeOutt(int employeeId)
        {
            /*var timeLog = dt.Employee_Attendance.FirstOrDefault(t => t.Account_ID == employeeId && t.Time_Out == null);
            if (timeLog == null)
            {
                return Json(new { message = "Employee has not clocked in yet!" });
            }

            timeLog.Time_Out = DateTime.Now;
            timeLog.Total_Hour_Worked = Employee_Attendance.CalculateTotalHours(timeLog.Time_In, timeLog.Time_Out);
            dt.SaveChanges();
            return Json(new { message = "Time Out recorded successfully!", totalHours = timeLog.Total_Hour_Worked });*/
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
            //TimeSpan totalHours = Time_In - Time_Out;
            // = Math.Floor(totalHours.TotalHours); // Ignore minutes and seconds
            // Employee_Attendance. = totalHours;
            return View(db.Employee_Attendance.ToList());
        }
        public ActionResult Hr_RequestLeave()
        {
            DatabaseConnectionEntities dbe = new DatabaseConnectionEntities();
            List<Leave_Request> Lr = dbe.Leave_Request.ToList();

            return View(dbe.Leave_Request.ToList());
        }

        public ActionResult HR_Leave()
        {
            Leave_Request model = new Leave_Request();
            return View(model);
        }

        [HttpPost]
        public ActionResult HR_Leave(Leave_Request model)
        {
            Leave_Request dbe = new Leave_Request();
            if (ModelState.IsValid)
            {
                // Save the data to the database
                dt.Leave_Request.Add(model);
                dt.SaveChanges();

                // Redirect to a success page or perform other actions
                return RedirectToAction("Success");
            }

            // If the model state is not valid, return to the form with validation errors
            return View(model);
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
        
        /*VVV-- Dito ako nagkakaproblema once na debug run mo siya, ayaw magsubmit ng data..
         * kasi kapag nagsubmit ka dapat mapupunta sa Employee List as indicator kung gumagana --VVV*/
        public ActionResult HR_CreateAccount(EmployeeAccount dta)
        {
            try 
            {
                using (DatabaseConnectionEntities dbModel = new DatabaseConnectionEntities())
                {
                    dbModel.EmployeeAccounts.Add(dta);
                    dbModel.SaveChanges();
                }
                return RedirectToAction("Hr_Employee_L");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                
                return View(dta);
            }
        }
    }

    }
