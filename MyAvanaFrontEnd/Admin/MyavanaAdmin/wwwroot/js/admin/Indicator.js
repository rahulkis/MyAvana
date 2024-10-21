
function onRenderProductType(data, type, row, meta) {
    var tr = '<a href="/Products/CreateIndicator/' + row.ProductIndicatorId + '" Title="Edit Indicator" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Indicator" onclick="deleteConfirmProductIndicator(this)" data-code="' + row.ProductIndicatorId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
     return tr;
}

function deleteConfirmProductIndicator(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductIndicator(value);
    });
}

function DeleteProductIndicator(productTypeId) {
    var indicatorModel = {
        ProductIndicatorId: productTypeId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteIndicator",
        data: indicatorModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Indicator deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductIndicators'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function SaveProductIndicator() {
    if ($('#txtIndicatorName').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var indicatorModel = {
        ProductIndicatorId: $('#productTypeId').val(),
        Description: $('#txtIndicatorName').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateIndicator",
        data: indicatorModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Indicator saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/ProductIndicators'; }, 3000);
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }

            $('.preloader').css('display', 'none');
        },
    });
}
