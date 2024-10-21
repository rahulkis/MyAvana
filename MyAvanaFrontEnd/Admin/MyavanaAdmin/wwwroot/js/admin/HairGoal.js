﻿function SaveHairGoal() {
    if ($('#txtDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var hairGoalEntity = {
        HairGoalId: $('#hairGoalsId').val(),
        Description: $('#txtDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateHairGoal",
        data: hairGoalEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Hair goal saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/HairGoal'; }, 3000);
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