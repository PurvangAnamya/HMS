var Details = function (id) {
    var url = "/Medicines/Details?id=" + id;
    $('#titleBigModal').html("Medicines Details");
    loadBigModal(url);
};

var UpdateQuantity = function (id) {
    var url = "/Medicines/UpdateQuantity?id=" + id;
    $('#titleBigModal').html("Update Quantity. Item ID: " + id);
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/Medicines/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Medicines");
    }
    else {
        $('#titleExtraBigModal').html("Add Medicines");
    }
    loadExtraBigModal(url);
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
                url: "/Medicines/Delete?id=" + id,
                success: function (result) {
                    var message = "Medicines has been deleted successfully. Medicines ID: " + result.Id;
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
