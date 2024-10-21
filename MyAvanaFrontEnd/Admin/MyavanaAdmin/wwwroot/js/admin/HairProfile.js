var strandPhotos = [];
var health = [];
var observation = [];
var observationElasticity = [];
var observationChemicalProduct = [];
var observationPhysicalProduct = [];
var observationDamage = [];
var observationBreakage = [];
var observationSplitting = [];
var porosity = [];
var elasticity = [];
var products = [];
var productCategories = [];
var productTypes = [];
var productsStyling = [];
var ingredients = [];
var tools = [];
var videos = [];
var regimens = [];
var stylist = [];
var brands = [];
var topLeftFiles = [];
var topRightFiles = [];
var bottomLeftFiles = [];
var bottomRightFiles = [];
var CrownFiles = [];
// strands photos
$('#btnStrandsPhotos').click(function () {
    $('#StrandsPhotos').click();
});
$('#StrandsPhotos').change(function () {
    for (var i = 0; i < $(this).get(0).files.length; ++i) {
        names.push($(this).get(0).files[i].name);
    }
});

//top left
$('#btnTopLeftPhoto').click(function () {
    $('#TopLeftPhoto').click();
});

var tldata = new FormData();
$("#TopLeftPhoto").change(function () {
    tldata = new FormData();
    var files = $(this).get(0).files;

    for (var i = 0; i < files.length; i++) {
        files[i].name = (files[i].name).replace(" ", "");
        tldata.append("files", files[i]);
    }
    SaveTopLeftFile(tldata);
});
function SaveTopLeftFile(tldata) {
    $('#showLoader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/AddTopLeftFiles",
        data: tldata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result !== null) {
                $('#showLoader').css('display', 'none');
                for (var i = 0; i < result.length; i++) {
                    $('#TopLeftPhotoSelected').append("<span id=" + result[i].fileName + '_tl' + "  class='spnTopLeft' >" + result[i].fileName + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteTopLeft('" + result[i].fileName + "')></i></span>");
                    $('#TopLeftPhotoSelectedUnique').append("<span id=" + result[i].fileName + '_tl' + "  class='spnTopLeftUnique' >" + result[i].uniqueFileName + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
                }
            }
        }
    });
}

function deleteTopLeft(fname) {
    var uniqFileName = "";
    $('.spnTopLeft').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    $('.spnTopLeftUnique').each(function (i, obj) {
        $('span[id^="' + fname + '_tl' + '"]').css('display', 'none');
        uniqFileName = $(this).text();
    });
    var fileName = fname.toString();

    var files = tldata.getAll("files");
    files.splice($("[type='files']").index(fileName), 1);
    tldata.delete("files");
    $.each(files, function (i, v) {
        tldata.append("files", v);
    });
    $.ajax({
        type: "POST",
        url: "/Questionnaire/RemoveTopLeftFiles",
        data: { 'fileName': uniqFileName },
        success: function (result) {
            if (result === "1") {
            }
        }
    });
}

//top right
$('#btnTopRightPhoto').click(function () {
    $('#TopRightPhoto').click();
});

var trdata = new FormData();
$("#TopRightPhoto").change(function () {
    trdata = new FormData();
    var files = $(this).get(0).files;
    for (var i = 0; i < files.length; i++) {
        files[i].name = (files[i].name).replace(" ", "");
        trdata.append("files", files[i]);
    }
    SaveTopRightFile(trdata);
});

function SaveTopRightFile(trdata) {
    $('#showRightLoader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/AddTopRightFiles",
        data: trdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result !== null) {
                $('#showRightLoader').css('display', 'none');
                for (var i = 0; i < result.length; i++) {
                    $('#TopRightPhotoSelected').append("<span id=" + result[i].fileName + '_tr' + "  class='spnTopRight' >" + result[i].fileName + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteTopRight('" + result[i].fileName + "')></i></span>");
                    $('#TopRightPhotoSelectedUnique').append("<span id=" + result[i].fileName + '_tr' + "  class='spnTopRightUnique' >" + result[i].uniqueFileName + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
                }
            }
        }
    });
}

function deleteTopRight(fname) {
    var uniqFileName = "";
    $('.spnTopRight').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    $('.spnTopRightUnique').each(function (i, obj) {
        $('span[id^="' + fname + '_tr' + '"]').css('display', 'none');
        uniqFileName = $(this).text();
    });
    var fileName = fname.toString();

    var files = trdata.getAll("files");
    files.splice($("[type='files']").index(fileName), 1);
    trdata.delete("files");
    $.each(files, function (i, v) {
        trdata.append("files", v);
    });

    $.ajax({
        type: "POST",
        url: "/Questionnaire/RemoveTopRightFiles",
        data: { 'fileName': uniqFileName },
        success: function (result) {
            if (result === "1") {
            }
        }
    });
}

//bottom left
$('#btnBottomLeftPhoto').click(function () {
    $('#BottomLeftPhoto').click();
});

