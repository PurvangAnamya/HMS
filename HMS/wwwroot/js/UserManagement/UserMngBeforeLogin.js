
var RegisterAccount = function () {
    var url = "/Account/Register";
    $('#titleExtraBigModal').html("Add User");
    loadExtraBigModal(url);
    SendGeolocation();
    setTimeout(function () {
        $('#FirstName').focus();
    }, 500);
};

var SaveUserInLoginUI = function () {
    if (!$("#frmAddUserAccount").valid()) {
        return;
    }

    if (!FieldValidation('#FirstName')) {
        FieldValidationAlert('#FirstName', 'First Name is Required.', "warning");
        return;
    }
    if (!FieldValidation('#LastName')) {
        FieldValidationAlert('#LastName', 'Last Name is Required.', "warning");
        return;
    }

    $("#btnSave").prop('value', 'Creating User');
    $('#btnSave').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "/Account/Register",
        data: PreparedFormObj(),
        processData: false,
        contentType: false,
        success: function (result) {
            $('#btnSave').prop('disabled', false);
            $("#btnSave").prop('value', 'Save');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    console.log(result);
                    document.getElementById("btnAddEditUserAccountClose").click();
                    $("#Email").val(result.ModelObject.Email);
                    $("#btnUserLogin").trigger('click');
                    //document.location.href = "/";
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#EmailInBeforeLoginReg').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var loadExtraBigModal = function (url) {
    $("#ExtraBigModalDiv").load(url, function () {
        $("#ExtraBigModal").modal("show");
    });
};


//Get GEO Loc
var lat;
var long;

var GetGeolocation = function () {
    $("#btnUserLogin").prop("disabled", true);
    setTimeout(function () {
        $("#btnUserLogin").removeAttr('disabled');
    }, 1000);

    if (navigator.geolocation) {
        parseFloat(navigator.geolocation.getCurrentPosition(showPosition));
    } else {
        lat = "Geolocation is not supported by this browser.";
        long = "Geolocation is not supported by this browser.";
    }
    function showPosition(position) {
        lat = position.coords.latitude;
        long = position.coords.longitude;
    }
};


var SendGeolocation = function () {
    $('#Latitude').val(lat);
    $('#Longitude').val(long);
};


var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('UserProfileId', $("#UserProfileId").val())
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('ProfilePictureDetails', $('#ProfilePictureDetails')[0].files[0])

    _FormData.append('FirstName', $("#FirstName").val())
    _FormData.append('LastName', $("#LastName").val())
    _FormData.append('EmployeeTypeId', $("#EmployeeTypeId").val())
    _FormData.append('PhoneNumber', $("#PhoneNumber").val())
    _FormData.append('Email', $("#EmailInBeforeLoginReg").val())
    _FormData.append('PasswordHash', $("#PasswordHash").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    _FormData.append('Address', $("#Address").val())
    _FormData.append('Country', $("#Country").val())
    _FormData.append('RoleId', $("#RoleId").val())
    _FormData.append('HospitalId', $("#HospitalId").val())
    _FormData.append('IsApprover', $("#IsApprover").val())

    _FormData.append('EmployeeId', $("#EmployeeId").val())
    _FormData.append('DateOfBirth', $("#DateOfBirth").val())
    _FormData.append('Designation', $("#Designation").val())
    _FormData.append('Department', $("#Department").val())
    _FormData.append('SubDepartment', $("#SubDepartment").val())
    _FormData.append('JoiningDate', $("#JoiningDate").val())
    _FormData.append('LeavingDate', $("#LeavingDate").val())

    _FormData.append('CurrentURL', $("#CurrentURL").val())
    return _FormData;
}

var FieldValidation = function (FieldName) {
    var _FieldName = $(FieldName).val();
    if (_FieldName == "" || _FieldName == null) {
        return false;
    }
    return true;
};

var FieldValidationAlert = function (FieldName, Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype,
        onAfterClose: () => {
            $(FieldName).focus();
        }
    });
}

