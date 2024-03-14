var Details = function (id) {
    var url = "/PaymentCategories/Details?id=" + id;
    $('#titleBigModal').html("Payment Categories Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/PaymentCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Payment Categories");
    }
    else {
        $('#titleMediumModal').html("Add Payment Categories");
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
                url: "/PaymentCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Payment Categories has been deleted successfully. PaymentCategories ID: " + result.Id;
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
