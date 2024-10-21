var StepId = 1;
fileModel = new FormData();

function deleteConfirmRegimens(event) {

    var regimensId = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteIngredient(regimensId);
    });
}


function DeleteIngredient(regimensId) {
    var regimensModel = {
        RegimensId: regimensId
	}
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Regimens/DeleteRegimens",
        data: regimensModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Regimens deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () {
                    window.location.href = '/Regimens/Regimens';
                }, 2000);
			}
			$('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}


function AddImage(id) {
	if (id != null || id != undefined)
		StepId = id
	else
		StepId = parseInt($('#hdnStep').val());
	$('#inpStep' + StepId + 'Image').click();
};

function changeImage(event) {
	var stepImage = $('#inpStep' + StepId + 'Image').get(0).files[0];
	fileModel.append('Step' + StepId + 'Photo', stepImage);
	fileModel.append('Step' + StepId + 'PhotoName', stepImage.name);
	if (stepImage != null || stepImage != undefined) {
			$('#span' + StepId + 'Image').text('');
			$('#span' + StepId + 'Image').text(stepImage.name);
		}
}


function SaveRegimens() {

    if (($('#txtStep1Instruction').val().trim() == '') || ($('#span1Image').text() == '') || ($('#txtName').val().trim() == '') || ($('#txtTitle').val().trim() == '') || ($('#txtDescription').val().trim() == '')) {
		$('#failureMessage').text("Please fill all the required Fields to Save Regimen.");
		$('.alert-danger').css("display", "block");
		$('.alert-danger').delay(3000).fadeOut();
		return false;
	}
	var stepsCount = $('#hdnStep').val();
	fileModel.append('RegimensId', $('#regimensId').val());
    fileModel.append('Name', $('#txtName').val().trim());
    fileModel.append('Title', $('#txtTitle').val().trim());
    fileModel.append('Description', $('#txtDescription').val().trim());
	for (var i = 1; i <= stepsCount; i++) {
		fileModel.append('Step' + i + 'Instruction', $('#txtStep' + i + 'Instruction').val());
	}
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Regimens/CreateRegimens",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Regimens saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Regimens/Regimens'; }, 3000);
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

function AddNewStep() {
    var oldStep = parseInt($('#hdnStep').val());
    if (($('#txtStep' + oldStep + 'Instruction').val() == '') || ($('#span' + oldStep + 'Image').text() == '')) {
        $('#failureMessage').text("Please fill Instruction and Image for prior step.");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var step = oldStep + 1;

    $('.nav-link').removeClass('active');
    $('#v-pills-step' + step + '-tab').addClass('active');

    $('.tab-pane').removeClass('show');
    $('.tab-pane').removeClass('active');
    $('#v-pills-step' + step + '').addClass('show');
    $('#v-pills-step' + step + '').addClass('active');

    $('#hdnStep').val(step);
    //var img = "@ViewBag.step"+ step +"Image";
    $('#v-pills-tab').append('<a class="nav-link active" id="v-pills-step' + step + '-tab" data-toggle="pill" href="#v-pills-step' + step + '" role="tab" aria-controls="v-pills-step' + step + '" aria-selected="true">Step ' + step + '</a>');
    $('#v-pills-tabContent').append('<div class="tab-pane fade show active" id="v-pills-step' + step + '" role="tabpanel" aria-labelledby="v-pills-step' + step + '-tab"><div class="row">' +
        '<div class="col-sm-12 mb-4"><label class="w-100">Instruction*:</label><textarea rows="3" id="txtStep' + step + 'Instruction" placeholder="Enter Instruction" asp-for="Step' + step + 'Instruction" class="form-control"></textarea>' +
        '</div><div class="col-sm-12"><label class="w-100">Image*:</label><button class="btn btn-outline-secondary" onclick="AddImage(' + step + ')">Choose image</button>' +
        '<span id="span' + step + 'Image"></span><input type="file" id="inpStep' + step + 'Image" accept="image/*" onchange="changeImage(this)"  style="display:none;" /></div></div></div>');

}

