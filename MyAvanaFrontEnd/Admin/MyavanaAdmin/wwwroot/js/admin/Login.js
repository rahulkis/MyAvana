$(document).keypress(function (e) {
    if (e.which == 13) {
        if ($('#ForgotAdminPassword').hasClass('show')) {
            ForgotPassword();
        }
        else {
            loginAdmin();
        }
    }                                                                                                                                                                                                                                                                                                                                                                     
});
document.addEventListener('DOMContentLoaded', function () {
    const rememberRadio = document.getElementById('remember');

    // Track the checked state
    let isChecked = false;

    rememberRadio.addEventListener('click', function () {
        // Toggle the checked state
        if (isChecked) {
            this.checked = false; // Uncheck if it was already checked
            isChecked = false; // Update the state
        } else {
            isChecked = true; // Set state to checked
        }
    });
});
function loginAdmin() {

    if ($('#txtLogin').val().trim() == '' || $('#txtPassword').val().trim() == '') {
        $('#failureMessage').text("Please enter username and password!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }

    if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#txtLogin').val().trim())) {
        $('#failureMessage').text("Please enter valid email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    //$('.preloader').css('display', 'block');
    $('.preloader').css('display', 'block').css('width', '100%');
    var rememberme;
    const rememberRadio = document.getElementById('remember');
    if (rememberRadio.checked) {
        rememberme = true;
    }
    else {
        rememberme = false;
    }

    webLgoin = {
        UserEmail: $('#txtLogin').val(),
        Password: $('#txtPassword').val(),
        RememberMe: rememberme
    }

    $.ajax({
        type: "POST",
        url: "/Auth/Login",
        data: webLgoin,
        success: function (response) {
            if (response == 1) {
                window.location.href = "/Articles/ViewArticles";
                localStorage.setItem('navbar', 1);
            } else if (response == 2) {
                window.location.href = "/Metrics/QAMetrics";
                localStorage.setItem('navbar', 2);
            }
            else if (response == 3) {
                window.location.href = "/Products/ProductsCategory";
                localStorage.setItem('navbar', 18);
            }
            else {
                $('#failureMessage').text("UserName or Password is incorrect!");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(2000).fadeOut();

                //$('#txtLogin').val(''),
                $('#txtPassword').val('')
            }
            $('.preloader').css('display', 'none');
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
   /* localStorage.setItem('navbar', 1);*/
}
function OpenForgotPassword() {
    ClearForgotPasswordValues();
    $('#ForgotAdminPassword').modal('show');
}

function ClearForgotPasswordValues() {
    validMessage = true;
    $('#txtEmail').val("");
    $("#textEmailErr").text("");
    $("#textEmailValidation").removeClass("field-validation-error");
}

function ForgotPassword() {

    if ($('#txtEmail').val().trim() == '') {
        $("#textEmailValidation").addClass("field-validation-error");
        $("#textEmailErr").text("Please enter you registered email.");
        return false;
    }

    if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#txtEmail').val().trim())) {
        $("#textEmailValidation").addClass("field-validation-error");
        $("#textEmailErr").text("Please enter valid email!");
        return false;
    }


    ;
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Auth/ForgotAdminPassword",
        data: { 'email': $('#txtEmail').val().trim() },
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response == "1") {
                $('#ForgotAdminPassword').modal('hide');
                $('#successMessage').text("We have sent you a secure code on your email. Please use that to reset your password.");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = "/Auth/ResetPwd" }, 3000);
            }
            else {
                $('#textEmailErr').text("Email id does not exist in application!");
            }
        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });

}
function ResetPassword() {

    
    if ($('#txtCode').val().trim() == '' || $('#txtPassword').val().trim() == '' || $('#txtCPassword').val().trim() == '') {
        $('#failureMessage').text("Please fill mandatory fields");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    var pattern = new RegExp(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{8,}$/);
    if (!pattern.test($('#txtPassword').val().trim())) {
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

    setPwd = {
        Email: $('#userEmail').val().trim(),
        Code: $('#txtCode').val().trim(),
        Password: $('#txtPassword').val().trim()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Auth/ResetUserPassword",
        data: setPwd,
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response == "0") {
                $('#failureMessage').text("Either Email or Code is incorrect. Please check and try again.");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();

            }
            else if (response == "1") {
                $('#successMessage').text("Password Updated Successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                window.location.href = "/Auth/Login";
            }

        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });
}