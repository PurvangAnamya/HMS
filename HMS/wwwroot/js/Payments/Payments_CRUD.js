var Details = function (id) {
    var url = "/Payments/Details?id=" + id;
    $('#titleExtraBigModal').html("Payments Details");
    loadExtraBigModal(url);
};

var PrintPaymentInvoice = function (PaymentId) {
    location.href = "/Payments/PrintPaymentInvoice?_PaymentId=" + PaymentId;
};

var AddEdit = function (id) {
    var url = "/Payments/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Payments");
    }
    else {
        $('#titleExtraBigModal').html("Add Payments");
    }
    localStorage.removeItem('PaymentId');
    localStorage.removeItem('CurrentURL');
    loadExtraBigModal(url);
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
                url: "/Payments/Delete?id=" + id,
                success: function (result) {
                    var message = "Payments has been deleted successfully. Payments ID: " + result.Id;
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
