$(document).ready(function () {
    document.title = 'User Role';

    $("#tblManageUserRoles").DataTable({
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
            "url": "/ManageUserRoles/GetDataTabelData",
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
                    let profileImage = "/images/UserIcon/Admin.png";
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.ProfilePicture + "','Left Menu Image');><div class='image'><img src='" + row.ProfilePicture + "' class='img-circle elevation-2 imgCustom' alt='Left Menu Image'></div></a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    let profileImage = "/images/UserIcon/Admin.png";
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.DashboardPicture + "','Dashboard Image');><div class='image'><img src='" + row.DashboardPicture + "' class='img-circle elevation-2 imgCustom' alt='Dashboard Picture'></div></a>";
                }
            },
            { "data": "Name", "name": "Name" },
            { "data": "Description", "name": "Description" },

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
            'targets': [4, 5],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});