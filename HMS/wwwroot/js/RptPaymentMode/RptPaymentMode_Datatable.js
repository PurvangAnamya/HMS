$(document).ready(function () {
    LoadDataTables();
});

var CustomRangeDataFilter = function () {
    var _StartDate = $("#StartDate").val();
    var _EndDate = $("#EndDate").val();
    location.href = "/RptPaymentMode/Index?StartDate= " + _StartDate + "&EndDate= " + _EndDate;
};


var LoadDataTables = function () {
    document.title = 'Payment Mode Report';
    $("#tblPaymentModeReport").DataTable({


        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            var intVal = function (i) {
                return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
            };
            
            //Total over this page: 6
            pageTotal6 = api
                .column(6, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(6).footer()).html(
                'Σ: ' + pageTotal6
            );

            //Total over this page: 7
            pageTotal7 = api
                .column(7, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(7).footer()).html(
                'Σ: ' + pageTotal7
            );
            //Total over this page: 8
            pageTotal8 = api
                .column(8, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(8).footer()).html(
                'Σ: ' + pageTotal8
            );
            //Total over this page: 5
            pageTotal9 = api
                .column(9, { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            $(api.column(9).footer()).html(
                'Σ: ' + pageTotal9
            );
        },


        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',

        buttons: [
            'pageLength',
            {
                extend: 'collection',
                text: 'Export',
                buttons: [
                    {
                        extend: 'pdfHtml5',
                        customize: function (doc) {
                            //doc.content[1].margin = [100, 0, 100, 0];
                            //Remove the title created by datatTables
                            doc.content.splice(0, 1);
                            //Create a date string that we use in the footer. Format is dd-mm-yyyy
                            var now = new Date();
                            var jsDate = now.getDate() + '-' + (now.getMonth() + 1) + '-' + now.getFullYear();

                            doc.pageMargins = [20, 60, 20, 30];
                            // Set the font size fot the entire document
                            doc.defaultStyle.fontSize = 7;
                            // Set the fontsize for the table header
                            doc.styles.tableHeader.fontSize = 10;


                            doc['header'] = (function () {
                                return {
                                    columns: [
                                        {
                                            alignment: 'left',  //center
                                            italics: true,
                                            text: 'Payment Mode Report',
                                            fontSize: 18,
                                            margin: [0, 0]
                                        }
                                    ],
                                    margin: 20
                                }
                            });

                            // Create a footer object with 2 columns
                            doc['footer'] = (function (page, pages) {
                                return {
                                    columns: [
                                        {
                                            alignment: 'left',
                                            text: ['Created on: ', { text: jsDate.toString() }]
                                        },
                                        {
                                            alignment: 'right',
                                            text: ['page ', { text: page.toString() }, ' of ', { text: pages.toString() }]
                                        }
                                    ],
                                    margin: 5
                                }
                            });
                            // Change dataTable layout (Table styling)
                            // To use predefined layouts uncomment the line below and comment the custom lines below
                            // doc.content[0].layout = 'lightHorizontalLines'; // noBorders , headerLineOnly
                            var objLayout = {};
                            objLayout['hLineWidth'] = function (i) { return .5; };
                            objLayout['vLineWidth'] = function (i) { return .5; };
                            objLayout['hLineColor'] = function (i) { return '#aaa'; };
                            objLayout['vLineColor'] = function (i) { return '#aaa'; };
                            objLayout['paddingLeft'] = function (i) { return 4; };
                            objLayout['paddingRight'] = function (i) { return 4; };
                            doc.content[0].layout = objLayout;
                        },


                        orientation: 'portrait', // landscape
                        pageSize: 'A4',
                        pageMargins: [0, 0, 0, 0], // try #1 setting margins
                        margin: [0, 0, 0, 0], // try #2 setting margins
                        text: '<u>PDF</u>',
                        key: { // press E for export PDF
                            key: 'e',
                            altKey: false
                        },
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10], //column id visible in PDF
                            modifier: {
                                // DataTables core
                                order: 'index',  // 'current', 'applied', 'index',  'original'
                                page: 'all',      // 'all',     'current'
                                search: 'none'     // 'none',    'applied', 'removed'
                            }
                        }
                    },
                    'copyHtml5',
                    'excelHtml5',
                    /*'csvHtml5',*/
                    {
                        extend: 'csvHtml5',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                            page: 'all'
                        }
                    },
                    {
                        extend: 'print',
                        exportOptions: {
                            columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
                            page: 'all'
                        }
                    }
                ]
            }
        ],

        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/RptPaymentMode/GetDataTableData",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            { "data": "Id", "name": "Id" },
            {
                data: "PatientName", "name": "PatientName", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.PatientName + "</a>";
                }
            },
            { "data": "PatientType", "name": "PatientType" },
            { "data": "Insurance", "name": "Insurance" },
            { "data": "PaymentStatus", "name": "PaymentStatus" },
            { "data": "ModeofPayment", "name": "ModeofPayment" },
            { "data": "Amount", "name": "Amount" },
            { "data": "TotalAmount", "name": "TotalAmount" },
            { "data": "TotalPaidAmount", "name": "TotalPaidAmount" },

            { "data": "TotalDueAmount", "name": "TotalDueAmount" },
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

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });
};





