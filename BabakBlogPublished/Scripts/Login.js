function getPasswordHash(passwordElement, nonceElement, hashElement)
{
    var password = $("#" + passwordElement + "").val();
    var nonce = $("#" + nonceElement + "").val();
    $("#" + hashElement).attr('value', $.sha256(password + nonce));
    $("#" + passwordElement).attr('value', '');
}