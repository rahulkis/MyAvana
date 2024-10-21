function PayViaStripe() {
    if ($('#expireYear').val() == '0' || $('#txtCardName').val() == '' || $('#txtCardNumber').val() == '' || $('#expireMonth').val() == '0' ||
        $('#txtCvv').val() == '') {
        $('#failureMessage').text("fields cannot be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    var selectedPaymentAmount;
    var isSubscriptionPayment;
    var selectedPaymentType = $('input[name=paymentType]:checked').val();
    if (selectedPaymentType == "oneTime") {
        selectedPaymentAmount = parseFloat($("#lbloneTimePayment").text().replace('$', ''));
    }
    else {
        selectedPaymentAmount = parseFloat($("#lblSubscriptionPayment").text().replace('$', ''));
        isSubscriptionPayment = true;
    }
    var checkoutrequest = {
        amount: selectedPaymentAmount,
        cardownerfirstname: $('#txtCardName').val().trim(),
        cardownerlastname: $('#txtCardName').val().trim(),
        expirationyear: $('#expireYear').val().trim(),
        cardnumber: $('#txtCardNumber').val().trim(),
        expirationmonth: $('#expireMonth').val().trim(),
        cvv2: $('#txtCvv').val().trim(),
        state: "",
        IsSubscriptionPayment: isSubscriptionPayment
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "post",
        url: "/auth/PackagePayments",
        data: checkoutrequest,
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response === "1") {
                var mToken = $('#mobToken').val();
                if (mToken != null & mToken != '') {
                    redirectToSame();
                }
                $('#divRequestSuccesss').show();
                $('#divWelcome').hide();
            }
            else {
                $('#failureMessage').text(response);
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(2000).fadeOut();
            }

        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function redirectTo() {
    window.location.href = "/Questionaire/start?id=";
}
function redirectToSame() {
    window.location.href = "/Auth/PackagePayment?message=success";
}

function HairAIPaymentViaStripe() {
    if ($('#expireYear').val() == '0' || $('#txtCardName').val() == '' || $('#txtCardNumber').val() == '' || $('#expireMonth').val() == '0' ||
        $('#txtCvv').val() == '') {
        $('#failureMessage').text("fields cannot be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    }
    var selectedPaymentAmount;
    var isSubscriptionPayment;
    var selectedPaymentType = $('input[name=paymentType]:checked').val();
    if (selectedPaymentType == "oneTime") {
        selectedPaymentAmount = parseFloat($("#lblDiscountedTotal").text().replace('$', ''));
    }
    else {
        selectedPaymentAmount = parseFloat($("#lblSubscriptionPayment").text().replace('$', ''));
        isSubscriptionPayment = true;
    }
    var signupAndPayment = {
        Email: $('#txtEmail').val().trim(),
        FirstName: $('#txtFirstName').val().trim(),
        LastName: $('#txtLastName').val().trim(),
        PhoneNo: $('#txtPhoneNo').val().trim(),
        CountryCode: $("#txtCountryCode").intlTelInput("getSelectedCountryData").dialCode,
        Password: $('#txtPassword').val().trim(),
        BuyHairKit: $('#checkboxHairKit').is(":checked"),
        KitSerialNumber: $('#txtSerialNumber').val().trim(),
        amount: selectedPaymentAmount,
        cardownerfirstname: $('#txtCardName').val().trim(),
        cardownerlastname: $('#txtCardName').val().trim(),
        expirationyear: $('#expireYear').val().trim(),
        cardnumber: $('#txtCardNumber').val().trim(),
        expirationmonth: $('#expireMonth').val().trim(),
        cvv2: $('#txtCvv').val().trim(),
        stateId: $('#txtState').val().trim(),
        city: $('#txtCity').val().trim(),
        countryId: $('#txtCountry').val().trim(),
        address: $('#txtAddress').val().trim(),
        Zipcode: $('#txtZipCode').val().trim(),
        Country: $('#txtCountry option:selected').text().trim(),
        state: $('#txtState option:selected').text().trim(),
        IsSubscriptionPayment: isSubscriptionPayment
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "post",
        url: "/auth/HairAIPackagePayments",
        data: signupAndPayment,
        success: function (response) {
            $('.preloader').css('display', 'none');
            if (response.startsWith('-1')) {
                const myArray = response.split(",");
                $('#failureMessage').text(myArray[1]);
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            else if (response == "0") {
                $('#failureMessage').text("User already exist. Please try with different Username");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            else if (response == "-1") {
                $('#failureMessage').text("Something went wrong. Please try later");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            else {
                redirectTo();
            }
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}