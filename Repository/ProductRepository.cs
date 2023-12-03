using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIT3J_SuperSystem_Final.Repository
{
    public class ProductRepository
    {
        private DatabaseConnectionEntities objSBIT3JEntities;

        public ProductRepository()
        {
            objSBIT3JEntities = new DatabaseConnectionEntities();
        }
        public IEnumerable<SelectListItem> GetAllProduct()
        {
            IEnumerable<SelectListItem> objSelectListItems = new List<SelectListItem>();
            objSelectListItems = (from obj in objSBIT3JEntities.Product_Info
                                  select new SelectListItem()
                                  {
                                      Text = obj.Product_Code,
                                      Value = obj.Product_ID.ToString(),
                                      Selected = true
                                  }).ToList();
            return objSelectListItems;
        }
    }
}