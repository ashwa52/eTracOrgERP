$(document).ready(function () {
    $("#SaveApplicantInfo").click(function () {
        debugger
        var url = window.location.origin;
        dataObject = $("#msform").serialize();
        $.ajax({
            type: "POST",
            url: url+'/NewAdmin/SaveApplicant',
            data: dataObject,//{ objWorkRequestAssignmentModel: dataObject, fileData: file },
            //beforeSend: function () {
            //    new fn_showMaskloader('Please wait...');
            //},
            success: function (Data) {
                debugger
                var url = "../Guest/_W4Form";
                //var url = "../Guest/BenifitSection";
                $('#RenderPageId').load(url);
                //$("#RenderPageId").html(Data);
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
