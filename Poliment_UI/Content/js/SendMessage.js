
function Main() {
    SaveValidate();
}
function SaveValidate() {
    $('#btnSendMessage').click(function () {
        var subject = $("#txtSubject").val();
        if (subject == null || subject == "") {
            alert("Please enter subject");
            $("#txtSubject").focus();
            return false;
        }
        var message = $("#txtMessagebody").val();
        if (message == null || message == "") {
            alert("Please enter message");
            $("#txtMessagebody").focus();
            return false;
        }
        var postedFile = $("#postedFile").val();
        if (postedFile != undefined && postedFile != null && postedFile != "") {
            var allowedFormat = "pdf,doc,docx,xls,xlsx,ppt,pptx,jpeg,jpg,png,gif,tif,psd";
            var postedFileSplit = postedFile.split('.');
            var fileType = postedFileSplit[1];
            if (allowedFormat.indexOf(fileType) != -1)
            {

            }
            else
            {
                alert("This file format is not allowed. Allowed file format type is : "+allowedFormat+" ");
                return false;
            }
        }
    });
}
