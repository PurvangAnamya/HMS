var Details = function (id) {
    var url = "/CheckupSummary/Details?id=" + id;
    $('#titleExtraBigModal').html("Checkup Details");
    loadExtraBigModal(url);
};

var PaymentDetails = function (id) {
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