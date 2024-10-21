var salonDetails = new FormData();
function SaveSalon() {
    if (!ValidateForm()) {
        return false;
    }
    
    //var salonDetails = {
    //    SalonId: $('#salonId').val(),
    //    SalonName: $('#SalonName').val().trim(),
    //    EmailAddress: $('#EmailAddress').val().trim(),
    //    PhoneNumber: $('#PhoneNumber').val().trim(),
    //    Address: $('#Address').val().trim(),
    //    IsActive: $('#IsActive').is(":checked"),
    //    IsPublicNotes: $('#IsPublicNotes').is(":checked"),
    //    SalonLogo: $('#txtImageName').val()

         
    //}
    salonDetails.append('SalonId', $('#salonId').val());
    salonDetails.append('SalonName', $('#SalonName').val().trim());
    salonDetails.append('EmailAddress', $('#EmailAddress').val().trim());
    salonDetails.append('PhoneNumber', $('#PhoneNumber').val().trim());
    salonDetails.append('Address', $('#Address').val().trim());
    salonDetails.append('IsActive', $('#IsActive').is(":checked"));
    salonDetails.append('IsPublicNotes', $('#IsPublicNotes').is(":checked"));
    var file = $('#SalonImage').get(0).files[0];
   //if (file != null || file != undefined) {
        salonDetails.append('File', file);
   // } else {
    salonDetails.append('SalonLogo', $("#txtImageName").text());
   // }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Salon/CreateNewSalon",
        data: salonDetails,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Salon saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Salon/Salons'; }, 3000);
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
// vallidate Form
function ValidateForm() {
    var valid = true;
    if ($('#SalonName').val().trim() == "") {
        $('#failureMessage').text("Please enter name !");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        valid = false;
    }
    else if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test($('#EmailAddress').val().trim())) {
        $('#failureMessage').text("Please enter valid email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        valid = false;
    }
    return valid;
}
$('#btnImageClick').click(function () {
    $('#SalonImage').click();
});
$('#SalonImage').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        //$('#selectedImage').css('display', 'block');
        $('#txtImageName').text('');
        $('#txtImageName').text(filename);
    }
});