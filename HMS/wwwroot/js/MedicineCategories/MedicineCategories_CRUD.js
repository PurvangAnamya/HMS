var Details = function (id) {
    var url = "/MedicineCategories/Details?id=" + id;
    $('#titleBigModal').html("Medicine Categories Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/MedicineCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Medicine Categories");
    }
    else {
        $('#titleMediumModal').html("Add Medicine Categories");
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
                url: "/MedicineCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Medicine Categories has been deleted successfully. MedicineCategories ID: " + result.Id;
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
