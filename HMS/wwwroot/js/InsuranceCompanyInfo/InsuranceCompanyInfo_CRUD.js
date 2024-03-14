var Details = function (id) {
    var url = "/InsuranceCompanyInfo/Details?id=" + id;
    $('#titleBigModal').html("Insurance Company Info Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/InsuranceCompanyInfo/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Insurance Company Info");
    }
    else {
        $('#titleBigModal').html("Add Insurance Company Info");
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
                url: "/InsuranceCompanyInfo/Delete?id=" + id,
                success: function (result) {
                    var message = "Insurance Company has been deleted successfully. Insurance Company ID: " + result.Id;
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
