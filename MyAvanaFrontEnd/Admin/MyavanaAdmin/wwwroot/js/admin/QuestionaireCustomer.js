var userId, userEmail, userName, messageData = new FormData(), passwordData = new FormData(), validMessage = true;
function onRenderId(data, type, row, meta) {
    var tr;
    if (row.IsQuestionnaire == false) {
        tr = '<a href="/Questionnaire/start/' + row.UserId + '" Title="Add QA" ><img src="http://admin.test.com/images/questionaireAdmin.png"/></i></a>';
        tr += '<a href="/Questionnaire/CreateNewCustomer/' + row.UserId + '" Title="Edit customer" class="edit_customer"><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    }
    else {
        tr = '<a href="/Questionnaire/QuestionaireSurvey?id=update&userId=' + row.UserId + '" href="/Questionnaire/start/' + row.UserId + '" Title="Update QA" ><img src="http://admin.test.com/images/QAupdate.png"/></i></a>';
        tr += '<a href="/Questionnaire/CreateNewCustomer/' + row.UserId + '" Title="Edit customer" class="edit_customer"><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    }
    return tr;
}
function  onRenderViewScalpAnalysisButtonHtml(data, type, row, meta) {
    var tr;
    if (row.IsQuestionnaire == false) {      
        tr = '<a class="btn btn-primary" href="/Questionnaire/start/' + row.UserId + '" >Run Hair AI</a>';     
    }
    else { 
        tr = '<a class="btn btn-primary" href="/Questionnaire/DigitalAssessment?userId=' + row.UserId + '" >Run Hair AI</a>';
  
    }
    return tr;
}
function onRenderCustomerMsg(data, type, row, meta) {
    var tr;
    var dataArray = new Array();
    dataArray.push(row.UserId);
    dataArray.push(row.UserEmail);
    tr = '<a onclick="openCustomerMessageModal(\'' + row.UserId + '\',\'' + row.UserEmail + '\',\'' + row.UserName + '\')" Title="Send Message" ><i class="fa fa-envelope"></i></a>';
    return tr;
}
function onRenderCustomerChangeHistory(data, type, row, meta) {
    var tr;
    tr = '<a onclick="openCustomerTypeHistoryModal(\'' + row.UserId + '\',)" Title="Customer Type History" ><i class="fa fa-history"></i></a>';
    return tr;
}
function onRenderCustomerHairDiary(data, type, row, meta) {
    var tr;
    tr = '<a onclick="openCustomerHairDiaryModal(\'' + row.UserEmail + '\',)" Title="Customer Type History" ><i class="fa fa-history"></i></a>';
    return tr;
}
function openCustomerTypeHistoryModal(Id) {
    $.ajax({
        type: "GET",
        url: "/Questionnaire/GetCustomerTypeHistory",
        data: { 'Id': Id },
        success: function (response) {
            $('#ShowCustomerTypeHistory').modal('show');
            $('#tBodyMessage').html('');
            for (var d in response.data) {
                var data = response.data[d];
                $('#tBodyMessage').append($('<tr>')
                    .append($('<td>', { text: data.oldCustomerType }))
                    .append($('<td>', { text: data.newCustomerType }))
                    .append($('<td>', { text: data.updatedBy }))
                    .append($('<td>', { text: data.comment }))
                    .append($('<td>', { text: data.createdDate }))
                    .append($('</tr>'))
                )
            }




        },
        error: function (response) {

        },
        complete: function () {

        }
    });

}

var Id;

$(document).ready(function () {
    function initializeDatepicker() {
        $('.custom-datepicker').datepicker({
            dateFormat: 'yy-mm-dd',
            beforeShow: function (input, inst) {
                inst.dpDiv.appendTo($('#ShowCustomerHairDiary'));
            }
        });
    }

    initializeDatepicker();

    $('#prevDateBtn').on('click', function () {
        var currentDate = $(".custom-datepicker").datepicker("getDate");
        currentDate.setDate(currentDate.getDate() - 1);

        var formattedDate = $.datepicker.formatDate('yy-mm-dd', currentDate);

        $(".custom-datepicker").datepicker("setDate", currentDate);
        fetchData(Id, formattedDate);
    });

    $('#nextDateBtn').on('click', function () {
        var currentDate = $(".custom-datepicker").datepicker("getDate");
        currentDate.setDate(currentDate.getDate() + 1);

        var formattedDate = $.datepicker.formatDate('yy-mm-dd', currentDate);

        $(".custom-datepicker").datepicker("setDate", currentDate);
        fetchData(Id, formattedDate);
    });

    $("#datepicker").click(function () {
        $(".custom-datepicker").datepicker("show");
    });

    $('#ShowCustomerHairDiary').on('hidden.bs.modal', function () {
        $(".custom-datepicker").datepicker("destroy");
        initializeDatepicker();

        $('#tBodyHairDiary').html('');
    });
});

function fetchData(Id, selectedDate) {
    var Track = {
        UserId: Id,
        TrackTime: selectedDate
    };

    var dataAvailable = false;

    $.ajax({
        type: "GET",
        url: "/Questionnaire/GetDailyRoutineWeb",
        data: Track,
        success: function (response) {
            if (response.data && response.data.length > 0) {
                dataAvailable = true;

                $('#tBodyHairDiary').html('');
                for (var d in response.data) {
                    var data = response.data[d];
                    var thumbnailImage = '';
                    if (data.profileImage) {
                        thumbnailImage = '<div class="col-md-3 thumbnail-container">' +
                            '<a href="' + data.profileImage + '" data-lightbox="image-set">' +
                            '<img src="' + data.profileImage + '" style="max-width: 50px; max-height: 50px;" class="img-thumbnail">' +
                            '<div class="hover-text">Click to view image</div>' +
                            '</a>' +
                            '</div>';
                    }

                    $('#tBodyHairDiary').append($('<tr>')
                        .append($('<td>').html(thumbnailImage))
                        .append($('<td>', { text: data.hairStyle }))
                        .append($('<td>', { text: data.description }))
                        .append($('<td>', { text: data.currentMood }))
                        .append($('<td>', { text: data.guidanceNeeded }))
                        .append($('</tr>'))
                    );
                }
            }
        },
        error: function (response) {

        },
        complete: function () {
            if (!dataAvailable) {
                $('#tBodyHairDiary').html('<tr><td class="center-text" colspan="5">No data available</td></tr>');
            }

            $('#ShowCustomerHairDiary').modal('show');
        }
    });
}




function openCustomerHairDiaryModal(customerId) {
    Id = customerId;
    var currentDate = new Date().toISOString().split('T')[0];

    fetchData(Id, currentDate);

    $('#ShowCustomerHairDiary').modal('show');

    $(".custom-datepicker").datepicker("option", "onSelect", function (newDate) {
        fetchData(Id, newDate);
    });

    $(".custom-datepicker").datepicker("setDate", currentDate);

    $('#ShowCustomerHairDiary').on('hidden.bs.modal', function () {
        $(".custom-datepicker").datepicker("option", "onSelect", null);
    });
}







function onRenderViewCustomerMsg(data, type, row, meta) {
    var tr;
    tr = '<a href="/Questionnaire/CustomerMessages?userId=' + row.UserId + '" Title="View Messages" ><i class="fa fa-history"></i></a>';
    return tr;
}

function onRenderResetPassword(data, type, row, meta) {
    var tr;
    var dataArray = new Array();
    dataArray.push(row.UserId);
    dataArray.push(row.UserEmail);
    tr = '<a onclick="openResetCustomerPassword(\'' + row.UserId + '\',\'' + row.UserEmail + '\',\'' + row.UserName + '\')" Title="Reset Password" ><i class="fa fa-key"></i></a>';
    return tr;
}

function onRenderCustomerChange(data, type, row, meta) {
    var tr;
    var dataArray = new Array();
    dataArray.push(row.UserId);
    dataArray.push(row.UserEmail);
    //if (row.IsProCustomer == false)
    tr = '<a onclick="openChangeCustomerType(\'' + row.UserId + '\',\'' + row.UserName + '\')" Title="Change Customer Type" ><i class="fa fa-user"></i></a>';
    //else
    //tr = "";
    return tr;
}
function onRenderCustomerStatusTracker(data, type, row, meta) {
    var tr="";
    if (row.StatusTrackerStatus == "") {
        tr = '<a onclick="OpenCustomerStatusTracker(\'' + row.UserId + '\',\'' + row.UserName + '\')" Title="Add To Status Tracker" ><i class="fa fa-plus"></i></a>';
        
   }
    return tr;
}
function onRenderActivate(data, type, row, meta) {
    var tr;
    if (row.Active == false)
        tr = '<a onclick="openActivateCustomer(\'' + row.UserId + '\',\'' + row.Active + '\')" Title="Activate Customer" ><i class="fa fa-toggle-off"></i></a>';
    else
        tr = '<a onclick="openActivateCustomer(\'' + row.UserId + '\',\'' + row.Active + '\')" Title="Deactivate Customer" ><i class="fa fa-toggle-on"></i></a>';
    return tr;
}

function openResetCustomerPassword(Id, email, name) {
    userId = Id;
    userEmail = email;
    userName = name;
    clearResetPasswordValues();
    $('#resetCustomerPassword').modal('show');

}

function openChangeCustomerType(Id, name) {
    userId = Id;
    userName = name;
    $('#confirmChangeModalText').text('Are you sure you want to change customer type?');
    $('#confirmChangeModal').modal('show');
    $("#confirmChangeMethod").prop("onclick", null).off("click");
    $("#confirmChangeMethod").click(function () {

        ChangeCustomerType(userId);
    });

}
function OpenCustomerStatusTracker(Id, name) {
    userId = Id;
    userName = name;
    
    $('#StatusTrackerModal').modal('show');
    $("#confirmAddMethod").prop("onclick", null).off("click");
    $("#confirmAddMethod").click(function () {
        AddToStatusTracker(userId);
    });

}
function openActivateCustomer(Id, IsActive) {
    userId = Id;
    userName = name;
    if (IsActive == "true") {
        $('#confirmActivateCustomerModalText').text('Are you sure you want to deactivate this customer account?');
        $('#confirmActivateCustomerMethod').text('Deactivate');
    } else {
        $('#confirmActivateCustomerModalText').text('Are you sure you want to activate this customer account?');
        $('#confirmActivateCustomerMethod').text('Activate');
    }
    $('#confirmActivateCustomerModal').modal('show');
    $("#confirmActivateCustomerMethod").prop("onclick", null).off("click");
    $("#confirmActivateCustomerMethod").click(function () {
        $('#confirmActivateCustomerModal').modal('hide');
        ActivateCustomer(userId, IsActive);
    });

}
function ActivateCustomer(userId, IsActive) {
    var userEntity = {
        UserId: userId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/ActivateCustomer",
        data: userEntity,
        success: function (response) {
            if (response === "1") {
                if (IsActive == "true") {
                    $('#successMessage').text("Customer account deactivated successfully !");
                } else {
                    $('#successMessage').text("Customer account activated successfully !");
                }
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmActivateCustomerModal').modal('hide');
                setTimeout(function () { window.location.href = '/Questionnaire/QuestionnaireCustomer'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function ChangeCustomerType(userId) {
    var chekValidation = true;
    if ($('#ddlCustomerType').val() == '0') {
        $("#CustomerTypeValidation").addClass("field-validation-error");
        $("#CustomerTypeError").text("CustomerType required.");
        chekValidation = false;
    }
    if (!chekValidation) {
        return false;
    }
    var userEntity = {
        UserId: userId,
        CustomerTypeId: $('#ddlCustomerType').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/ChangeCustomerType",
        data: userEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Customer type changed successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                $('#confirmChangeModal').modal('hide');
                setTimeout(function () { window.location.href = '/Questionnaire/QuestionnaireCustomer'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function AddToStatusTracker(userId) {
    var chekValidation = true;
    if ($('#ddlHairAnalysisStatus').val() == '0') {
        $("#StatusValidation").addClass("field-validation-error");
        $("#StatusError").text("Hair analysis status required.");
        chekValidation = false;
    }
    if (!chekValidation) {
        return false;
    }
    var trackerEntity = {
        CustomerId: userId,
        HairAnalysisStatusId: $('#ddlHairAnalysisStatus').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairAnalysisStatusTracker/AddToStatusTracker",
        data: trackerEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Customer added to status tracker successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#StatusTrackerModal').modal('hide');
               
                setTimeout(function () { window.location.href = '/Questionnaire/QuestionnaireCustomer'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function clearMessageValues() {
    validMessage = true;
    $('#txtSubject').val("");
    $('#txtMessageText').val("");
    $("#messageError").text("");
    $("#subjectError").text("");
    $("#subjectValidation").removeClass("field-validation-error");
    $("#messageValidation").removeClass("field-validation-error");
    $('#spnFileAttach').html('');
    $('#spnFileAttach').css('display', 'block');
    messageData = new FormData();
}

function clearResetPasswordValues() {
    validMessage = true;
    $('#txtNewPassword').val("");
    $('#txtConfirmPassword').val("");
    $("#newPasswordError").text("");
    $("#confirmPasswordError").text("");
    $("#newPasswordValidation").removeClass("field-validation-error");
    $("#confirmPasswordValidation").removeClass("field-validation-error");
    passwordData = new FormData();
}

$('#btnImageClick').click(function () {
    $('#attachmentFile').click();
});

$('#attachmentFile').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        $('#spnFileAttach').html('');
        $('#spnFileAttach').css('display', 'flex');
        $('#spnFileAttach').append("<span id=" + filename + " class='spnAttachment' >" + filename + "<i class='fa fa-times' onclick=deleteFile('" + filename + "')></i></span>");
    }
});

function sendCustomerMessage() {
    if ($('#txtSubject').val() == '') {
        $("#subjectValidation").addClass("field-validation-error");
        $("#subjectError").text("Subject required.");
        validMessage = false;
    }
    if ($('#txtMessageText').val() == '') {
        $("#messageValidation").addClass("field-validation-error");
        $("#messageError").text("Message required.");
        validMessage = false;
    }
    if (!validMessage) {
        return false;
    }
    var file = $('#attachmentFile').get(0).files[0];

    messageData.append("UserId", userId);
    messageData.append("EmailAddress", userEmail);
    messageData.append("Subject", $('#txtSubject').val());
    messageData.append("Message", $('#txtMessageText').val());
    messageData.append("AttachmentFile", $('#selectedImage').val());
    messageData.append("File", file);
    messageData.append("UserName", userName);

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/SaveCustomerMessage",
        data: messageData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Message sent successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#sendCustomerMessage').modal('hide');
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
                $('#sendCustomerMessage').modal('hide');
            }
            $('.preloader').css('display', 'none');
        },
    });
}

function resetCustomerPassword() {
    validMessage = true;
    if ($('#txtNewPassword').val() == '') {
        $("#newPasswordValidation").addClass("field-validation-error");
        $("#newPasswordError").text("New password required.");
        validMessage = false;
    }
    if ($('#txtConfirmPassword').val() == '') {
        $("#confirmPasswordValidation").addClass("field-validation-error");
        $("#confirmPasswordError").text("Confirm password required.");
        validMessage = false;
    }
    if (!validMessage) {
        return false;
    }
    if ($('#txtNewPassword').val() != $('#txtConfirmPassword').val()) {
        $("#confirmPasswordValidation").addClass("field-validation-error");
        $("#confirmPasswordError").text("Passwords did not match.");
        return false;
    }

    passwordData.append("Email", userEmail);
    passwordData.append("Password", $('#txtNewPassword').val());

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/ResetCustomerPassword",
        data: passwordData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Password reset successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#resetCustomerPassword').modal('hide');
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
                $('#resetCustomerPassword').modal('hide');
            }
            $('.preloader').css('display', 'none');
        },
    });
}

function signupUser() {


    if ($('#txtEmail').val().trim() == '' || $('#txtFirstName').val().trim() == '' || $('#txtLastName').val().trim() == '') {
        $('#failureMessage').text("Please fill mandatory fields");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#txtEmail').val().trim())) {
        $('#failureMessage').text("Please enter valid email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    if ($("#chkAutoPwd").is(":checked") != true) {
        if ($('#txtPassword').val().trim() == '' || $('#txtCPassword').val().trim() == '') {
            $('#failureMessage').text("Please fill mandatory fields");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(2000).fadeOut();
            return false;
        }
        var pattern = new RegExp(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{8,}$/);
        if (!pattern.test($('#txtCPassword').val().trim())) {
            $('#failureMessage').text("Password pattern doesn't match. Must atleast 8 character length.One upper case, one lower case, one digit and 1 special chanracter");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
        if ($('#txtCPassword').val().trim() != $('#txtPassword').val().trim()) {
            $('#failureMessage').text("Password and confirm password doesn't match");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }
    if (!/^$|^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test($("#txtPhoneNo").val().trim())) {
        $('#failureMessage').text("Enter valid phone no.");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    signup = {
        Email: $('#txtEmail').val().trim(),
        FirstName: $('#txtFirstName').val().trim(),
        LastName: $('#txtLastName').val().trim(),
        PhoneNo: $('#txtPhoneNo').val().trim(),
        CountryCode: $("#txtCountryCode").intlTelInput("getSelectedCountryData").dialCode,
        Password: $('#txtPassword').val().trim(),
     
    }
    // Check if UserTypeId cookie is "1" and add IsInfluencer field if true
    if (document.cookie.split('; ').find(row => row.startsWith('UserTypeId='))?.split('=')[1] === "1") {
        signup.IsInfluencer = $('#IsInfluencer').is(":checked");
    }
    $('.preloader').css('display', 'block');

    $.ajax({
        type: "POST",
        url: "/Questionnaire/CreateNewCustomer",
        data: signup,
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response == "0") {
                $('#failureMessage').text("User already exist. Please try with different Username");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();

            }
            else if (response == "-1") {
                $('#failureMessage').text("Something went wrong. Please try later");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();

                //$('#txtLogin').val(''),
                //$('#txtPassword').val('')
            }
            else {
                window.location.href = "/Questionnaire/QuestionnaireCustomer"
            }

        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });
}

function updateUser() {

    if ($('#txtEmail').val().trim() == '' || $('#txtFirstName').val().trim() == '' || $('#txtLastName').val().trim() == '') {
        $('#failureMessage').text("Please fill mandatory fields");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#txtEmail').val().trim())) {
        $('#failureMessage').text("Please enter valid email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    if (!/^$|^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test($("#txtPhoneNo").val().trim())) {
        $('#failureMessage').text("Enter valid phone no.");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    signup = {
        Email: $('#txtEmail').val().trim(),
        FirstName: $('#txtFirstName').val().trim(),
        LastName: $('#txtLastName').val().trim(),
        PhoneNo: $('#txtPhoneNo').val().trim(),
        UserId: $('#txtUserId').val().trim(),
        
    }
    // Check if UserTypeId cookie is "1" and add IsInfluencer field if true
    if (document.cookie.split('; ').find(row => row.startsWith('UserTypeId='))?.split('=')[1] === "1") {
        signup.IsInfluencer = $('#IsInfluencer').is(":checked");
    }
    $('.preloader').css('display', 'block');

    $.ajax({
        type: "POST",
        url: "/Questionnaire/UpdateCustomer",
        data: signup,
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response == "4") {
                $('#failureMessage').text("Unable to update user details as user has already subscribed for Hair AI.");
                $('#txtEmail').val($('#txtOrgEmail').val())
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
           else if (response == "3") {
                $('#failureMessage').text("Unable to update user details as HHCP was already created for the user.");
                $('#txtEmail').val($('#txtOrgEmail').val())
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            else if (response == "-1") {
                $('#failureMessage').text("Something went wrong. Please try later.");
                $('#txtEmail').val($('#txtOrgEmail').val())
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();

            }
            else if (response == "2") {
                $('#failureMessage').text("Email already exists.");
                $('#txtEmail').val($('#txtOrgEmail').val())
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();

            }
            else {
                $('#successMessage').text("Customer details updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Questionnaire/QuestionnaireCustomer'; }, 3000);
            }

        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });
}