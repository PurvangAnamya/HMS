$(document).ready(function () {
    document.title = 'Vital Signs';

    $("#tblVitalSigns").DataTable({
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
            "url": "/VitalSigns/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "VisitId", "name": "VisitId", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.VisitId + "</a>";
                }
            },
            { "data": "SerialNo", "name": "SerialNo" },
            { "data": "PatientName", "name": "PatientName" },
            { "data": "DoctorName", "name": "DoctorName" },

            { "data": "BPSystolic", "name": "BPSystolic" },
            { "data": "BPDiastolic", "name": "BPDiastolic" },
            { "data": "RespirationRate", "name": "RespirationRate" },
            { "data": "Temperature", "name": "Temperature" },
            { "data": "NursingNotes", "name": "NursingNotes" },
            

            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');>Add-Edit</a>";
                }
            },
        ],

        'columnDefs': [{
            'targets': [10],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

