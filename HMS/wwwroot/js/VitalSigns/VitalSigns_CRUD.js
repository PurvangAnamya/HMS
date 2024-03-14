var Details = function (id) {
    var url = "/CheckupSummary/Details?id=" + id;
    $('#titleExtraBigModal').html("Checkup Details");
    loadExtraBigModal(url);
};


var AddEdit = function (id) {
    var url = "/VitalSigns/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Vital Signs");
    }
    else {
        $('#titleMediumModal').html("Add Vital Signs");
    }
    loadMediumModal(url);
};

