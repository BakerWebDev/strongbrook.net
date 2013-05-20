//
//	jQuery Validate example script
//
//	Prepared by David Cochran
//
//	Free for your use -- No warranties, no guarantees!
//

$(document).ready(function () {

    $('#form1').validate({
        rules: {
            txtFirstName: {
                minlength: 2,
                required: true
            },
            txtLastName: {
                minlength: 2,
                required: true
            },
            txtPhone1: {
                minlength: 2,
                required: true
            },
           

            txtEmail: {
                required: true,
                email: true
            }
        },
        highlight: function (element) {
            $(element).closest('.control-group').removeClass('success').addClass('error');
        },
        success: function (element) {
            element
            .text('OK!').addClass('valid')
            .closest('.control-group').removeClass('error').addClass('success');
        }
    });

}); // end document.ready