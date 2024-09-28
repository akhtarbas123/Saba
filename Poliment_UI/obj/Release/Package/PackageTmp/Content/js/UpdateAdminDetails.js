/// <reference path="E:\WorkSpace\Poliment\Poliment_UI\Scripts/jquery-3.3.1.min.js" />
function MainFunctionCall() {
    Save();
}

function Save() {
    $('#btnSave').click(function () {
        var userId = $("#Id").val();
        var firstName = $("#FirstName").val();
        if (firstName == null || firstName == "") {

            $('#spnErrorMessage').html("Please enter first name");
            return false;
        }
        var lastName = $("#LastName").val();
        if (lastName == null || lastName == "") {
            $('#spnErrorMessage').html("Please enter last name");
            return false;
        }
        var passWord = $("#Password").val();
        if (passWord == null || passWord == "") {
            $('#spnErrorMessage').html("Please enter password");
            return false;
        }
        var mobile = $("#Mobile").val();
        if (mobile == null || mobile == "") {
            $('#spnErrorMessage').html("Please enter mobile number");
            return false;
        }
        if (!MobileValidate(mobile)) {
            $('#spnErrorMessage').html("Please enter valid mobile number");
            return false;
        }
        var email = $("#Email").val();
        if (email == null || email == "") {
            $('#spnErrorMessage').html("Please enter email id");
            return false;
        }
        if (!IsValidEmail(email)) {
            $('#spnErrorMessage').html("Please enter valid email id");
            return false;
        }
        var dob = $("#DateOfBirth").val();
        if (dob == null || dob == "") {
            $('#spnErrorMessage').html("Please select date of birth");
            return false;
        }
        var gender = $("input[name='Gender']:checked").val();
        var politicalParty = $("#PoliticalParty").val();

        $('#spnErrorMessage').html('');
        var sdata = {};
        sdata.Id = userId;
        sdata.FirstName = firstName;
        sdata.LastName = lastName;       
        sdata.PassWord = passWord;
        sdata.Mobile = mobile;
        sdata.Email = email;
        sdata.DOB = dob;
        sdata.Gender = gender;
        sdata.PoliticalParty = politicalParty;
        $.ajax({
            url: '/Admin/UpdateAdminDetails',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (result) {
                if (result != undefined) {
                    if (result == 'success') {
                        alert("Admin details updated successfully");
                    }

                }
            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}
function ClearUser() {
    $("#FirstName").val('');
    $("#LastName").val('');
    $("#UserName").val('');
    $("#Password").val('');
    $("#Mobile").val('');
    $("#Email").val('');
    $("#DateOfBirth").val('');
    $("#PoliticalParty").val('');
    $('#CreateUserFile').val('');
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

