$('#btnImageClick').click(function () {
    $('#mobileHelpImage').click();
});

$('#mobileHelpImage').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        $('#txtImageName').text('');
        $('#txtImageName').text(filename);
        $('#removeImageButton').show();
    }
});

$('#btnVideothumbnailClick').click(function () {
    $('#mobileHelpVideothumbnail').click();
});

$('#mobileHelpVideothumbnail').change(function (event) {
    var filename = event.target.files[0].name;
    if (filename != null || filename != undefined) {
        $('#txtVideothumbnailName').text('');
        $('#txtVideothumbnailName').text(filename);
        $('#removeVideoThumbnailButton').show();
    }
});

var fileModel = new FormData();
function SaveMobileHelpFAQ() {
    if ($('#MobileHelpTopicId').val() == "") {
        $('#failureMessage').text("Please select topic!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#txtTitle').val().trim() == "" || $('#txtDescription').val().trim() == "") {
        $('#failureMessage').text("Title and Description can't be empty!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#txtVideothumbnailName').text().trim() != "" && $('#txtVideo').val().trim() == "") {
        $('#failureMessage').text("Please enter video URL for thumbnail image!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(3000).fadeOut();
        return false;
    }
    if ($('#txtVideo').val().trim() != "") {
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
    }

    var file = $('#mobileHelpImage').get(0).files[0];
    var videothumbnailfile = $('#mobileHelpVideothumbnail').get(0).files[0];
    fileModel.append('MobileHelpFAQId', $('#MobileFaqId').val());
    fileModel.append('Title', $("#txtTitle").val().trim());
    fileModel.append('Description', $("#txtDescription").val().trim());
    fileModel.append('Videolink', $('#txtVideo').val());
    fileModel.append('MobileHelpTopicId', $('#MobileHelpTopicId').val());
    
    if (file != null && file != undefined) {
        fileModel.append('File', file);
    } else {
        if ($('#removeImageButton').data('remove') === true && file == undefined) {
            fileModel.append('ImageLink', '');
        }
        if ($("#image").val() !== null && $("#image").val() !== undefined)
            fileModel.append('ImageLink', $("#image").val());
    }

    if (videothumbnailfile != null || videothumbnailfile != undefined) {
        fileModel.append('imageFile', videothumbnailfile);
    } else {
        if ($('#removeVideoThumbnailButton').data('remove') === true) {
            fileModel.append('VideoThumbnail', '');
        }
        if ($("#videothumbnail").val() != null && $("#videothumbnail").val() !== undefined)
            fileModel.append('VideoThumbnail', $("#videothumbnail").val());
    }

    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/MobileHelp/CreateMobileHelpFAQ",
        data: fileModel,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response == "1") {
                $('#successMessage').text("Mobile Help FAQ saved/updated successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                setTimeout(function () { window.location.href = '/MobileHelp/MobileHelpFAQList'; }, 3000);
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

function removeImage() {
    $('#mobileHelpImage').val('');
    $('#txtImageName').text('');
    $('#removeImageButton').data('remove', true);
    $('#removeImageButton').hide();
}

function removeVideoThumbnail() {
    $('#mobileHelpVideothumbnail').val('');
    $('#txtVideothumbnailName').text('');
    $('#removeVideoThumbnailButton').data('remove', true);
    $('#removeVideoThumbnailButton').hide();
}

function RenderVideoHtml(data, type, row, meta) {
    if (row.Videolink != null) {
        var videoSource = row.Videolink.includes('youtube')
            ? row.Videolink.replace('watch?v=', 'embed/')
            : row.Videolink;

        var thumbnailImage = row.VideoThumbnail || '';
        var containerId = 'videoContainer_' + meta.row;

        var tr = '<div style="position: relative; width: 320px; height: 150px;">';

        // Check if there is a thumbnail image
        if (thumbnailImage) {
            tr += '<a href="' + videoSource + '" target="_blank">';
            tr += '<img src="' + thumbnailImage + '" style="width: 100%; height: 100%; cursor: pointer;" alt="Video Thumbnail">';
            tr += '</a>';
        } else {
            // If no thumbnail, use a default thumbnail for YouTube videos
            if (videoSource.includes('youtube')) {
                var defaultThumbnail = 'https://i.ytimg.com/vi/' + getYoutubeVideoId(row.Videolink) + '/mqdefault.jpg';
                tr += '<a href="' + videoSource + '" target="_blank">';
                tr += '<img src="' + defaultThumbnail + '" style="width: 100%; height: 100%; cursor: pointer;" alt="Default Thumbnail">';
                tr += '</a>';
            } else {
                // For non-YouTube videos, directly link to the video in a new tab
                tr += '<a href="' + videoSource + '" target="_blank">';
                tr += '<div style="width: 100%; height: 100%; max-width: 320px; max-height: 150px; overflow: hidden;">';
                tr += '<blockquote class="instagram-media" data-instgrm-permalink="' + row.Videolink + '" data-instgrm-version="13" style="max-width: 100%;"></blockquote>';
                tr += '</div>';
                tr += '<script async src="//www.instagram.com/embed.js"></script>';
                tr += '</a>';
            }
        }

        tr += '</div>';

        return tr;
    } else {
        return "";
    }
}

// Function to extract YouTube video ID from the URL
function getYoutubeVideoId(url) {
    var match = url.match(/[?&]v=([^&]+)/);
    return match ? match[1] : null;
}

function playVideo(videoSource, containerId, youtubeLink) {
    // Open the video in a new tab
    if (youtubeLink && youtubeLink.includes('youtube')) {
        window.open(videoSource, '_blank');
        return;
    }

    var videoContainer = document.getElementById(containerId);

    // Remove existing content from the container
    videoContainer.innerHTML = '';

    // Display Instagram video
    var instagramVideoContainer = document.createElement('div');
    instagramVideoContainer.style.maxWidth = '100%';
    instagramVideoContainer.innerHTML = '<blockquote class="instagram-media" data-instgrm-permalink="' + videoSource + '" data-instgrm-version="13" style="max-width: 100%; width: 320px; height: 150px;"></blockquote>';
    videoContainer.appendChild(instagramVideoContainer);

    function loadInstagramScript(callback) {
        if (window.instgrm) {
            callback();
        } else {
            var script = document.createElement('script');
            script.async = true;
            script.defer = true;
            script.src = 'https://www.instagram.com/embed.js';
            script.onload = callback;
            document.head.appendChild(script);
        }
    }

    loadInstagramScript(function () {
        window.instgrm.Embeds.process();
    });
}




function onRenderImage(data, type, row, meta) {
    if (row.ImageLink != null && row.ImageLink != 'undefined') {
        return "<a href='" + row.ImageLink + "' target='_blank'><img src='" + row.ImageLink + "' style='width:100px;height:140px;' /></a>";
    } else {
        return "";
    }
}

function RenderHtml(data, type, row, meta) {
    var tr = '<a href="/MobileHelp/CreateMobileHelpFAQ/' + row.MobileHelpFAQId + '" Title="Edit FAQ" ><i class="fa fa-pencil" aria-hidden="true"></i></a>';
    tr += '<a Title="Delete FAQ" onclick="deleteConfirmFAQ(this)" data-code="' + row.MobileHelpFAQId + '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function RenderDescription(data, type, row, meta) {
    var maxLength = 500;
    var truncatedData = data.substring(0, maxLength);

    if (data.length > maxLength) {
        truncatedData += ' <a href="javascript:void(0);" class="read-more" data-full-description="' + data + '" title="Read More">...Read More</a>';
    }

    return truncatedData;
}

document.addEventListener('click', function (event) {
    var target = event.target;
    if (target.classList.contains('read-more') || target.classList.contains('read-less')) {
        var fullDescription = target.getAttribute('data-full-description');
        var previousSibling = target.previousSibling;
        if (previousSibling) {
            if (target.classList.contains('read-more')) {
                previousSibling.textContent = fullDescription;
                target.textContent = ' Read Less';
                target.classList.remove('read-more');
                target.classList.add('read-less');
            } else {
                previousSibling.textContent = fullDescription.substring(0, 500); // Truncate to initial length
                target.textContent = '...Read More';
                target.classList.remove('read-less');
                target.classList.add('read-more');
            }
        }
    }
});


function deleteConfirmFAQ(event) {
    var value = $(event).attr('data-code');

    $('#confirmModalHeader').text('Delete');
    $('#confirmModalText').text('Are you sure you want to delete?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('Delete');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
        DeleteMobileHelpFaq(value);
    });
}

function DeleteMobileHelpFaq(Id) {
    var mobileHelpFaqModel = {
        MobileHelpFAQId: Id
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/MobileHelp/DeleteMobileHelpFaq",
        data: mobileHelpFaqModel,
        success: function (response) {
            if (response === "1") {
                $('#successMessage').text("Mobile Help FAQ deleted successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/MobileHelp/MobileHelpFAQList'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
