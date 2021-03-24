using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductionHoursLosses.Models;

namespace ProductionHoursLosses.SharedHelp
{
    public class CommonFunctions
    {
        private static PRD_HRS_DBEntities db = new PRD_HRS_DBEntities();
        public static void CreateLog(string action, string element, int element_id, string old_val, string new_val, string user)
        {
            LOG CreateLog = new LOG();

            CreateLog.ACTION = action;
            CreateLog.DATE = System.DateTime.Now;
            CreateLog.ELEMENT = element;
            CreateLog.ELEMENT_ID = element_id;
            CreateLog.OLD_VALUE = old_val;
            CreateLog.NEW_VALUE = new_val;
            CreateLog.USER = user;
            db.LOG.Add(CreateLog);
            db.SaveChanges();
        }
    }
}