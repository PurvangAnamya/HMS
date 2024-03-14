$(document).ready(function () {
    document.title = 'Patient Info';

    $("#tblPatientInfo").DataTable({
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
            "url": "/PatientInfo/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "FirstName", "name": "FirstName", render: function (data, type, row) {
                    return "<a href='#' onclick=DetailsPatientInfo('" + row.Id + "');>" + row.FirstName + "</a>";
                }
            },
            { "data": "LastName", "name": "LastName" },
            { "data": "MaritalStatus", "name": "MaritalStatus" },
            { "data": "Gender", "name": "Gender" },
            { "data": "SpouseName", "name": "SpouseName" },
            { "data": "BloodGroup", "name": "BloodGroup" },
            {
                "data": "DateOfBirth",
                "name": "DateOfBirth",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddCheckup('" + row.Id + "');>New Checkup</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEditPatientInfo('" + row.Id + "');>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=DeletePatientInfo('" + row.Id + "'); >Delete</a>";
                }
            }
        ],

        'columnDefs': [{
            'targets': [8, 9, 10],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

