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