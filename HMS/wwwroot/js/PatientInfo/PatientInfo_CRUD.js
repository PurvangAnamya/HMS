var DetailsPatientInfo = function (id) {
    var url = "/PatientInfo/Details?id=" + id;
    $('#titleExtraBigModal').html("Patient Info Details");
    loadExtraBigModal(url);
};


var AddEditPatientInfo = function (id) {
    var url = "/PatientInfo/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Patient Info");
    }
    else {
        $('#titleExtraBigModal').html("Add Patient Info");
    }
    loadExtraBigModal(url);
};

var AddCheckup = function (id) {
    var url = "/CheckupSummary/AddFromPatientInfo?id=" + id;
    $('#titleExtraBigModal').html("Add Checkup");
    loadExtraBigModal(url);
};


var SavePatientInfo = function () {
    if (!$("#frmPatientInfo").valid()) {
        return;
    }
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _frmPatientInfo = $("#frmPatientInfo").serialize();
    $.ajax({
        type: "POST",
        url: "/PatientInfo/AddEdit",
        data: _frmPatientInfo,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblPatientInfo').DataTable().ajax.reload();
                });
            }
            else {
                SwalSimpleAlert(result.AlertMessage, "warning");
            }
            $("#btnSave").val("Save");
            $('#btnSave').removeAttr('disabled');
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeletePatientInfo = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/PatientInfo/Delete?id=" + id,
                success: function (result) {
                    var message = "Patient Info has been deleted successfully. Currency ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblPatientInfo').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};

