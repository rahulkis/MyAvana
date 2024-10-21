$(document).ready(function () {
    $(".showHide:first").addClass('active');
    $('.showHideActive:first').addClass('active');
    $('.showHideActive:first').addClass('show');
    $("#datepicker").datepicker({
        minDate: 0
       
    });
   // $("#cnfbtn").prop("disabled", true);

});
var valsel = false;
var valtimeslot = false;
$(function () {
    $("#datepicker").on("change", function () {
        var selected = $(this).val();

        var date = new Date(selected);
        var sel = date.toString();
        var arr1 = sel.split(' ');
        $("#datetimetext").html(arr1[0] + ', ' + arr1[2] + ' ' + arr1[1]);
        $("#datetimetext1").html(arr1[0] + ', ' + arr1[2] + ' ' + arr1[1]);
        valsel = true;
        if (valtimeslot) {
            
            $("#cnfbtn").prop("disabled", false);
           
          
        }

    });
   
});
$(function () {
$(".timeslotstextselected").click(function () {
    valtimeslot = true;
    if (valsel) {
        
        $("#cnfbtn").prop("disabled", false);
      
    }
});
});
    function showHide(sender) {
        var id = $(sender).attr('href');
        $('.showHideActive').removeClass('active');
        $('.showHideActive').removeClass('show');
        $('.showHide').removeClass('active');
        $(sender).addClass('active');
        $(id).addClass('active');
        $(id).addClass('show');
    }

    $(document).ready(function () {
        $(".showHide1:first").addClass('active');
        $('.showHide1Active:first').addClass('active');
        $('.showHide1Active:first').addClass('show');
    });

    function showHide1(sender) {
        var id = $(sender).attr('href');
        $('.showHide1Active').removeClass('active');
        $('.showHide1Active').removeClass('show');
        $('.showHide1').removeClass('active');
        $(sender).addClass('active');
        $(id).addClass('active');
        $(id).addClass('show');
    }

    $('.crousalnav').owlCarousel({
        loop: false,
        center: false,
        margin: 20,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true,
                loop: false
            },
            600: {
                items: 1,
                nav: true,
                loop: false
            },
            1000: {
                items: 3,
                nav: true,
                loop: false
            }
        }
    })

    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 00,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true,
                loop: true
            },
            600: {
                items: 1,
                nav: true,
                loop: true
            },
            1000: {
                items: 1,
                nav: true,
                loop: true
            }
        }
    })

    $('.crousalslide').owlCarousel({
        loop: true,
        margin: 00,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true,
                loop: true
            },
            600: {
                items: 1,
                nav: true,
                loop: true
            },
            1000: {
                items: 1,
                nav: true,
                loop: true
            }
        }
    })

    $(document).on('show', '.accordion', function (e) {
        //$('.accordion-heading i').toggleClass(' ');
        $(e.target).prev('.accordion-heading').addClass('accordion-opened');
    });

    $(document).on('hide', '.accordion', function (e) {
        $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
        //$('.accordion-heading i').toggleClass('fa-chevron-right fa-chevron-down');
    });
    $(document).on('show', '.accordion', function (e) {
        //$('.accordion-heading i').toggleClass(' ');
        $(e.target).prev('.accordion-heading').addClass('accordion-opened');
    });

    $(document).on('hide', '.accordion', function (e) {
        $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
        //$('.accordion-heading i').toggleClass('fa-chevron-right fa-chevron-down');
    });

    $(document).ready(function () {
        $(".showHide:first").addClass('active');
        $('.showHideActive:first').addClass('active');
        $('.showHideActive:first').addClass('show');
        $('.demo').nailthumb({
            imageCustomFinder: function (el) {
                var image = $('<img />').attr('src', el.attr('href').replace('/full/', '/small/')).css('display', 'none');
                image.attr('alt', el.attr('title'));
                el.append(image);
                return image;
            },
            titleAttr: 'alt'
        });



    });
    jQuery.fn.load = function (callback) {
        $(window).on("load", function () {
            Shadowbox.init();
            $('.demo').css("display", "block");
        })

    };


    $(function () {
        $("#datepicker").datepicker({ firstDay: 1 });
    });

    function PayViaStripe() {

       // SaveCustomerDetails();

        if ($('#expireYear').val() == '0' || $('#txtCardName').val() == '' || $('#txtCardNumber').val() == '' || $('#expireMonth').val() == '0' ||
            $('#txtCvv').val() == '') {
            $('#failureMessage').text("fields cannot be empty!");
            $('.alert-danger').css("display", "block");
            $('.alert-danger').delay(2000).fadeOut();
            return false;
        }
        var checkoutrequest = {
            amount: 45,
            cardownerfirstname: $('#txtCardName').val().trim(),
            cardownerlastname: $('#txtCardName').val().trim(),
            expirationyear: $('#expireYear').val().trim(),
            cardnumber: $('#txtCardNumber').val().trim(),
            expirationmonth: $('#expireMonth').val().trim(),
            cvv2: $('#txtCvv').val().trim(),
            state: ""
        }

        $.ajax({
            type: "post",
            url: "/liveconsultation/cardpayment",
            data: checkoutrequest,
            success: function (response) {
                if (response === "1") {
                    //$('#successMessage').text("payment done successfully! you can move forward");
                    //$('.alert-success').css("display", "block");
                    //$('.alert-success').delay(2000).fadeOut();

                    SaveCustomerDetails();
                   
                }
               // $('.preloader').css('display', 'none');
            },
            error: function (response) {

            },
            complete: function () {

            }
        });
    }
    

