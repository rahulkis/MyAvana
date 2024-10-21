var hairChallenges = [];
var hairGoals = [];

function SaveMediaLink() {
    if ($('#txtVideo').val().trim() == "" || $('#txtDescription').val().trim() == "" || $('#txtTitle').val().trim() == "") {
        $('#failureMessage').text("Fields can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#VideoCategoryId').val() == "") {
        $('#failureMessage').text("Please select category!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    //if ($('#hairChallenges').val() == "") {
    //    $('#failureMessage').text("Please select hair challenges!");
    //    $('.alert-danger').css("display", "block");
    //    $('.alert-danger').delay(3000).fadeOut();
    //    return false;
    //}
    //if ($('#hairGoals').val() == "") {
    //    $('#failureMessage').text("Please select hair goals!");
    //    $('.alert-danger').css("display", "block");
    //    $('.alert-danger').delay(3000).fadeOut();
    //    return false;
    //}
    if ($('#txtVideo').val().includes('youtube')) {
        if (! /^(https|http):\/\/?(?:www\.)?youtube\.com\/.(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]*)/.test($('#txtVideo').val().trim())) {
            $('#failureMessage').text("Wrong UrL!");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    } else {
        if (! /^(https|http):\/\/?(?:www\.)?instagram\.com\/.(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]*)/.test($('#txtVideo').val().trim())) {
            $('#failureMessage').text("Wrong UrL!");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(3000).fadeOut();
            return false;
        }
    }

    if (/^(https|http):\/\/?(?:www\.)?instagram\.com\/.(?:(?:\w)*#!\/)?(?:pages\/)?(?:[\w\-]*\/)*([\w\-\.]*)/.test($('#txtVideo').val().trim())) {
        var videosrc = $('#txtVideo').val().trim().substring(0, $('#txtVideo').val().trim().lastIndexOf('/'));
        var videourl = videosrc;//+ "/?_ _a=1";

        SaveVideos(videourl);
        //var thumbnail = {
        //    videourl: videourl
        //}
        //$.ajax({
        //    type: "POST",
        //    url: "/SocialMedia/VideoThumbnail",
        //    data: thumbnail,
        //    success: function (response) {
        //        if (response != null) {
        //            videothumbnail = "http://admin.myavana.com/Thumbnails/" + response;
        //            SaveVideos();
        //        }
        //        else {
        //            $('#failureMessage').text("Thumbnail not created");
        //            $('.alert-danger').css("display", "block");
        //            $('.alert-danger').delay(3000).fadeOut();
        //            return false;
        //        }
        //    },
        //    error: function (res) {
        //    }
        //});

    }

    else {
        SaveVideos();
    }


}

function SaveVideos(videourl) {
    var ownerSalonsArray = [];
    $('.select2 option:selected').each(function (i, sel) {
        var salonOwners = {};
        salonOwners.SalonId = $(this).val();
        ownerSalonsArray.push(salonOwners);
    });
    /*var file = $('#EducationalImage').get(0).files[0];*/

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
    var mediaLinkEntityModel = {
        Id: $('#medaiLinkId').val(),
        VideoId: $('#txtVideo').val().trim(),
        Description: $("#txtDescription").val().trim(),
        Title: $("#txtTitle").val().trim(),
        CategoryId: $('#VideoCategoryId').val(),
        ImageLink: videourl,
        userSalons: ownerSalonsArray,
        ShowOnMobile: $('#ShowOnMobile').val(),
        HairChallenges: JSON.stringify(hairChallenges),
        HairGoals: JSON.stringify(hairGoals)
    }
    //if (file != null || file != undefined) {
    //    mediaLinkEntityModel.append('File', file);
    //}
    $('.preloader').css('display', 'block');

    $.ajax({
        type: "POST",
        url: "/SocialMedia/CreateVideo",
        data: mediaLinkEntityModel,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Video saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/SocialMedia/EducationalVideos'; }, 3000);
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


$('#btnImageClick').click(function () {
    $('#EducationalImage').click();
});
$('#EducationalImage').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        $('#txtImageName').text('');
        $('#txtImageName').text(filename);
    }
});
function RenderVideoHtml(data, type, row, meta) {
    var videoSource = row.VideoId;
    

    // Check if the video source is a YouTube Shorts URL
    if (videoSource.includes('youtube.com/shorts/')) {
        // Extract the Shorts ID
        var shortsId = videoSource.split('/shorts/')[1].split('?')[0];
        videoSource = 'https://www.youtube.com/embed/' + shortsId;
    }
    else if (videoSource.includes('watch?v=')) {
        // Convert standard YouTube video URL to embed URL
        videoId = videoSource.split('watch?v=')[1].split('&')[0];
        videoSource = 'https://www.youtube.com/embed/' + videoId;
    } else if (videoSource.includes('youtu.be/')) {
        // Shortened YouTube URL
        videoId = videoSource.split('youtu.be/')[1].split('?')[0];
        videoSource = 'https://www.youtube.com/embed/' + videoId;
    } else if (videoSource.includes('youtube.com/embed/')) {
        // Embedded YouTube URL
        videoId = videoSource.split('embed/')[1].split('?')[0];
        videoSource = 'https://www.youtube.com/embed/' + videoId;
    }
    else if (videoSource.includes('instagram.com')) {
        var instagramId;

        if (videoSource.includes('/tv/')) {
            // IGTV video
            instagramId = videoSource.split('/tv/')[1].split('/')[0];
            videoSource = 'https://www.instagram.com/tv/' + instagramId + '/embed/';
        } else if (videoSource.includes('/reel/')) {
            // Instagram Reel
            instagramId = videoSource.split('/reel/')[1].split('/')[0];
            videoSource = 'https://www.instagram.com/reel/' + instagramId + '/embed/';
        } else if (videoSource.includes('/p/')) {
            // Standard Instagram video post
            instagramId = videoSource.split('/p/')[1].split('/')[0];
            videoSource = 'https://www.instagram.com/p/' + instagramId + '/embed/';
        }
       
    }
    // Create the iframe HTML
    var tr = '<iframe width="320" height="150" src="' + videoSource + '" allowfullscreen></iframe>';
    //var tr = '<iframe width="' + width + '" height="' + height + '" src="' + videoSource + '" allowfullscreen style="border: none;"></iframe>';
    return tr;
}
function RenderHtml(data, type, row, meta) {
    var tr = '<a href="/SocialMedia/CreateVideo/' + row.id + '" Title="Edit Video" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete video" onclick="deleteConfirmCode(this)" data-code="' + row.id + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
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
        DeleteVideo(value);
    });
}

function DeleteVideo(id) {
    var mediaLinkEntityModel = {
        Id: id
    }
    $('.preloader').css('display', 'block');

    $.ajax({
        type: "POST",
        url: "/SocialMedia/DeleteVideo",
        data: mediaLinkEntityModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Video deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/SocialMedia/EducationalVideos'; }, 2000);
            }
            $('.preloader').css('display', 'none');


        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}