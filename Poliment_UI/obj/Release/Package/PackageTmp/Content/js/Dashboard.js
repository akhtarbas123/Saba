/// <reference path="E:\WorkSpace\Poliment\Poliment_UI\Scripts/jquery-3.3.1.min.js" />
function MainFunctionCall() {
    GetAreaByBlock();
    Save();
    CheckUserNameOnChange();
    CheckMobileOnChange();
    CheckEmailOnChange();

}
function ConfirmDeleteUser() {
    $('.clsDelete').click(function () {
        var result = confirm("Are you sure want to delete this user");
        if (result == false) {
            return false;
        }
    });
}

function GetAreaByBlock() {

    $('#BlockName').change(function () {
        var blockId = $('#BlockName option:selected').val();
        var blockName = $('#BlockName option:selected').text();
        if (blockId != undefined && blockId != null && blockId != "") {
            var sdata = {};
            sdata.blockName = blockName;
            $.ajax({
                url: '/Admin/GetArea',
                type: "POST",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(sdata),
                success: function (result) {
                    if (result != undefined) {
                        var totalLength = result.length;
                        $('#AreaName').html('');
                        //    $('#ddlArea').append("<option value=0>Select</option>");
                        for (var i = 0; i < totalLength; i++) {
                            $('#AreaName').append("<option value='" + result[i].Id + "'>" + result[i].AreaName + "</option>");
                        }
                    }

                },
                error: function () {
                    alert("An error occured");
                }
            });
        }
        else {
            return false;
        }
    });

}

function GetAreaByBlockForSearch() {

    $('#BlockName').change(function () {
        var blockId = $('#BlockName option:selected').val();
        var blockName = $('#BlockName option:selected').text();
        if (blockId != undefined && blockId != null && blockId != "") {
            var sdata = {};
            sdata.blockName = blockName;
            $.ajax({
                url: '/Admin/GetArea',
                type: "POST",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(sdata),
                success: function (result) {
                    if (result != undefined) {
                        var totalLength = result.length;
                        $('#AreaName').html('');
                        $('#AreaName').append("<option value=0>Select</option>");
                        for (var i = 0; i < totalLength; i++) {
                            $('#AreaName').append("<option value='" + result[i].Id + "'>" + result[i].AreaName + "</option>");
                        }
                    }

                },
                error: function () {
                    alert("An error occured");
                }
            });
        }
        else {
            return false;
        }
    });

}

