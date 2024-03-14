$(document).ready(function () {
    document.title = 'Medicine Label';

    $("#tblMedicineLabel").DataTable({
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
            "url": "/MedicineLabel/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "PatientId", "name": "PatientId" },
            {
                data: "VisitId", "name": "VisitId", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.VisitId + "</a>";
                }
            },
            { "data": "PatientName", "name": "PatientName" },
            { "data": "DoctorName", "name": "DoctorName" },          
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
                    return "<a href='#' class='btn btn-info btn-xs' onclick=PrintMedicineLabel('" + row.VisitId + "');><span class='fa fa-print'>Print</span></a>";
                }
            },           
        ],

        'columnDefs': [{
            'targets': [5],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

