function format(data) {
    console.log(data)
    var html = '';
    var result1 = data.filter(v => v.serialNo >= 1 && v.serialNo <= 3).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result2 = data.filter(z => z.serialNo >= 4 && z.serialNo <= 11).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result3 = data.filter(z => z.serialNo >= 12 && z.serialNo <= 14).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result4 = data.filter(z => z.serialNo >= 15 && z.serialNo <= 16).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result5 = data.filter(z => z.serialNo >= 17 && z.serialNo <= 23).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result6 = data.filter(z => z.serialNo >= 24 && z.serialNo <= 25).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });
    var result7 = data.filter(z => z.serialNo >= 26 && z.serialNo <= 30).sort(function (a, b) {
        var a1 = a.serialNo, b1 = b.serialNo;
        if (a1 == b1) return 0;
        return a1 > b1 ? 1 : -1;
    });


    var sno = 1;
    html += '<br /><h3 class="text-center"><u>Getting to Know You and Your Hair</u></h3><br />';
    $.each(result1, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>'
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Hair and Scalp Characteristics</u></h3><br />';
    $.each(result2, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Hair Color</u></h3><br />';

    $.each(result3, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Chemical Hair Services</u></h3><br />';

    $.each(result4, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Hair Care Maintenance</u></h3><br />';

    $.each(result5, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Hair Styling</u></h3><br />';

    $.each(result6, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
        });
        sno += 1;
    });
    html += '<br /> <h3 class="text-center"><u>Hair Product Preference</u></h3><br />';

    $.each(result7, function (index, value) {
        html += '<strong class="d-block mb-2 stgQuestion">' + sno + '. ' + value.question + '</strong>';
        $.each(value.answerList, function (i, v) {
            if (value.questionId == 22) {
                html += '<div class="d-flex justify-content-between divAnswer"><a href="' + v.answer + '" target="_blank"><img src="' + v.answer + '" style="height:100px; width:100px" /></a></div>'
            } else {
                html += '<div class="d-flex justify-content-between divAnswer"><span class="instrustioncontent">' + v.answer + '</span></div>'
            }
        });
        sno += 1;
    });

    return (
        '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
        '<tr>' +
        '<td>' + html + '</td>' +
        '</tr>' +
        '</table>'
    );
}


function onRender(data, type, row, meta) {
    var tr = '';
    if (row.isDraft == true) {
        tr += '<i class="fa fa-file viewdraft mr-2" Title="Draft" aria-hidden="true"></i>'
    }
    tr += '<a href="javascript:void(0);" onclick="moveToHairProfile(\'' + row.UserEmail + '\', \'' + row.UserName + '\', \'' + row.QuestionnaireCompleted + '\',\'' + row.UserId + '\',\'' + row.QA + '\')" Title="Move to hair profile"><i class="fa fa-user viewuser" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Questionnaire" class="ml-2" onclick="deleteConfirmQuest(\'' + row.UserEmail + '\',\'' + row.QA + '\')"><i class="fa fa-trash" aria-hidden="true"></i></a>'
    return tr;
}

function moveToHairProfile(userEmail, userName, questionnaireCompleted,UserId,QA) {
    if (questionnaireCompleted.toLowerCase() === "yes") {
        $('#AutoGenerateHHCPModal').modal('show');
        $('#AutoGenerateHHCPModal').modal('show').on('shown.bs.modal', function () {
            $('#AutoGenerateHHCPModal .continue-btn').off('click').on('click', function () {
                $('.preloader').css('display', 'block');
                var hairProfile = {
                    UserEmail: userEmail,
                    QA: QA,
                    UserId: UserId
                }

                $.ajax({
                    url: '/Questionnaire/AutoGenerateHHCP',
                    type: 'POST',
                    data: hairProfile,
                    success: function (response) {
                        if (response) {
                            $('#AutoGenerateHHCPModal').modal('hide');
                             $('.preloader').css('display', 'none');
                            window.location.href = '/Questionnaire/AddHairProfile?id=' + userEmail + '&name=' + userName + '&QaNumber=' + QA;
                        } else {
                            $('#AutoGenerateHHCPModal').modal('hide');
                            $('.preloader').css('display', 'none');
                            $('#failureMessage').text("Oops something goes wrong !");
                            $('.alert-danger').css("display", "block");
                            $('.alert-danger').delay(3000).fadeOut();
                        }
                    },
                    error: function () {
                        $('#AutoGenerateHHCPModal').modal('hide');
                        $('.preloader').css('display', 'none');
                        $('#failureMessage').text("Oops something goes wrong !");
                        $('.alert-danger').css("display", "block");
                        $('.alert-danger').delay(3000).fadeOut();
                    }
                });
            });
            $('#AutoGenerateHHCPModal .cancel-btn').off('click').on('click', function () {

                $('#AutoGenerateHHCPModal').modal('hide');

                window.location.href = '/Questionnaire/AddHairProfile?id=' + userEmail + '&name=' + userName + '&QaNumber=' + QA;
            });

        });
    }
    else {
        
        window.location.href = '/Questionnaire/AddHairProfile?id=' + userEmail + '&name=' + userName;
    }
}

//search box jquery
$("#questionnaire").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#questionaireTable tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

function deleteConfirmQuest(emailId,QA) {
    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteQuest(emailId, QA);
    });
}

function onRenderHHCPExist(data, type, row, meta) {
    var tr = '';
    if (row.isHHCPExist == true) {
        tr = 'Yes';
    } else {
        tr = 'No';
    }


    return tr;
}

function DeleteQuest(emailId, QA) {
    debugger;
    var quest = {
        Email: emailId,
        QA: QA
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/DeleteQuest",
        data: quest,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Questionnaire deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Questionnaire/Questionnaire'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}