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
            },

            ddlTimeZone: {
                required: true
            },

            'choice': {
                required: true
            },

            'Date1': {
                required: true
            },

            'ddlAppTime': {
                required: true
            },

            'firstAvailableTime': {
                required: true
            }

        },
        highlight: function (element) {
            $(element).closest('.control-group').removeClass('success').addClass('error');
        },
        success: function (element) {
            element
            .text('OK!').addClass('valid')
            .closest('.control-group').removeClass('error').addClass('success');
        },
        messages: {
            'choice': {required: "Please make a selection."}
        },

        invalidHandler: function (event, validator) {
            var rdosh = 0;
            var rdorq = 0;
            if ($('#RadioButtonRequest').is(':checked')) { rdorq = 1; rdosh = 0 };
            if (rdorq == 1) {
                $('#form1').validate({ ignore: '#Date1' });
                $('#form1').validate({ ignore: '#ddlAppTime' });
            }
            if ($('#RadioButtonSchedule').is(':checked')) { rdorq = 0; rdosh = 1 };
            if (rdosh == 1) {
                $('#form1').validate({ ignore: '#firstAvailableTime' });
                $('#txtDate1Label').addClass("txtDate1LabelLoaded");
                $('#ddlAppTimeLabel').addClass("ddlAppTimeLabelLoaded");
                $('#ddlfirstAvailableTimeLabel').addClass("ddlfirstAvailableTimeLabelLoaded");
            }
        }
    });

}); // end document.ready