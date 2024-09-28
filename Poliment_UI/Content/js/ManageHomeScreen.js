/// <reference path="E:\WorkSpace\Poliment\Poliment_UI\Scripts/jquery-3.3.1.min.js" />
function Main() {
    SaveHeading();
    SaveShortDescription();
    SaveLongDescription();
    SaveAdminName();
    SaveFirstAddress();
    SaveSecondAddress();
    SaveVotingHeading();
}

function SaveHeading() {
    $('#btnUpdateHeading').click(function () {
        var heading = $("#txtHeading").val();
        if (heading == undefined || heading == null || heading == "") {
            alert("Please enter heading");
            $("#txtHeading").focus();
            return false;
        }

        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = '';
        sdata.Heading = heading;
        sdata.UpdateName = '';
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = 0;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#txtHeading").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });

    });
}

function SaveShortDescription() {
    $('#btnUpdateShortDescription').click(function () {
        var shortDescription = $("#txtShortDescription").val();
        if (shortDescription == undefined || shortDescription == null || shortDescription == "") {
            alert("Please enter short description");
            $("#txtShortDescription").focus();
            return false;
        }

        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = shortDescription;
        sdata.Heading = '';
        sdata.UpdateName = '';
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = 0;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#txtShortDescription").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}

function SaveLongDescription() {
    $('#btnUpdateLongDescription').click(function () {
        var longDescription = $("#txtLongDescription").val();
        if (longDescription == undefined || longDescription == null || longDescription == "") {
            alert("Please enter long description");
            $("#txtLongDescription").focus();
            return false;
        }

        var sdata = {};
        sdata.LongDescription = longDescription;
        sdata.ShortDescription = '';
        sdata.Heading = '';
        sdata.UpdateName = '';
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = 0;

        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#txtLongDescription").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}

function SaveAdminName() {
    $('#btnUpdateName').click(function () {
        var adminName = $("#adminName").val();
        if (adminName == undefined || adminName == null || adminName == "") {
            alert("Please enter name");
            $("#adminName").focus();
            return false;
        }
        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = '';
        sdata.Heading = '';
        sdata.UpdateName = adminName;
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = 0;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#adminName").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}

function SaveFirstAddress() {
    $('#btnUpdateFirstAddress').click(function () {
        var firstAddress = $("#firstAddress").val();
        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = '';
        sdata.Heading = '';
        sdata.UpdateName = '';
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = firstAddress;
        sdata.VotingSurveyHeading = 0;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#firstAddress").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}

function SaveSecondAddress() {
    $('#btnUpdateSecondAddress').click(function () {
        var secondAddress = $("#secondAddress").val();

        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = '';
        sdata.Heading = '';
        sdata.UpdateName = '';
        sdata.AddSecondAddress = secondAddress;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = 0;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#secondAddress").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}

function SaveVotingHeading() {
    $('#btnUpdateVotingHeading').click(function () {
        var votingHeading = $("#votingHeading").val();
        var sdata = {};
        sdata.LongDescription = '';
        sdata.ShortDescription = '';
        sdata.Heading = '';
        sdata.UpdateName = '';
        sdata.AddSecondAddress = 0;
        sdata.AddFirstAddress = 0;
        sdata.VotingSurveyHeading = votingHeading;
        $.ajax({
            url: '/Admin/SaveManageHomeScreen',
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(sdata),
            success: function (myresult) {
                if (myresult == "error") {
                    alert("An error occured");
                }
                else {
                    $("#votingHeading").val('');
                    alert("Data saved successfully");
                }

            },
            error: function () {
                alert("An error occured");
            }
        });
    });
}