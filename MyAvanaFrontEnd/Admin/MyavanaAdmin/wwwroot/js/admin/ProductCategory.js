$(document).ready(function () {
 	$("input[type='search']").val("");
    var hair = $('#isHair').val();
    var regimen = $('#isRegimen').val();
    if (hair == "value")
        $('#productTypeCategory').val(1);
    else if (regimen == "value")
        $('#productTypeCategory').val(2);
    else
        $('#productTypeCategory').val(0);
});

function onRenderProductType(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductCategory/' + row.ProductTypeId + '" Title="Edit Category" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Category" onclick="deleteConfirmProductType(this)" data-code="' + row.ProductTypeId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
     return tr;
}


function deleteConfirmProductType(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductType(value);
    });
}

function DeleteProductType(productTypeId) {
    var productTypeEntity = {
        Id: productTypeId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductCategory",
        data: productTypeEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product Category deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductsCategory'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}


function onRenderHair(data, type, row, meta) {
    var tr = "";
    if (row.IsHair == true) {
        tr = '<i class="fa fa-check-circle green-color" ></i>';
        return tr;
    }
    return tr;
}

function onRenderRegimen(data, type, row, meta) {
    var tr = "";
    if (row.IsRegimen == true) {
        tr = '<i class="fa fa-check-circle green-color" ></i>';
        return tr;
    }
    return tr;
}


function SaveProductCategory() {
    if ($('#productTypeCategory').val() == 0 || $('#txtCategoryName').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var productTypeEntity = {
        ProductTypeId: $('#productTypeId').val(),
        CategoryName: $('#txtCategoryName').val(),
        IsHair: $('#productTypeCategory').val() == 1 ? true : null,
        IsRegimen: $('#productTypeCategory').val() == 2 ? true : null
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateProductCategory",
        data: productTypeEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Product Category saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/ProductsCategory'; }, 3000);
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