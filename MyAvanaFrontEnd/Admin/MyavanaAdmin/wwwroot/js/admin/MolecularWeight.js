function SaveMolecularWeight() {
    if ($('#txtMolecularWeightDescription').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    var MolecularWeightEntity = {
        MolecularWeightId: $('#molecularWeightId').val(),
        Description: $('#txtMolecularWeightDescription').val(),
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/CreateMolecularWeight",
        data: MolecularWeightEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Molecular Weight saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Brand/MolecularWeight'; }, 3000);
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
function onRenderMolecularWeight(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateMolecularWeight/' + row.MolecularWeightId + '" Title="Edit Molecular Weight" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    // Check if UserTypeId is not equal to 3 i.e. dataentry 
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Molecular Weight" onclick="deleteConfirmMolecularWeight(this)" data-code="' + row.MolecularWeightId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}
function deleteConfirmMolecularWeight(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteMolecularWeight(value);
    });
}
function DeleteMolecularWeight(MolecularWeightId) {
    var MolecularWeightEntity = {
        MolecularWeightId: MolecularWeightId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteMolecularWeight",
        data: MolecularWeightEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Molecular Weight deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/MolecularWeight'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}