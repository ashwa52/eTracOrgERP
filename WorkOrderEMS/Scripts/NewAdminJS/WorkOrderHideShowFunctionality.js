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
    var priorityVal = this.value;
    if (priorityVal == 11) {
        
        $(".displayEmployee").hide();
    }
    else if (priorityVal == 12) {
        $(".displayEmployee").show();
    }
});
//This is for radio button change hide and show employee data
$("#rdAssignToUserIdY, #rdAssignToUserIdN").change(function () {
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
//Change fields as per Work Order project type
$("#ddlWorkRequestProjectType").change(function () {
    var WOTypeValue = $("#ddlWorkRequestProjectType :selected").val();
    if(WOTypeValue == 128){
        $(".NormalWODIV, .WorkRequestDIV").show();
        $(".SpecialWODIV, .AssignToUserForSPecial, .ContinuesWODIV, .AssignToUserForSpecialWO, .FacilityRequestDIV").hide();
    }
    else if (WOTypeValue == 129) {
        $(".SpecialWODIV, .AssignToUserForSPecial").show();
        $(".WorkRequestDIV, .NormalWODIV, .ContinuesWODIV, .FacilityRequestDIV").hide();
    }
    else if (WOTypeValue == 256) {
        $(".FacilityRequestDIV").show();
        $(".NormalWODIV, .WorkRequestDIV, .SpecialWODIV, .ContinuesWODIV,  .AssignToUserForSPecial").hide();
    }
    else {
        $(".ContinuesWODIV, .AssignToUserForSPecial").show();
        $(".NormalWODIV, .WorkRequestDIV, .SpecialWODIV, .FacilityRequestDIV").hide();
        var startDate = new Date();
        var FromEndDate = new Date();
        var ToEndDate = new Date();
        ToEndDate.setDate(ToEndDate.getDate());
        //Week Checkboxx 
        $('#StartDate').datepicker({
            format: "mm/dd/yyyy",
            startDate: new Date(),
            autoclose: true
        }).on('changeDate', function (selected) {
             startDate = new Date(selected.date.valueOf());
             startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
             $('#EndDate').datepicker('setStartDate', startDate);
        });
        $('#EndDate').datepicker({
            format: "mm/dd/yyyy",
            startDate: startDate,
            autoclose: true
        }).on('changeDate', function (selected) {
            debugger
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('#StartDate').datepicker('setEndDate', FromEndDate);
            var stDate = $('#StartDate').val();
            var edDate = $('#EndDate').val();
            if (stDate != edDate) {
                $('#greaterval').hide();
            }
            else {
                $('#greaterval').show();
            }
            var sDate = new Date($('#StartDate').val());
            var eDate = new Date($(this).val());
            var totalDays = mydiff(sDate, eDate, "days");
            var weekday = new Array(7);
            weekday[0] = "Sunday";
            weekday[1] = "Monday";
            weekday[2] = "Tuesday";
            weekday[3] = "Wednesday";
            weekday[4] = "Thursday";
            weekday[5] = "Friday";
            weekday[6] = "Saturday";
            sDate.setDate(sDate.getDate() - 1);
            $("#chkboxdivCR input:checkbox").removeAttr('checked');
            $("#chkboxdivCR").find("span").each(function () {
                $(this).removeClass('show');
                $(this).addClass('hide');
            });
            for (var i = 0 ; i <= totalDays; i++) {
                sDate.setDate(sDate.getDate() + 1);
                $("#chkboxdivCR").find("span").each(function () {
                    debugger
                    if ($(this).hasClass(weekday[sDate.getDay()])) {
                        $(this).removeClass('hide');
                        $(this).addClass('show');
                        $(this).children('input:checkbox').attr('checked', 'true');
                        $(this).children().text(weekday[sDate.getDay()]);
                    }
                    else {
                        if ($(this).hasClass('show')) {

                        }
                        else {
                            $(this).addClass('hide');
                        }
                    }
                });
            }
        });
    }
});
function mydiff(date1, date2, interval) {

    var second = 1000, minute = second * 60, hour = minute * 60, day = hour * 24, week = day * 7;
    date1 = new Date(date1);
    date2 = new Date(date2);
    var timediff = date2 - date1;
    if (isNaN(timediff)) return NaN;
    switch (interval) {
        case "years": return date2.getFullYear() - date1.getFullYear();
        case "months": return (
            (date2.getFullYear() * 12 + date2.getMonth())
            -
            (date1.getFullYear() * 12 + date1.getMonth())
        );
        case "weeks": return Math.floor(timediff / week);
        case "days": return Math.floor(timediff / day);
        case "hours": return Math.floor(timediff / hour);
        case "minutes": return Math.floor(timediff / minute);
        case "seconds": return Math.floor(timediff / second);
        default: return undefined;
    }
}
$("#ddlAssignToUserForSpecialWO").change(function () {
    $(".AssignToUserForSpecialWO").show();
})
$(".nodisable").change(function () {
    var $_this = this.value;
    if ($_this == "1") {
        $(".AssetImage, .assetDropdown").show();
    }
    else {
        $(".AssetImage, .assetDropdown").hide();
    }
});
$(".EmployeeAssingClass").change(function () {
    var $_this = this.value;
    if ($_this == "1") {
        $(".displayEmployee").show();
        $(".hideshowEmployeeClass").show();        
    }
    else {
        $(".displayEmployee").hide();
        $(".hideshowEmployeeClass").show();
    }
});

$(".safetyHazardClass").change(function () {
    var $_this = this.value;
    if ($_this == "True") {
        $(".displayEmployee").hide();        
        $('#ddlPriorityLevel').attr('disabled', 'disabled').css("background-color", "#EBEBE4");
        $('#ddlPriorityLevel').val(11);
    }
    else {
        $('#ddlPriorityLevel').val("");
        $('#ddlPriorityLevel').attr('disabled', false).css("background-color", "white");
        $(".displayEmployee").show();
    }
})
