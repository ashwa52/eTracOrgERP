//This is to change asset image as per asset
$("#ddlAsset").change(function () {
    if ($("#ddlAsset :selected").val() > 0) {
        var CodeData = $("#ddlWorkRequestProjectType :selected").text().substr(0, 3);
        $("#WorkOrderCode").val(CodeData);
        var AssetImage = $('#ddlAsset').find('option:selected').attr('data-imagepath');
        if (AssetImage != null && AssetImage != "") {
            $('#imgWorkRequest').prop('src', AssetImage);
            $(".AssetImage").show();
        }
        else {
            $('#imgWorkRequest').prop('src', "../Content/Images/ProjectLogo/no-profile-pic.jpg");
        }
    }
});
//This is use to change employee fields as per priority level

$("#ddlPriorityLevel").change(function () {
    debugger
    $(".displayEmployee").show();
});
//This is for radio button change hide and show employee data
$("#rdAssignToUserIdY, #rdAssignToUserIdN").change(function () {
debugger
    if ($('#rdAssignToUserIdY').is(':checked')) {
        $('.AssignToUser').css("display", "block");
    }
    else if ($('#rdAssignToUserIdN').is(':checked')) {
        $("#ddlAssignToUser").val('');
        $("#ddlAssignToEmployee").val('');
        $("#EmployeeImage").attr("src", "../Content/Images/ProjectLogo/no-profile-pic.jpg");
        $('.AssignToUser').css("display", "none");
    }
});