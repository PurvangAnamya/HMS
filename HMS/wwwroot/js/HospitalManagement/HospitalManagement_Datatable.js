$(document).ready(function () {
    document.title = 'Hospital';
    $("#tblHospitalAccount").DataTable({
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
            "url": "/HospitalManagement/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: null, render: function (data, type, row) {
                    debugger
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.HospitalLogoImagePath + "','User_Image');><div class='image'><img src='" + row.HospitalLogoImagePath + "' class='img-circle elevation-2 imgCustom' alt='Asset Image'></div></a>";
                }
            },
            {
                data: "HospitalName", "name": "Name", render: function (data, type, row) {
                    debugger
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.HospitalName + "</a>";
                }
            },
            { "data": "Description", "name": "Description" },
            { "data": "Address", "name": "Address" },
            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    try {
                        debugger
                        var date = new Date(data);
                        var month = date.getMonth() + 1;
                        console.log("Month:", month);
                        return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                    } catch (error) {
                        console.error("Error in CreatedDate rendering:", error);
                        return "";  // Return a default value or an empty string in case of an error.
                    }
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
            'targets': [],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

