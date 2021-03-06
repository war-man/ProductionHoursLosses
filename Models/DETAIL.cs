//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProductionHoursLosses.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DETAIL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DETAIL()
        {
            this.DETAIL_LOSSES = new HashSet<DETAIL_LOSSES>();
        }
    
        public int ID { get; set; }
        public int HEADER_ID { get; set; }
        public System.DateTime START_TIME { get; set; }
        public System.DateTime END_TIME { get; set; }
        public int PRODUCT_ID { get; set; }
        public string BATCH_NO { get; set; }
        public string WORK_ORDER { get; set; }
        public byte SHIFT { get; set; }
        public Nullable<byte> ACTUAL_HRS { get; set; }
        public Nullable<decimal> UNIT_WEIGHT { get; set; }
        public Nullable<byte> SPEED_MACHINE_RPM { get; set; }
        public Nullable<decimal> ACTUAL_QTY { get; set; }
        public Nullable<byte> NUM_PEOPLE { get; set; }
        public Nullable<int> UNITS { get; set; }
        public Nullable<byte> ACTUAL_MINS { get; set; }
    
        public virtual HEADER HEADER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETAIL_LOSSES> DETAIL_LOSSES { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
