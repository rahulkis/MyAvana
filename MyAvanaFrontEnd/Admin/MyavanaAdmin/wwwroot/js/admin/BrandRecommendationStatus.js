function onRenderStatus(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateBrandRecommendationStatus/' + row.BrandRecommendationStatusId + '" Title="Edit brand recommendation Status" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    // Check if UserTypeId is not equal to 3 i.e. dataentry 
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Brand Recommendation Status" onclick="deleteConfirmRecommendationStatus(this)" data-code="' + row.BrandRecommendationStatusId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}

function SaveRecommendationStatus() {
    if ($('#txtRecommendatinStatusDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    var RecommendationStatusEntity = {
        BrandRecommendationStatusId: $('#hairRecommendationStatusId').val(),
        Description: $('#txtRecommendationStatusDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/CreateBrandRecommendationStatus",
        data: RecommendationStatusEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Brand Recommendation Status saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Brand/BrandRecommendationStatus'; }, 3000);
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

function deleteConfirmRecommendationStatus(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteRecommendationStatus(value);
    });
}
function DeleteRecommendationStatus(BrandRecommendationStatusId) {
    var RecommendationStatusEntity = {
        BrandRecommendationStatusId: BrandRecommendationStatusId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteBrandRecommendationStatus",
        data: RecommendationStatusEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Recommendation Status deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/BrandRecommendationStatus'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}