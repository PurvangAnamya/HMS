var Details = function (id) {
    var url = "/LabTestConfiguration/Details?id=" + id;
    $('#titleBigModal').html("Lab Test Configuration Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/LabTestConfiguration/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Lab Test Configuration");
    }
    else {
        $('#titleMediumModal').html("Add Lab Test Configuration");
    }
    loadBigModal(url);
};

var DeleteLabTestConfigurationItem = function (id) {
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
                url: "/LabTestConfiguration/Delete?id=" + id,
                success: function (result) {
                    var message = "Lab Test Configuration has been deleted successfully. Lab Test Configuration ID: " + result.Id;
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
