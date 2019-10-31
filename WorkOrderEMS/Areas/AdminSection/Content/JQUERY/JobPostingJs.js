$(document).ready(function () { 
    $("#SaveJobPost").click(function () {
        debugger

        if ($("#SaveJobPostingFormVSC").valid()); {
            var value = $("#JobTitleId option:selected").text();
            $("SaveJobTitleWithText").val(value);
            var dataObject = $("#SaveJobPostingFormVSC").serialize();
            $.ajax({
                type: 'POST',
                //contentType: "application/json",
                url: '../EPeople/SaveJobPostingData/',
                data: dataObject,//{ objWorkRequestAssignmentModel: dataObject, fileData: file },
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (result) {
                    debugger
                    $("#divOpenJobPostForm").html("");
                    $("#myModalToAddJobPost").modal('hide');
                },
                error: function (err) {
                },
                complete: function () {
                    fn_hideMaskloader();
                }
            });
        }
    });
});
