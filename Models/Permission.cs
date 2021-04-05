using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionHoursLosses.Models
{
    public class Permission
    {
        public class PermissionDetail
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private Permission(string name, string description)
        {
            Detail = new PermissionDetail();
            Detail.Description = description;
            Detail.Name = name;
        }

        public PermissionDetail Detail { get; set; }

        public static Permission ManageHeader { get { return new Permission("ManageHeader", "Manage Header"); } }

    }
}