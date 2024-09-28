/// <reference path="E:\WorkSpace\Poliment\Poliment_UI\Scripts/jquery-3.3.1.min.js" />
function MainFunctionCall() {
    GetAreaByBlock();
    Save();
    CheckMobileOnChange();
}

function GetAreaByBlock() {

    $('#BlockName').change(function () {
        var blockId = $('#BlockName option:selected').val();
        var blockName = $('#BlockName option:selected').text();
        if (blockId != undefined && blockId != null && blockId != "") {
            var sdata = {};
            sdata.blockName = blockName;
            sdata.blockId = blockId;
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
            sdata.blockId = blockId;
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
        var firstName = $("#Name").val();
        if (firstName == undefined || firstName == null || firstName == "") {

            $('#spnErrorMessage').html("Please enter name");
            return false;
        }
        var mobile = $("#Mobile").val();
        if (mobile == undefined || mobile == null || mobile == "") {
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
        var gender = $("input[name='Gender']:checked").val();
        var voterId = $("#VoterId").val();
        var address = $("#Address").val();
        var pinCode = $("#PinCode").val();

        $('#spnErrorMessage').html('');
        var sdata = {};
        sdata.Id = userId;
        sdata.BlockId = blockId;
        sdata.BlockName = blockName;
        sdata.AreaId = areaId;
        sdata.AreaName = areaName;
        sdata.Name = firstName;
        sdata.VoterId = voterId;
        sdata.Address = address;
        sdata.PinCode = pinCode;
        sdata.Mobile = mobile;
        sdata.Gender = gender;
        $.ajax({
            url: '/User/EnterVoterDetails',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (result) {
                if (result != undefined) {
                    if (result == 'success') {
                        alert("Saved successfully");
                        ClearUser();
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
    $("#Name").val('');
    $("#VoterId").val('');
    $("#Address").val('');
    $("#PinCode").val('');
    $("#Mobile").val('');
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

function CheckMobileExist(mobile) {
    var mobileExistResult = '';
    var sdata = {};
    sdata.mobileValue = mobile;
    $.ajax({
        url: '/User/CheckVotersMobile',
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
