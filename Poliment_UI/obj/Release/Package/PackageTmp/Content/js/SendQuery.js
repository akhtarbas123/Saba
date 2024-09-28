
/// <reference path="E:\WorkSpace\Poliment\Poliment_UI\Scripts/jquery-3.3.1.min.js" />
function MainFunctionCall() {
    GetAreaByBlock();
    SaveGuestQuery();

}

function SaveGuestQuery() {
    $('#btnSaveGuestQuery').click(function () {

        $('#spnSuccessMessage').html('');
        var queryTypeId = $("#ddlQueryType option:selected").val();
        var queryTypeName = $("#ddlQueryType option:selected").text();
        var blockId = $("#BlockName option:selected").val();
        var blockName = $("#BlockName option:selected").text();
        var areaId = $("#AreaName option:selected").val();
        var areaName = $("#AreaName option:selected").text();
        var fullName = $("#fullName").val();
        if (queryTypeId == undefined || queryTypeId == null || queryTypeId == "" || queryTypeId == "0") {
            alert("Please select query type");
            $("#ddlQueryType").focus();
          //  $('#spnErrorMessage').html("Please select query type");
            return false;
        }
        if (fullName == undefined || fullName == null || fullName == "") {
            alert("Please enter name");
            $("#fullName").focus();
          //  $('#spnErrorMessage').html("Please enter name");
            return false;
        }
        var mobile = $("#mobileNumber").val();
        if (mobile == undefined || mobile == null || mobile == "") {
            alert("Please enter mobile number");
            $("#mobileNumber").focus();
           // $('#spnErrorMessage').html("Please enter mobile number");
            return false;
        }
        if (!MobileValidate(mobile)) {
            $('#spnErrorMessage').html("Please enter valid mobile number");
            return false;
        }
        var email = $("#Email").val();
        var pin = $("#Pin").val();
        if (pin == undefined || pin == null || pin == "") {
            alert("Please enter four digit pin");
            $("#Pin").focus();
           // $('#spnErrorMessage').html("Please enter four digit pin");
            return false;
        }
        var address = $("#Address").val();
        if (address == undefined || address == null || address == "") {
            alert("Please enter address");
            $("#Address").focus();
           // $('#spnErrorMessage').html("Please enter address");
            return false;
        }
        var queryHeading = $("#queryHeading").val();
        if (queryHeading == undefined || queryHeading == null || queryHeading == "") {
            alert("Please enter query heading");
            $("#queryHeading").focus();
          //  $('#spnErrorMessage').html("Please enter query heading");
            return false;
        }
        var queryDescription = $("#queryDescription").val();
        if (queryDescription == undefined || queryDescription == null || queryDescription == "") {
            alert("Please enter query description");
            $("#queryDescription").focus();
          //  $('#spnErrorMessage').html("Please enter query description");
            return false;
        }
        $('#spnErrorMessage').html('');

        var sdata = {};
        sdata.QueryTypeId = queryTypeId;
        sdata.QueryTypeValue = queryTypeName;
        sdata.BlockId = blockId;
        sdata.BlockName = blockName;
        sdata.AreaId = areaId;
        sdata.AreaName = areaName;
        sdata.Name = fullName;
        sdata.Mobile = mobile;
        sdata.Email = email;
        sdata.FourDigitPin = pin;
        sdata.Address = address;
        sdata.QueryHeading = queryHeading;
        sdata.QueryDescription = queryDescription;

        $.ajax({
            url: '/Home/SaveGuestQuery',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "0") {
                    alert("An error occured");
                }
                else {
                    ClearGuestQuery();
                    alert("Your query subitted successfully.Your reference number is : " + myresult + "");
                    $('#spnSuccessMessage').html("Your query subitted successfully.Your reference number is : " + myresult + "");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });

    });

}

function ClearGuestQuery() {
    $("#ddlQueryType").val('0');
    $("#fullName").val('');
    $("#mobileNumber").val('');
    $("#Email").val('');
    $("#Pin").val('');
    $("#Address").val('');
    $("#queryHeading").val('');
    $("#queryDescription").val('');
}

function GetAreaByBlock() {

    $('#BlockName').change(function () {
        var blockId = $('#BlockName option:selected').val();
        var blockName = $('#BlockName option:selected').text();
        if (blockId != undefined && blockId != null && blockId != "") {
            var sdata = {};
            sdata.blockName = blockName;
            $.ajax({
                url: '/Home/GetArea',
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
                url: '/Home/GetArea',
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
