var $_UserId;
$(document).ready(function () {
    //$(".AddFilesClass").click(function () {
    //    debugger
    //    var $_this = this;
    //    var Id = $($_this).attr("data-id");
    //    $.ajax({
    //        type: "POST",
    //        // data: { 'Id': item.id},
    //        url: base_url + '/EPeople/GetFileViewTest?EMPId=' + Id,
    //        beforeSend: function () {
    //            new fn_showMaskloader('Please wait...');
    //        },
    //        contentType: "application/json; charset=utf-8",
    //        error: function (xhr, status, error) {
    //        },
    //        success: function (result) {
    //            debugger
    //            $("#ContaierAddFile").html(result);
    //            $("#myModalForAddFileData").modal('show');
    //        },
    //        complete: function () {
    //            fn_hideMaskloader();
    //        }
    //    });
    //});
    $("#AddDemotion").click(function () {
        debugger
        $.ajax({
            type: "GET",
            // data: { 'Id': item.id},
            url: base_url + '/EPeople/OpenDemotionForm?Id=' + $_UserId,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (FormView) {
                $("#divOpenDemotionForm").html(FormView);
                $("#myModalForDemotionEmployee").modal("show");
                $("#myModalForChangeStatusData").modal("hide");
                $("#ChangeTitle").html("Demotion");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    });
    $("#EmploymentStatusChange").click(function () {
        $.ajax({
            type: "GET",
            // data: { 'Id': item.id},
            url: base_url + '/EPeople/OpenEmploymentStatusChange?Id=' + $_UserId,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (FormView) {
                $("#divOpenDemotionForm").html(FormView);
                $("#myModalForDemotionEmployee").modal("show");
                $("#myModalForChangeStatusData").modal("hide");
                $("#ChangeTitle").html("Employment Status Change");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    });
    $("#LocationTransfer").click(function () {
        $.ajax({
            type: "GET",
            // data: { 'Id': item.id},
            url: base_url + '/EPeople/OpenLocationForTransfer?Id=' + $_UserId,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (FormView) {
                $("#divOpenDemotionForm").html(FormView);
                $("#myModalForDemotionEmployee").modal("show");
                $("#myModalForChangeStatusData").modal("hide");
                $("#ChangeTitle").html("Location Transfer");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    });
});
function AddFileData($_this) {
    debugger
    var Id = $($_this).attr("data-id");
    $.ajax({
        type: "POST",
        // data: { 'Id': item.id},
        url: base_url + '/EPeople/GetFileViewTest?EMPId=' + Id,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            debugger
            $("#ContaierAddFile").html(result);
            $("#myModalForAddFileData").modal('show');
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function EmployeeStatusChange($_this) {
    $_UserId = $($_this).attr("data-id");
    $("#myModalForChangeStatusData").modal('show');
}
function EditUserInfo($_this) {
    var Id = $($_this).attr("data-id");
    $.ajax({
        type: "POST",
        // data: { 'Id': item.id},
        url: base_url + '/EPeople/GetEmployeeDetailsForEdit?Id=' + Id,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $("#ContaierEditUserInfo").html(result);
            $("#myModalForeditUserInfoData").modal('show');
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function EmployeeOrientation($_this) {
    debugger
    var getEmpId = $($_this).attr("data-empId");
    $("#GetEmployeeIdForOrientation").html(getEmpId);
    //var origin_Url = window.location.origin;
    $("#myModalToAddEmployeeOrietation").modal('show');
}
$("#SaveOrientation").click(function () {
    debugger
    var origin_Url = window.location.origin;
    $("#OTN_EMP_EmployeeID").val($("#GetEmployeeIdForOrientation").text());
    var dataOrientation = $("#SaveOrientationForm").serialize();
    $.ajax({
        type: "POST",
        url: origin_Url + '/EPeople/SaveOriantation', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
        data: dataOrientation,
        success: function (result) {
            $("#GetEmployeeIdForOrientation").html("");
            $("#ONT_OrientationDate").val('');
            $("#ONT_Orientationtime").val('');
            $("#myModalToAddEmployeeOrietation").modal('hide');
            if (result.isSaved == true) {
                toastr.success(result.message);

            }
            else { toastr.error(result.message); }

        },
        error: function (err) {
            alert(err);
        }
    });
})


