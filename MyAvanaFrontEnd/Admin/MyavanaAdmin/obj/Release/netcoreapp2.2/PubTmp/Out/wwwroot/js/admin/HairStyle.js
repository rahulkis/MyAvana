function onRenderHairStyle(data, type, row, meta) {
    var tr = '<a href="/Products/CreateHairStyle/' + row.Id + '" Title="Edit Hair Style" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Hair Style" onclick="deleteConfirmHairStyle(this)" data-code="' + row.Id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
     return tr;
}
function deleteConfirmHairStyle(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteHairStyle(value);
    });
}
function DeleteHairStyle(Id) {
    var hairStyleEntity = {
        Id: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteHairStyle",
        data: hairStyleEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Hair Style deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/HairStyle'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function SaveHairStyle() {
    if ($('#txtStyle').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var hairStyleEntity = {
        Id: $('#hairStylesId').val(),
        Style: $('#txtStyle').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateHairStyle",
        data: hairStyleEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Hair style saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/HairStyle'; }, 3000);
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