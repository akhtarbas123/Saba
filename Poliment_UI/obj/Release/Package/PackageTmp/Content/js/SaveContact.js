function SaveContact()
{
    $('#btnSaveContact').click(function () {

        var fullName = $("#fullName").val();
        if (fullName == undefined || fullName == null || fullName == "") {
            alert("Please enter name");
            $("#fullName").focus();
            return false;
        }
        var mobile = $("#mobileNumber").val();
        if (mobile == undefined || mobile == null || mobile == "") {
            alert("Please enter mobile number");
            $("#mobileNumber").focus();
            return false;
        }
        if (!MobileValidate(mobile)) {
            alert("Please enter valid mobile number");
            return false;
        }
        var email = $("#Email").val();
        if (email != undefined && email != null && email != "") {
            if (!IsValidEmail(email)) {
                alert("Please enter valid email id");
                return false;
            }
        }

        var message = $("#message").val();
        if (message == undefined || message == null || message == "") {
            alert("Please enter address");
            $("#message").focus();
            return false;
        }
        var sdata = {};
        sdata.FullName = fullName;
        sdata.Mobie = mobile;
        sdata.Email = email;
        sdata.Message = message;
        $.ajax({
            url: '/Home/Contact',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#fullName").val('');
                    $("#mobileNumber").val('');
                    $("#Email").val('');
                    $("#message").val('');
                    alert("Your message subitted successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });

    });
}
function MobileValidate(mobile) {
    var pattern = /^\d{10}$/;
    if (mobile != null || mobile != "") {
        if (!pattern.test(mobile)) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }

}
function IsValidEmail(str) {

    if (str.length > 3) {
        emailRe = /^\w+([\.+-]?\w+)*@\w+([\.-]?\w+)*\.(\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum|jobs|mobi|asia|tel|travel))$/
        if (!emailRe.test(str)) {
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return false;
    }
}