using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductionHoursLosses.Models.ViewModels
{
    public class HeaderViewModel
    {
        [Required(ErrorMessage = "Date is a mandatory field.")]
        public DateTime? SelectedDate { get; set; }

        [Required(ErrorMessage = "Factory is a mandatory field.")]
        public int? SelectedFactoryID { get; set; }
        public FACTORY SelectedFactory { get; set; }

        [Required(ErrorMessage = "Room is a mandatory field.")]
        public int? SelectedRoomID { get; set; }
        public ROOM SelectedRoom { get; set; }

        [Required(ErrorMessage = "Avail. Hours is a mandatory field.")]
        public int? SelectedAvailHours { get; set; }

        [Required(ErrorMessage = "Status is a mandatory field.")]
        public int? SelectedStatusID { get; set; }
        public STATUS SelectedStatus { get; set; }

        public HEADER HeaderModel { get; set; }

        [DataType(DataType.Time)]
        public DateTime? SelectedStartTime { get; set; }

        [DataType(DataType.Time)]
        public DateTime? SelectedEndTime { get; set; }
        public int? SelectedProductId { get; set; }
        public PRODUCT SelectedProduct { get; set; }
        public string SelectedBatchNo { get; set; }
        public string SelectedWorkOrder { get; set; }
        public int? SelectedShift { get; set; }
        public int? SelectedActualHours { get; set; }
        public decimal? SelectedUnitWeight { get; set; }

        [Range (0,100)]
        public int? SelectedSpeedMachineRpm { get; set; }
        public decimal? SelectedActualQuantity { get; set; }
        public int? SelectedNumPeople { get; set; }
        public int? SelectedUnits { get; set; }

        public int? SelectedLossesId { get; set; }
        public LOSSES SelectedLosses { get; set; } 
        public int? SelectedLossesMins { get; set; }

        public string SelectedDetailToBeDeleted { get; set; }
        public string SelectedDetailLossToBeDeleted { get; set; }

        public List<DetailExtended> DetailsList { get; set; }

        //public List<DETAIL_LOSSES> DetailLossesList { get; set; }

        public string SelectedDetailToAddLossAA { get; set; }
        
        public string SelectedDetailToUpdateAA { get; set; }
        [DataType(DataType.Time)]
        public DateTime? SelectedDetailToUpdateStartTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? SelectedDetailToUpdateEndTime { get; set; }
        public int? SelectedDetailToUpdateProductId { get; set; }
        public string SelectedDetailToUpdateBatchNo { get; set; }
        public string SelectedDetailToUpdateWorkOrder { get; set; }
        public int? SelectedDetailToUpdateShift { get; set; }
        public int? SelectedDetailToUpdateActualHours { get; set; }
        public decimal? SelectedDetailToUpdateUnitWeight { get; set; }

        [Range(0, 100)]
        public int? SelectedDetailToUpdateSpeedMachineRpm { get; set; }
        public decimal? SelectedDetailToUpdateActualQuantity { get; set; }
        public int? SelectedDetailToUpdateNumPeople { get; set; }
        public int? SelectedDetailToUpdateUnits { get; set; }

        public string SelectedDetailLossToUpdateAA { get; set; }
        public int? SelectedDetailLossToUpdateLossID { get; set; }
        public int? SelectedDetailLossToUpdateDuration { get; set; }

        public bool IsUpdate { get; set; }
        public string Password { get; set; }
        public int SelectedStep { get; set; }

    }
}