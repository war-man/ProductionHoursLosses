$('#SelectedDate').change(function () {
    var date = document.getElementById("SelectedDate").value;
    ("#SelectedDate").val(date);

});

function OnDropDownFactoryChange() {
    var selectItem = $('#SelectedFactoryDropDownID').val();
    $('#SelectedFactoryID').val(selectItem);

    // reset room id 
    $('#SelectedRoomDropDownId').val(null).trigger('change');

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}


$("#SelectedFactoryDropDownID").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetFactoryList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a factory',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectItemId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function formatRepoSelection(repo) {
    return repo.text;
}

function formatRepoSelectItemId(repo) {
    if (repo.loading) {
        return repo.text;
    }

    var markup = "<div class='select2-result-repository clearfix'>" +
        "<div class='select2-result-repository__meta'>" +
        "<div class='select2-result-repository__title'>" + repo.text + "</div>" +
        "<div class='select2-result-repository__description'>" + repo.itemDescription + "</div>" +
        "<div class='select2-result-repository__statistics'>" +
        "</div>" +
        "</div></div>";

    return markup;
}

function OnDropDownRoomChange() {
    var selectItem = $('#SelectedRoomDropDownId').val();
    $('#SelectedRoomID').val(selectItem);

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}


$("#SelectedRoomDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetRoomList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1,
                FactoryId: params.FactoryId = $('#SelectedFactoryID').val()
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a room',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectItemId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});


function formatRepoSelectId(repo) {
    if (repo.loading) {
        return repo.text;
    }

    var markup = "<div class='select2-result-repository clearfix'>" +
        "<div class='select2-result-repository__meta'>" +
        "<div class='select2-result-repository__title'>" + repo.text + "</div>" +
        "<div class='select2-result-repository__statistics'>" +
        "</div>" +
        "</div></div>";

    return markup;
}


function OnDropDownStatusChange() {
    var selectItem = $('#SelectedStatusDropDownId').val();
    $('#SelectedStatusID').val(selectItem);

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}

$("#SelectedStatusDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetStatusList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a status',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});


function OnDropDownAvailHoursChange() {
    var selectItem = $('#SelectedAvailHoursDropDownId').val();
    $('#SelectedAvailHours').val(selectItem);

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}

$("#SelectedAvailHoursDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetAvailHrsList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for avail. hrs',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});


function OnDropDownProductChange() {
    var selectItem = $('#SelectedProductDropDownId').val();
    $('#SelectedProductId').val(selectItem);
}

$("#SelectedProductDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetProductList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a product',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function AddDetailClicked() {
    document.getElementById("divDetails").style.display = "block";

    //if (document.getElementById("addDetail").clicked == true) {
    //    document.getElementById("divDetailLosses").style.display = "block";
    //}
    //else {
    //    document.getElementById("divDetailLosses").style.display = "none";
    //}
    
}

function AddDetailLossesClicked() {
    document.getElementById("divDetailLosses").style.display = "block";
}

function OnDropDownShiftChange() {
    var selectItem = $('#SelectedShiftDropDownId').val();
    $('#SelectedShift').val(selectItem);

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}

$("#SelectedShiftDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetShiftList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for shift',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function OnDropDownActualHoursChange() {
    var selectItem = $('#SelectedActualHoursDropDownId').val();
    $('#SelectedActualHours').val(selectItem);

    //if (selectItem !== "") {
    //    GetUpdateSelectedMasterDetails();
    //}
}

$("#SelectedActualHoursDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetAvailHrsList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for actual hrs',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function OnDropDownLossesChange() {
    var selectItem = $('#SelectedLossesDropDownId').val();
    $('#SelectedLossesId').val(selectItem);
}

$("#SelectedLossesDropDownId").select2({
    width: '500px',
    ajax: {
        url: "/Header/GetLossesList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a loss',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectItemId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function DeleteDetail(aa) {
    var result = confirm("Are you sure you Want to Delete Selected Record?");
    if (result) {
        $('#SelectedDetailToBeDeleted').val(aa);
        document.getElementById("formSubmit").submit();
    }
}

function DeleteDetailLoss(aa, detaa) {
    var result = confirm("Are you sure you Want to Delete Selected Record?");
    if (result) {
        $('#SelectedDetailLossToBeDeleted').val(aa);
        $('#SelectedDetailToBeDeleted').val(detaa);
        document.getElementById("formSubmit").submit();
    }
}


function Update() {
    var aa = $('#editTableRow #RowTableMasterAA').val();
    var startTime = $('#editTableRow #StartTimeModalFormId').val();
    var endTime = $('#editTableRow #EndTimeModalFormId').val();
    var product = $('#editTableRow #ProductIdModalFormId').val();
    var batchNo = $('#editTableRow #BatchNoModalFormId').val();
    var workOrder = $('#editTableRow #WorkOrderModalFormId').val();
    var shift = $('#editTableRow #ShiftModalFormId').val();
    var actualHours = $('#editTableRow #ActualHrsModalFormId').val();
    var unitWeight = $('#editTableRow #UnitWeightModalFormId').val();
    var speedMachine = $('#editTableRow #SpeedMachineModalFormId').val();
    var actualQty = $('#editTableRow #ActualQtyModalFormId').val();
    var numPeople = $('#editTableRow #NumPeopleModalFormId').val();
    var units = $('#editTableRow #UnitsModalFormId').val();


    $('#SelectedDetailToUpdateAA').val(aa);
    $('#SelectedDetailToUpdateStartTime').val(startTime);
    $('#SelectedDetailToUpdateEndTime').val(endTime);
    $('#SelectedDetailToUpdateProductId').val(product);
    $('#SelectedDetailToUpdateBatchNo').val(batchNo);
    $('#SelectedDetailToUpdateWorkOrder').val(workOrder);
    $('#SelectedDetailToUpdateShift').val(shift);
    $('#SelectedDetailToUpdateActualHours').val(actualHours);
    $('#SelectedDetailToUpdateUnitWeight').val(unitWeight);
    $('#SelectedDetailToUpdateSpeedMachineRpm').val(speedMachine);
    $('#SelectedDetailToUpdateActualQuantity').val(actualQty);
    $('#SelectedDetailToUpdateNumPeople').val(numPeople);
    $('#SelectedDetailToUpdateUnits').val(units);

    document.getElementById("formSubmit").submit();
}

function UpdateDetailLoss() {
    var aa = $('#editTableLossesRow #RowTableDetailAA').val();
    var detaa = $('#editTableLossesRow #RowTableDetailLossAA').val();
    var lossid = $('#editTableLossesRow #LossModalFormId').val();
    var duration = $('#editTableLossesRow #LossDurationModalFormId').val();

    $('#SelectedDetailLossToUpdateAA').val(aa);
    $('#SelectedDetailToUpdateAA').val(detaa);
    $('#SelectedDetailLossToUpdateLossID').val(lossid);
    $('#SelectedDetailLossToUpdateDuration').val(duration);

    document.getElementById("formSubmit").submit();
}

function OnDropDownProductIdChangeCreateDetailFromModal() {
    var selectItem = $("#SelectProductIdDropDownIdCreateDetailFromModal").val();
    $('#SelectedProductId').val(selectItem);
}

$("#SelectProductIdDropDownIdCreateDetailFromModal").select2({
    width: '300px',
    ajax: {
        url: "/Header/GetProductList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a product',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function OnDropDownShiftChangeCreateDetailFromModal() {
    var selectItem = $("#SelectShiftDropDownIdCreateDetailFromModal").val();
    $('#SelectedShift').val(selectItem);
}

$("#SelectShiftDropDownIdCreateDetailFromModal").select2({
    width: '150px',
    ajax: {
        url: "/Header/GetShiftList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for shift',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});


function OnDropDownActualHrsChangeCreateDetailFromModal() {
    var selectItem = $("#SelectActualHrsDropDownIdCreateDetailFromModal").val();
    $('#SelectedActualHours').val(selectItem);
}

$("#SelectActualHrsDropDownIdCreateDetailFromModal").select2({
    width: '150px',
    ajax: {
        url: "/Header/GetAvailHrsList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for actual hrs',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

$(document).ready(function () {
    $('#editTableRow').on('show.bs.modal', function (e) {
        var aa = $(e.relatedTarget).data('id');
        var startTime = $(e.relatedTarget).data('starttime');
        var endTime = $(e.relatedTarget).data('endtime');
        var product = $(e.relatedTarget).data('productid');
        var productName = $(e.relatedTarget).data('productname');
        var batchNo = $(e.relatedTarget).data('batchno');
        var workOrder = $(e.relatedTarget).data('workorder');
        var shift = $(e.relatedTarget).data('shift');
        var actualHours = $(e.relatedTarget).data('actualhours');
        var unitWeight = $(e.relatedTarget).data('unitweight');
        var speedMachine = $(e.relatedTarget).data('speedmachine');
        var actualQty = $(e.relatedTarget).data('actualqty');
        var numPeople = $(e.relatedTarget).data('numpeople');
        var units = $(e.relatedTarget).data('units');


        $('#editTableRow #RowTableMasterAA').val(aa);
        $('#editTableRow #StartTimeModalFormId').val(startTime);
        $('#editTableRow #EndTimeModalFormId').val(endTime);
        $('#editTableRow #ProductIdFormId').val(product);
        $('#editTableRow #ProductNameFormId').val(productName);
        $('#editTableRow #BatchNoModalFormId').val(batchNo);
        $('#editTableRow #WorkOrderModalFormId').val(workOrder);
        $('#editTableRow #ShiftModalFormId').val(shift);
        $('#editTableRow #ActualHrsModalFormId').val(actualHours);
        $('#editTableRow #UnitWeightModalFormId').val(unitWeight);
        $('#editTableRow #SpeedMachineModalFormId').val(speedMachine);
        $('#editTableRow #ActualQtyModalFormId').val(actualQty);
        $('#editTableRow #NumPeopleModalFormId').val(numPeople);
        $('#editTableRow #UnitsModalFormId').val(units);

        var selectOption = new Option(productName, product, true, true);
        $('#SelectProductIdDropDownIdCreateDetailFromModal').append(selectOption).trigger('change');

        var selectOption2 = new Option((actualHours+" hrs"), actualHours, true, true);
        $('#SelectActualHrsDropDownIdCreateDetailFromModal').append(selectOption2).trigger('change');

        var shiftname;
        if (shift == '1')
            shiftname = "1st";
        else if (shift == '2')
            shiftname = "2nd";
        else if (shift == '3')
            shiftname = "3rd";

        var selectOption3 = new Option(shiftname, shift, true, true);
        $('#SelectShiftDropDownIdCreateDetailFromModal').append(selectOption3).trigger('change');

        document.getElementById("modalOK").disabled = false;
    });
});

$(document).ready(function () {
    $('#editTableLossesRow').on('show.bs.modal', function (e) {
        $.fn.bootstrapBtn = $.fn.button.noConflict();
        var aa = $(e.relatedTarget).data('id');
        var detaa = $(e.relatedTarget).data('detailid');
        var lossid = $(e.relatedTarget).data('lossid');
        var lossname = $(e.relatedTarget).data('lossname');
        var duration = $(e.relatedTarget).data('duration');

        $('#editTableLossesRow #RowTableDetailAA').val(aa);
        $('#editTableLossesRow #RowTableDetailLossAA').val(detaa); 
        $('#editTableLossesRow #LossDurationModalFormId').val(duration); 

        var selectOption = new Option(lossname, lossid, true, true);
        $('#SelectLossIdDropDownIdCreateDetailLossFromModal').append(selectOption).trigger('change');

        document.getElementById("modal2OK").disabled = true;
    });
});


$(document).ready(function () {
    $('#addDetailLoss').on('show.bs.modal', function (e) {
        $.fn.bootstrapBtn = $.fn.button.noConflict();
        var aa = $(e.relatedTarget).data('id');

        $('#addDetailLoss #RowTableMasterAA').val(aa);

        document.getElementById("modal2OK").disabled = true;
    });
});


$(function () {
    $('#LossDurationModalFormId').change(function () {
        CheckDuration();
    });
});

function CheckDuration() {
    var duration = document.getElementById("LossDurationModalFormId").value;
    if (duration > 0) {
        document.getElementById("modal2OK").disabled = false;
    }
}

function OnDropDownLossIdChangeCreateDetailLossFromModal() {
    var selectItem = $("#SelectLossIdDropDownIdCreateDetailLossFromModal").val();
    $('#SelectedLossesId').val(selectItem);
    CheckDuration();
}

$("#SelectLossIdDropDownIdCreateDetailLossFromModal").select2({
    width: '450px',
    ajax: {
        url: "/Header/GetLossesList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a loss',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectItemId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});

function AddDetailLoss() {
    var aa = $('#addDetailLoss #RowTableMasterAA').val();
    var lossid = $('#SelectedLossesId').val();
    var duration = $('#addDetailLoss #LossDurationModalFormId').val();


    $('#SelectedDetailToAddLossAA').val(aa);
    $('#SelectedLossesId').val(lossid);
    $('#SelectedLossesMins').val(duration);

    document.getElementById("formSubmit").submit();
}

function OnDropDownLossIdChangeUpdateDetailLossFromModal() {
    var selectItem = $("#SelectLossIdDropDownIdUpdateDetailLossFromModal").val();
    $('#SelectedDetailLossToUpdateLossID').val(selectItem);
    CheckDuration();
}

$("#SelectLossIdDropDownIdUpdateDetailLossFromModal").select2({
    width: '450px',
    ajax: {
        url: "/Header/GetLossesList",
        dataType: 'json',
        type: "POST",
        data: function (params) {
            return {
                q: params.term, // search term
                page: params.page || 1
            };
        },
        ProcessResults: function (data, params) {
            params.page = params.page || 1;

            return {
                results: data.items,
                pagination: {
                    more: (params.page * 30) < data.total_count
                }
            };
        },
        cache: true
    },
    placeholder: 'Search for a loss',
    escapeMarkup: function (markup) { return markup; },
    minimumInputLength: -1,
    templateResult: formatRepoSelectItemId,
    templateSelection: formatRepoSelection,
    allowClear: true,
    multiple: true,
    maximumSelectionLength: 1
});