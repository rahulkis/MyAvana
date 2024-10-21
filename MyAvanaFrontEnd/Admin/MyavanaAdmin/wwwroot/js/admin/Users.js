function setUserType() {
    $('#userType').val($('#selectUserType').val())
    var selectedUserType = $('#selectUserType').val();
    if (selectedUserType == 2) {
        showSalons = true;
        $("#divSalonOwner").css("display", "block");
        $('.select2').select2({
            placeholder: "--Select--"
        });
    } else {
        showSalons = false;
        $("#divSalonOwner").css("display", "none");
    }
}
function SaveUser() {
    var ownerSalonsArray = [];
    $('.select2 option:selected').each(function (i, sel) {
        var salonOwners = {};
        if ($(this).val() != "") {
            salonOwners.SalonId = $(this).val();
            ownerSalonsArray.push(salonOwners);
        }
    });
    if (!ValidateUser(ownerSalonsArray)) {
        return false;
    }
    var userType;
    if ($('#userType'))
        userType = $('#selectUserType  option:selected').val();
    else {
        userType = $('#userType').val();
    }
    if (userType == undefined && getCookie("UserTypeId") == 2) {
            userType = getCookie("UserTypeId");
        }
    var webLogin = {
        UserId: $('#userId').val(),
        UserEmail: $('#txtEmail').val().trim(),
        OwnerSalonId: $('#OwnerSalonId').val(),
        //UserType: userType == 1 ? true : false,
        UserTypeId: userType,
        userSalons: ownerSalonsArray
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Auth/CreateNewUser",
        data: webLogin,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("User saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Auth/Users'; }, 3000);
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
// vallidate user fields
function ValidateUser(ownerSalonsArray) {
    var validUser = true;
    if ($('#txtEmail').val().trim() == "") {
        $('#failureMessage').text("Please enter email !");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        validUser = false;

    } else if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#txtEmail').val().trim())) {
            $('#failureMessage').text("Please enter valid email!");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            validUser = false;
        }

    userList.forEach(function (db, i) {
        if (db.UserEmail.toLowerCase() == $('#txtEmail').val().toLowerCase().trim() && $('#userId').val() != db.UserId) {
            $('#failureMessage').text("This email is already registered!");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(2000).fadeOut();
            validUser = false;
        }
    });
    if (showSalons && ownerSalonsArray.length == 0) {
        $('#failureMessage').text("Please select salon !");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        validUser = false;
    }
    return validUser;
}

function onRender(data, type, row, meta) {
    var tr = '<a href="/Auth/CreateNewUser/' + row.UserId + '" style="margin:2px;" Title="Edit User" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete User" onclick="deleteConfirmUser(' + row.UserId + ')" data-code="' + row.UserId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}
function onRenderSalonUsers(data, type, row, meta) {
    var tr = '<a Title="Delete User" onclick="deleteConfirmUser(' + row.UserId + ')" data-code="' + row.UserId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}
function onRenderUserType(data, type, row, meta) {
    var tr = '';
    if (row.UserTypeId == 2) {
        tr = '<span>B2B</span>';
    }
    else if (row.UserTypeId === 3) {
        tr = '<span>DataEntry</span>';
    }
    else {
        tr = '<span>Admin</span>';
    }
    return tr;
}

function deleteConfirmUser(userId) {
    var value = $(event).attr('data-code');
    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        deleteUser(userId);
    });
}

function deleteUser(userId) {
    var webLogin = {
        UserId: userId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Auth/DeleteUser",
        data: webLogin,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("User deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Auth/Users'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}