$(document).on('keypress', ':input[type="number"]', function (e) {
    debugger
    if (isNaN(e.key)) {
        return false;
    }
});




function IsEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
}

function IsPhoneNumber(phone) {
    var regex = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (!regex.test(phone)) {
        return false;
    } else {
        return true;
    }
}

function validateUser() {

    if ($("#myFormName").val() == "") {
        //alert("Please provide your name!");
        $('#failureMessage').text("Please provide your name!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormName").focus();
        return false;
    }

    if ($("#myFormEmail").val() == "") {
        //alert("Please provide your Email!");
        $('#failureMessage').text("Please provide your Email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormName").focus();
        return false;
    }
    if (!IsEmail($("#myFormEmail").val())) {
        //alert("Please provide correct Email!");
        $('#failureMessage').text("Please provide correct Email!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormEmail").focus();
        return false;

    }
    if ($("#myFormPhone").val() == "") {
        //alert("Please provide your Contact Number!");
        $('#failureMessage').text("Please provide your Contact Number!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormPhone").focus();
        return false;
    }
    if (!IsPhoneNumber($("#myFormPhone").val())) {
        $('#failureMessage').text("Please provide correct Number!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormPhone").focus();
        return false;
    }
    if ($("#myFormtextarea").val() == "") {
        //alert("Please provide the Answer!");
        $('#failureMessage').text("Please provide the Answer!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        $("#myFormtextarea").focus();
        return false;
    }
    else {
        nextTab();
    }
}
function confirmOrderNumber() {
    SaveCustomerDetails();
}
function SaveCustomerDetails() {

        var a = $("#resultdatetimezone").html().split(",");
        var savetime = a[0];
        var timeslot = savetime.split("-");
        var t = timeslot[0];
        var p = ' PM'
        if (t.split(":")[0] >= 9 && t.split(":")[0] < 12) {
            p = ' AM';
        }
        var str = $("#datepicker").datepicker('getDate');
        var arr1 = str.toString().split(" ");
        var monthnumber = ("JanFebMarAprMayJunJulAugSepOctNovDec".indexOf(arr1[1]) / 3 + 1);
        var savedate = arr1[3] + '-' + monthnumber + '-' + arr1[2] + ' ' + t + p;
        var savemail = $("#myFormEmail").val();

        var LiveConsultationUserDetails = {
            UserEmail: savemail,
            Time: savetime,
            Date: savedate,

            TimeZone: $("#Timezonetext").html(),
            FocusAreaDescription: $("#myFormtextarea").val(),
            ContactNo: $("#myFormPhone").val(),

            IsActive: true

        }
        $.ajax({
            type: "POST",
            url: "/LiveConsultation/SaveConsultationDetails",
            data: LiveConsultationUserDetails,
            complete: function () {

                //$('.next').trigger('click');
                nextTab();
            }
        });
}

function nextTab() {

    current_fs = $(".fieldset" + i).parent();
    next_fs = current_fs.next();
    i++;

    //activate next step on progressbar using the index of next_fs

    $("#progressbar fieldset").eq($("fieldset").index(next_fs)).addClass("active");

    current_fs.addClass("inActive");
    next_fs.removeClass("inActive");
    //show the next fieldset
    next_fs.show();
    //hide the current fieldset with style
    current_fs.animate({ opacity: 0 }, {
        step: function (now, mx) {
            //as the opacity of current_fs reduces to 0 - stored in "now"
            //1. scale current_fs down to 80%
            scale = 1 - (1 - now) * 0.2;
            //2. bring next_fs from the right(50%)
            left = (now * 50) + "%";
            //3. increase opacity of next_fs to 1 as it moves in
            opacity = 1 - now;
            current_fs.css({
                'transform': 'scale(' + scale + ')',
                'position': 'absolute'
            });
            next_fs.css({ 'left': left, 'opacity': opacity });
        },
        duration: 1800,
        complete: function () {
            current_fs.hide();
            animating = false;
        },
        //this comes from the custom easing plugin
        easing: 'easeInOutBack'
    });
}

function validateDateTime() {
    if (valsel && !valtimeslot) {
        $('#failureMessage').text("Please select time!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    } else if (!valsel && valtimeslot) {
        $('#failureMessage').text("Please select date!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    } else if (!valsel && !valtimeslot) {
        $('#failureMessage').text("Please select date and time!");
        $('.alert-danger').css("display", "block");
        $('.alert-danger').delay(2000).fadeOut();
        return false;
    } else {
        nextTab();
    }

}
function checkSchedule(schDate) {
    var sdate = new Date(schDate);
    var currdate = new Date();
    if (currdate > sdate) {
        alert("This appointment is expired. Please contact to support")
        return false;
    }
    var diff = sdate - currdate;

    var diffSeconds = diff / 1000;
    var HH = Math.floor(diffSeconds / 3600);
    var MM = Math.floor(diffSeconds % 3600) / 60;

    if (HH === 0 && MM < 2 && MM > 0) {
        alert("valid to join")
    } else {
        alert("You will join before 2 min of your scheduled time.")
    }

}

function redirectTo() {
    window.location.href = '/HairProfile/CustomerHair';
}

