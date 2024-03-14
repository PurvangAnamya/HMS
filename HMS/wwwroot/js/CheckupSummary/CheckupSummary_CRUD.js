var Details = function (id) {
    var url = "/CheckupSummary/Details?id=" + id;
    $('#titleExtraBigModal').html("Checkup Details");
    loadExtraBigModal(url);
};

var PatientHistory = function (id) {
    var url = "/CheckupSummary/PatientHistory?id=" + id;
    $('#titleExtraBigModal').html("Patient History");
    loadExtraBigModal(url);
};

var PrintCheckup = function (id) {
    location.href = "/CheckupSummary/PrintCheckup?id=" + id;
};

var AddEdit = function (id) {
    var url = "/CheckupSummary/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Checkup");
    }
    else {
        $('#titleExtraBigModal').html("Add Checkup");
    }

    localStorage.removeItem('CheckupId');
    localStorage.removeItem('CurrentURL');
    loadExtraBigModal(url);
};

var SaveCheckup = function () {
    if (!FieldValidation('#PatientId')) {
        FieldValidationAlert('#PatientId', 'Patient Name is Required.', "warning");
        return;
    }
    if (!FieldValidation('#DoctorId')) {
        FieldValidationAlert('#DoctorId', 'Doctor Name is Required.', "warning");
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    $.ajax({
        type: "POST",
        url: "/CheckupSummary/AddEdit",
        data: PreparedFormObj(),
        dataType: "json",
        success: function (result) {
            if (result.IsSuccess) {
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');

                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    var _CurrentURL = localStorage.getItem('CurrentURL');
                    var _CheckupId = localStorage.getItem('CheckupId');
                    document.getElementById("btnClose").click();

                    if (_CurrentURL == null) {
                        //location.href = "/CheckupSummary/Index/";
                        $('#tblCheckupSummary').DataTable().ajax.reload();
                    }
                    else {
                        location.href = _CurrentURL + "?id=" + _CheckupId;
                    }
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var SaveCheckupMedicineDetailsDB = function (CheckupMedicineDetailsCRUDViewModel) {
    //var _PreparedFormObj = PreparedFormObj();
    //_PreparedFormObj.CheckupMedicineDetailsCRUDViewModel = CheckupMedicineDetails;

    $.ajax({
        type: "POST",
        url: "/CheckupSummary/SaveCheckupMedicineDetails",
        data: CheckupMedicineDetailsCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Checkup medicine item has been created successfully. Med ID: " + result.MedicineId, 'Success');
            AddEdit(result.CheckupId);          
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

var DeleteCheckupMedicineDetailsDB = function (CheckupMedicineDetailsCRUDViewModel) {
    //var _PreparedFormObj = PreparedFormObj();
    //_PreparedFormObj.CheckupMedicineDetailsCRUDViewModel = CheckupMedicineDetails;

    $.ajax({
        type: "POST",
        url: "/CheckupSummary/DeleteCheckupMedicineDetails",
        data: CheckupMedicineDetailsCRUDViewModel,
        dataType: "json",
        success: function (result) {
            toastr.success("Checkup medicine details item has been deleted successfully. Med ID: " + result.MedicineId, 'Success');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


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
                url: "/CheckupSummary/Delete?id=" + id,
                success: function (result) {
                    var message = "Checkup has been deleted successfully. Checkup ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        text: 'Deleted!',
                        onAfterClose: () => {
                            location.reload();
                        }
                    });
                }
            });
        }
    });
};


var PreparedFormObj = function () {
    var _CheckupSummaryCRUDViewModel = {
        Id: $("#Id").val(),
        VisitId: $("#VisitId").val(),
        PaymentId: $("#PaymentId").val(),
        CreatedDate: $("#CreatedDate").val(),
        CreatedBy: $("#CreatedBy").val(),
        PatientId: $("#PatientId").val(),
        PatientType: $("#PatientType option:selected").text(),
        DoctorId: $("#DoctorId").val(),

        Symptoms: $("#Symptoms").val(),
        Diagnosis: $("#Diagnosis").val(),
        HPI: $("#HPI").val(),
        VitalSigns: $("#VitalSigns").val(),
        PhysicalExamination: $("#PhysicalExamination").val(),

        CheckupDate: $("#CheckupDate").val(),
        NextVisitDate: $("#NextVisitDate").val(),
        Advice: $("#Advice").val(),
        Comments: $("#Comments").val(),

        BPSystolic: $("#BPSystolic").val(),
        BPDiastolic: $("#BPDiastolic").val(),
        RespirationRate: $("#RespirationRate").val(),
        Temperature: $("#Temperature").val(),
        NursingNotes: $("#NursingNotes").val(),

    };

    var SendObject = {
        CheckupSummaryCRUDViewModel: _CheckupSummaryCRUDViewModel,
        CheckupMedicineDetailsCRUDViewModel: null,
        listCheckupMedicineDetailsCRUDViewModel: listMedicineDetails,
        listPatientTestDetailCRUDViewModel: listPatientTestDetail
    };
    return SendObject;
}