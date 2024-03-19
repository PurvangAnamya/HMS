$(document).ready(function () {
    document.title = 'Bed';

    $("#tblBed").DataTable({
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
            "url": "/Bed/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "BedCategoryName", "name": "BedCategoryName", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.BedCategoryName + "</a>";
                }
            },
            { "data": "BedCategoryPrice", "name": "BedCategoryPrice" },
            { "data": "No", "name": "No" },
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
            { "data": "Hospital", "name": "Hospital" },
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
            'targets': [5, 6],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]],
        "initComplete": function (settings, json) {
            var column = this.api().column(6); // Index of the "Hospital" column
            var data = column.data().toArray(); // Convert to array
            var isEmpty = data.every(function (value) {
                return value === null || value.trim() === '';
            });
            column.visible(!isEmpty);
        }
    });

});

