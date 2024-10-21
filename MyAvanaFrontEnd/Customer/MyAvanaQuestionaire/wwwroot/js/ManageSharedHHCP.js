function onRender(data, type, row, meta) {
    var tr="";
    if (row.IsRevoked == false) {
        tr = '<a class="btn btn-danger text-white" onclick="openRevokeAccess(\'' + row.Id + '\',\'' + row.SharedWith + '\')" Title="Revoke Access" >Revoke Access</a>';
        
    }
    
    return tr;
}
function formatRevokedOn(data) {
    if (!data) return null;

    // Create a Date object from the input
    const date = new Date(data);

    // Check if the date is valid
    if (isNaN(date.getTime())) return null;

    // Get the components of the date
    const optionsDate = {
        year: 'numeric',
        month: 'short',
        day: '2-digit'
    };
    const optionsTime = {
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    };

    // Format the date and time
    const formattedDate = date.toLocaleDateString('en-US', optionsDate);
    const formattedTime = date.toLocaleTimeString('en-US', optionsTime);

    // Combine the date and time with the desired format
    return `${formattedDate} | ${formattedTime}`;
}
function openRevokeAccess(Id, SharedWith) {
    Id = Id;
    SharedWithUserId = SharedWith;
    $('#confirmRevokeModalText').text('Are you sure you want to revoke access for this HHCP?');
    $('#confirmRevokeModal').modal('show');
    $("#confirmRevokeMethod").prop("onclick", null).off("click");
    $("#confirmRevokeMethod").click(function () {

        RevokeAccessHHCP(Id, SharedWithUserId);
    });

}
function RevokeAccessHHCP(SharedHHCPId,SharedWith) {
    var sharedHHCP = {
        Id: SharedHHCPId,
        SharedWith: SharedWith
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairProfile/RevokeAccessHHCP",
        data: sharedHHCP,
        success: function (response) {
            if (response === "1") {
              
                $('#successMessage').text("Access revoked successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmRevokeModal').modal('hide');
                setTimeout(function () { window.location.href = '/HairProfile/ManageSharedHHCP'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function openEmailModal() {
    $('#emailModal').modal('show');
    $('#emailInput').val('');
}

function validateEmail(email) {
    if (!RegExp(/^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/).test(email)) {
        return false;
    }
    return true;
}
function checkEmailExists(email, hairProfileId) {

    $.ajax({
        type: 'POST',
        url: '/HairProfile/ShareHHCP',
        data: { email: email, hairProfileId: hairProfileId },
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Your Profile is shared successfully!");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#emailModal').modal('hide');
                setTimeout(function () { window.location.href = '/HairProfile/ManageSharedHHCP'; }, 3000);
            }
            else if (response == "2") {
                document.getElementById('emailValidationMessage').textContent = 'Entered email does not exist!';
                $('#emailValidationMessage').show();
            }
            else {
                $('#failureMessage').text("Something went wrong.Please try later");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
                $('#emailModal').modal('hide');
            }
        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });

}
function shareEmail() {
    var email = $('#emailInput').val();
    $('#emailValidationMessage').hide();
    $('#emailExistMessage').hide();

    if (validateEmail(email) == true) {
        var hairProfileId = $('#ddlHHCP').val();
        checkEmailExists(email, hairProfileId);
    } else {
        document.getElementById('emailValidationMessage').textContent = 'Please enter a valid email.';
        $('#emailValidationMessage').show();
    }
}


// Bind the share button click event
$(document).ready(function () {
    $('#shareButton').click(function () {
        shareEmail();
    });

    // Optionally, hide validation message when input changes
    $('#emailInput').on('input', function () {
        $('#emailValidationMessage').hide();
    });
});

function Clearfileds() {
    $('#emailInput').val('');
    $('#emailValidationMessage').hide();
    $('#ddlHHCP').find('option:first').prop('selected', true);
}
