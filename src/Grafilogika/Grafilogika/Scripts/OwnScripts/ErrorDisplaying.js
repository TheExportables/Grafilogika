function DisplayErrorMessage(message) {
    if (message instanceof Array) {
        for (var i in message)
            DisplayErrorMessage(message[i]);
        return;
    }
    jQuery(".validationMessageContainer").prepend('<div class="row">' +
                                            '<div class="col-md-1"></div>' +
                                            '<div class="col-md-9 alert alert-danger alert-dismissible" role="alert" style="display: none">' +
                                                '<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +
                                                '<strong>Warning!</strong> ' + message +
                                            '</div>' +
                                        '</div>'
                                    );
    jQuery(".alert").show();
}

function changePassword() {
    var oldPw = $("#oldpw").val();
    var newPw = $("#newpw").val();
    var cPw = $("#cpw").val();
    if (newPw != cPw)
    {
        $.fancybox({
            content: "A megadott jelszavak nem egyeznek.",
        });
        return;
    }
    $.ajax({
        url: '/Home/ChangePassword',
        data: { 'OldPassword': oldPw, 'NewPassword': newPw, 'ConfirmPassword': cPw },
        type: 'POST',
        success: function (result) {
            $.fancybox({
                content: result,
            });
            if (result.indexOf("sikeresen") > -1)
            {
                $("#oldpw").val("");
                $("#newpw").val("");
                $("#cpw").val("");
            }
        }
    });
}