var bldata = new FormData();
$("#BottomLeftPhoto").change(function () {
    bldata = new FormData();
    var files = $(this).get(0).files;

    for (var i = 0; i < files.length; i++) {
        files[i].name = (files[i].name).replace(" ", "");
        bldata.append("files", files[i]);
    }
    SaveBottomLeftFile(bldata);
});
function SaveBottomLeftFile(bldata) {
    $('#showLeftLoader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/AddBottomLeftFiles",
        data: bldata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result !== null) {
                $('#showLeftLoader').css('display', 'none');
                for (var i = 0; i < result.length; i++) {
                    $('#BottomLeftPhotoSelected').append("<span id=" + result[i].fileName + '_bl' + "  class='spnBottomLeft' >" + result[i].fileName + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteBottomLeft('" + result[i].fileName + "')></i></span>");
                    $('#BottomLeftPhotoSelectedUnique').append("<span id=" + result[i].fileName + '_bl' + "  class='spnBottomLeftUnique' >" + result[i].uniqueFileName + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
                }
            }
        }
    });
}

function deleteBottomLeft(fname) {
    var uniqFileName = "";
    $('.spnBottomLeft').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    $('.spnBottomLeftUnique').each(function (i, obj) {
        $('span[id^="' + fname + '_bl' + '"]').css('display', 'none');
        uniqFileName = $(this).text();
    });
    var fileName = fname.toString();

    var files = bldata.getAll("files");
    files.splice($("[type='files']").index(fileName), 1);
    bldata.delete("files");
    $.each(files, function (i, v) {
        bldata.append("files", v);
    });

    $.ajax({
        type: "POST",
        url: "/Questionnaire/RemoveBottomLeftFiles",
        data: { 'fileName': uniqFileName },
        success: function (result) {

        }
    });
}

//bottom right
$('#btnBottomRightPhoto').click(function () {
    $('#BottomRightPhoto').click();
});

var brdata = new FormData();
$("#BottomRightPhoto").change(function () {
    brdata = new FormData();
    var files = $(this).get(0).files;

    for (var i = 0; i < files.length; i++) {
        files[i].name = (files[i].name).replace(" ", "");
        brdata.append("files", files[i]);
    }
    SaveBottomRightFile(brdata);
});
function SaveBottomRightFile(brdata) {
    $('#showBottomRightLoader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/AddBottomRightFiles",
        data: brdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result !== null) {
                $('#showBottomRightLoader').css('display', 'none');
                for (var i = 0; i < result.length; i++) {
                    $('#BottomRightPhotoSelected').append("<span id=" + result[i].fileName + '_br' + "  class='spnBottomRight' >" + result[i].fileName + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteBottomRight('" + result[i].fileName + "')></i></span>");
                    $('#BottomRightPhotoSelectedUnique').append("<span id=" + result[i].fileName + '_br' + "  class='spnBottomRightUnique' >" + result[i].uniqueFileName + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
                }
            }
        }
    });
}

function deleteBottomRight(fname) {
    var uniqFileName = "";
    $('.spnBottomRight').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    $('.spnBottomRightUnique').each(function (i, obj) {
        $('span[id^="' + fname + '_br' + '"]').css('display', 'none');
        uniqFileName = $(this).text();
    });
    var fileName = fname.toString();

    var files = brdata.getAll("files");
    files.splice($("[type='files']").index(fileName), 1);
    brdata.delete("files");
    $.each(files, function (i, v) {
        brdata.append("files", v);
    });

    $.ajax({
        type: "POST",
        url: "/Questionnaire/RemoveBottomRightFiles",
        data: { 'fileName': uniqFileName },
        success: function (result) {
        }
    });
}

//crown photo
$('#btnCrownPhoto').click(function () {
    $('#CrowntPhoto').click();
});

var crdata = new FormData();
$("#CrowntPhoto").change(function () {
    crdata = new FormData();
    var files = $(this).get(0).files;

    for (var i = 0; i < files.length; i++) {
        files[i].name = (files[i].name).replace(" ", "");
        crdata.append("files", files[i]);
    }
    SaveCrownFile(crdata);
});
function SaveCrownFile(crdata) {
    $('#showCrownLoader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Questionnaire/AddCrownFiles",
        data: crdata,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result !== null) {
                $('#showCrownLoader').css('display', 'none');
                for (var i = 0; i < result.length; i++) {
                    $('#CrownPhotoSelected').append("<span id=" + result[i].fileName + '_cr' + "  class='spnCrown' >" + result[i].fileName + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteCrown('" + result[i].fileName + "')></i></span>");
                    $('#CrownPhotoSelectedUnique').append("<span id=" + result[i].fileName + '_cr' + "  class='spnCrownUnique' >" + result[i].uniqueFileName + "<i class='fa fa-times' style='cursor: pointer'</i></span>");
                }
            }
        }
    });
}

function deleteCrown(fname) {
    var uniqFileName = "";
    $('.spnCrown').each(function (i, obj) {
        if ($(this).text() === fname)
            $(this).css('display', 'none');
    });

    $('.spnCrownUnique').each(function (i, obj) {
        $('span[id^="' + fname + '_cr' + '"]').css('display', 'none');
        uniqFileName = $(this).text();
    });
    var fileName = fname.toString();

    var files = crdata.getAll("files");
    files.splice($("[type='files']").index(fileName), 1);
    crdata.delete("files");
    $.each(files, function (i, v) {
        crdata.append("files", v);
    });

    $.ajax({
        type: "POST",
        url: "/Questionnaire/RemoveCrownFiles",
        data: { 'fileName': uniqFileName },
        success: function (result) {
        }
    });
}

