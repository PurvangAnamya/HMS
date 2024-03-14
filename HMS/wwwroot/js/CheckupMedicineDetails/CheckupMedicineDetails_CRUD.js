var Details = function (id) {
    var url = "/CheckupMedicineDetails/Details?id=" + id;
    $('#titleMediumModal').html("CheckupMedicineDetails Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/CheckupMedicineDetails/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit CheckupMedicineDetails");
    }
    else {
        $('#titleMediumModal').html("Add CheckupMedicineDetails");
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
                url: "/CheckupMedicineDetails/Delete?id=" + id,
                success: function (result) {
                    var message = "CheckupMedicineDetails has been deleted successfully. CheckupMedicineDetails ID: " + result.Id;
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
