var Details = function (id) {
    var url = "/Bed/Details?id=" + id;
    $('#titleMediumModal').html("Bed Details");
    loadMediumModal(url);
};


var AddEdit = function (id) {
    var url = "/Bed/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Bed");
    }
    else {
        $('#titleMediumModal').html("Add Bed");
    }
    loadMediumModal(url);
};

var SaveData = function () {
    var _frmVaidate = $('#frmBed');
    $.validator.unobtrusive.parse(_frmVaidate);
    _frmVaidate.validate();

    if (_frmVaidate.valid()) {
        $("#btnSave").val("Please Wait");
        $('#btnSave').attr('disabled', 'disabled');
        var formData = $("#frmBed").serialize();

        $.ajax({
            type: "POST",
            url: "/Bed/AddEdit",
            data: formData,
            dataType: "json",
            success: function (result) {
                if (result == "Success") {
                    //document.location.href = "/";
                    window.location.reload();
                }
                else {
                    Swal.fire({
                        title: result,
                        text: 'Alert!',
                        onAfterClose: () => {
                            $("#btnSave").val("Save");
                            $('#btnSave').removeAttr('disabled');
                            setTimeout(function () {
                                $('#No').focus();
                            }, 500);
                        }
                    });
                }
            },
            error: function (result) {
                Swal.fire({
                    title: result,
                    text: 'Alert!'
                });
            }
        });
    };
};


var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {

            axios.post("/Bed/Delete?id=" + id)
                .then(function (response) {
                    //$("#orderNumber").val(response.data.number);
                    //$("#salesOrderId").val(response.data.salesOrderId);
                    //initPosLine();
                    toastr.success("Bed has been deleted successfully. Bed ID: " + response.data.Id, 'Success');
                    var _tblBed = $('#tblBed').DataTable();
                    _tblBed.ajax.reload();
                })
                .catch(function (error) {

                })
                .then(function () {

                });
            return false;

            //$.ajax({
            //    type: "POST",
            //    url: "/Bed/Delete?id=" + id,
            //    success: function (result) {
            //        var message = "Bed has been deleted successfully. Bed ID: " + result.Id;
            //        Swal.fire({
            //            title: message,
            //            text: 'Deleted!',
            //            onAfterClose: () => {
            //                location.reload();
            //            }
            //        });
            //    }
            //});
        }
    });
};
