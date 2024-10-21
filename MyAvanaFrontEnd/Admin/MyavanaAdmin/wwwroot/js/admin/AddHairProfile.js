function loadTab3(data, repo) {
    var response = repo;
    var pType = data;

if (response.allRecommendedProductsEssential != null) {
    if (response.allRecommendedProductsEssential.length > 0) {
        var arr = [];
        for (var i = 0; i < response.allRecommendedProductsEssential.length; i++) {
            arr.push(response.allRecommendedProductsEssential[i].productId);
        }
        $('#allProductEssential').val(arr);
        $('#allProductEssential').trigger('change');
    }
}

if (response.allRecommendedProductsStyling != null) {
    if (response.allRecommendedProductsStyling.length > 0) {
        var arr = [];
        for (var i = 0; i < response.allRecommendedProductsStyling.length; i++) {
            arr.push(response.allRecommendedProductsStyling[i].productId);
        }
        $('#allProductStyling').val(arr);
        $('#allProductStyling').trigger('change');
    }
}
    if (response.recommendedProducts != null) {
        var productLength = response.recommendedProducts.length;
        if (productLength > 0) {
            for (var i = 0; i < productLength; i++) {
                if (parenNameCheck.includes(response.recommendedProducts[i].productId))
                    continue;
                else {
                    parenNameCheck.push(response.recommendedProducts[i].productId);
                    productParentNameList.push(response.recommendedProducts[i].productId);
                }
                var productTypeLength = response.recommendedProducts[i].productsTypes.length;
                if (productTypeLength > 0) {
                    for (var j = 0; j < productTypeLength; j++) {
                        if (productTypeCheck.includes(response.recommendedProducts[i].productsTypes[j].productId))
                            continue;
                        else {
                            productTypeCheck.push(response.recommendedProducts[i].productsTypes[j].productId);
                            productTypeList.push(response.recommendedProducts[i].productsTypes[j].productId);
                        }

                        var productsLength = response.recommendedProducts[i].productsTypes[j].products.length;
                        if (productsLength > 0) {
                            for (var k = 0; k < productsLength; k++) {
                                if (!(dupCheck.includes(response.recommendedProducts[i].productsTypes[j].products[k].brandName))) {
                                    dupCheck.push(response.recommendedProducts[i].productsTypes[j].products[k].brandName);
                                    brandList.push(response.recommendedProducts[i].productsTypes[j].products[k].brandName);
                                    classificationList = arrayUnique(classificationList.concat(response.recommendedProducts[i].productsTypes[j].products[k].productClassifications));
                                    hairChallegeList = arrayUnique(hairChallegeList.concat(response.recommendedProducts[i].productsTypes[j].products[k].hairChallenges));
                                }

                                if (prodCheck.includes(response.recommendedProducts[i].productsTypes[j].products[k].id))
                                    continue;
                                else {
                                    prodCheck.push(response.recommendedProducts[i].productsTypes[j].products[k].id);
                                    productList.push(response.recommendedProducts[i].productsTypes[j].products[k].id);
                                }
                            }
                           
                            $('#productClassificationSection').val(classificationList);
                            $('#productClassificationSection').trigger('change');
                            $('#hairChallengeSection').val(hairChallegeList);
                            $('#hairChallengeSection').trigger('change');
                            $('#brand').val(brandList);
                            $('#brand').trigger('change');
                        }
                    }
                }
            }
        }

        if (pType != null) {
            for (j = 0; j < pType.length; j++) {
                if (productParentNameList.includes(pType[j].parentId)) {
                    for (var q = 1; q <= 6; q++) {
                        $('#productType_' + q).select2();
                        $('#productType_' + q).val(productTypeList);
                        $('#product').val(productList);
                        $('#product').trigger('change');
                    }
                }
            }
            $('#brand').val(brandList);
            $('#brand').trigger('change');
        }
    }
var productLength = response.recommendedProductsStyling;
if (productLength != null) {
    for (var i = 0; i < productLength.length; i++) {
        if (parenNameCheckStyling.includes(response.recommendedProductsStyling[i].productId))
            continue;
        else {
            parenNameCheckStyling.push(response.recommendedProductsStyling[i].productId);
            productParentNameListStyling.push(response.recommendedProductsStyling[i].productId);
        }
        var productTypeLength = response.recommendedProductsStyling[i].productsTypes.length;
        if (productTypeLength > 0) {
            for (var j = 0; j < productTypeLength; j++) {
                if (productTypeCheckStyling.includes(response.recommendedProductsStyling[i].productsTypes[j].productId))
                    continue;
                else {
                    productTypeCheckStyling.push(response.recommendedProductsStyling[i].productsTypes[j].productId);
                    productTypeListStyling.push(response.recommendedProductsStyling[i].productsTypes[j].productId);
                }

                var productsLength = response.recommendedProductsStyling[i].productsTypes[j].products.length;
                if (productsLength > 0) {
                    for (var k = 0; k < productsLength; k++) {
                        if (!(dupCheckStyling.includes(response.recommendedProductsStyling[i].productsTypes[j].products[k].brandName))) {
                            dupCheckStyling.push(response.recommendedProductsStyling[i].productsTypes[j].products[k].brandName);
                            brandListStyling.push(response.recommendedProductsStyling[i].productsTypes[j].products[k].brandName);
                            classificationListStyling = arrayUnique(classificationListStyling.concat(response.recommendedProductsStyling[i].productsTypes[j].products[k].productClassifications));
                            hairChallegeListStyling = arrayUnique(hairChallegeListStyling.concat(response.recommendedProductsStyling[i].productsTypes[j].products[k].hairChallenges));
                        }

                        if (prodCheckStyling.includes(response.recommendedProductsStyling[i].productsTypes[j].products[k].id))
                            continue;
                        else {
                            prodCheckStyling.push(response.recommendedProductsStyling[i].productsTypes[j].products[k].id);
                            productListStyling.push(response.recommendedProductsStyling[i].productsTypes[j].products[k].id);
                        }
                    }

                    $('#productClassificationStyling').val(classificationListStyling);
                    $('#productClassificationStyling').trigger('change');
                    $('#hairChallengeStyling').val(hairChallegeListStyling);
                    $('#hairChallengeStyling').trigger('change');
                    $('#brandStyling').val(brandListStyling);
                    $('#brandStyling').trigger('change');
                }
            }
        }
    }
}

$("#productTypeStyling").html('').append($("<option></option>").val("").html("Select Sub-Content"));
    if (pType != null) {
    for (j = 0; j < pType.length; j++) {
        if (productParentNameListStyling.includes(pType[j].parentId)) {
            for (var m = 1; m <= 5; m++) {
                $('#productTypeStyling_' + m).select2();
                $('#productTypeStyling_' + m).val(productTypeListStyling);
                //selectedProductTypesIdsStyling = productTypeListStyling;
                    $('#productStyling').val(productListStyling);
                    $('#productStyling').trigger('change');
                
            }
        }
    }
    $('#brandStyling').val(brandListStyling);
    $('#brandStyling').trigger('change');
    }
}


