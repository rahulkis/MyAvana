function SaveProductType() {
    if ($('#productTypeCategory').val() == "" || $('#txtProductName').val() == "" || $('#txtRank').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var rankValue = $('#txtRank').val();
    if (!(/^\d+$/.test(rankValue))) {
        $('#failureMessage').text("Rank field accepts only integer values!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var productTypeEntity = {
        Id: $('#productTypeId').val(),
        CategoryId: $('#productTypeCategory').val(),
        ProductName: $('#txtProductName').val(),
        Rank: rankValue,
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateProductType",
        data: productTypeEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Product saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/ProductsType'; }, 3000);
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