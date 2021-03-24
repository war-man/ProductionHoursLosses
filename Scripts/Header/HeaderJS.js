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
    document.getElementById("divDetailLosses").style.display = "block";

    //if (document.getElementById("addDetail").clicked == true) {
    //    document.getElementById("divDetailLosses").style.display = "block";
    //}
    //else {
    //    document.getElementById("divDetailLosses").style.display = "none";
    //}
    
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