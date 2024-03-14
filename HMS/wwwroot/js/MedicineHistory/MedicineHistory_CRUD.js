var Details = function (id) {
    var url = "/MedicineHistory/Details?id=" + id;
    $('#titleBigModal').html("Medicine History Details");
    loadBigModal(url);
};

