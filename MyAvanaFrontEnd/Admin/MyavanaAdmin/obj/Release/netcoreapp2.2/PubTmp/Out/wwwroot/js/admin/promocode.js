function changeDayWeek() {
    if ($("#expireDayWeek option:selected").val() == 1) {
        $('#expireDate').css('display', 'block');
        $('#expireWeek').css('display', 'none');
    }
    else {
        $('#expireWeek').css('display', 'block');
        $('#expireDate').css('display', 'none');
    }
}


    
  var  possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

function generateCode(length) {
    var generated = [];
        var text = "";

        for (var i = 0; i < length; i++) {
            text += possible.charAt(Math.floor(Math.random() * possible.length));
        }

        if (generated.indexOf(text) == -1) {
            generated.push(text);
        } else {
            generateCode();
    }
    return generated;
}

$('#generateCode').on('click', function () {
    
    var code =  generateCode(7);
    
    $('#txtRandomNumber').val(code);
});

function SavePromoCode() {

    if ($('#txtRandomNumber').val() == "") {
        $('#failureMessage').text("Promocode field cannot be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }


	if ($('#subscriptionType option:selected').val() == "") {
		$('#failureMessage').text("Please select subscription to continue!");
		$('.alert-danger').css("display", "block");
		$('.alert-danger').delay(3000).fadeOut();
		return false;
	}
    var expireDate = null;

    var dateNow = new Date(); // yyyy-mm-dd
    var days = null;
        if ($("#expireDayWeek option:selected").val() == 1) {
            days = $('#expireDate option:selected').val();
            dateNow.setDate(dateNow.getDate() + parseInt(days));
        }
        else if ($("#expireDayWeek option:selected").val() == 2) {
            days = $('#expireWeek option:selected').val();
            var cdate = parseInt(days) * 7;
            dateNow.setDate(dateNow.getDate() + cdate);
        }

    expireDate = new Date(dateNow).toISOString();


    var promoCodeModel = {
        PromoCode: $('#txtRandomNumber').val(),
		InitialDate: expireDate.substring(0, expireDate.indexOf('T')),
		StripePlanId: $('#subscriptionType option:selected').val()
    }
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/PromoCode/PromoCode",
        data: promoCodeModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("PromoCode saved successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/PromoCode/ViewCodes'; }, 3000);
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
			}
			$('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

            $(".preloader").css("display", "none");
        }
    });
};
function RenderHtml(data, type, row, meta) {
    //var tr = '<a href="/Articles/CreateArticles/' + row.blogArticleId + '" Title="Edit Article" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var tr = '<a Title="Delete Code" onclick="deleteConfirmCode(this)" data-code="' + row.promoCode + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function deleteConfirmCode(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeletePromoCode(value);
    });
}

function DeletePromoCode(promo) {
    var promoCode = {
        PromoCode: promo
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/PromoCode/DeletePromoCode",
        data: promoCode,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("PromoCode deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/PromoCode/ViewCodes'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
