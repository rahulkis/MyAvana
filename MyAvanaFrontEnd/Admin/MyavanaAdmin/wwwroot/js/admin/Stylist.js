var specialty = [];
function SaveStylist() {
    if ($('#stylishSpecialty').val().length == 0 || $('#txtStylishName').val().trim() == '' || $('#txtSalonName').val().trim() == '' || $('#txtCity').val().trim() == '' || $('#txtState').val().trim() == '' ||
        $('#txtEmail').val().trim() == '' || $('#txtPhoneNumber').val().trim() == '' || $('#txtAddress').val().trim() == '' ||
        $('#txtBackground').val().trim() == '' || $('#txtNotes').val().trim() == '') {
        $('#failureMessage').text("Fields cannot be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#txtWebsite').val().trim() != '') {
        if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#txtWebsite').val().trim())) {
            $('#failureMessage').text("Wrong website Url");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }
    if (! /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/.test($('#txtEmail').val().trim())) {
        $('#failureMessage').text("Please enter valid email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if (! /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/.test($("#txtPhoneNumber").val().trim().trim())) {
        $('#failureMessage').text("Enter valid phone no.");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#txtInstagram').val().trim() != '') {
        if (! /^(https|http):\/\/?(?:www\.)?instagram\.com\/.(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]*)/.test($('#txtInstagram').val().trim())) {
            $('#failureMessage').text("wrong Instagram Url! Please copy and paste your social media link here.");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }
    if ($('#txtFacebook').val().trim() != '') {
        if (! /^(https|http):\/\/?(?:www\.)?facebook\.com\/.(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]*)/.test($('#txtFacebook').val().trim())) {
            $('#failureMessage').text("Wrong Facebook Url! Please copy and paste your social media link here.");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }

    $('.preloader').css('display', 'block');

    $('#stylishSpecialty option:selected').each(function (i, sel) {
        var stylishSpecialty = {};
        stylishSpecialty.StylistSpecialtyId = $(this).val();
        stylishSpecialty.Description = $(this).text();
        specialty.push(stylishSpecialty);
    });

    var stylist = {
        StylistId: $('#stylistId').val(),
        StylistName: $('#txtStylishName').val().trim(),
        SalonName: $('#txtSalonName').val().trim(),
        City: $('#txtCity').val().trim(),
        State: $('#txtState').val().trim(),
        ZipCode: $('#txtZipCode').val().trim(),
        Website: $('#txtWebsite').val().trim(),
        Email: $('#txtEmail').val().trim(),
        PhoneNumber: $('#txtPhoneNumber').val().trim(),
        Address: $('#txtAddress').val().trim(),
        Instagram: $('#txtInstagram').val().trim(),
        Facebook: $('#txtFacebook').val().trim(),
        Background: $('#txtBackground').val().trim(),
        Notes: $('#txtNotes').val().trim(),
        SalonId: $('#SalonId').val(),
        StylistSpecialty: JSON.stringify(specialty)
    }

    $.ajax({
        type: "POST",
        url: "/Stylist/AddUpdateStylist",
        data: stylist,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Stylist Add/Updated Successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Stylist/Stylist' }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function deleteConfirmStylist(event) {

    var stylistId = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteStylist(stylistId);
    });
}

function DeleteStylist(stylistId) {
    var stylistModel = {
        StylistId: stylistId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Stylist/DeleteStylist",
        data: stylistModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Stylist deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () {
                    window.location.href = '/Stylist/Stylist';
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

var fileModel = new FormData();

function UploadSelectedExcelsheet() {
	fileModel = new FormData();
	$('#excelUpload').click();
};

function changeImage() {
	
	var excelUpload = $('#excelUpload').get(0).files[0];
	var validExtensions = ['xlsx', 'xlsm', 'xlsb', 'xltx', 'xltm', 'xls', 'xlt']; //array of valid extensions
	var fileName = excelUpload.name;
	var fileNameExt = fileName.substr(fileName.lastIndexOf('.') + 1);
	if ($.inArray(fileNameExt, validExtensions) == -1) {
		$('#failureMessage').text("Please select an excel file !");
		$('.alert-danger').css("display", "block");
		$('.alert-danger').delay(3000).fadeOut();
		return false;
	}
	fileModel.append('file', excelUpload);
	$('.preloader').css('display', 'block');
	$.ajax({
		type: "POST",
		url: "/Stylist/UploadExcelsheet",
		contentType: false,
		processData: false,
		data: fileModel,
		success: function (result) {
			$('.preloader').css('display', 'none');
			if (result != "0" && result != "1") {
				$('#failureMessage').text(result);
				$('.alert-danger').css("display", "block");
				$('.alert-danger').delay(3000).fadeOut();
				$("#excelUpload")[0].value = '';
				return false;
			}
			else if (result == "1") {
				window.location.reload();
			}
			else {
				$('#failureMessage').text("Something goes wrong !!");
				$('.alert-danger').css("display", "block");
				$('.alert-danger').delay(3000).fadeOut();
				return false;
			}

		},
		error: {
		}
	});
}

$(document).ready(function () {
	$('#excelDownloadChange').on('click', function (e) {
		$("#stylistTableToDownload").table2excel({
			exclude: ".noExport",
			name: "stylistExcel",
			filename: "Workbook",
		});
	});
});

