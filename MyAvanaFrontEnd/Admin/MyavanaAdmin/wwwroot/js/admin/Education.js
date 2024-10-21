
function SaveEducationTip() {

    if ($('#description').val().trim() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    
    var educationTipModel = {
        EducationTipsId: $('#tipId').val(),
        Description: $('#description').val(),
        ShowOnMobile: $('#ShowOnMobile').val()
    }
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/SocialMedia/CreateEducationalTip",
        data: educationTipModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Education tip saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/SocialMedia/EducationalTips'; }, 3000);
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
			}

			$('.preloader').css('display', 'none');
        },
    });
};

function onRenderEducationalTip(data, type, row, meta) {
    var tr = '<a href="/SocialMedia/CreateEducationalTip/' + row.EducationTipsId + '" Title="Edit Tip" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Tip" onclick="deleteConfirmAlexaFAQ(this)" data-code="' + row.EducationTipsId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}


function deleteConfirmAlexaFAQ(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteAlexaFAQ(value);
    });
}

function DeleteAlexaFAQ(faqId) {
    var alexaFAQ = {
        Id: faqId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Alexa/DeleteAlexaFAQ",
        data: alexaFAQ,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("FAQ deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Alexa/ViewAlexaFAQs'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}