function SaveBrandClassification() {
    if ($('#txtProductDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var brandClassificationModel = {
        BrandClassificationId: $('#brandsClassificationId').val(),
        Description: $('#txtProductDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateBrandClassification",
        data: brandClassificationModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Brand classification saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/BrandClassification'; }, 3000);
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
var fileModel = new FormData();
function SaveProductImages() {
    var files = $('#productImage').get(0).files;
    for (var i = 0; i < files.length; i++) {
        fileModel.append('files', files[i]);
    }

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/UploadProductImages",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Product saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
            }
            else {
                $('#failureMessage').text(response);
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            $('.preloader').css('display', 'none');
        },
    });
}