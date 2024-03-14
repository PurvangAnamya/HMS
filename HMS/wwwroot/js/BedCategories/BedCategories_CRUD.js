var Details = function (id) {
    var url = "/BedCategories/Details?id=" + id;
    $('#titleBigModal').html("Bed Categories Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/BedCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Bed Categories");
    }
    else {
        $('#titleMediumModal').html("Add Bed Categories");
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
                url: "/BedCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Bed Categories has been deleted successfully. BedCategories ID: " + result.Id;
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
