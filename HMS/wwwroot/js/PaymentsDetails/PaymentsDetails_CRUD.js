var Details = function (id) {
    var url = "/PaymentsDetails/Details?id=" + id;
    $('#titleMediumModal').html("PaymentsDetails Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/PaymentsDetails/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit PaymentsDetails");
    }
    else {
        $('#titleMediumModal').html("Add PaymentsDetails");
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
                url: "/PaymentsDetails/Delete?id=" + id,
                success: function (result) {
                    var message = "PaymentsDetails has been deleted successfully. PaymentsDetails ID: " + result.Id;
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
