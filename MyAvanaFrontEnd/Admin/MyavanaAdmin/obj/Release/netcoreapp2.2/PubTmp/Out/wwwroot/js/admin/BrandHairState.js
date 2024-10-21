function SaveHairState() {
    if ($('#txtHairStateDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    var HairStateEntity = {
        HairStateId: $('#hairstateId').val(),
        Description: $('#txtHairStateDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/CreateHairState",
        data: HairStateEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Brand Hair State saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Brand/BrandHairState'; }, 3000);
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
function onRenderHairState(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateHairState/' + row.HairStateId + '" Title="Edit Hair State" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    // Check if UserTypeId is not equal to 3 i.e. dataentry 
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Hair State" onclick="deleteConfirmHairState(this)" data-code="' + row.HairStateId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}
function deleteConfirmHairState(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteHairState(value);
    });
}
function DeleteHairState(HairStateId) {
    var HairStateEntity = {
        HairStateId: HairStateId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteHairState",
        data: HairStateEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Hair State deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/BrandHairState'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}