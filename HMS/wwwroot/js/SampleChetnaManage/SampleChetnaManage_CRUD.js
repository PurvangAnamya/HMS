var Details = function (id) {
  
    var url = "/SampleChetnaManage/Details?id=" + id;
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
        // Invalid file type selected
        document.getElementById('profilePictureError').textContent = 'Invalid file type. Please select a JPEG, PNG, or GIF image.';
        input.value = ''; // Clear the file input to allow selecting another file
    }
}

var isDateOfBirthValid = function (dateOfBirth) {
    // Parse the date string to ensure proper comparison
    var dob = new Date(Date.parse(dateOfBirth));
    var today = new Date();
    var age = today.getFullYear() - dob.getFullYear();
    var m = today.getMonth() - dob.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
        age--;
    }
    return age >= 20;
};
var AddEdit = function (id) {
    
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/SampleChetnaManage/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit SampleChetnaManage");
    }
    else {
        $('#titleExtraBigModal').html("Add SampleChetnaManage");
    }
    loadExtraBigModal(url);
};

var Save = function () {
    if (!$("#frmSampleChetnaManage").valid()) {
        return;
    }

    var dateOfBirth = $("#DateOfBirth").val();
    if (!isDateOfBirthValid(dateOfBirth)) {
        // Show error message within the existing validation span
        $("#DateOfBirth").siblings(".text-danger").text("Date of Birth must be greater than 20 years from today.");
        return;
    } else {
        // Clear the error message if Date of Birth is valid
        $("#DateOfBirth").siblings(".text-danger").text("");
    }  
    
    // Serialize form data excluding file input
    var _frmUserRoles = $("#frmSampleChetnaManage").serialize();

    // Construct FormData object to include file input
    var formData = new FormData($("#frmSampleChetnaManage")[0]);

    // Append serialized form data to FormData object
    formData.append("frmUserRoles", _frmUserRoles);
    // Check if a new image file is provided
    var fileInput = document.getElementById("ProfilePictureDetails");
    if (fileInput.files.length > 0) {
        // Upload the new image file
        formData.append("ProfilePictureDetails", fileInput.files[0]);
    } else {
        // Retain the existing ProfilePicture value
        var existingProfilePicture = $("#ProfilePicture").val();
        formData.append("ProfilePicture", existingProfilePicture);
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SampleChetnaManage/AddEdit",
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
                $('#tblDataSampleChetnaManage').DataTable().ajax.reload();
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
                url: "/SampleChetnaManage/Delete?id=" + id,
                success: function (result) {
                    var message = "Item been deleted successfully. Item ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        icon: 'info',
                        onAfterClose: () => {
                            $('#tblDataSampleChetnaManage').DataTable().ajax.reload();
                        }
                    });
                }
            });
        }
    });
};