function Save() {
    $('#btnSave').click(function () {
        var userId = $("#Id").val();
        var blockId = $("#BlockName option:selected").val();
        var blockName = $("#BlockName option:selected").text();
        var areaId = $("#AreaName option:selected").val();
        var areaName = $("#AreaName option:selected").text();
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
        var userName = $("#UserName").val();
        var mobile = $("#Mobile").val();
        var email = $("#Email").val();
        var oldUserName = $("#hdnUserName").val();
        var oldMobile = $("#hdnMobile").val();
        var oldEmail = $("#hdnEmail").val();

        if (userId != undefined && userId != null && userId != "") {
            if (userName == undefined || userName == null || userName == "") {
                $('#spnErrorMessage').html("Please enter user name");
                return false;
            }
            if (oldUserName != userName) {
                var userNameAvailable = CheckUserNameExist(userName);
                if (userNameAvailable == 'Exist' || userNameAvailable == 'Error') {
                    $('#spnErrorMessage').html("User name already exist.Please enter another user name");
                    return false;
                }
            }
            if (oldMobile != mobile) {
                if (mobile == null || mobile == "") {
                    $('#spnErrorMessage').html("Please enter mobile number");
                    return false;
                }
                if (!MobileValidate(mobile)) {
                    $('#spnErrorMessage').html("Please enter valid mobile number");
                    return false;
                }
                var mobileAvailable = CheckMobileExist(mobile);
                if (mobileAvailable == 'Exist' || mobileAvailable == 'Error') {
                    $('#spnErrorMessage').html("Mobile number already exist.Please enter another mobile number");
                    return false;
                }
            }
            if (oldEmail != email) {
                if (email == null || email == "") {
                    $('#spnErrorMessage').html("Please enter email id");
                    return false;
                }
                if (!IsValidEmail(email)) {
                    $('#spnErrorMessage').html("Please enter valid email id");
                    return false;
                }

                var emailAvailable = CheckEmailExist(email);
                if (emailAvailable == 'Exist' || emailAvailable == 'Error') {
                    $('#spnErrorMessage').html("Email id already exist.Please enter another email id");
                    return false;
                }
            }
        }
        else
        {
            if (userName == undefined || userName == null || userName == "") {
                $('#spnErrorMessage').html("Please enter user name");
                return false;
            }
            var userNameAvailable = CheckUserNameExist(userName);
            if (userNameAvailable == 'Exist' || userNameAvailable == 'Error') {
                $('#spnErrorMessage').html("User name already exist.Please enter another user name");
                return false;
            }


            if (mobile == null || mobile == "") {
                $('#spnErrorMessage').html("Please enter mobile number");
                return false;
            }
            if (!MobileValidate(mobile)) {
                $('#spnErrorMessage').html("Please enter valid mobile number");
                return false;
            }
            var mobileAvailable = CheckMobileExist(mobile);
            if (mobileAvailable == 'Exist' || mobileAvailable == 'Error') {
                $('#spnErrorMessage').html("Mobile number already exist.Please enter another mobile number");
                return false;
            }

            if (email == null || email == "") {
                $('#spnErrorMessage').html("Please enter email id");
                return false;
            }
            if (!IsValidEmail(email)) {
                $('#spnErrorMessage').html("Please enter valid email id");
                return false;
            }

            var emailAvailable = CheckEmailExist(email);
            if (emailAvailable == 'Exist' || emailAvailable == 'Error') {
                $('#spnErrorMessage').html("Email id already exist.Please enter another email id");
                return false;
            }
        }

        var passWord = $("#Password").val();
        if (passWord == null || passWord == "") {
            $('#spnErrorMessage').html("Please enter password");
            return false;
        }
        var dob = $("#DOB").val();
        if (dob == null || dob == "") {
            $('#spnErrorMessage').html("Please select date of birth");
            return false;
        }
        var gender = $("input[name='Gender']:checked").val();
        var designation = $("#Designation").val();
        var politicalParty = $("#PoliticalParty").val();
        var imageNamevalue = $('#CreateUserFile').val();
        if (window.FormData !== undefined) {

            var fileUpload = $("#CreateUserFile").get(0);
            var files = fileUpload.files;
            // Create FormData object
            var fileData = new FormData();
            // Looping over all files and add it to FormData object
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
        } else {
            alert("FormData is not supported.");
        }
        var postedFile = $("#CreateUserFile").val();
        if (postedFile != undefined && postedFile != null && postedFile != "") {
            var allowedFormat = "jpeg,jpg,png,gif";
            var postedFileSplit = postedFile.split('.');
            var fileType = postedFileSplit[1];
            if (allowedFormat.indexOf(fileType) != -1) {

            }
            else {
                alert("This file format is not allowed. Allowed file format type is : " + allowedFormat + " ");
                return false;
            }
        }

        $('#spnErrorMessage').html('');
        var sdata = {};
        sdata.Id = userId;
        sdata.BlockId = blockId;
        sdata.BlockName = blockName;
        sdata.AreaId = areaId;
        sdata.AreaName = areaName;
        sdata.FirstName = firstName;
        sdata.LastName = lastName;
        sdata.UserName = userName;
        sdata.PassWord = passWord;
        sdata.Mobile = mobile;
        sdata.Email = email;
        sdata.DOB = dob;
        sdata.Gender = gender;
        sdata.Designation = designation;
        sdata.PoliticalParty = politicalParty;
        $.ajax({
            url: '/Admin/CreateUser',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (result) {
                if (result != undefined) {
                    if (parseInt(result) > 0) {

                        if (imageNamevalue == undefined || imageNamevalue == null || imageNamevalue == "") {
                            alert("User Saved Successfully");
                            ClearUser();
                        }
                        else {
                            // Adding one more key to FormData object
                            fileData.append('userId', result);
                            $.ajax({
                                url: '/Admin/UploadFiles',
                                type: "POST",
                                contentType: false, // Not to set any content header
                                processData: false, // Not to process data
                                data: fileData,
                                success: function (res) {
                                    if (res != undefined) {
                                        if (res == 'success') {
                                            alert("User Saved Successfully");
                                            ClearUser();
                                        }
                                    }
                                },
                                error: function (err) {
                                    alert(err.statusText);
                                }
                            });
                        }


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
    $("#DOB").val('');
    $("#Designation").val('');
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

function CheckUserNameOnChange() {
    $('#UserName').change(function () {
        var userName = $("#UserName").val();
        if (userName == undefined || userName == null || userName == "") {
            alert("Please enter user name");
            return false;
        }
        var userNameAvailable = CheckUserNameExist(userName);
        if (userNameAvailable == 'Exist' || userNameAvailable == 'Error') {
            alert("User name already exist.Please enter another user name");
        }
    });
}

function CheckMobileOnChange() {
    $('#Mobile').change(function () {
        var mobile = $("#Mobile").val();
        if (mobile == undefined || mobile == null || mobile == "") {
            alert("Please enter mobile");
            return false;
        }
        var mobileAvailable = CheckMobileExist(mobile);
        if (mobileAvailable == 'Exist' || mobileAvailable == 'Error') {
            alert("Mobile number already exist.Please enter another mobile number");
        }
    });
}

function CheckEmailOnChange() {
    $('#Email').change(function () {
        var email = $("#Email").val();
        if (email == undefined || email == null || email == "") {
            alert("Please enter email");
            return false;
        }
        var emailAvailable = CheckEmailExist(email);
        if (emailAvailable == 'Exist' || emailAvailable == 'Error') {
            alert("Email id already exist.Please enter another email id");
        }
    });
}

function CheckUserNameExist(userName) {
    var userExistResult = '';
    var sdata = {};
    sdata.userNameValue = userName;
    $.ajax({
        url: '/Admin/CheckUserNameExist',
        type: "POST",
        dataType: 'json',
        async: false,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(sdata),
        success: function (userNameResult) {
            if (userNameResult != undefined) {
                userExistResult = userNameResult;
            }
        },
        error: function () {
            alert("An error occured");
        }
    });
    return userExistResult;
}

function CheckMobileExist(mobile) {
    var mobileExistResult = '';
    var sdata = {};
    sdata.mobileValue = mobile;
    $.ajax({
        url: '/Admin/CheckMobileExist',
        type: "POST",
        dataType: 'json',
        async: false,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(sdata),
        success: function (mobileResult) {
            if (mobileResult != undefined) {
                mobileExistResult = mobileResult;
            }
        },
        error: function () {
            alert("An error occured");
        }
    });
    return mobileExistResult;
}

function CheckEmailExist(email) {
    var emailExistResult = '';
    var sdata = {};
    sdata.emailValue = email;
    $.ajax({
        url: '/Admin/CheckEmailExist',
        type: "POST",
        dataType: 'json',
        async: false,
        cache: false,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(sdata),
        success: function (emailResult) {
            if (emailResult != undefined) {
                emailExistResult = emailResult;
            }
        },
        error: function () {
            alert("An error occured");
        }
    });
    return emailExistResult;
}