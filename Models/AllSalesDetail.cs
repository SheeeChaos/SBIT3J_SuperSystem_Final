//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SBIT3J_SuperSystem_Final.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AllSalesDetail
    {
        public int Transaction_ID { get; set; }
        public int Transaction_Detail_ID { get; set; }
        public string Product_Code { get; set; }
        public string Product_Name { get; set; }
        public Nullable<int> Total_Quantity { get; set; }
        public Nullable<decimal> Capital_Price { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Discount_Amount { get; set; }
        public Nullable<decimal> Total_Capital { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
