
toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": false,
    "progressBar": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

var loadCommonModal = function (url) {
    $("#SmallModalDiv").load(url, function () {
        $("#SmallModal").modal("show");
    });
};

var loadMediumModal = function (url) {
    $("#MediumModalDiv").load(url, function () {
        $("#MediumModal").modal("show");
    });
};

var loadBigModal = function (url) {
    $("#BigModalDiv").load(url, function () {
        $("#BigModal").modal("show");
    });
};

var loadExtraBigModal = function (url) {
    $("#ExtraBigModalDiv").load(url, function () {
        $("#ExtraBigModal").modal("show");
    });
};

var loadPrintModal = function (url) {
    $("#PrintModalDiv").load(url, function () {
        $("#PrintModal").modal("show");
    });
};

var loadImageViewModal = function () {
    $("#ImageViewModal").modal("show");
};

var SearchInHTMLTable = function () {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("inputRoleSearch");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

var SearchByInHTMLTable = function (TableName) {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("inputRoleSearch");
    filter = input.value.toUpperCase();

    tr = TableName.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[1];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

var BacktoPreviousPage = function () {
    window.history.back();
}

var removeFromArray = function (array) {
    var what, a = arguments, L = a.length, ax;
    while (L > 1 && array.length) {
        what = a[--L];
        while ((ax = array.indexOf(what)) !== -1) {
            array.splice(ax, 1);
        }
    }
    return array;
}

var SearchInArray = function (array, obj) {
    for (var i = 0; i < array.length; i++) {
        if (array[i].PaymentCategoriesId == obj) {
            return true;
        }
    }
    return false;
}

var printDiv = function (divName) {
    var printContents = document.getElementById("printableArea").innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
    //location.reload();
}

var ComplaintDetails = function (id) {
    var url = "/Complaint/DetailsOnly?id=" + id;
    $('#titleExtraBigModal').html("Complaint Details");
    loadExtraBigModal(url);
};

var SwalSimpleAlert = function (Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype
    });
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


function wait(ms) {
    var start = new Date().getTime();
    var end = start;
    while (end < start + ms) {
        end = new Date().getTime();
    }
}

var DataTableCustomSearchBox = function (width, placeholder) {
    $('.dataTables_filter input[type="search"]').
        attr('placeholder', placeholder).
        css({ 'width': width, 'display': 'inline-block' });
};

var ViewImage = function (imageURL, Title) {
    $('#titleImageViewModal').html(Title);
    $("#UserImage").attr("src", imageURL);
    $("#ImageViewModal").modal("show");
};

var DemoUserAccountLockAll = function () {
    var IsDemoUser = 0;
    if (IsDemoUser == 1) {
        SwalSimpleAlert("You are not allowed to change demo user info", "warning");
        return IsDemoUser;
    }
    else {
        return IsDemoUser;
    }
}