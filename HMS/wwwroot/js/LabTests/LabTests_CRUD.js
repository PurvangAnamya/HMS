var Details = function (id) {
    var url = "/LabTests/Details?id=" + id;
    $('#titleBigModal').html("Lab Tests Details");
    loadBigModal(url);
};

var AddConfigureTest = function (id) {   
    $('#titleBigModal').html("Add Test Configuration");
    var url = "/LabTestConfiguration/AddConfigureTest?id=" + id;
    loadBigModal(url);
};

var EditConfigureTest = function (id) {
    $('#titleBigModal').html("Edit Test Configuration");
    var url = "/LabTestConfiguration/EditConfigureTest?id=" + id;
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/LabTests/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Lab Tests");
    }
    else {
        $('#titleMediumModal').html("Add Lab Tests");
    }
    loadMediumModal(url);
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
                url: "/LabTests/Delete?id=" + id,
                success: function (result) {
                    var message = "Lab Tests has been deleted successfully. LabTests ID: " + result.Id;
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
                            ConfigureTest(result.LabTestsId);
                        }
                    });
                }
            });
        }
    });
};