var Details = function (id) {
    var url = "/CheckupSummary/Details?id=" + id;
    $('#titleExtraBigModal').html("Checkup Details");
    loadExtraBigModal(url);
};


var PrintMedicineLabel = function (id) {
    location.href = "/MedicineLabel/PrintMedicineLabel?VisitId=" + id;
};
