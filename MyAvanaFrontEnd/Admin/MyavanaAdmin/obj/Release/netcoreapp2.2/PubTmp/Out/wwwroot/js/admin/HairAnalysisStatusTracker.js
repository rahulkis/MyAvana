
function onRenderStatusChange(data, type, row, meta) {
    var tr;
    tr = '<a onclick="openChangeStatus(\'' + row.StatusTrackerId + '\', \'' + row.HairAnalysisStatus + '\')" Title="Change Hair Analysis Status" ><i class="fa fa-user"></i></a>';
    return tr;
}
function openChangeStatus(StatusTrackerId, HairAnalysisStatus) {

    $('#confirmChangeModal').modal('show');
    $('#ddlStatus option:contains("' + HairAnalysisStatus + '")').prop('selected', true);
    $("#confirmChangeMethod").prop("onclick", null).off("click");
    $("#confirmChangeMethod").click(function () {

        ChangeHairAnalysisStatus(StatusTrackerId);
    });
}
function ChangeHairAnalysisStatus(StatusTrackerId) {
    var checkValidation = true;
    if ($('#ddlStatus').val() == '0') {
        $("#StatusValidation").addClass("field-validation-error");
        $("#StatusError").text("Please select Hair Analysis Status.");
        checkValidation = false;
    }
    if (!checkValidation) {
        return false;
    }
    var StatusEntity = {
        StatusTrackerId: StatusTrackerId,
        HairAnalysisStatusId: $('#ddlStatus').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/HairAnalysisStatusTracker/ChangeHairAnalysisStatus",
        data: StatusEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Hair Analysis Status changed successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                $('#confirmChangeModal').modal('hide');
                setTimeout(function () { window.location.href = '/HairAnalysisStatusTracker/HairAnalysisStatusTracker'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function onRenderStatusChangeHistory(data, type, row, meta) {
    var tr;
    console.log(row)
    tr = '<a onclick="ShowStatusHistory(\'' + row.CustomerId + '\',)" Title="Hair Analysis Status History" ><i class="fa fa-history"></i></a>';
    return tr;
}
function ShowStatusHistory(Id) {
    $.ajax({
        type: "GET",
        url: "/HairAnalysisStatusTracker/GetCustomerHairAnalysisStatusHistory",
        data: { 'Id': Id },
        success: function (response) {
            $('#ShowStatusHistory').modal('show');
            $('#tBody').html('');
            for (var d in response.data) {
                var data = response.data[d];

                $('#tBody').append($('<tr>')
                    .append($('<td>', { text: data.oldStatusName }))
                    .append($('<td>', { text: data.statusName }))
                    .append($('<td>', { text: data.updatedByUser }))
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