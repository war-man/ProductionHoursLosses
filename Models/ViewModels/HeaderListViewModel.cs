using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductionHoursLosses.Models.ViewModels
{
    public class HeaderListViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? SelectedStartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? SelectedEndDate { get; set; }
        public List<ItemIdName> SelectedHeaderStatusList { get; set; }
        public string SelectedHeaderStatusIds { get; set; }
        public List<ItemIdName> SelectedHeaderFactoryList { get; set; }
        public string SelectedHeaderFactoryIds { get; set; }
        public List<ItemIdName> SelectedHeaderRoomList { get; set; }
        public string SelectedHeaderRoomIds { get; set; }

        public PagedList.IPagedList<HEADER> HeaderList { get; set; }

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public bool IsNextPageRequest { get; set; }

        public string Password { get; set; }
    }

    public class ItemIdName
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}