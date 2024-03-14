$(document).ready(function () {
    document.title = 'Doctors Info';

    $("#tblDoctorsInfo").DataTable({
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
            "url": "/DoctorsInfo/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=Details('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.ProfilePicture + "','User_Image');><div class='image'><img src='" + row.ProfilePicture + "' class='img50px img-circle elevation-2' alt='Asset Image'></div></a>";
                }
            },
            {
                data: "FirstName", "name": "FirstName", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.FirstName + "</a>";
                }
            },
            { "data": "LastName", "name": "LastName" },
            { "data": "PhoneNumber", "name": "PhoneNumber" },
            { "data": "Email", "name": "Email" },
            { "data": "DesignationDisplay", "name": "DesignationDisplay" },
            { "data": "DoctorFee", "name": "DoctorFee" },
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
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.Id + "');>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.Id + "'); >Delete</a>";
                }
            }
        ],

        'columnDefs': [{
            'targets': [8, 9],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

