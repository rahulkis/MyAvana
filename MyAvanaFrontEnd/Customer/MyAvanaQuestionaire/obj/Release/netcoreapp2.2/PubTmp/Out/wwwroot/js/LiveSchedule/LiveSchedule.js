$(document).on('keypress', ':input[type="number"]', function (e) {
    debugger
    if (isNaN(e.key)) {
        return false;
    }
});

 


function IsEmail(email) {
    debugger
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
        return false;
    } else {
        return true;
    }
}

function validate() {
    debugger

    if ($("#myFormName").val() == "") {
        alert("Please provide your name!");
        $("#myFormName").focus();
        return false;
    }

    //if ($("#myFormEmail").val() == "") {
    //    alert("Please provide your Email!");
    //    $("#myFormName").focus();
    //    return false;
    //}
    //if (!IsEmail($("#myFormEmail").val())) {
    //    alert("Please provide correct Email!");
    //    $("#myFormEmail").focus();
    //    return false;

    //}
    if ($("#myFormPhone").val() == "") {
        alert("Please provide your Contact Number!");
        $("#myFormPhone").focus();
        return false;
    }
    if ($("#myFormtextarea").val() == "") {
        alert("Please provide the Answer!");
        $("#myFormtextarea").focus();
        return false;
    }
    //if ($("#myFormText").val() == "") {
    //    alert("Please provide the Number!");
    //    $("#myFormText").focus();
    //    return false;
    //}
    else {
        return (true);


    }


}
function SaveCustomerDetails() {
    if (validate() == true) {
       
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
           
            TimeZone : $("#Timezonetext").html(),
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
    else
        return false;

}