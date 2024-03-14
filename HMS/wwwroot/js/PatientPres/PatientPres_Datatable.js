$(document).ready(function () {
    document.title = 'Checkup Summary';

    $("#tblCheckupSummary").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/PatientPres/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "VisitId", "name": "VisitId", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.VisitId + "</a>";
                }
            },
            { "data": "SerialNo", "name": "SerialNo" },
            { "data": "PatientName", "name": "PatientName" },
            { "data": "DoctorName", "name": "DoctorName" },
            { "data": "Symptoms", "name": "Symptoms" },
            { "data": "Diagnosis", "name": "Diagnosis" },
            { "data": "Advice", "name": "Advice" },

            {
                "data": "CheckupDate",
                "name": "CheckupDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-success btn-xs' onclick=PatientHistory('" + row.PatientId + "');>History</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=PrintCheckup('" + row.Id + "');><span class='fa fa-print'>Print</span></a>";
                }
            },
        ],

        'columnDefs': [{
            'targets': [],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
});

var Details = function (id) {
    var url = "/CheckupSummary/Details?id=" + id;
    $('#titleExtraBigModal').html("Checkup Details");
    loadExtraBigModal(url);
};

var PatientHistory = function (id) {
    var url = "/CheckupSummary/PatientHistory?id=" + id;
    $('#titleExtraBigModal').html("Patient History");
    loadExtraBigModal(url);
};

var PrintCheckup = function (id) {
    location.href = "/CheckupSummary/PrintCheckup?id=" + id;
};
