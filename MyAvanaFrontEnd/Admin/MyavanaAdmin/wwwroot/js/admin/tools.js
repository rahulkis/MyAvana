
$('#btnImageClick').click(function () {
    debugger;
    $('#toolImage').click();
});

$('#toolImage').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        //$('#selectedImage').css('display', 'block');
        $('#txtImageName').text('');
        $('#txtImageName').text(filename);
    }
});

var fileModel = new FormData();
function SaveTool() {
    var onlyNumbers = /^[0-9\s]*$/;
    if ($('#txtProductName').val().trim() == "" || $('#txtImageName').text().trim() == "") {
        $('#failureMessage').text("Tool Name and Image can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    if ($("#txtLink").val().trim() != '') {
        if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#txtLink').val())) {
            $('#failureMessage').text("Wrong image Url");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }

    if ($('#txtActualPrice').val().trim() != '') {
        if (!onlyNumbers.test($('#txtActualPrice').val().trim())) {
            $('#failureMessage').text("Enter valid amount");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }

    if ($('#txtDecimalPrice').val().trim() != '') {
        if (!onlyNumbers.test($('#txtDecimalPrice').val().trim())) {
            $('#failureMessage').text("Enter valid amount");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }

    var file = $('#toolImage').get(0).files[0];
    fileModel.append('Id', $('#productid').val());
    fileModel.append('ToolName', $("#txtProductName").val().trim());
    fileModel.append('BrandName', $("#txtBrandName").val().trim());
    fileModel.append('ActualName', $("#txtActualName").val().trim());
    //fileModel.append('ImageName', $("#txtImageName").val().trim());
    // fileModel.append('Ingredients', $("#txtIngredients").val().trim());
    fileModel.append('ToolDetails', $("#txtProductDetails").val().trim());
    fileModel.append('ToolLink', $("#txtLink").val().trim());
    fileModel.append('Price', $('#txtActualPrice').val().trim() + "." + $('#txtDecimalPrice').val().trim());

    if (file != null || file != undefined) {
        fileModel.append('File', file);
    } else {
        fileModel.append('Image', $("#image").val())
    }

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Tools/CreateTool",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            var actualPrice = response.price;
            if (response == "1") {
                $('#successMessage').text("Tool saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Tools/Toolslist'; }, 3000);
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

function onRenderImage(data, type, row, meta) {
    return "<a href='" + data + "' target='_blank'><img src='" + data + "' style='width:100px;height:auto;' /></a>";
}

function onRender(data, type, row, meta) {
    var tr = '<a href="/Tools/CreateTool/' + row.Id + '" Title="Edit Tool" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Tool" onclick="deleteConfirmProduct(this)" data-code="' + row.Id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}


function deleteConfirmProduct(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProduct(value);
    });
}

function DeleteProduct(Id) {
    var toolsModel = {
        Id: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Tools/DeleteTool",
        data: toolsModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Tool deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Tools/Toolslist'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

