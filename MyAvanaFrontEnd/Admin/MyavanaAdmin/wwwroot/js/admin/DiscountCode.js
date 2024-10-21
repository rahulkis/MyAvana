function SaveDiscountCode() {
    if ($('#txtDiscountCode').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    if ($('#txtDiscountPercent').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    if ($('#txtExpireDate').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var selectedDate = $('#txtExpireDate').datepicker('getDate');
    var formattedExpireDate = moment(selectedDate).format('YYYY-MM-DD HH:mm:ss.SSSSSSS');

    var discountCodeEntity = {
        DiscountCodeId: $('#discountCodeId').val(),
        DiscountCode: $('#txtDiscountCode').val(),
        DiscountPercent: $('#txtDiscountPercent').val(),
        ExpireDate: formattedExpireDate
    }

    //var discountCodeEntity = {
    //    DiscountCodeId: $('#discountCodeId').val(),
    //    DiscountCode: $('#txtDiscountCode').val(),
    //    DiscountPercent: $('#txtDiscountPercent').val(),
    //    ExpireDate: $('#txtExpireDate').val(),
    //}
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/DiscountCode/CreateDiscountCode",
        data: discountCodeEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Discount code saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/DiscountCode/ViewDiscountCode'; }, 3000);
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

function DeleteDiscountCode(discountCodeId) {
    var discountCodeEntity = {
        DiscountCodeId: discountCodeId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/DiscountCode/DeleteDiscountCode",
        data: discountCodeEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product tag deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/DiscountCode/ViewDiscountCode'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function deleteConfirmDiscountCode(event) {
    var value = $(event).attr('data-code');
     
    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteDiscountCode(value);
    });
}

function onRenderDiscountCode(data, type, row, meta) {
    var tr = '<a href="/DiscountCode/CreateDiscountCode/' + row.DiscountCodeId + '" Title="Edit Discount Code" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Discount Code" onclick="deleteConfirmDiscountCode(this)" data-code="' + row.DiscountCodeId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}