function loadTab4(repo) {
    var response = repo;
    if (response.recommendedIngredients != null) {
        if (response.recommendedIngredients.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedIngredients.length; i++) {
                arr.push(response.recommendedIngredients[i].ingredientId);
            }
            $('#ingredients').val(arr);
            $('#ingredients').trigger('change');
        }
    }
    //}
    if (response.recommendedTools != null) {
        if (response.recommendedTools.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedTools.length; i++) {
                arr.push(response.recommendedTools[i].toolId);
            }
            $('#tools').val(arr);
            $('#tools').trigger('change');
        }
    }
    if (response.videoCategoryIds != null && response.videoCategoryIds.length > 0) {
        $('#videoCategories').val(response.videoCategoryIds);
        $('#videoCategories').trigger('change');
    }

    if (response.recommendedVideos != null) {
        if (response.recommendedVideos.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedVideos.length; i++) {
                arr.push(response.recommendedVideos[i].mediaLinkEntityId);
            }
            $('#videos').val(arr);
            $('#videos').trigger('change');
        }
    }

   

    if (response.recommendedRegimens != null) {
        if (response.recommendedRegimens.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedRegimens.length; i++) {
                arr.push(response.recommendedRegimens[i].regimenId);
            }
            $('#regimens').val(arr);
            $('#regimens').trigger('change');
        }
    }

    if (response.recommendedStylist != null) {
        if (response.recommendedStylist.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedStylist.length; i++) {
                arr.push(response.recommendedStylist[i].stylistId);
            }
            $('#stylist').val(arr);
            $('#stylist').trigger('change');
        }
    }
}
function loadTab5(repo) {
    var response = repo;

    $('#hairStyles').val(response.hairStyleId);
    if (response.recommendedProductsStyleRecipe != null) {
        if (response.recommendedProductsStyleRecipe.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedProductsStyleRecipe.length; i++) {
                arr.push(response.recommendedProductsStyleRecipe[i].productId);
            }
            $('#StylingRecipeProducts').val(arr);
            $('#StylingRecipeProducts').trigger('change');
        }
    }
    
    if (response.styleRecipeVideoCategoryIds != null && response.styleRecipeVideoCategoryIds.length > 0) {
        $('#videoCategoriesStyling').val(response.styleRecipeVideoCategoryIds);
        $('#videoCategoriesStyling').trigger('change');
    }

    if (response.recommendedStyleRecipeVideos != null) {
        if (response.recommendedStyleRecipeVideos.length > 0) {
            var arr = [];
            for (var i = 0; i < response.recommendedStyleRecipeVideos.length; i++) {
                arr.push(response.recommendedStyleRecipeVideos[i].mediaLinkEntityId);
            }
            $('#videosStyling').val(arr);
            $('#videosStyling').trigger('change');
        }
    }


}
function loadTab2(repo) {
    var response = repo;

    // Clear porosity, elasticity, health, observation, and observation-related checkboxes
    $('input[type=radio]').prop('checked', false);  // Uncheck all radio buttons
    $('input[type=checkbox]').prop('checked', false);  // Uncheck all checkboxes

    $('#txtHairAnalysisNotes').val(response.hairAnalysisNotes == null ? '' : response.hairAnalysisNotes);
    if (response.topLeft != null) {
        if (response.topLeft.topLeftPhoto.length > 0) {
            $('#TopLeftPhotoSelected').html('');
            $('#TopLeftPhotoSelectedUnique').html('');
            tlfileName = [];
            for (var i = 0; i < response.topLeft.topLeftPhoto.length; i++) {
                tlfileName[i] = $.trim(response.topLeft.topLeftPhoto[i].strandImage);
            }
            for (var i = 0; i < tlfileName.length; i++) {
                var filename = "";
                if (tlfileName[i].indexOf("_") > -1)
                    filename = tlfileName[i].substring(tlfileName[i].indexOf('_') + 1);
                else
                    filename = tlfileName[i];

                $('#TopLeftPhotoSelected').append("<span id=" + filename + '_tl' + " class='spnTopLeft'>" + filename + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteTopLeft('" + filename + "')></i></span>");
                $('#TopLeftPhotoSelectedUnique').append("<span id=" + filename + '_tl' + "  class='spnTopLeftUnique' >" + tlfileName[i] + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
            }
        }
        $('#txtStrandDiameter1').val(response.topLeft.topLeftStrandDiameter);

    }
    if (response.topLeft != null) {
        if (response.topLeft.pororsity != null)
            $('#Porosity1_' + response.topLeft.pororsity.id).prop('checked', true);
    }
    if (response.topRight.pororsity != null)
        $('#Porosity2_' + response.topRight.pororsity.id).prop('checked', true);
    if (response.bottomLeft.pororsity != null)
        $('#Porosity3_' + response.bottomLeft.pororsity.id).prop('checked', true);
    if (response.bottomRight.pororsity != null)
        $('#Porosity4_' + response.bottomRight.pororsity.id).prop('checked', true);
    if (response.crownStrand.pororsity != null)
        $('#Porosity5_' + response.crownStrand.pororsity.id).prop('checked', true);

    if (response.topLeft.elasticity != null)
        $('#Elasticity1_' + response.topLeft.elasticity.id).prop('checked', true);
    if (response.topRight.elasticity != null)
        $('#Elasticity2_' + response.topRight.elasticity.id).prop('checked', true);
    if (response.bottomLeft.elasticity != null)
        $('#Elasticity3_' + response.bottomLeft.elasticity.id).prop('checked', true);
    if (response.bottomRight.elasticity != null)
        $('#Elasticity4_' + response.bottomRight.elasticity.id).prop('checked', true);
    if (response.crownStrand.elasticity != null)
        $('#Elasticity5_' + response.crownStrand.elasticity.id).prop('checked', true);

    if (response.topLeft.health != null) {
        if (response.topLeft.health.length > 0) {
            for (var i = 0; i < response.topLeft.health.length; i++) {
                $('#health1_' + response.topLeft.health[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.health != null) {
        if (response.topRight.health.length > 0) {
            for (var i = 0; i < response.topRight.health.length; i++) {
                $('#health2_' + response.topRight.health[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.health != null) {
        if (response.bottomLeft.health.length > 0) {
            for (var i = 0; i < response.bottomLeft.health.length; i++) {
                $('#health3_' + response.bottomLeft.health[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.health != null) {
        if (response.bottomRight.health.length > 0) {
            for (var i = 0; i < response.bottomRight.health.length; i++) {
                $('#health4_' + response.bottomRight.health[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.health != null) {
        if (response.crownStrand.health.length > 0) {
            for (var i = 0; i < response.crownStrand.health.length; i++) {
                $('#health5_' + response.crownStrand.health[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.observation != null) {
        if (response.topLeft.observation.length > 0) {
            for (var i = 0; i < response.topLeft.observation.length; i++) {
                $('#observation1_' + response.topLeft.observation[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.observation != null) {
        if (response.topRight.observation.length > 0) {
            for (var i = 0; i < response.topRight.observation.length; i++) {
                $('#observation2_' + response.topRight.observation[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.observation != null) {
        if (response.bottomLeft.observation.length > 0) {
            for (var i = 0; i < response.bottomLeft.observation.length; i++) {
                $('#observation3_' + response.bottomLeft.observation[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.observation != null) {
        if (response.bottomRight.observation.length > 0) {
            for (var i = 0; i < response.bottomRight.observation.length; i++) {
                $('#observation4_' + response.bottomRight.observation[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.observation != null) {
        if (response.crownStrand.observation.length > 0) {
            for (var i = 0; i < response.crownStrand.observation.length; i++) {
                $('#observation5_' + response.crownStrand.observation[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.obsBreakages != null) {
        if (response.topLeft.obsBreakages.length > 0) {
            for (var i = 0; i < response.topLeft.obsBreakages.length; i++) {
                $('#observationBreak1_' + response.topLeft.obsBreakages[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.obsBreakages != null) {
        if (response.topRight.obsBreakages.length > 0) {
            for (var i = 0; i < response.topRight.obsBreakages.length; i++) {
                $('#observationBreak2_' + response.topRight.obsBreakages[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.obsBreakages != null) {
        if (response.bottomLeft.obsBreakages.length > 0) {
            for (var i = 0; i < response.bottomLeft.obsBreakages.length; i++) {
                $('#observationBreak3_' + response.bottomLeft.obsBreakages[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.obsBreakages != null) {
        if (response.bottomRight.obsBreakages.length > 0) {
            for (var i = 0; i < response.bottomRight.obsBreakages.length; i++) {
                $('#observationBreak4_' + response.bottomRight.obsBreakages[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.obsBreakages != null) {
        if (response.crownStrand.obsBreakages.length > 0) {
            for (var i = 0; i < response.crownStrand.obsBreakages.length; i++) {
                $('#observationBreak5_' + response.crownStrand.obsBreakages[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.obsChemicalProducts != null) {
        if (response.topLeft.obsChemicalProducts.length > 0) {
            for (var i = 0; i < response.topLeft.obsChemicalProducts.length; i++) {
                $('#observationChem1_' + response.topLeft.obsChemicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.obsChemicalProducts != null) {
        if (response.topRight.obsChemicalProducts.length > 0) {
            for (var i = 0; i < response.topRight.obsChemicalProducts.length; i++) {
                $('#observationChem2_' + response.topRight.obsChemicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.obsChemicalProducts != null) {
        if (response.bottomLeft.obsChemicalProducts.length > 0) {
            for (var i = 0; i < response.bottomLeft.obsChemicalProducts.length; i++) {
                $('#observationChem3_' + response.bottomLeft.obsChemicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.obsChemicalProducts != null) {
        if (response.bottomRight.obsChemicalProducts.length > 0) {
            for (var i = 0; i < response.bottomRight.obsChemicalProducts.length; i++) {
                $('#observationChem4_' + response.bottomRight.obsChemicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.obsChemicalProducts != null) {
        if (response.crownStrand.obsChemicalProducts.length > 0) {
            for (var i = 0; i < response.crownStrand.obsChemicalProducts.length; i++) {
                $('#observationChem5_' + response.crownStrand.obsChemicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.obsElasticities != null) {
        if (response.topLeft.obsElasticities.length > 0) {
            for (var i = 0; i < response.topLeft.obsElasticities.length; i++) {
                $('#observationElas1_' + response.topLeft.obsElasticities[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.obsElasticities != null) {
        if (response.topRight.obsElasticities.length > 0) {
            for (var i = 0; i < response.topRight.obsElasticities.length; i++) {
                $('#observationElas2_' + response.topRight.obsElasticities[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.obsElasticities != null) {
        if (response.bottomLeft.obsElasticities.length > 0) {
            for (var i = 0; i < response.bottomLeft.obsElasticities.length; i++) {
                $('#observationElas3_' + response.bottomLeft.obsElasticities[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.obsElasticities != null) {
        if (response.bottomRight.obsElasticities.length > 0) {
            for (var i = 0; i < response.bottomRight.obsElasticities.length; i++) {
                $('#observationElas4_' + response.bottomRight.obsElasticities[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.obsElasticities != null) {
        if (response.crownStrand.obsElasticities.length > 0) {
            for (var i = 0; i < response.crownStrand.obsElasticities.length; i++) {
                $('#observationElas5_' + response.crownStrand.obsElasticities[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.obsPhysicalProducts != null) {
        if (response.topLeft.obsPhysicalProducts.length > 0) {
            for (var i = 0; i < response.topLeft.obsPhysicalProducts.length; i++) {
                $('#observationPhy1_' + response.topLeft.obsPhysicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.obsPhysicalProducts != null) {
        if (response.topRight.obsPhysicalProducts.length > 0) {
            for (var i = 0; i < response.topRight.obsPhysicalProducts.length; i++) {
                $('#observationPhy2_' + response.topRight.obsPhysicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.obsPhysicalProducts != null) {
        if (response.bottomLeft.obsPhysicalProducts.length > 0) {
            for (var i = 0; i < response.bottomLeft.obsPhysicalProducts.length; i++) {
                $('#observationPhy3_' + response.bottomLeft.obsPhysicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.obsPhysicalProducts != null) {
        if (response.bottomRight.obsPhysicalProducts.length > 0) {
            for (var i = 0; i < response.bottomRight.obsPhysicalProducts.length; i++) {
                $('#observationPhy4_' + response.bottomRight.obsPhysicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.obsPhysicalProducts != null) {
        if (response.crownStrand.obsPhysicalProducts.length > 0) {
            for (var i = 0; i < response.crownStrand.obsPhysicalProducts.length; i++) {
                $('#observationPhy5_' + response.crownStrand.obsPhysicalProducts[i].id).prop('checked', true);
            }
        }
    }

    if (response.topLeft.obsSplittings != null) {
        if (response.topLeft.obsSplittings.length > 0) {
            for (var i = 0; i < response.topLeft.obsSplittings.length; i++) {
                $('#observationSplit1_' + response.topLeft.obsSplittings[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.obsSplittings != null) {
        if (response.topRight.obsSplittings.length > 0) {
            for (var i = 0; i < response.topRight.obsSplittings.length; i++) {
                $('#observationSplit2_' + response.topRight.obsSplittings[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomLeft.obsSplittings != null) {
        if (response.bottomLeft.obsSplittings.length > 0) {
            for (var i = 0; i < response.bottomLeft.obsSplittings.length; i++) {
                $('#observationBreak3_' + response.bottomLeft.obsSplittings[i].id).prop('checked', true);
            }
        }
    }

    if (response.bottomRight.obsSplittings != null) {
        if (response.bottomRight.obsSplittings.length > 0) {
            for (var i = 0; i < response.bottomRight.obsSplittings.length; i++) {
                $('#observationSplit4_' + response.bottomRight.obsSplittings[i].id).prop('checked', true);
            }
        }
    }

    if (response.crownStrand.obsSplittings != null) {
        if (response.crownStrand.obsSplittings.length > 0) {
            for (var i = 0; i < response.crownStrand.obsSplittings.length; i++) {
                $('#observationSplit5_' + response.crownStrand.obsSplittings[i].id).prop('checked', true);
            }
        }
    }

    if (response.topRight.topRightPhoto != null) {
        if (response.topRight.topRightPhoto.length > 0) {
            $('#TopRightPhotoSelected').html('');
            $('#TopRightPhotoSelectedUnique').html('');
            trfileName = [];
            for (var i = 0; i < response.topRight.topRightPhoto.length; i++) {
                trfileName[i] = $.trim(response.topRight.topRightPhoto[i].strandImage);
            }

            for (var i = 0; i < trfileName.length; i++) {
                var filename = "";
                if (trfileName[i].indexOf("_") > -1)
                    filename = trfileName[i].substring(trfileName[i].indexOf('_') + 1);
                else
                    filename = trfileName[i];

                $('#TopRightPhotoSelected').append("<span id=" + filename + '_tr' + " class='spnTopRight' >" + filename + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteTopRight('" + filename + "')></i></span>");
                $('#TopRightPhotoSelectedUnique').append("<span id=" + filename + '_tr' + "  class='spnTopRightUnique' >" + trfileName[i] + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
            }
        }
    }

    $('#txtStrandDiameter2').val(response.topRight.topRightStrandDiameter);

    if (response.bottomLeft.bottomLeftPhoto != null) {
        if (response.bottomLeft.bottomLeftPhoto.length > 0) {
            $('#BottomLeftPhotoSelected').html('');
            $('#BottomLeftPhotoSelectedUnique').html('');
            blfileName = [];
            for (var i = 0; i < response.bottomLeft.bottomLeftPhoto.length; i++) {
                blfileName[i] = $.trim(response.bottomLeft.bottomLeftPhoto[i].strandImage);
            }

            for (var i = 0; i < blfileName.length; i++) {

                var filename = "";
                if (blfileName[i].indexOf("_") > -1)
                    filename = blfileName[i].substring(blfileName[i].indexOf('_') + 1);
                else
                    filename = blfileName[i];

                $('#BottomLeftPhotoSelected').append("<span id=" + filename + '_bl' + " class='spnBottomLeft'>" + filename + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteBottomLeft('" + filename + "')></i></span>");
                $('#BottomLeftPhotoSelectedUnique').append("<span id=" + filename + '_bl' + "  class='spnBottomLeftUnique' >" + blfileName[i] + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
            }
        }
    }
    $('#txtStrandDiameter3').val(response.bottomLeft.bottomLeftStrandDiameter);

    if (response.bottomRight.bottomRightPhoto != null) {
        if (response.bottomRight.bottomRightPhoto.length > 0) {
            $('#BottomRightPhotoSelected').html('');
            $('#BottomRightPhotoSelectedUnique').html('');
            brfileName = [];
            for (var i = 0; i < response.bottomRight.bottomRightPhoto.length; i++) {
                brfileName[i] = $.trim(response.bottomRight.bottomRightPhoto[i].strandImage);
            }

            for (var i = 0; i < brfileName.length; i++) {
                var filename = "";
                if (brfileName[i].indexOf("_") > -1)
                    filename = brfileName[i].substring(brfileName[i].indexOf('_') + 1);
                else
                    filename = brfileName[i];

                $('#BottomRightPhotoSelected').append("<span id=" + filename + '_br' + " class='spnBottomRight'>" + filename + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteBottomRight('" + filename + "')></i></span>");
                $('#BottomRightPhotoSelectedUnique').append("<span id=" + filename + '_br' + "  class='spnBottomRightUnique' >" + brfileName[i] + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
            }
        }
    }
    $('#txtStrandDiameter4').val(response.bottomRight.bottomRightStrandDiameter);

    if (response.crownStrand.crownPhoto != null) {
        $('#CrownPhotoSelected').html('');
        $('#CrownPhotoSelectedUnique').html('');
        if (response.crownStrand.crownPhoto.length > 0) {
            crfileName = [];
            for (var i = 0; i < response.crownStrand.crownPhoto.length; i++) {
                crfileName[i] = $.trim(response.crownStrand.crownPhoto[i].strandImage);
            }

            for (var i = 0; i < crfileName.length; i++) {

                var filename = "";
                if (crfileName[i].indexOf("_") > -1)
                    filename = crfileName[i].substring(crfileName[i].indexOf('_') + 1);
                else
                    filename = crfileName[i];

                $('#CrownPhotoSelected').append("<span id=" + filename + '_cr' + " class='spnCrown'>" + filename + "<i class='fa fa-times' style='cursor: pointer' onclick=deleteCrown('" + filename + "')></i></span>");
                $('#CrownPhotoSelectedUnique').append("<span id=" + filename + '_cr' + "  class='spnCrownUnique' >" + crfileName[i] + "<i class='fa fa-times' style='cursor: pointer' </i></span>");
            }
        }
    }
    $('#txtStrandDiameter5').val(response.crownStrand.crownStrandDiameter);
}

function arrayUnique(array) {
    var a = array.concat();
    for (var i = 0; i < a.length; ++i) {
        for (var j = i + 1; j < a.length; ++j) {
            if (a[i] === a[j])
                a.splice(j--, 1);
        }
    }

    return a;
}






