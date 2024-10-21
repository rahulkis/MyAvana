var hairType = [];
var hairChallenges = [];
var hairGoals = [];
var productTypes = [];
var productIndicators = [];
var productTags = [];
var productClassification = [];
var brandClassification = [];
var hairStates = [];
var recommendationStatus = [];
var molecularWeight = [];

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
function onCreatedOnRenderHairStates(data, type, row, meta) {
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
function onCreatedOnRenderRecommendationStatus(data, type, row, meta) {
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
function onCreatedOnRenderMolecularWeight(data, type, row, meta) {
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

function deleteConfirmBrand(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteBrand(value);
    });
}

function DeleteBrand(brandid) {
    var productsEntity = {
        brandId: brandid
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteBrand",
        data: productsEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Brand deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/Brands'; }, 2000);
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
function SaveBrand() {
    var onlyNumbers = /^[0-9\s]*$/;
    
    //  if ($('#txtBrandName').val().trim() == "" || $('#hairType').val().length == 0 ||
    //      $('#productTags').val().length == 0 || $('#brandClassification').val().length == 0) {
    //    $('#failureMessage').text("Fields can't be empty!");
    //    $('.alert-danger').css("display", "block");
    //    $('.alert-danger').delay(3000).fadeOut();
    //    return false;
    //}
    if ($('#txtBrandName').val().trim() == "" || $('#txtRank').val() == "") {
        $('#failureMessage').text("Fields can't be empty!");
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
    
    $('#productTags option:selected').each(function (i, sel) {
        var productTagsList = {};
        productTagsList.TagsId = $(this).val();
        productTagsList.Description = $(this).text();
        productTags.push(productTagsList);
    });

    $('#brandClassification option:selected').each(function (i, sel) {
        var brandClassificationList = {};
        brandClassificationList.BrandClassificationId = $(this).val();
        brandClassificationList.Description = $(this).text();
        brandClassification.push(brandClassificationList);
    });
    $('#hairStates option:selected').each(function (i, sel) {
        var hairStatesList = {};
        hairStatesList.HairStateId = $(this).val();
        hairStatesList.Description = $(this).text();
        hairStates.push(hairStatesList);
    });

    $('#recommendationStatus option:selected').each(function (i, sel) {
        var recommendationStatusList = {};
        recommendationStatusList.BrandRecommendationStatusId = $(this).val();
        recommendationStatusList.Description = $(this).text();
        recommendationStatus.push(recommendationStatusList);
    });

    $('#molecularWeight option:selected').each(function (i, sel) {
        var molecularWeightList = {};
        molecularWeightList.MolecularWeightId = $(this).val();
        molecularWeightList.Description = $(this).text();
        molecularWeight.push(molecularWeightList);
    });

    var rankValue = $('#txtRank').val();
    if (!(/^\d+$/.test(rankValue))) {
        $('#failureMessage').text("Rank field accepts only integer values!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    fileModel.append('BrandId', $('#brandid').val());
    fileModel.append('BrandName', $("#txtBrandName").val().trim());
    fileModel.append('HairChallenges', JSON.stringify(hairChallenges));
    fileModel.append('HairGoals', JSON.stringify(hairGoals));
    fileModel.append('HairTypes', JSON.stringify(hairType));
    fileModel.append('BrandTags', JSON.stringify(productTags));
    fileModel.append('BrandClassification', JSON.stringify(brandClassification));
    fileModel.append('HairStates', JSON.stringify(hairStates));
    fileModel.append('RecommendationStatuses', JSON.stringify(recommendationStatus));
    fileModel.append('MolecularWeights', JSON.stringify(molecularWeight));
    fileModel.append('FeaturedIngredients', $("#txtFeaturedIngredients").val().trim());
    fileModel.append('Rank', $("#txtRank").val().trim());

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/SaveBrand",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            var actualPrice = response.price;
            if (response == "1") {
                $('#successMessage').text("Brand saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Brand/Brands'; }, 3000);
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
    tr += '<a Title="Delete Product" onclick="deleteConfirmProductType(this)" data-code="' + row.Id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderProductTag(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductTag/' + row.ProductTagsId + '" Title="Edit Product Tag" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Product Tag" onclick="deleteConfirmProductTag(this)" data-code="' + row.ProductTagsId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderProductClassification(data, type, row, meta) {
    var tr = '<a href="/Products/CreateProductClassification/' + row.ProductClassificationId + '" Title="Edit Product Classification" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Product Classification" onclick="deleteConfirmProductClassification(this)" data-code="' + row.ProductClassificationId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderBrandClassification(data, type, row, meta) {
    var tr = '<a href="/Products/CreateBrandClassification/' + row.BrandClassificationId + '" Title="Edit Brand Classification" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Brand Classification" onclick="deleteConfirmBrandClassification(this)" data-code="' + row.BrandClassificationId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderHairGoal(data, type, row, meta) {
    var tr = '<a href="/Products/CreateHairGoal/' + row.HairGoalId + '" Title="Edit Hair Goal" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Hair Goal" onclick="deleteConfirmHairGoal(this)" data-code="' + row.HairGoalId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderHairChallenge(data, type, row, meta) {
    var tr = '<a href="/Products/CreateHairChallenge/' + row.HairChallengeId + '" Title="Edit Hair Challenge" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Hair Challenge" onclick="deleteConfirmHairChallenge(this)" data-code="' + row.HairChallengeId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderHairState(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateHairState/' + row.HairStateId + '" Title="Edit Brand Hair State" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Brand Hair State" onclick="deleteConfirmHairState(this)" data-code="' + row.HairStateId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderStatus(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateBrandRecommendationStatus/' + row.BrandRecommendationStatusId + '" Title="Edit brand recommendation Status" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete Brand Recommendation Status" onclick="deleteConfirmRecommendationStatus(this)" data-code="' + row.BrandRecommendationStatusId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function onRenderMolecularWeight(data, type, row, meta) {
    var tr = '<a href="/Brand/CreateMolecularWeight/' + row.MolecularWeightId + '" Title="Edit Molecular Weight" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
        tr += '<a Title="Delete Molecular Weight" onclick="deleteConfirmMolecularWeight(this)" data-code="' + row.MolecularWeightId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
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
function deleteConfirmHairState(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteHairState(value);
    });
}
function deleteConfirmRecommendationStatus(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteRecommendationStatus(value);
    });
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

function DeleteHairState(HairStateId) {
    var HairStateEntity = {
        HairStateId: HairStateId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteHairState",
        data: HairStateEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Brand Hair State deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/BrandHairState'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}

function DeleteRecommendationStatus(BrandRecommendationStatusId) {
    var RecommendationStatusEntity = {
        BrandRecommendationStatusId: BrandRecommendationStatusId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/DeleteBrandRecommendationStatus",
        data: RecommendationStatusEntity,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Recommendation Status deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/BrandRecommendationStatus'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
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
function UpdateShowHide(brandId,HideInSearch) {
    var brandEntity = {
        BrandId: brandId,
        HideInSearch:HideInSearch
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Brand/ShowHideBrand",
        data: brandEntity,
        success: function (response) {
            if (response === "1") {

                if (HideInSearch == "false") {
                    var chk = "#check" + brandId;
                    $(chk).removeClass("fa fa-toggle-off");
                    $(chk).addClass("fa fa-toggle-on");
                    $('#successMessage').text("Brand hidden successfully !");
                } else {
                    var chk = "#check" + brandId;
                    $(chk).removeClass("fa fa-toggle-on");
                    $(chk).addClass("fa fa-toggle-off");
                    $('#successMessage').text("Brand unhidden successfully !");
                }
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmHideInSearchModal').modal('hide');
                setTimeout(function () { window.location.href = '/Brand/Brands'; }, 2000);
               
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
function RenderHtmlHide(data, type, row, meta) {
    var tr;
    if (row.HideInSearch == false || row.HideInSearch==null )
        tr = '<a onclick="openHideModal(\'' + row.brandId + '\',\'' + row.HideInSearch + '\')" Title="Hide Brand" ><i id="check' + row.brandId + '" class="fa fa-toggle-off" style="padding-left:34px;"></i></a>';
    else
        tr = '<a onclick="openHideModal(\'' + row.brandId + '\',\'' + row.HideInSearch + '\')" Title="Show Brand" ><i id="check' + row.brandId +'" class="fa fa-toggle-on" style="padding-left:34px;"></i></a>';
    return tr;
}
function openHideModal(brandId, HideInSearch) {
    
    if (HideInSearch == "true") {
        $('#confirmHideModalText').text('Are you sure you want to show this brand?');
        $('#confirmHideMethod').text('Show');
    } else {
        $('#confirmHideModalText').text('Are you sure you want to hide this brand?');
        $('#confirmHideMethod').text('Hide');
    }
    $('#confirmHideInSearchModal').modal('show');
    $("#confirmHideMethod").prop("onclick", null).off("click");
    $("#confirmHideMethod").click(function () {
        $('#confirmHideInSearchModal').modal('hide');
        UpdateShowHide(brandId, HideInSearch);
    });

}