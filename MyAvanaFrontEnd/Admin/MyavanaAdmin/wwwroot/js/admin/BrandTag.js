function SaveTag() {
    if ($('#txtTagDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    var TagEntity = {
        TagId: $('#tagId').val(),
        Description: $('#txtTagDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/CreateTag",
        data: TagEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Tag saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Brand/Tags'; }, 3000);
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
function onRenderTag(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateTag/' + row.TagId + '" Title="Edit Tag" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    // Check if UserTypeId is not equal to 3 i.e. dataentry 
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Tag" onclick="deleteConfirmTag(this)" data-code="' + row.TagId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}
function deleteConfirmTag(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteTag(value);
    });
}
function DeleteTag(TagId) {
    var TagEntity = {
        TagId: TagId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteTag",
        data: TagEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Tag deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/Tags'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}