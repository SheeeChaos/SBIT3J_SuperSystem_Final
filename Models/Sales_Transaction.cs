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
    
    public partial class Sales_Transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sales_Transaction()
        {
            this.Return_Item = new HashSet<Return_Item>();
            this.Sales_Transaction_Details = new HashSet<Sales_Transaction_Details>();
        }
    
        public int Transaction_ID { get; set; }
        public Nullable<int> Account_ID { get; set; }
        public Nullable<decimal> Total_Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public IEnumerable<Sales_Transaction_Details> ListofOrderDetailViewModel { get; set; }
        public string Account_name { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual EmployeeAccount EmployeeAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Return_Item> Return_Item { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sales_Transaction_Details> Sales_Transaction_Details { get; set; }
    }
}
