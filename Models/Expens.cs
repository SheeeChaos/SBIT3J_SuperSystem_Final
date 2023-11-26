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
    
    public partial class Expens
    {
        public int Expenses_ID { get; set; }
        public Nullable<int> Loss_Damage_ID { get; set; }
        public Nullable<int> Loss_Fraud_ID { get; set; }
        public Nullable<int> Electric_Bill { get; set; }
        public Nullable<int> Water_Bill { get; set; }
        public Nullable<int> Internet_Bill { get; set; }
        public Nullable<int> Payroll_Salary_ID { get; set; }
        public Nullable<int> Other_Services_ID { get; set; }
        public Nullable<System.DateTime> Date_Time { get; set; }
    
        public virtual Loss_Damages Loss_Damages { get; set; }
        public virtual Loss_Fraud Loss_Fraud { get; set; }
        public virtual Other_Services Other_Services { get; set; }
    }
}