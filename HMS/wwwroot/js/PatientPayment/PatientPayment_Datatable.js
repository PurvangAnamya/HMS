$(document).ready(function () {
    document.title = 'Patient Payment';
    var _PatientPaymentsListType = $("#PatientPaymentsListType").val();

    $("#tblPatientPayment").DataTable({
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
            "url": "/PatientPayment/GetDataTabelData?_PatientPaymentsListType=" + _PatientPaymentsListType,
            "type": "POST",
            "datatype": "json"
        },


        "columns": [            
            {
                data: "PaymentId", "name": "PaymentId", render: function (data, type, row) {
                    return "<a href='#' onclick=PaymentDetails('" + row.PaymentId + "');>" + row.PaymentId + "-Details</a>";
                }
            },
            {
                data: "VisitId", "name": "VisitId", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.Id + "');>" + row.VisitId + "</a>";
                }
            },            
            { "data": "PatientName", "name": "PatientName" },
            { "data": "PatientType", "name": "PatientType" },
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
                    return "<a href='#' class='btn btn-success btn-xs' onclick=PrintPaymentInvoice('" + row.PaymentId + "');>Invoice</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=AddEdit('" + row.PaymentId + "');>Update Payment</a>";
                }
            },           
        ],

        'columnDefs': [{
            'targets': [6,7],
            'orderable': false,
        }],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

