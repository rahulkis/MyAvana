var hairType = [];
var hairChallenges = [];
var hairGoals = [];
var productTypes = [];
var productIndicators = [];
var productTags = [];
var customerPrefrences = [];
var hairStyles = [];
var productClassification = [];
var brandClassification = [];
var productRecommendationStatuses = [];
var molecularWeights = [];

function onRender(data, type, row, meta) {
    //var tr = '<div style="display:flex"><div><span>' + row.ProductLink + '</span></div><div style="margin-left: 15px;"><a href="/Products/CreateProduct/' + row.guid + '" Title="Edit Product" ><i class="fa fa-pencil" aria-hidden="true"></i></a> <a Title="Delete Product" onclick="deleteConfirmProduct(this)" data-code="' + row.guid + '"><i class="fa fa-trash" aria-hidden="true"></i></a></div></div>'
    //var tr = '<span>"' + row.ProductLink + '"</span>'
    var tr = '<a href="/Products/CreateProduct/' + row.guid + '" Title="Edit Product" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Product" onclick="deleteConfirmProduct(this)" data-code="' + row.guid + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function seachText() {
    $('#ProductList').DataTable().search('The body shop').draw();

    
}
function onRenderImage(data, type, row, meta) {
    return "<a href='" + data + "' target='_blank'><img src='" + data + "' style='width:100px;height:auto;' /></a>";
}

function onCreatedOnRenderHairType(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>"

    });
    td += '</td>';
    return td;
}
function onCreatedOnRenderHairChallenge(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>";

    });
    td += '</td>';
    return td;
}
function onCreatedOnRenderProductIndicator(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>";

    });
    td += '</td>';
    return td;
}
function onCreatedOnRenderProductTags(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>";

    });
    td += '</td>';
    return td;
}
function onCreatedOnRenderProductClassification(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>";

    });
    td += '</td>';
    return td;
}

function onCreatedOnRenderCustomerPreference(data, type, row, meta) {
    var td = '<td>';
    data.forEach(function (attachments, i) {
        var html = '';
        td += '<span>';
        if (attachments.value != null) {
            html += '(' + attachments.value + ')';
        }

        td += '' + attachments.description + ', ' + html;
        td += "</span>";

    });
    td += '</td>';
    return td;
}

function deleteConfirmProduct(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProduct(value);
    });
}

