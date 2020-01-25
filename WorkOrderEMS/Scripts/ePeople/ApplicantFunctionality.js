$(document).ready(function () {
    $("#SaveApplicantInfo").click(function () {
        debugger

        //dataObject = $("#msform").serialize();
        $.ajax({
            type: "POST",
            url: '../NewAdmin/SaveApplicant',
            //data: dataObject,//{ objWorkRequestAssignmentModel: dataObject, fileData: file },
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            success: function (Data) {
                debugger
                var url = "../Guest/_W4Form";
                //$('#RenderPageId').load(url);
                $("#RenderPageId").html(Data);
                //toastr.success(Data.Message);
            },
            error: function (err) {
            },
            //complete: function () {
            //    fn_hideMaskloader();
            //}
        });
    });
})