
function SaveFAQ() {

    if ($('#description').val().trim() == "" || $('#keywords').val().trim() == "" || $('#shortResponse').val().trim() == "" || $('#detailedResponse').val().trim() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    
    var alexaFAQtModel = {
        Id: $('#faqId').val(),
        Description: $('#description').val().trim(),
        Keywords: $("#keywords").val().trim(),
        ShortResponse: $("#shortResponse").val().trim(),
        DetailedResponse: $("#detailedResponse").val().trim(),
        IsDeleted: 0,
        Category: $("#category").val().trim()
    }
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Alexa/CreateAlexaFAQ",
        data: alexaFAQtModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("FAQ saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Alexa/ViewAlexaFAQs'; }, 3000);
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

function onRenderAlexaFAQ(data, type, row, meta) {
    var tr = '<a href="/Alexa/CreateAlexaFAQ/' + row.Id + '" Title="Edit FAQ" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete FAQ" onclick="deleteConfirmAlexaFAQ(this)" data-code="' + row.Id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
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