function DeleteProduct(guid) {
    var productsEntity = {
        guid: guid
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProduct",
        data: productsEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/Products'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function UpdateShowHide(guid, HideInSearch) {
    var productEntity = {
        guid: guid,
        HideInSearch: HideInSearch
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/ShowHideProduct",
        data: productEntity,
        success: function (response) {
            if (response === "1") {
                var chk = "#check_" + guid;
                if (HideInSearch == "false") {
                    
                    $(chk).removeClass("fa fa-toggle-off");
                    $(chk).addClass("fa fa-toggle-on");
                    $('#successMessage').text("Product hidden successfully !");
                } else {
                    
                    $(chk).removeClass("fa fa-toggle-on");
                    $(chk).addClass("fa fa-toggle-off");
                    $('#successMessage').text("Product unhidden successfully !");
                }
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmHideInSearchModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/Products'; }, 2000);

            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function openHideModal(guid, HideInSearch) {

    if (HideInSearch == "true") {
        $('#confirmHideModalText').text('Are you sure you want to show this product?');
        $('#confirmHideMethod').text('Show');
    } else {
        $('#confirmHideModalText').text('Are you sure you want to hide this product?');
        $('#confirmHideMethod').text('Hide');
    }
    $('#confirmHideInSearchModal').modal('show');
    $("#confirmHideMethod").prop("onclick", null).off("click");
    $("#confirmHideMethod").click(function () {
        $('#confirmHideInSearchModal').modal('hide');
        UpdateShowHide(guid, HideInSearch);
    });

}
$('#btnImageClick').click(function () {
    $('#productImage').click();
});
var imagedata = new FormData();
$('#productImage').change(function (event) {
    imagedata = new FormData();
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        //$('#selectedImage').css('display', 'block');
        $('#txtImageName').text('');
        $('#txtImageName').text(filename);
    }
    var files = event.target.files;
    $('#txtImageName').text('');
    for (var i = 0; i < files.length; i++) {
        $('#txtImageName').append("<span id=" + files[i].name + '_tl' + "  class='spnTopLeft' >" + files[i].name + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteFile('" + files[i].name + "')></i></span>");
        files[i].name = (files[i].name).replace(" ", "");
        imagedata.append("files", files[i]);
    }

});

function deleteFile(fname) {
    $('.spnTopLeft').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    
    var fileName = fname.toString();

    var files = imagedata.getAll("files");;
    files.splice($("[type='files']").index(fileName), 1);
    imagedata.delete("files");
    $.each(files, function (i, v) {
        imagedata.append("files", v);
    });
    console.log(imagedata)
}

function deleteFileAWS(fname, fId) {
    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete? It will not recover again');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductImage(fId, fname);
    });
}

var fileModel = new FormData();
function SaveProduct() {
    var onlyNumbers = /^[0-9\s]*$/;
    if ($('#productType').val() == "" || $('#txtProductName').val().trim() == "" || $('#txtActualName').val().trim() == "" ||
        $('#ddlBrandName').val().trim() == null || $('#hairType').val().length == 0 || $('#txtImageName').text().trim() == "" ||
        $('#txtIngredients').val().trim() == "" || $('#txtProductDetails').val().trim() == "" || $('#txtProductLink').val().trim() == "" ||
        $('#txtActualPrice').val().trim().trim() == "" || $('#txtDecimalPrice').val().trim() == "" || $('#productIndicators').val().length == 0 ||
        $('#productTags').val().length == 0 || $('#productClassification').val().length == 0 || $('#customerPreference').val().length == 0 ) {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    //if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#txtImageName').val())) {
    //    $('#failureMessage').text("Wrong image Url");
    //    $('.alert-danger').css("display", "block");
    //    $('.alert-danger').delay(3000).fadeOut();
    //    return false;
    //}
    if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#txtProductLink').val())) {
        $('#failureMessage').text("Wrong image Url");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if (!onlyNumbers.test($('#txtActualPrice').val().trim())) {
        $('#failureMessage').text("Enter valid amount");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if (!onlyNumbers.test($('#txtDecimalPrice').val().trim())) {
        $('#failureMessage').text("Enter valid amount");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    $('#hairType option:selected').each(function (i, sel) {
        var hairTypeList = {};
        hairTypeList.HairTypeId = $(this).val();
        hairTypeList.Description = $(this).text();
        hairType.push(hairTypeList);
    });

    $('#hairChallenges option:selected').each(function (i, sel) {
        var hairChallengesList = {};
        hairChallengesList.HairChallengeId = $(this).val();
        hairChallengesList.Description = $(this).text();
        hairChallenges.push(hairChallengesList);
    });
    $('#hairGoals option:selected').each(function (i, sel) {
        var hairGoalsList = {};
        hairGoalsList.HairGoalId = $(this).val();
        hairGoalsList.Description = $(this).text();
        hairGoals.push(hairGoalsList);
    });
    $('#productType option:selected').each(function (i, sel) {
        var productTypesList = {};
        productTypesList.Id = $(this).val();
        productTypesList.Description = $(this).text();
        productTypes.push(productTypesList);
    });
    $('#productIndicators option:selected').each(function (i, sel) {
        var productIndicatorsList = {};
        productIndicatorsList.ProductIndicatorId = $(this).val();
        productIndicatorsList.Description = $(this).text();
        productIndicators.push(productIndicatorsList);
    });

    $('#productTags option:selected').each(function (i, sel) {
        var productTagsList = {};
        productTagsList.ProductTagsId = $(this).val();
        productTagsList.Description = $(this).text();
        productTags.push(productTagsList);
    });

    $('#customerPreference option:selected').each(function (i, sel) {
        var customerPreferenceList = {};
        customerPreferenceList.CustomerPreferenceId = $(this).val();
        customerPreferenceList.Description = $(this).text();
        customerPrefrences.push(customerPreferenceList);
    });
    $('#hairStyles option:selected').each(function (i, sel) {
        var hairStylesList = {};
        hairStylesList.Id = $(this).val();
        hairStylesList.Style = $(this).text();
        hairStyles.push(hairStylesList);
    });
    $('#productClassification option:selected').each(function (i, sel) {
        var productClassificationList = {};
        productClassificationList.ProductClassificationId = $(this).val();
        productClassificationList.Description = $(this).text();
        productClassification.push(productClassificationList);
    });

    $('#brandClassification option:selected').each(function (i, sel) {
        var brandClassificationList = {};
        brandClassificationList.BrandClassificationId = $(this).val();
        brandClassificationList.Description = $(this).text();
        brandClassification.push(brandClassificationList);
    });

    $('#productRecommendationStatuses option:selected').each(function (i, sel) {
        var productRecommendationStatusesList = {};
        productRecommendationStatusesList.ProductRecommendationStatusId = $(this).val();
        productRecommendationStatusesList.Description = $(this).text();
        productRecommendationStatuses.push(productRecommendationStatusesList);
    });

    $('#molecularWeights option:selected').each(function (i, sel) {
        var molecularWeightsList = {};
        molecularWeightsList.MolecularWeightId = $(this).val();
        molecularWeightsList.Description = $(this).text();
        molecularWeights.push(molecularWeightsList);
    });

    var file = $('#productImage').get(0).files[0];
    fileModel.append('guid', $('#productid').val());
    //fileModel.append('ProductTypesId', $('#productType').val());
    fileModel.append('ProductName', $("#txtProductName").val().trim());
/*    fileModel.append('BrandName', $("#txtBrandName").val().trim());*/
    fileModel.append('ActualName', $("#txtActualName").val().trim());
    fileModel.append('TypeFor', JSON.stringify(hairType));
    //fileModel.append('ImageName', $("#txtImageName").val().trim());
    fileModel.append('Ingredients', $("#txtIngredients").val().trim());
    fileModel.append('ProductDetails', $("#txtProductDetails").val().trim());
    fileModel.append('ProductLink', $("#txtProductLink").val().trim());
    fileModel.append('Price', $('#txtActualPrice').val().trim() + "." + $('#txtDecimalPrice').val().trim());
    fileModel.append('HairChallenges', JSON.stringify(hairChallenges));
    fileModel.append('HairGoals', JSON.stringify(hairGoals));
    fileModel.append('ProductTypes', JSON.stringify(productTypes));
    fileModel.append('ProductIndicator', JSON.stringify(productIndicators));
    fileModel.append('ProductTags', JSON.stringify(productTags));
    fileModel.append('CustomerPreferences', JSON.stringify(customerPrefrences));
    fileModel.append('ProductClassification', JSON.stringify(productClassification));
    fileModel.append('BrandClassification', JSON.stringify(brandClassification));
    fileModel.append('HairStyles', JSON.stringify(hairStyles));
    fileModel.append('ProductRecommendationStatuses', JSON.stringify(productRecommendationStatuses));
    fileModel.append('MolecularWeights', JSON.stringify(molecularWeights));
    fileModel.append('UPCCode', $("#txtUPCCode").val().trim());
    fileModel.append('BrandId', $("#ddlBrandName").val().trim());
    if (file != null || file != undefined) {
        fileModel.append('File', file);
    } else {
        fileModel.append('ImageName', $("#image").val())
    }
    var files = $('#productImage').get(0).files;
    for (var i = 0; i < files.length; i++) {
        fileModel.append('files', files[i]);
    }
    
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateProduct",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            var actualPrice = response.price;
            if (response == "1") {
                $('#successMessage').text("Product saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/Products'; }, 3000);
            }
            else {
                $('#failureMessage').text(response);
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
            }
            $('.preloader').css('display', 'none');
        },
    });
}

function onRenderProductType(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductType/' + row.Id + '" Title="Edit Product" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Product" onclick="deleteConfirmProductType(this)" data-code="' + row.Id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
     return tr;
}

function onRenderProductTag(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductTag/' + row.ProductTagsId + '" Title="Edit Product Tag" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Product Tag" onclick="deleteConfirmProductTag(this)" data-code="' + row.ProductTagsId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
    return tr;
}

function onRenderProductClassification(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductClassification/' + row.ProductClassificationId + '" Title="Edit Product Classification" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Product Classification" onclick="deleteConfirmProductClassification(this)" data-code="' + row.ProductClassificationId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
        return tr;
}
function onRenderCustomerPreference(data, type, row, meta) {
    var tr = '<a href="/Products/CreateCustomerPreference/' + row.CustomerPreferenceId + '" Title="Edit Customer Preference" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Customer Preference" onclick="deleteConfirmCustomerPreference(this)" data-code="' + row.CustomerPreferenceId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}
function onRenderBrandClassification(data, type, row, meta) {
    var tr = '<a href="/Products/CreateBrandClassification/' + row.BrandClassificationId + '" Title="Edit Brand Classification" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Brand Classification" onclick="deleteConfirmBrandClassification(this)" data-code="' + row.BrandClassificationId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
    return tr;
}

function onRenderHairGoal(data, type, row, meta) {
    var tr = '<a href="/Products/CreateHairGoal/' + row.HairGoalId + '" Title="Edit Hair Goal" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Hair Goal" onclick="deleteConfirmHairGoal(this)" data-code="' + row.HairGoalId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    }
    return tr;
}

function onRenderHairChallenge(data, type, row, meta) {
    var tr = '<a href="/Products/CreateHairChallenge/' + row.HairChallengeId + '" Title="Edit Hair Challenge" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Hair Challenge" onclick="deleteConfirmHairChallenge(this)" data-code="' + row.HairChallengeId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
     return tr;
}

function onRenderProductRecommendationStatus(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductRecommendationStatus/' + row.ProductRecommendationStatusId + '" Title="Edit Product Recommendation Status" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    var userTypeId = getCookie("UserTypeId");
    if (userTypeId !== "3") {
        tr += '<a Title="Delete Hair Challenge" onclick="deleteConfirmProductRecommendationStatus(this)" data-code="' + row.ProductRecommendationStatusId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';

    }
    return tr;
}

function deleteConfirmProductType(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductType(value);
    });
}

function deleteConfirmProductTag(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductTag(value);
    });
}

