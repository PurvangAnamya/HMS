var Details = function (id) {
    var url = "/Unit/Details?id=" + id;
    $('#titleMediumModal').html("Unit Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Unit/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Unit");
    }
    else {
        $('#titleMediumModal').html("Add Unit");
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
                url: "/Unit/Delete?id=" + id,
                success: function (result) {
                    var message = "Unit has been deleted successfully. Unit ID: " + result.Id;
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
