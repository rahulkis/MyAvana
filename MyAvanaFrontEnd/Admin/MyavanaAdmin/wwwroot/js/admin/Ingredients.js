function onRender(data, type, row, meta) {
    var tr = '<a href="/Ingredients/CreateIngredient/' + row.ingedientsEntityId + '" Title="Edit Ingredient" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Ingredient" onclick="deleteConfirmIngredient(this)" data-code="' + row.ingedientsEntityId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderImage(data, type, row, meta) {
    if (row.Image != null) {
        var imageUrl = "http://admin.myavana.com/Ingredients/" + row.Image;
        return '<img src=' + imageUrl + ' style="width:100px;height:auto;" />';
    }
    else
        return "";
}

function deleteConfirmIngredient(event) {

    var ingedientsEntityId = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteIngredient(ingedientsEntityId);
    });
}



function DeleteIngredient(ingedientsEntityId) {
    var ingredientsEntity = {
        IngedientsEntityId: ingedientsEntityId
	}
	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Ingredients/DeleteIngredient",
        data: ingredientsEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Ingredient deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () {
                    window.location.href = '/Ingredients/Ingredients';
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

$('#btnImageClick').click(function () {
    $('#ingredientImage').click();
});

$('#ingredientImage').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        //$('#selectedImage').css('display', 'block');
        $('#selectedImage').text('');
        $('#selectedImage').text(filename);
    }
});

fileModel = new FormData();
function SaveIngredient() {

    if ($('#txtName').val().trim() == "" || $('#txtType').val().trim() == "" || $('#selectedImage').text() == "" || $('#txtDecription').val().trim() == "" || $('#txtChallenges').val().trim() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    var file = $('#ingredientImage').get(0).files[0];

    fileModel.append('IngedientsEntityId', $('#ingredientid').val());
    fileModel.append('Name', $('#txtName').val().trim());
    fileModel.append('Type', $("#txtType").val().trim());
    fileModel.append('Description', $("#txtDecription").val().trim());
    fileModel.append('Challenges', $("#txtChallenges").val().trim());

    if (file != null || file != undefined) {
        fileModel.append('File', file);
    } else {
        fileModel.append('Image', $("#image").val())
    }

	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Ingredients/CreateIngredient",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Ingredient saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Ingredients/Ingredients'; }, 3000);
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