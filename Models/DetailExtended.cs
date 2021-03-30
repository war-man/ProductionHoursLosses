using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionHoursLosses.Models
{
    public class DetailExtended : DETAIL
    {
        public Guid AA { get; set; }

        public List<DETAIL_LOSSES> DetailLossesList { get; set; }
        public int? SelectedLossesId { get; set; }
        public LOSSES SelectedLosses { get; set; }
        public int? SelectedLossesMins { get; set; }
    }
}