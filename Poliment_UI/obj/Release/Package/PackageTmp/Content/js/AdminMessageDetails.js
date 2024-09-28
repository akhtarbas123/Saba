
function Main() {
    SaveValidate();
    ConfirmDeleteMessage();
}
function SaveValidate() {
    $('#RowReplyMessage').hide();
    $('#RowReplyAttachment').hide();
    $('#RowSendMessage').hide();
    $('#ReplyMessage').click(function () {
        $('#RowReplyMessage').show();
        $('#RowReplyAttachment').show();
        $('#RowSendMessage').show();
    });
   
    $('#btnSendMessage').click(function () {
        var subject = $("#txtSubject").val();
        if (subject == null || subject == "") {
            alert("Please enter subject");
            return false;
        }
        var subject = $("#txtMessagebody").val();
        if (subject == null || subject == "") {
            alert("Please enter message");
            return false;
        }
        var replyMessagebody = $("#txtReplyMessagebody").val();
        if (replyMessagebody == null || replyMessagebody == "") {
            alert("Please enter reply message");
            return false;
        }
    });
}
function ConfirmDeleteMessage() {
    $('.clsDeleteMessage').click(function () {
        var result = confirm("Are you sure want to delete this message");
        if (result == false) {
            return false;
        }
    });
}