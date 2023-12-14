using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Repository
{
    public class DiscountRepository
    {

        private DatabaseConnectionEntities objSBIT3JEntities;

        public DiscountRepository()
        {
            objSBIT3JEntities = new DatabaseConnectionEntities();
        }
        public IEnumerable<SelectListItem> GetAllDiscount()
        {
            IEnumerable<SelectListItem> objSelectListItems = new List<SelectListItem>();

            // Get the current year
            DateTime currentYear = new DateTime(2023, 1, 1);

            objSelectListItems = (from obj in objSBIT3JEntities.Discounts
                                  where obj.End_Date.HasValue && obj.End_Date.Value >= currentYear
                                  select new SelectListItem()
                                  {
                                      Text = obj.Discount_Name,
                                      Value = obj.Discount_ID.ToString(),
                                      Selected = true
                                  }).ToList();

            return objSelectListItems;
        }


    }
}