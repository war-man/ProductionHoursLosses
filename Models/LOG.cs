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
    
    public partial class LOG
    {
        public int ID { get; set; }
        public string USER { get; set; }
        public System.DateTime DATE { get; set; }
        public string ACTION { get; set; }
        public string ELEMENT { get; set; }
        public int ELEMENT_ID { get; set; }
        public string OLD_VALUE { get; set; }
        public string NEW_VALUE { get; set; }
    }
}
