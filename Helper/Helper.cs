using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionHoursLosses.Helper
{
    public class Helper
    {

        public static string GetUserNameWithoutDomain(string username)
        {
            return username.Replace(@"MEDOCHEMIE\", "");
        }
    }
}