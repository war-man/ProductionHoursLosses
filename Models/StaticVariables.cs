using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionHoursLosses.Models
{
    public static class StaticVariables
    {
        public static string DomainName = "MEDOCHEMIE";
        public static string ImageFolderPath = HttpContext.Current.Server.MapPath("~/Content/Images/");
    }
}