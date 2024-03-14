var Details = function (id) {
    var url = "/Expenses/Details?id=" + id;
    $('#titleBigModal').html("Expenses Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/Expenses/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Expenses");
    }
    else {
        $('#titleMediumModal').html("Add Expenses");
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
                url: "/Expenses/Delete?id=" + id,
                success: function (result) {
                    var message = "Expenses has been deleted successfully. Expenses ID: " + result.Id;
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
