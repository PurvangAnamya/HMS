var Details = function (id) {
    var url = "/BedAllotments/Details?id=" + id;
    $('#titleMediumModal').html("Bed Allotments Details");
    loadMediumModal(url);
};

var AddEdit = function (id) {
    var url = "/BedAllotments/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleMediumModal').html("Edit Bed Allotments");
    }
    else {
        $('#titleMediumModal').html("Add Bed Allotments");
    }
    loadMediumModal(url);
};

// Function to load bed price based on Edit Loading
function LoadBedPrice(selectedCategoryId) {
    $.ajax({
        url: '/BedAllotments/GetBedPrice',
        type: 'GET',
        data: { categoryId: selectedCategoryId },
        success: function (data) {
            $('#BedCategoryPrice').val(data);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}


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
                url: "/BedAllotments/Delete?id=" + id,
                success: function (result) {
                    var message = "Bed Allotments has been deleted successfully. BedAllotments ID: " + result.Id;
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


var GetAvailableBedList = function () {
    var _BedCategoryId = $("#BedCategoryId").val();
    //$('#BedId').reload();

    LoadBedPrice(_BedCategoryId);
    $.ajax({
        type: "GET",
        url: "/BedAllotments/GetAvailableBedList?id=" + _BedCategoryId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#BedId').find('option:not(:first)').remove();
            if (result.length == 0) {
                Swal.fire({
                    title: "Selected category bed not available.",
                    text: 'Alert!',
                    onAfterClose: () => {
                        return;
                    }
                });
            }
            var option = '';
            for (var i = 0; i < result.length; i++) {
                option += '<option value="' + result[i].Id + '">' + result[i].Name + '</option>';
            }
            $('#BedId').append(option);
        },
        error: function (response) {
            console.log(response);
        }
    });
};
