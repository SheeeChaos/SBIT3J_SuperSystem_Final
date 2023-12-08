using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBIT3J_SuperSystem_Final.Repository
{
    public class RestockRepository
    {
        private DatabaseConnectionEntities objSBIT3JEntities;

        public RestockRepository()
        {
            objSBIT3JEntities = new DatabaseConnectionEntities();
        }
        public bool AddRestock(Restock objRestock)
        {
            Restock restock = objRestock;
            restock.Date = DateTime.Now;
            restock.Total_Amount = objRestock.Total_Amount;

            objSBIT3JEntities.Restocks.Add(restock);
            objSBIT3JEntities.SaveChanges();
            int RestockID = restock.Restock_ID;

            foreach (var item in objRestock.ListofRestockDetailViewModel)
            {
                Restock_Detail objRestock_Detail = new Restock_Detail();

                objRestock_Detail.Restock_ID = RestockID;
                objRestock_Detail.Product_ID = item.Product_ID;
                objRestock_Detail.Quantity_Added = item.Quantity_Added;
                objRestock_Detail.Total_Amount = item.Total_Amount;
                objSBIT3JEntities.Restock_Detail.Add(objRestock_Detail);
                objSBIT3JEntities.SaveChanges();

            }

            return true;
        }

    }
}