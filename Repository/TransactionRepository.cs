using SBIT3J_SuperSystem_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBIT3J_SuperSystem_Final.Repository
{
    public class TransactionRepository
    {

        private DatabaseConnectionEntities objSBIT3JEntities;

        public TransactionRepository()
        {
            objSBIT3JEntities = new DatabaseConnectionEntities();
        }
        public bool AddOrder(Sales_Transaction objSalesTransaction)
        {
            Sales_Transaction objSaleTransaction = objSalesTransaction;

            objSaleTransaction.Account_ID = objSalesTransaction.Account_ID;
            objSaleTransaction.Total_Amount = objSalesTransaction.Total_Amount;
            objSaleTransaction.Date = DateTime.Now;
            objSBIT3JEntities.Sales_Transaction.Add(objSaleTransaction);
            objSBIT3JEntities.SaveChanges();
            int TransactionID = objSaleTransaction.Transaction_ID;

            foreach (var item in objSaleTransaction.ListofOrderDetailViewModel)
            {
                Sales_Transaction_Details objSalesTransactionDetail = new Sales_Transaction_Details();
                objSalesTransactionDetail.Transaction_ID = TransactionID;
                objSalesTransactionDetail.Product_ID = item.Product_ID;
                objSalesTransactionDetail.Total_Quantity = (-1) * item.Total_Quantity;
                objSalesTransactionDetail.Discount_ID = item.Discount_ID;
                objSBIT3JEntities.Sales_Transaction_Details.Add(objSalesTransactionDetail);
                objSBIT3JEntities.SaveChanges();
            }

            return true;
        }
    }
}