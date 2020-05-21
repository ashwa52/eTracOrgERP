$(document).ready(function () {
    $("#SaveAssetsApplicant").click(function () {
        debugger
        var dataObject = $("#SaveAssetsAllocation").serialize();
        $.ajax({
            type: "POST",
            url: base_url + '/NewAdmin/SaveShowAssetsForApplicant', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
            data: dataObject,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                debugger
                if (Data == true) {
                    toastr.success("Assets saved");
                }
                else {
                    toastr.success("Assets not saved please add");
                }
                $("#ModalForAssetsAdd").modal('hide');
            },
            error: function (err) {
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    })
})