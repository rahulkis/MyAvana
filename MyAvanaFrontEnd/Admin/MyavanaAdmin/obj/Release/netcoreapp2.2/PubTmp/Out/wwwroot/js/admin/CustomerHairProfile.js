function changeHHCP() {
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const userId = urlParams.get('id')
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "GET",
        url: "/HairProfile/GetHHCP?id=" + $('#ddlHHCP').val() + "&&userId=" + userId,
        success: function (response) {
            $('.preloader').css('display', 'none');
            console.log(response);

            $('#divPartial').html(response);
            $('.demo').nailthumb({
                imageCustomFinder: function (el) {
                    var image = $('<img />').attr('src', el.attr('href').replace('/full/', '/small/')).css('display', 'none');
                    image.attr('alt', el.attr('title'));
                    el.append(image);
                    return image;
                },
                titleAttr: 'alt'
            });
            $('.demo').css("display", "block");
            //var note = $('#txtSalonNotes').val(); 
            //$('#txtMyNotes').val(note);
        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });


}

function createHHCPHairKitUser() {
    var createHHCPModel = {
        UserId: $('#UserId').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairProfile/CreateHHCPHairKitUser",
        data: createHHCPModel,
        success: function (response) {
            if (response == "1") {
                window.location.reload();
            }
            else {

            }

            $('.preloader').css('display', 'none');
        },
    });
}
function scrolldiv() {
    $('.frame-wrapper').animate({
        scrollTop: 150
    }, 1000);
}
function OpenNotesModal() {

    $('#addNotesModal').modal('show');

}
function SaveNotes() {
    var chekValidation = true;
    if ($('#txtMyNotes').val() == '') {
        $("#NotesValidation").addClass("field-validation-error");
        $("#NotesError").text("Please add notes.");
        chekValidation = false;
    }
    if (!chekValidation) {
        return false;
    }
    var notesData = {
        SalonNotes: $('#txtMyNotes').val(),
        HairProfileId: $('#ddlHHCP').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairProfile/SaveSalonNotes",
        data: notesData,
        success: function (response) {
            if (response === "1") {
                $('#txtMyNotes').val('');
                $('#addNotesModal').modal('hide');
                $('#successMessage').text("Note saved successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {
            setTimeout(function () {
                changeHHCP();
            }, 3000)

        }
    });
}
function showHide(sender) {
    var id = $(sender).attr('href');
    $('.showHide2Active').removeClass('active');
    $('.showHide2Active').removeClass('show');
    $('.showHide2').removeClass('active');
    $(sender).addClass('active');
    $(id).addClass('active');
    $(id).addClass('show');
}



function showHide1(sender) {
    var id = $(sender).attr('href');
    $('.showHide1Active').removeClass('active');
    $('.showHide1Active').removeClass('show');
    $('.showHide1').removeClass('active');
    $(sender).addClass('active');
    $(id).addClass('active');
    $(id).addClass('show');
}



$("#SalonId").change(function () {

    var model = {
        HairProfileId: $('#HairProfileId').val(),
        SalonId: parseInt($('#SalonId option:selected').val()),
        UserId: $('#UserId').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairProfile/UpdateHairProfileSalon",
        data: model,
        success: function (response) {
            if (response == "1") {

            }
            else {

            }

            $('.preloader').css('display', 'none');
        },
    });
});

$('#IsViewEnabled').change(function () {
    if (this.checked) {
        enableDisableView(true)
    } else {
        enableDisableView(false)
    }
});

function enableDisableView(isViewEnabled) {
    var model = {
        HairProfileId: $('#HairProfileId').val(),
        IsViewEnabled: isViewEnabled
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairProfile/EnableDisableProfileView",
        data: model,
        success: function (response) {
            if (response == "1") {

            }
            else {

            }

            $('.preloader').css('display', 'none');
        },
    });
}


function GetHHCPStrands() {
    if ($('#divPartialHairAnalysis').hasClass('loadedonce')) {
        return false;
    }
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const userId = urlParams.get('id')
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "GET",
        url: "/HairProfile/GetHHCPStrands?id=" + $('#ddlHHCP').val() + "&&userId=" + userId,
        success: function (response) {
            $('.preloader').css('display', 'none');

            $('#divPartialHairAnalysis').html(response);
            $('#divPartialHairAnalysis').addClass('loadedonce');
        },
        failure: function (response) {
        },
        error: function (response) {
        },
    });


}