function SaveProfile(saveType) {
    fileModel = new FormData();
    //------------------------------------------

    var strandPhotos = [];
    var health = [];
    var observation = [];
    var observationElasticity = [];
    var observationChemicalProduct = [];
    var observationPhysicalProduct = [];
    var observationDamage = [];
    var observationBreakage = [];
    var observationSplitting = [];
    var porosity = [];
    var elasticity = [];
    var products = [];
    var productCategories = [];
    var productTypes = [];
    var productsStyling = [];
    var allProductsEssential = [];
    var allProductsStyling = [];
    var ingredients = [];
    var tools = [];
    var videos = [];
    var regimens = [];
    var stylist = [];
    var brands = [];
    var topLeftFiles = [];
    var topRightFiles = [];
    var bottomLeftFiles = [];
    var bottomRightFiles = [];
    var CrownFiles = [];
    var CurrentTabNo = [];
    var styleRecipeVideos = [];
    var productsStyleRecipe = [];

    CurrentTabNo = $('#txttab').val();
    //alert(CurrentTabNo);
    //---------------------------------------------------
    if ($('#txtHairId').val() == '' && $('#txtConsultantNotes').val() == '' && $('#txtHealthSummary').val() == '' && $('#txtStrandDiameter1').val() == '' && $('#txtStrandDiameter2').val() == '' &&
        $('#txtStrandDiameter3').val() == '' && $('#txtStrandDiameter4').val() == '' && $('#txtStrandDiameter5').val() == '' && $('#product').val().length == 0 && $('#productStyling').val().length == 0 && $('#ingredients').val().length == 0 && $('#tools').val().length == 0 && $('#videos').val().length == 0 && $('#videosStyling').val().length == 0 && $('#StylingRecipeProducts').val().length == 0 && $('#regimens').val().length == 0 && $('#stylist').val().length == 0 &&
        $('.health1:checked').val() == undefined && $('.observations1:checked').val() == undefined && $('.observationElas1:checked').val() == undefined && $('.observationChem1:checked').val() == undefined && $('.observationPhy1:checked').val() == undefined && $('.observationBreak1:checked').val() == undefined && $('.observationSplit1:checked').val() == undefined && $('.Porosity1:checked').val() == undefined && $('.Elasticity1:checked').val() == undefined &&
        $('.health2:checked').val() == undefined && $('.observations2:checked').val() == undefined && $('.Porosity2:checked').val() == undefined && $('.Elasticity2:checked').val() == undefined && $('.observationElas2:checked').val() == undefined && $('.observationChem2:checked').val() == undefined && $('.observationPhy2:checked').val() == undefined && $('.observationDa21:checked').val() == undefined && $('.observationBreak2:checked').val() == undefined && $('.observationSplit2:checked').val() == undefined &&
        $('.health3:checked').val() == undefined && $('.observations3:checked').val() == undefined && $('.Porosity3:checked').val() == undefined && $('.Elasticity3:checked').val() == undefined && $('.observationElas3:checked').val() == undefined && $('.observationChem3:checked').val() == undefined && $('.observationPhy3:checked').val() == undefined && $('.observationDam3:checked').val() == undefined && $('.observationBreak3:checked').val() == undefined && $('.observationSplit3:checked').val() == undefined &&
        $('.health4:checked').val() == undefined && $('.observationElas4checked').val() == undefined && $('.observationChem4:checked').val() == undefined && $('.observationPhy4:checked').val() == undefined && $('.observationDam4:checked').val() == undefined && $('.observationBreak4:checked').val() == undefined && $('.observationSplit4:checked').val() == undefined &&
        $('.observations4:checked').val() == undefined && $('.Porosity4:checked').val() == undefined && $('.Elasticity4:checked').val() == undefined &&
        $('.halth5:checked').val() == undefined && $('.observations5:checked').val() == undefined && $('.Porosity5:checked').val() == undefined && $('.Elasticity5:checked').val() == undefined && $('.observationElas5:checked').val() == undefined && $('.observationChem5:checked').val() == undefined && $('.observationPhy5:checked').val() == undefined && $('.observationDam5:checked').val() == undefined && $('.observationBreak5:checked').val() == undefined && $('.observationSplit5:checked').val() == undefined &&
        $('#TopLeftPhotoSelected').text() == "" && $('#TopRightPhotoSelected').text() == "" && $('#BottomLeftPhotoSelected').text() == "" &&
        $('#BottomRightPhotoSelected').text() == "" && $('#CrownPhotoSelected').text() == "") {

        $('#failureMessage').text("Atleast one field needs to have a value to save Profile");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    $('.preloader').css('display', 'block');

    //top left
    var topLeftImages = "";
    $('.spnTopLeftUnique').each(function (i, obj) {
        if ($(this).css('display') != 'none')
            topLeftImages = topLeftImages + $(this).text() + ",";
    });

    $('.health1:checked').each(function () {
        var healthList = {};
        healthList.Id = $(this).val();
        healthList.Description = $(this).text().trim();
        healthList.IsTopLeft = true;
        health.push(healthList);
    });

    $('.observations1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observation.push(obsList);
    });
    $('.observationElas1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observationElasticity.push(obsList);
    });
    $('.observationChem1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observationChemicalProduct.push(obsList);
    });
    $('.observationPhy1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observationPhysicalProduct.push(obsList);
    });
    //$('.observationDam1:checked').each(function () {
    //    var obsList = {};
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    obsList.IsTopLeft = true;
    //    observationDamage.push(obsList);
    //});
    $('.observationBreak1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observationBreakage.push(obsList);
    });
    $('.observationSplit1:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopLeft = true;
        observationSplitting.push(obsList);
    });

    $('.Porosity1:checked').each(function () {
        var porsList = {};
        porsList.Id = $(this).val();
        porsList.Description = $(this).text().trim();
        porsList.IsTopLeft = true;
        porosity.push(porsList);
    });
    $('.Elasticity1:checked').each(function () {
        var elasList = {};
        elasList.Id = $(this).val();
        elasList.Description = $(this).text().trim();
        elasList.IsTopLeft = true;
        elasticity.push(elasList);
    });

    //top right

    var topRightImages = "";
    $('.spnTopRightUnique').each(function (i, obj) {
        if ($(this).css('display') != 'none')
            topRightImages = topRightImages + $(this).text() + ",";
    });

    $('.health2:checked').each(function () {
        var healthList = {};
        healthList.Id = $(this).val();
        healthList.Description = $(this).text().trim();
        healthList.IsTopRight = true;
        health.push(healthList);
    });

    $('.observations2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observation.push(obsList);
    });
    $('.observationElas2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observationElasticity.push(obsList);
    });
    $('.observationChem2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observationChemicalProduct.push(obsList);
    });
    $('.observationPhy2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observationPhysicalProduct.push(obsList);
    });

    $('.observationBreak2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observationBreakage.push(obsList);
    });
    $('.observationSplit2:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsTopRight = true;
        observationSplitting.push(obsList);
    });

    $('.Porosity2:checked').each(function () {
        var porsList = {};
        porsList.Id = $(this).val();
        porsList.Description = $(this).text().trim();
        porsList.IsTopRight = true;
        porosity.push(porsList);
    });
    $('.Elasticity2:checked').each(function () {
        var elasList = {};
        elasList.Id = $(this).val();
        elasList.Description = $(this).text().trim();
        elasList.IsTopRight = true;
        elasticity.push(elasList);
    });

    // bottom left

    var bottomLeftImages = "";
    $('.spnBottomLeftUnique').each(function (i, obj) {
        if ($(this).css('display') != 'none')
            bottomLeftImages = bottomLeftImages + $(this).text() + ",";
    });

    $('.health3:checked').each(function () {
        var healthList = {};
        healthList.Id = $(this).val();
        healthList.Description = $(this).text().trim();
        healthList.IsBottomLeft = true;
        health.push(healthList);
    });

    //$('.observations3:checked').each(function () {
    //    var obsList = {};
    //    //obsList.UserId = $('#userId').val();
    //    obsList.IsBottomLeft = true;
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    observation.push(obsList);
    //});


    $('.observations3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observation.push(obsList);
    });
    $('.observationElas3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observationElasticity.push(obsList);
    });
    $('.observationChem3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observationChemicalProduct.push(obsList);
    });
    $('.observationPhy3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observationPhysicalProduct.push(obsList);
    });
    //$('.observationDam3:checked').each(function () {
    //    var obsList = {};
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    obsList.IsBottomLeft = true;
    //    observationDamage.push(obsList);
    //});
    $('.observationBreak3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observationBreakage.push(obsList);
    });
    $('.observationSplit3:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomLeft = true;
        observationSplitting.push(obsList);
    });


    $('.Porosity3:checked').each(function () {
        var porsList = {};
        //porsList.UserId = $('#userId').val();
        porsList.IsBottomLeft = true;
        porsList.Id = $(this).val();
        porsList.Description = $(this).text().trim();
        porosity.push(porsList);
    });
    $('.Elasticity3:checked').each(function () {
        var elasList = {};
        //elasList.UserId = $('#userId').val();
        elasList.IsBottomLeft = true;
        elasList.Id = $(this).val();
        elasList.Description = $(this).text().trim();
        elasticity.push(elasList);
    });

    //bottom right

    var bottomRightImages = "";
    $('.spnBottomRightUnique').each(function (i, obj) {
        if ($(this).css('display') != 'none')
            bottomRightImages = bottomRightImages + $(this).text() + ",";
    });

    $('.health4:checked').each(function () {
        var healthList = {};
        healthList.Id = $(this).val();
        healthList.Description = $(this).text().trim();
        healthList.IsBottomRight = true;
        health.push(healthList);
    });

    //$('.observations4:checked').each(function () {
    //    var obsList = {};
    //    obsList.IsBottomRight = true;
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    observation.push(obsList);
    //});


    $('.observations4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observation.push(obsList);
    });
    $('.observationElas4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observationElasticity.push(obsList);
    });
    $('.observationChem4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observationChemicalProduct.push(obsList);
    });
    $('.observationPhy4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observationPhysicalProduct.push(obsList);
    });
    //$('.observationDam4:checked').each(function () {
    //    var obsList = {};
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    obsList.IsBottomRight = true;
    //    observationDamage.push(obsList);
    //});
    $('.observationBreak4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observationBreakage.push(obsList);
    });
    $('.observationSplit4:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsBottomRight = true;
        observationSplitting.push(obsList);
    });

    $('.Porosity4:checked').each(function () {
        var porsList = {};
        porsList.IsBottomRight = true;
        porsList.Id = $(this).val();
        porsList.Description = $(this).text().trim();
        porosity.push(porsList);
    });
    $('.Elasticity4:checked').each(function () {
        var elasList = {};
        elasList.IsBottomRight = true;
        elasList.Id = $(this).val();
        elasList.Description = $(this).text().trim();
        elasticity.push(elasList);
    });

    //crown
    var crownImages = "";
    $('.spnCrownUnique').each(function (i, obj) {
        if ($(this).css('display') != 'none')
            crownImages = crownImages + $(this).text() + ",";
    });

    $('.health5:checked').each(function () {
        var healthList = {};
        healthList.Id = $(this).val();
        healthList.Description = $(this).text().trim();
        healthList.IsCrown = true;
        health.push(healthList);
    });

    //$('.observations5:checked').each(function () {
    //    var obsList = {};
    //    //obsList.UserId = $('#userId').val();
    //    obsList.IsCrown = true;
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    observation.push(obsList);
    //});


    $('.observations5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observation.push(obsList);
    });
    $('.observationElas5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observationElasticity.push(obsList);
    });
    $('.observationChem5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observationChemicalProduct.push(obsList);
    });
    $('.observationPhy5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observationPhysicalProduct.push(obsList);
    });
    //$('.observationDam5:checked').each(function () {
    //    var obsList = {};
    //    obsList.Id = $(this).val();
    //    obsList.Description = $(this).text().trim();
    //    obsList.IsCrown = true;
    //    observationDamage.push(obsList);
    //});

    $('.observationBreak5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observationBreakage.push(obsList);
    });
    $('.observationSplit5:checked').each(function () {
        var obsList = {};
        obsList.Id = $(this).val();
        obsList.Description = $(this).text().trim();
        obsList.IsCrown = true;
        observationSplitting.push(obsList);
    });

    $('.Porosity5:checked').each(function () {
        var porsList = {};
        //porsList.UserId = $('#userId').val();
        porsList.IsCrown = true;
        porsList.Id = $(this).val();
        porsList.Description = $(this).text().trim();
        porosity.push(porsList);
    });
    $('.Elasticity5:checked').each(function () {
        var elasList = {};
        //elasList.UserId = $('#userId').val();
        elasList.IsCrown = true;
        elasList.Id = $(this).val();
        elasList.Description = $(this).text().trim();
        elasticity.push(elasList);
    });

    // multiselect
    $('#product option:selected').each(function (i, sel) {
        var prodList = {};
        prodList.ProductId = $(this).val();
        prodList.Name = $(this).text();
        products.push(prodList);
    });
    $('#allProductEssential option:selected').each(function (i, sel) {
        var prodList = {};
        prodList.ProductId = $(this).val();
        prodList.Name = $(this).text();
        allProductsEssential.push(prodList);
    });
    $('#productTypeSection option:selected').each(function (i, sel) {
        var categoryList = {};
        categoryList.ProductTypeId = $(this).val();
        categoryList.Name = $(this).text();
        categoryList.IsHealthyRegimen = true;
        categoryList.IsStylingRegimen = false;
        productCategories.push(categoryList);
    });
    $('#productType option:selected').each(function (i, sel) {
        var prodTypeList = {};
        prodTypeList.ProductTypeId = $(this).val();
        prodTypeList.Name = $(this).text();
        prodTypeList.IsHealthyRegimen = true;
        prodTypeList.IsStylingRegimen = false;
        productTypes.push(prodTypeList);
    });
    $('#productTypeSectionStyling option:selected').each(function (i, sel) {
        var categoryList = {};
        categoryList.ProductTypeId = $(this).val();
        categoryList.Name = $(this).text();
        categoryList.IsHealthyRegimen = false;
        categoryList.IsStylingRegimen = true;
        productCategories.push(categoryList);
    });
    $('#productTypeStyling option:selected').each(function (i, sel) {
        var prodTypeList = {};
        prodTypeList.ProductTypeId = $(this).val();
        prodTypeList.Name = $(this).text();
        prodTypeList.IsHealthyRegimen = false;
        prodTypeList.IsStylingRegimen = true;
        productTypes.push(prodTypeList);
    });

    $('#productStyling option:selected').each(function (i, sel) {
        var prodList = {};
        prodList.ProductId = $(this).val();
        prodList.Name = $(this).text();
        productsStyling.push(prodList);
    });
    $('#allProductStyling option:selected').each(function (i, sel) {
        var prodList = {};
        prodList.ProductId = $(this).val();
        prodList.Name = $(this).text();
        allProductsStyling.push(prodList);
    });
    $('#ingredients option:selected').each(function (i, sel) {
        var ingredientsList = {};
        ingredientsList.IngredientId = $(this).val();
        ingredientsList.Name = $(this).text();
        ingredients.push(ingredientsList);

    });
    /* debugger;*/
    $('#tools option:selected').each(function (i, sel) {

        var ToolList = {};
        ToolList.Id = $(this).val();
        ToolList.Name = $(this).text();
        tools.push(ToolList);
    });
    $('#videos option:selected').each(function (i, sel) {
        var videosList = {};
        videosList.MediaLinkEntityId = $(this).val();
        videosList.Name = $(this).text();
        videos.push(videosList);
    });

    $('#regimens option:selected').each(function (i, sel) {
        var regimensList = {};
        regimensList.RegimenId = $(this).val();
        regimensList.Name = $(this).text();
        regimens.push(regimensList);
    });

    $('#stylist option:selected').each(function (i, sel) {
        var stylistList = {};
        stylistList.StylistId = $(this).val();
        stylistList.Name = $(this).text();
        stylist.push(stylistList);
    });
    $('#videosStyling option:selected').each(function (i, sel) {
        var videosStylingList = {};
        videosStylingList.MediaLinkEntityId = $(this).val();
        videosStylingList.Name = $(this).text();
        styleRecipeVideos.push(videosStylingList);
    });
    $('#StylingRecipeProducts option:selected').each(function (i, sel) {
        var prodList = {};
        prodList.ProductId = $(this).val();
        prodList.Name = $(this).text();
        productsStyleRecipe.push(prodList);
    });
    var selectedAnswer = {};
    selectedAnswer.TypeId = $('#typeId').val();
    selectedAnswer.TypeDescription = $('#typeDesc').val();
    selectedAnswer.TextureId = $('#textureId').val();
    selectedAnswer.TextureDescription = $('#textureDesc').val();
    selectedAnswer.HealthId = $('#healthId').val();
    selectedAnswer.HealthDescription = $('#healthDesc').val();
    selectedAnswer.PorosityId = $('#porosityId').val();
    selectedAnswer.PorosityDescription = $('#porosityDesc').val();
    selectedAnswer.ElasticityId = $('#elasticityId').val();
    selectedAnswer.ElasticityDescription = $('#elasticityDesc').val();
    selectedAnswer.DensityId = $('#densityId').val();
    selectedAnswer.DensityDescription = $('#densityDesc').val();
    goals = [];
    challenges = [];
    for (var i = 0; i < 3; i++) {
        goals.push($('#goal_' + i).val());
        challenges.push($('#challenge_' + i).val());
    }
    selectedAnswer.Goals = goals;
    selectedAnswer.Challenges = challenges;
    fileModel.append('UserId', $('#userId').val());
    fileModel.append('QA', $('#QANumber').val());
    fileModel.append('CreatedBy', $('#ddlanalyst').val());
    fileModel.append('ConsultantNotes', $('#txtConsultantNotes').val());
    fileModel.append('RecommendationNotes', $('#txtRecommendationNotes').val());
    fileModel.append('HairAnalysisNotes', $('#txtHairAnalysisNotes').val());
    fileModel.append('MyNotes', $('#txtMyNotes').val());
    fileModel.append('HairId', $('#txtHairId').val());
    fileModel.append('HealthSummary', $('#txtHealthSummary').val());
    fileModel.append('TopLeftStrandDiameter', $('#txtStrandDiameter1').val());
    fileModel.append('TopLeftHealthText', $('#txtHealth1').val());
    fileModel.append('TopRightStrandDiameter', $('#txtStrandDiameter2').val());
    fileModel.append('TopRightHealthText', $('#txtHealth2').val());
    fileModel.append('BottomLeftStrandDiameter', $('#txtStrandDiameter3').val());
    fileModel.append('BottomLeftHealthText', $('#txtHealth3').val());
    fileModel.append('BottoRightStrandDiameter', $('#txtStrandDiameter4').val());
    fileModel.append('BottomRightHealthText', $('#txtHealth4').val());
    fileModel.append('CrownStrandDiameter', $('#txtStrandDiameter5').val());
    fileModel.append('CrownHealthText', $('#txtHealth5').val());
    fileModel.append('TempSelectedAnswer', JSON.stringify(selectedAnswer));
    fileModel.append('TempHealth', JSON.stringify(health));
    fileModel.append('TempObservation', JSON.stringify(observation));

    fileModel.append('TempObservationElasticity', JSON.stringify(observationElasticity));
    fileModel.append('TempObservationChemicalProduct', JSON.stringify(observationChemicalProduct));
    fileModel.append('TempObservationPhysicalProduct', JSON.stringify(observationPhysicalProduct));
    //fileModel.append('TempObservationDamage', JSON.stringify(observationDamage));
    fileModel.append('TempObservationBreakage', JSON.stringify(observationBreakage));
    fileModel.append('TempObservationSplitting', JSON.stringify(observationSplitting));


    fileModel.append('TempPororsity', JSON.stringify(porosity));
    fileModel.append('TempElasticity', JSON.stringify(elasticity));

    fileModel.append('TempRecommendedProducts', JSON.stringify(products));
    fileModel.append('TempRecommendedProductsStylings', JSON.stringify(productsStyling));
    fileModel.append('TempAllRecommendedProductsEssential', JSON.stringify(allProductsEssential));
    fileModel.append('TempAllRecommendedProductsStyling', JSON.stringify(allProductsStyling));
    fileModel.append('TempRecommendedIngredients', JSON.stringify(ingredients));
    fileModel.append('TempRecommendedTools', JSON.stringify(tools));
    fileModel.append('TempRecommendedCategories', JSON.stringify(products));
    fileModel.append('TempRecommendedProductTypes', JSON.stringify(products));
    fileModel.append('TempRecommendedRegimens', JSON.stringify(regimens));
    fileModel.append('TempRecommendedVideos', JSON.stringify(videos));
    fileModel.append('TempRecommendedStylist', JSON.stringify(stylist));
    fileModel.append('TempRecommendedStyleRecipeVideos', JSON.stringify(styleRecipeVideos));
    fileModel.append('TempRecommendedProductsStyleRecipe', JSON.stringify(productsStyleRecipe));
    fileModel.append('HairStyleId', $('#hairStyles').val());
    fileModel.append('SaveType', saveType);
    //fileModel.append('HairProfileId', $('#ddlHHCP').val())
    // Check if the dropdown exists
    if ($('#ddlHHCP').length > 0) {
        // If the dropdown exists, append the selected value
        fileModel.append('HairProfileId', $('#ddlHHCP').val());
    } else {
        // If the dropdown doesn't exist, append the value from the hidden input
        fileModel.append('HairProfileId', $('#HairProfileId').val());
    }

    fileModel.append('TopLeftPhoto', topLeftImages);

    fileModel.append('TopRightPhoto', topRightImages);

    fileModel.append('BottomLeftPhoto', bottomLeftImages);

    fileModel.append('BottomRightPhoto', bottomRightImages);

    fileModel.append('CrownPhoto', crownImages);
    fileModel.append('TabNo', currentTab);
    fileModel.append('NotifyUser', $("input[name=NotifyUser]").val());
    fileModel.append('IsNewHHCP',$('#isNewHHCP').val());
    fileModel.append('ModifiedBy', $('#UserTypeId').val());
    $.ajax({
        type: "POST",
        url: "/Questionnaire/SaveProfile",
        //data: hairProfile,
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            var values = response.split(',');
            if (values[0] === "1") {
                $('#HairProfileId').val(values[1]);
                $('#successMessage').text("Profile Added/Updated Successfully!");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                //setTimeout(function () {
                //    window.location.href = '/Questionnaire/Questionnaire';
                //}, 2000);
            }

            if (values[0] === "2") {
                $('#HairProfileId').val(values[1]);
                $('#successMessage').text("Profile Added/Updated Successfully!");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {
            $('.preloader').css('display', 'none');
        }
    });

  
}
$(document).ready(function () {
    
    // Set initial state
    $('input[name=NotifyUser]').val($(this).is(':checked'));

    $("input[name=NotifyUser]").change(function () {
        debugger
        $(this).val(this.checked ? "TRUE" : "FALSE");
    });
});

