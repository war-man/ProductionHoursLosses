using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductionHoursLosses.Models
{
    public class FactoryMetadata
    {
        [StringLength(100)]
        [Display(Name = "Name")]
        public string NAME;

        [StringLength(300)]
        [Display(Name = "Address")]
        public string ADDRESS;
    }
}