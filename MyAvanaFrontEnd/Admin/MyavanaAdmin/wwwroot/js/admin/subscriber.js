function onRender(data, type, row, meta) {
    
    var tr = '<a Title="Cancel Subscription" onclick="CancelSubscrption(this)" data-code="' + row.UserEmail+ '"><i class="fa fa-trash" aria-hidden="true"></i></a>';
    return tr;
}

function CancelSubscrption(event) {
    var value = $(event).attr('data-code');
  
    $('#confirmModalHeader').text('Cancel Subscription');
    $('#confirmModalText').text('Are you sure you want to end subscription?');
    $('#confirmMethod').removeAttr('onclick');
    $('#confirmMethod').text('End Subscription');
    $('#confirmModal').modal('show');
    $("#confirmMethod").prop("onclick", null).off("click");
    $("#confirmMethod").click(function () {
       
        EndSubscription(value);
    });
}

function EndSubscription(UserEmail) {
  
    var subscriberModel = {
        UserEmail: UserEmail
    }
    $('.preloader').css('display', 'block');
    $.ajax({
        type: "POST",
        url: "/Subscriber/CancelSubscription", 
        data: subscriberModel,
        success: function (response) {
           
            if (response === "1") {
                $('#successMessage').text("Subcription cancelled successfully !");
                $('.alert-success').css("display", "block");
                $('.alert-success').delay(3000).fadeOut();
                $('#confirmModal').modal('hide');
                setTimeout(function () { window.location.href = '/subscriber/subscriber'; }, 2000);
            }
            $('.preloader').css('display', 'none');
        },
        error: function (response) {

        },
        complete: function () {

        }
    });
}