function SelectBrandStyling(data) {
    brandListStyling = [];
    if (arrProdStyling.length > 0) {
        arrProdStyling = [];
    }
    else {
        $('#brandStyling option:selected').each(function (i, sel) {

            var brandName = sel.text;
            brandListStyling.push(brandName);
        });
       
        if (brandListStyling.length > 0 || pTypeStylingIds.length > 0) {
            $('#productHidden').attr('hidden', false);
            var selectedProductList = $("#productStyling").val();
            $("#productStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
            for (i = 0; i < data.length; i++) {
                if (brandListStyling.includes(data[i].brandName)) {
                    if (pTypeStylingIds.length > 0) {
                        if (pTypeStylingIds.includes(data[i].productTypeId))
                            $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                    }
                    //if (selectedProductTypesIdsStyling.length > 0) {
                    //    if (data[i].productTypeId != null && selectedProductTypesIdsStyling.find(tIds => tIds.toString() == data[i].productTypeId.toString()) != undefined) {
                    //        $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                    //    }
                    //}
                    else {
                        $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                    }
                }
                //if (brandListStyling.length == 0) {
                //    if (pTypeStylingIds.length > 0) {
                //        if (pTypeStylingIds.includes(data[i].productTypeId))
                //            $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                //    }
                //}
            }
        } else {
            $("#productStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
        }
        if (arrProducts != null) {
            $('#productStyling').val(arrProducts);
            $('#productStyling').trigger('change');
        }
        if (typeof selectedProductList != 'undefined') {
            if (selectedProductList.length > 0) {
                var selectedValues = [];
                for (i = 0; i < data.length; i++) {
                    var id = data[i].id.toString();
                    if (brandListStyling.includes(data[i].brandName) && selectedProductList.includes(id)) {
                        selectedValues.push(data[i].id);
                    }
                }
                $('#productStyling').val(selectedValues);
                $('#productStyling').trigger('change');
            }
        }
    }
}


function changeTypeStyling2(data, pType) {
    pTypeStylingIds = [];
    for (var i = 1; i < 6; i++) {
        $('#productTypeStyling_' + i + ' option:selected').each(function (i, sel) {
            var typeId = parseInt(sel.value);
            pTypeStylingIds.push(typeId);
        });
    }


    var selectedProductList = $("#productStyling").val();
    var selectedBrandList = $("#brandStyling").val();
    var selectedProductStylingList = $("#allProductStyling").val();
    var selectProductTypeLst = []
    for (var i = 1; i < 5; i++) {
        if ($('#productTypeStyling_' + i).val() != '') {
            var typeArray = $('#productTypeStyling_' + i).val();
            $.each(typeArray, function (k) {
                selectProductTypeLst.push(typeArray[k]);
                selectedProductTypesIdsStyling.push(typeArray[k])
            });
        }
    }
    var brandLst = [];
    var tBrandList = [];
    var flag1 = false;
    $.each(selectProductTypeLst, function (k) {  //24

        var tempBrandList = [];

        for (j = 0; j < data.length; j++) {
            if (data[j].productTypeId == selectProductTypeLst[k]) {  // 18 == 18, 24
                if (!tempBrandList.includes(data[j].brandName))
                    tempBrandList.push(data[j].brandName);  //Kim Kimble, Koils by Nature, MIELLE, MIXED CHICKS
            }
        }
        if (!flag1) {
            flag1 = true;
            tempBrandList.forEach(function (brndName) {
                brandLst.push(brndName);  //Kim Kimble, Koils by Nature, MIELLE, MIXED CHICKS
                tBrandList.push(brndName);
            });
        }
        else {
            // Rahul -Start- changed loop to fix issue of product while edit
            $.each(tempBrandList, function (p) {
                var index = 0;
                var isExists = true;
                var bName = null;
                $.each(tBrandList, function (i) {
                    if (tBrandList[i] == tempBrandList[p]) {
                        isExists = true;
                        return false;
                    }
                    else {
                        isExists = false;
                    }
                });
                $.each(brandLst, function (i) {
                    if (brandLst[i] == tempBrandList[p]) {
                        return false;
                    }
                    index++;
                });
                if (isExists == false) {
                    brandLst.splice(index, 1, tempBrandList[p]);
                }
                    
            });
            // Rahul -End- changed loop to fix issue of product while edit

        }

    });
    if (brandLst.length > 0) {
        // var brandList = $("#brand").val();
        //$("#productStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
        //$("#allProductStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));

        $("#brandStyling").html(''); //.append($("<option></option>").val("").html("Select Sub-Content"));
        for (i = 0; i < brandLst.length; i++) {
            $("#brandStyling").append($("<option></option>").val(brandLst[i]).html(brandLst[i]));
        }

        for (i = 0; i < data.length; i++) {
            if (pTypeStylingIds.includes(data[i].productTypeId)) {
                if (brandLst.length > 0) {
                    if (brandLst.includes(data[i].brandName)) {
                    //    $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                        //$("#allProductStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                    }
                }
                else {
                    //$("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                    //$("#allProductStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                }
            }

            if (pTypeStylingIds.length == 0) {
                if (brandLst.length > 0) {
                    //if (brandLst.includes(data[i].brandName))
                       // $("#productStyling").append($("<option></option>").val(data[i].id).html(data[i].productName));
                }
            }
        }
    }



    //  setTimeout(function () {
    if (typeof selectedProductList != 'undefined') {
        var selectedProductTypeList = $("#productTypeStyling").val();
        if (selectedProductList.length > 0) {
            var selectedValues = [];
            for (i = 0; i < data.length; i++) {
                var id = data[i].id.toString();
                //if (brandList.includes(data[i].brandName) && selectedProductList.includes(id) && selectedProductTypeList.push(data[i].productTypeId)) {
                if (brandLst.includes(data[i].brandName) && selectedProductList.includes(id)) {
                    selectedValues.push(data[i].id);
                }
            }
            $('#productStyling').val(selectedValues);
            $('#productStyling').trigger('change');
        }
    }

    if (typeof selectedBrandList != 'undefined') {
        if (selectedBrandList.length > 0) {
            var selectedValues = [];
            for (i = 0; i < data.length; i++) {
                var id = data[i].id.toString();
                if (brandLst.includes(data[i].brandName) && selectedBrandList.includes(data[i].brandName)) {
                    selectedValues.push(data[i].brandName);
                }
            }
            $('#brandStyling').val(selectedValues);
        }
    }

    if (typeof selectedProductStylingList != 'undefined') {
        if (selectedProductStylingList.length > 0) {
            $('#allProductStyling').val(selectedProductStylingList);
        }
    }
}


function SelectVideoCategory(data) {
    var recVideosSelected = $('#videos').val();
    videoCategory = [];
    if (arrVidCategory.length > 0) {
        arrVidCategory = [];
    }
    else {
        $('#videoCategories option:selected').each(function (i, sel) {

            var video = sel.value;
            videoCategory.push(video);
        });

        if (videoCategory.length > 0) {
            $("#videos").html('').append($("<option></option>").val("").html("Select Sub-Content"));
            for (i = 0; i < data.length; i++) {
                if (videoCategory.includes(data[i].videoCategoryId.toString())) {
                    $("#videos").append($("<option></option>").val(data[i].MediaLinkEntityId.toString()).html(data[i].Title));
                }
            }
        } else {
            $("#videos").html('').append($("<option></option>").val("").html("Select Sub-Content"));
        }
        $('#videos').val(recVideosSelected);
        $('#videos').trigger('change');
    }
}
function SelectVideoCategoryStyling(data) {
    var recVideosSelected = $('#videosStyling').val();
    videoCategory = [];
    if (arrVidCategory.length > 0) {
        arrVidCategory = [];
    }
    else {
        $('#videoCategoriesStyling option:selected').each(function (i, sel) {

            var video = sel.value;
            videoCategory.push(video);
        });

        if (videoCategory.length > 0) {
            $("#videosStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
            for (i = 0; i < data.length; i++) {
                if (videoCategory.includes(data[i].videoCategoryId.toString())) {
                    $("#videosStyling").append($("<option></option>").val(data[i].MediaLinkEntityId.toString()).html(data[i].Title));
                }
            }
        } else {
            $("#videosStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
        }
        $('#videosStyling').val(recVideosSelected);
        $('#videosStyling').trigger('change');
    }
}
