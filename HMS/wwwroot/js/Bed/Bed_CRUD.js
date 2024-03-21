var Details = function (id) {
    var url = "/Bed/Details?id=" + id;
    $('#titleMediumModal').html("Bed Details");
    loadMediumModal(url);
};



var AddEdit = function (id) {
    var url = "/Bed/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Bed");
        // Set the value of the BedCategoryPrice textbox based on the selected category
        var selectedCategoryId = $('#BedCategoryId').val();
        console.log("AddEdit Function selectedCategoryId :" + selectedCategoryId);
        LoadBedPrice(selectedCategoryId); // Call the function to load bed price
    }
    else {
        $('#titleMediumModal').html("Add Bed");
    }

    loadMediumModal(url);
};

// Function to load bed price based on Edit Loading
function LoadBedPrice(selectedCategoryId) {
    $.ajax({
        url: '/Bed/GetBedPrice',
        type: 'GET',
        data: { categoryId: selectedCategoryId },

        success: function (data) {
            $('#BedCategoryPrice').val(data);

            console.log(" categoryId: " + categoryId + " selectedCategoryId :" + selectedCategoryId);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}



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

                    toastr.success("Bed has been deleted successfully. Bed ID: " + response.data.Id, 'Success');
                    var _tblBed = $('#tblBed').DataTable();
                    _tblBed.ajax.reload();
                })
                .catch(function (error) {

                })
                .then(function () {

                });
            return false;


        }
    });
};
