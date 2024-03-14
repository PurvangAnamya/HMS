var Details = function (id) {
    var url = "/PatientTest/Details?id=" + id;
    $('#titleExtraBigModal').html("Patient Test Summary");
    loadExtraBigModal(url);
};


var AddEdit = function (id) {
    var url = "/PatientTest/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Patient Test");
    }
    else {
        $('#titleExtraBigModal').html("Add Patient Test");
    }
    loadExtraBigModal(url);
};


$("body").on("click", "#btnAddPatientTest", function () {

    if (!FieldValidation('#PatientId')) {
        FieldValidationAlert('#PatientId', 'Patient Name is Required.', "warning");
        return;
    }

    if (listPatientTestDetail.length == 0) {
        FieldValidationAlert('#LabTestsId', 'Please add at least one lab test item.', "warning");
        return;
    }

    var _PreparedFormObj = PreparedFormObj();
    if (parseFloat($("#Id").val()) > 0) {
        var list = new Array();
        for (var i = 0; i < listPatientTestDetail.length; i++) {
            var PatientTestDetail = {};
            PatientTestDetail.Id = listPatientTestDetail[i].Id;
            PatientTestDetail.Result = $("#Result" + listPatientTestDetail[i].Id).val();
            PatientTestDetail.Remarks = $("#Remarks" + listPatientTestDetail[i].Id).val();
            list.push(PatientTestDetail);
        }
        _PreparedFormObj.listPatientTestResultUpdateViewModel = list;
    }

    $.ajax({
        type: "POST",
        url: "/PatientTest/AddEdit",
        data: _PreparedFormObj,
        dataType: "json",
        success: function (result) {
            var message = 'Patient Test Created Successfully. ID: ' + result.Id;
            Swal.fire({
                title: message,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $('#tblPatientTest').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/PatientTest/Delete?id=" + id,
                success: function (result) {
                    var message = "Patient Test has been deleted successfully. PatientTest ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        text: 'Deleted!',
                        onAfterClose: () => {
                            $('#tblPatientTest').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};


var PreparedFormObj = function () {
    var _PatientTestCRUDViewModel = {
        Id: $("#Id").val(),
        CreatedDate: $("#CreatedDate").val(),
        CreatedBy: $("#CreatedBy").val(),

        PatientId: $("#PatientId").val(),
        ConsultantId: $("#ConsultantId").val(),
        TestDate: $("#TestDate").val(),
        DeliveryDate: $("#DeliveryDate").val(),
        PaymentStatus: $("#PaymentStatus").val(),
    };

    var SendObject = {
        PatientTestCRUDViewModel: _PatientTestCRUDViewModel,
        listPatientTestDetailCRUDViewModel: listPatientTestDetail,
        listPatientTestResultUpdateViewModel: null
    }
    return SendObject;
}