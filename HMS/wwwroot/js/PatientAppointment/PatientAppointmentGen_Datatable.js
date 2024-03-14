$(document).ready(function () {
    document.title = 'Patient Appointment';

    $("#tblPatientAppointmentGen").DataTable({
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
            "url": "/PatientAppointment/GetDataTabelData",
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
            { "data": "DoctorName", "name": "DoctorName" },
            { "data": "SerialNo", "name": "SerialNo" },
            {
                "data": "AppointmentDate",
                "name": "AppointmentDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "AppointmentTimeDisplay", "name": "AppointmentTimeDisplay" },
            { "data": "Note", "name": "Note" },


            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
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

var Details = function (id) {
    var url = "/PatientAppointment/Details?id=" + id;
    $('#titleBigModal').html("Patient Appointment Details");
    loadBigModal(url);
};

