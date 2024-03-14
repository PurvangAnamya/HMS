var Details = function (id) {
    var url = "/DoctorsInfo/Details?id=" + id;
    $('#titleExtraBigModal').html("Doctors Info Details");
    loadExtraBigModal(url);
};

var ViewUserImage = function (imageURL) {
    $('#titleImageViewModal').html("User Profile Image ");
    $("#UserImage").attr("src", imageURL);
    loadImageViewModal();
};

var ResetPassword = function (id) {
    $('#titleSmallModal').html("<h4>Reset Password</h4>");
    var url = "/UserProfile/ResetPassword?id=" + id;
    loadCommonModal(url);
};

var AddEdit = function (id) {
    var url = "/DoctorsInfo/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Doctors Info");
    }
    else {
        $('#titleExtraBigModal').html("Add Doctors Info");
    }
    loadExtraBigModal(url);
};

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: "/DoctorsInfo/Delete?id=" + id,
                success: function (result) {
                    var message = "Doctors Info has been deleted successfully. DoctorsInfo ID: " + result.Id;
                    Swal.fire({
                        title: message,
                        text: 'Deleted!',
                        onAfterClose: () => {
                            location.reload();
                        }
                    });
                }
            });
        }
    });
};
