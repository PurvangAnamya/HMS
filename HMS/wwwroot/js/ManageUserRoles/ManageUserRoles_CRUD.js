var Details = function (id) {
    var url = "/ManageUserRoles/Details?id=" + id;
    $('#titleExtraBigModal').html("Role Details");
    loadExtraBigModal(url);
};

function validateImageFile(input) {
    const file = input.files[0];
    const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];

    if (file && allowedTypes.includes(file.type)) {
        // Valid image file selected
        document.getElementById('profilePictureError').textContent = '';
    } else {
        if (fileInput.files.length > 0) {
            // Upload the new image file
            formData.append("ProfilePictureDetails", fileInput.files[0]);
        }
        // Invalid file type selected
        document.getElementById('profilePictureError').textContent = 'Invalid file type. Please select a JPEG, PNG, or GIF image.';
        input.value = ''; // Clear the file input to allow selecting another file

        if (fileInput.files.length < 0) {
            document.getElementById('profilePicture').src = "/images/TheLegendPv/default-profile-image.png";
        }
    }
}


var AddEdit = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/ManageUserRoles/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Role");
    }
    else {
        $('#titleExtraBigModal').html("Add Role");
    }
    loadExtraBigModal(url);
};

var Save = function () {
    if (!$("#frmUserRoles").valid()) {
        return;
    }
    // Serialize form data excluding file input
    var _frmUserRoles = $("#frmUserRoles").serialize();

    // Construct FormData object to include file input
    var formData = new FormData($("#frmUserRoles")[0]);

    // Append serialized form data to FormData object
    formData.append("frmUserRoles", _frmUserRoles);

    // Check if a new image file is provided
    var fileInput = document.getElementById("ProfilePictureDetails");
    var profilePictureElement = document.getElementById("profilePicture");

    if (fileInput.files.length > 0) {
        // Upload the new image file
        formData.append("ProfilePictureDetails", fileInput.files[0]);
    }

    var fileInput = document.getElementById("DashboardPictureDetails");
    var dashboardPictureElement = document.getElementById("dashboardPicture");

    if (fileInput.files.length > 0) {
        // Upload the new image file
        formData.append("DashboardPictureDetails", fileInput.files[0]);
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/AddEdit",
        data: formData,
        contentType: false,
        processData: false,
        success: function (result) {
            Swal.fire({
                title: result,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblManageUserRoles').DataTable().ajax.reload();
            });
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/ManageUserRoles/Delete?id=" + id,
                success: function (result) {
                    var message = "Role has been deleted successfully. Role ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblManageUserRoles').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};