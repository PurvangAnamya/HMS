var Details = function (id) {
    var url = "/PatientAppointment/Details?id=" + id;
    $('#titleBigModal').html("Patient Appointment Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/PatientAppointment/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Patient Appointment");
    }
    else {
        $('#titleBigModal').html("Add Patient Appointment");
    }
    loadBigModal(url);
};

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
                url: "/PatientAppointment/Delete?id=" + id,
                success: function (result) {
                    var message = "Patient Appointment has been deleted successfully. PatientAppointment ID: " + result.Id;
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
