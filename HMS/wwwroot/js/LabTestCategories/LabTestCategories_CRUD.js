var Details = function (id) {
    var url = "/LabTestCategories/Details?id=" + id;
    $('#titleBigModal').html("Lab Test Categories Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/LabTestCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Lab Test Categories");
    }
    else {
        $('#titleMediumModal').html("Add Lab Test Categories");
    }
    loadMediumModal(url);
};

var Delete = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
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
                url: "/LabTestCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Lab Test Categories has been deleted successfully. LabTestCategories ID: " + result.Id;
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
