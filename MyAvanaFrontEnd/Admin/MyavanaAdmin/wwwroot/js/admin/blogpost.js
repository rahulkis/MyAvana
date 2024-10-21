var fileModel = new FormData();
$("#uploadImage").change(function () {

    fileModel = new FormData();

    var file = $("#uploadImage").get(0).files[0];
    fileModel.append("FormFile", file);
    $('#lblImage').val(file.name);
});

function SaveBlogs() {
    var blogProducts = [];
    var blogHairStyles = [];
    var blogMoods = [];
    var blogGuidances = [];
    if ($('#headline').val().trim() == "" || $('#details').val().trim() == "" || $('#imageUrl').val().trim() == "" || $('#additionalUrl').val().trim() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#imageUrl').val())) {
        $('#failureMessage').text("Wrong image Url");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if (! /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/.test($('#additionalUrl').val())) {
        $('#failureMessage').text("Wrong addition Url");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }

    $('#products option:selected').each(function (i, sel) {
        var productList = {};
        productList.ProductEntityId = $(this).val();
        productList.ActualName = $(this).text();
        blogProducts.push(productList);
    });
    $('#hairStyles option:selected').each(function (i, sel) {
        var hairStyleList = {};
        hairStyleList.HairStylesId = $(this).val();
        hairStyleList.Style = $(this).text();
        blogHairStyles.push(hairStyleList);
    });
    $('#moods option:selected').each(function (i, sel) {
        var moodList = {};
        moodList.MoodId = $(this).val();
        moodList.Mood = $(this).text();
        blogMoods.push(moodList);
    });
    $('#guidances option:selected').each(function (i, sel) {
        var guidanceList = {};
        guidanceList.GuidanceId = $(this).val();
        guidanceList.Guidance = $(this).text();
        blogGuidances.push(guidanceList);
    });

    var blogPostModel = {
        BlogArticleId: $('#articleId').val(),
        Headline: $('#headline').val().trim(),
        Details: $("#details").val().trim(),
        ImageUrl: $("#imageUrl").val().trim(),
        Url: $("#additionalUrl").val().trim(),
        ArticleProducts: JSON.stringify(blogProducts),
        ArticleHairStyles: JSON.stringify(blogHairStyles),
        ArticleMoods: JSON.stringify(blogMoods),
        ArticleGuidances: JSON.stringify(blogGuidances)
    }


	$('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Articles/CreateArticles",
        data: blogPostModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Article saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/Articles/ViewArticles'; }, 3000);
            }
            else {
                $('#failureMessage').text("Oops something goes wrong !");
                $('.alert-danger').css("display", "block");
                $('.alert-danger').delay(3000).fadeOut();
			}

			$('.preloader').css('display', 'none');
        },
    });
};

function RenderHtml(data, type, row, meta) {
    var tr = '<a href="/Articles/CreateArticles/' + row.blogArticleId + '" Title="Edit Article" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a  Title="Delete Article" onclick="deleteConfirmArticle(' + row.blogArticleId + ')"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function RenderImage(data, type, row, meta) {
    if (row.customThumbnail != '')
        return "<img src='" + row.imageUrl + "' style='width:100px;height:auto;' />";
    else
        return "";
}

function deleteConfirmArticle(blogArticleId) {

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeletePromoCode(blogArticleId);
    });
}
function DeletePromoCode(blogArticleId) {
    var article = {
        BlogArticleId: blogArticleId
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Articles/DeleteArticle",
        data: article,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Article deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/Articles/ViewArticles'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}