function deleteConfirmHairGoal(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteHairGoal(value);
    });
}

function deleteConfirmHairChallenge(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteHairChallenge(value);
    });
}

function deleteConfirmProductClassification(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductClassification(value);
    });
}
function deleteConfirmCustomerPreference(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteCustomerPreference(value);
    });
}
function deleteConfirmBrandClassification(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteBrandClassification(value);
    });
}

function deleteConfirmProductRecommendationStatus(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteProductRecommendationStatus(value);
    });
}

function DeleteProductType(productTypeId) {
    var productTypeEntity = {
        Id: productTypeId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductType",
        data: productTypeEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductsType'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function DeleteProductTag(productTagId) {
    var productTypeEntity = {
        ProductTagsId: productTagId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductTag",
        data: productTypeEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product tag deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductsTag'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function DeleteHairGoal(hairGoalId) {
    var hairGoalEntity = {
        HairGoalId: hairGoalId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteHairGoal",
        data: hairGoalEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Hair Goal deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/HairGoal'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function DeleteHairChallenge(hairChallengeId) {
    var hairChallengeEntity = {
        HairChallengeId: hairChallengeId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteHairChallenge",
        data: hairChallengeEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Hair Challenge deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/HairChallenge'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function DeleteProductClassification(Id) {
    var productClassificationModel = {
        ProductClassificationId: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductClassification",
        data: productClassificationModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product classification deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductClassification'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function DeleteCustomerPreference(Id) {
    var customerPreferenceModel = {
        CustomerPreferenceId: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteCustomerPreference",
        data: customerPreferenceModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Customer Preference deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/CustomerPreference'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}


function DeleteBrandClassification(Id) {
    var brandClassificationModel = {
        BrandClassificationId: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteBrandClassification",
        data: brandClassificationModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Brand classification deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/BrandClassification'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function DeleteProductRecommendationStatus(productRecommendationStatusId) {
    var productRecommendationStatusEntity = {
        ProductRecommendationStatusId: productRecommendationStatusId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductRecommendationStatus",
        data: productRecommendationStatusEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product Recommendation Status deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Products/ProductRecommendationStatus'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function SaveProductType() {
    if ($('#txtProductName').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    var productTypeEntity = {
        Id: $('#productTypeId').val(),
        ProductName: $('#txtProductName').val()
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/CreateProductType",
        data: productTypeEntity,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Product saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Products/ProductsType'; }, 3000);
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

$(document).ready(function () {
    $('#excelDownloadChange').on('click', function (e) {
        $("#productsTableToDownload").table2excel({
            exclude: ".noExport",
            name: "producttExcel",
            filename: "product",
        });
    });
});

var fileModel = new FormData();
function UploadSelectedExcelsheet() {
    fileModel = new FormData();
    $('#excelUpload').click();
};

function changeImage() {

    var excelUpload = $('#excelUpload').get(0).files[0];
    var validExtensions = ['xlsx', 'xlsm', 'xlsb', 'xltx', 'xltm', 'xls', 'xlt', 'csv']; //array of valid extensions
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
        url: "/Products/UploadExcelsheet",
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

$('#search').on('keyup', function () {
    $('#ProductList').DataTable().column(3)
        .search($('#search').val())
        .draw();
    //FilterProducts();
});

$('#search').on('keyup', function () {
    FilterProducts();
});

function FilterProducts()
{
    productListing = [];
    if (($('#search').val()) !== '') {
        productListing.push("searchtext/" + $('#search').val());
    }

    //var searchText = $('#search').val();
    $('#ProductList').DataTable().search(productListing).draw();
}


function DeleteProductImage(fileId, fileName) {
    var productImage = {
        Id: fileId,
        ImageName: fileName
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Products/DeleteProductImage",
        data: productImage,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Product image deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                $('#' + fileName + '_div').parent().remove();
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
