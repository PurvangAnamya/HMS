var Details = function (id) {
    var url = "/ExpenseCategories/Details?id=" + id;
    $('#titleMediumModal').html("Expense Categories Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/ExpenseCategories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Expense Categories");
    }
    else {
        $('#titleMediumModal').html("Add Expense Categories");
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
                url: "/ExpenseCategories/Delete?id=" + id,
                success: function (result) {
                    var message = "Expense Categories has been deleted successfully. ExpenseCategories ID: " + result.Id;
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
