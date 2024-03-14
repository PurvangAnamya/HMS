var Details = function (id) {
    var url = "/MedicineManufacture/Details?id=" + id;
    $('#titleBigModal').html("Medicine Manufacture Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/MedicineManufacture/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Medicine Manufacture");
    }
    else {
        $('#titleBigModal').html("Add Medicine Manufacture");
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
                url: "/MedicineManufacture/Delete?id=" + id,
                success: function (result) {
                    var message = "Medicine Manufacture has been deleted successfully. Medicine Manufacture ID: " + result.Id;
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
