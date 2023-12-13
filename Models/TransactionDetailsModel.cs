using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBIT3J_SuperSystem_Final.Models
{
    public class TransactionDetailsModel
    {
        public int TransactionID { get; set; }
        public int TransactionDetailID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ValueAddedTax { get; set; }
    }
}