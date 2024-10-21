

function deleteConfirmGroup(hairType) {
    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteGroup(hairType);
    });
}

function DeleteGroup(hairType) {
    var grpModel = {
        HairType: hairType
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Groups/DeleteGroup",
        data: grpModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Group deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Groups/GroupList'; }, 2000);
            }
            else {
                $('#failureMessage').text("Oop! something goes wrong. Please try later.");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
                return false;
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function approveRequest(id) {
    $('#confirmModalHeader').text('Approve');
    $('#confirmModalText').text('Are you sure you want to approve?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Approve');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        ApproveRequest(id);
    });

    function ApproveRequest(id) {
        var grpModel = {
            Id: id
        }
        $('.preloader').css('display', 'block');
        $.ajax({
            type: "POST",
            url: "/Groups/ApproveRequest",
            data: grpModel,
            success: function (response) {
                if (response === "1") {
                    $('#successMessage').text("Request approved successfully !");
                    $('.alert-success').css("display", "block");
                    $('.alert-success').delay(3000).fadeOut();
                    $('#confirmModal').modal('hide');
                    setTimeout(function () { window.location.href = '/Groups/GroupRequestList'; }, 2000);
                }
                else {
                    $('#failureMessage').text("Oop! something goes wrong. Please try later.");
                    $('.alert-danger').css("display", "block");
                    $('.alert-danger').delay(3000).fadeOut();
                    return false;
                }
                $('.preloader').css('display', 'none');
            },
            error: function (response) {

            },
            complete: function () {

            }
        });
    }
}

function CreateGroup() {
    var groupUserListingModel = [];
    var groupUserModel = {};
    var update = false;
    if (htype != null)
        update = true;
    var groupName = $('#hairType option:selected').val();
    if (groupName == "") {
        groupName = $('#grpName').val();
    }


    var withoutUser = true;
    $('.chkGroup:checkbox:checked').each(function () {
        var id = $(this).closest("tr")[0].id;
        groupUserModel = {};
        groupUserModel.HairType = groupName;
        groupUserModel.UserEmail = $('#eml_' + id).text();
        groupUserModel.IsUpdate = update;
        groupUserModel.IsPublic = $('#IsPublic').is(':checked');
        groupUserModel.ShowOnMobile = $('#ShowOnMobile').is(':checked');
        groupUserListingModel.push(groupUserModel);
        withoutUser = false;
    });
    if (withoutUser == true) {
        groupUserModel = {};
        groupUserModel.HairType = groupName;
        groupUserModel.IsUpdate = update;
        groupUserModel.IsPublic = $('#IsPublic').is(':checked');
        groupUserModel.ShowOnMobile = $('#ShowOnMobile').is(':checked');
        groupUserListingModel.push(groupUserModel);
    }
    $.ajax({
        type: "POST",
        url: "/Groups/CreateGroup",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(groupUserListingModel),
        success: function (result) {
            if (result == "1") {
                location.href = '/Groups/GroupList';
            }
            else if (result == "-1") {
                $('#failureMessage').text("Please select atleast one user to be added to group!");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
        }
    });
}

