$(document).ready(function () {
    LoadDataTables();
});

var CustomRangeDataFilter = function () {
    var _StartDate = $("#StartDate").val();
    var _EndDate = $("#EndDate").val();
    location.href = "/RptExpenses/Index?StartDate= " + _StartDate + "&EndDate= " + _EndDate;
};


var LoadDataTables = function () {
    document.title = 'Expenses Report';
    $("#tblExpensesReport").DataTable({

        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };

            //Total over this page: 2
            pageTotal2 = api
                .column(2, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            //Update footer
            $(api.column(2).footer()).html(
                'Σ: ' + pageTotal2
            );            
        },

        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',

        buttons: [
            'pageLength',
        ],

        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/RptExpenses/GetDataTableData",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "ExpenseCategoriesName", "name": "ExpenseCategoriesName", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.ExpenseCategoriesName + "</a>";
                }
            },
            { "data": "Amount", "name": "Amount" },
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
        ],

        'columnDefs': [{
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
};





