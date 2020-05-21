var url, data, FormId;
var base_url = window.location.origin;

function OpenCommonView(result, action) {
    debugger
    var getSubModuleName = $(result).attr("modulename");
    var getStatus = $(result).attr("status");
    var getSubModuleId = $(result).attr("submoduleId");
    var getNotificationId = $(result).attr("notificationId");
    switch (getSubModuleName + action) {
        case 'OnBoardingView':
            url = base_url+'/Notification/GetViewForApplicantDetails';
            data = { "ApplicantId": getSubModuleId, "ApplicantStatus": getStatus };
            break;
        case 'EvaluationComplete':
            url = base_url + '/Notification/GetViewForApplicantDetails';
            data = { "ApplicantId": getSubModuleId, "ApplicantStatus": getStatus };
            break;
        case 'ScheduleMeetingView':
            url = base_url + '/Notification/ViewForMeeting';
            data = { "emp_id": getSubModuleId, "NotificationId": Number(getNotificationId) };
            FormId = "ScheduleMeetingForm";
            break;
    }

    DoFunctionality(url, data, getSubModuleName, getStatus, getSubModuleId, action, getNotificationId);   
}

function DoFunctionality(url, data, getSubModuleName, getStatus, getSubModuleId, action, getNotificationId) {
    debugger
    if (action == "View") {
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (getData) {                
                ResultView(getData, action)
            },
            error: function (err) { },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    }
    if (action == "CompleteAssessment") {
        debugger
        $('#RenderPageId').load(base_url + "/NewAdmin/PerformanceManagement");
    }
    else if (action == "LaterAssessment") {
        debugger
        var dialog = bootbox.dialog({
            message: '<label class="text-center">You have only 72 hour to complete the assessment please complete within 72 hour otherwise your assessment screen lock.</label>',
            closeButton: false,
            buttons: {
                confirm: {
                    label: 'Complete Now',
                    className: 'btn-success',
                    callback: function () {
                        
                        $('#RenderPageId').load(base_url+"/NewAdmin/PerformanceManagement");
                    }
                },
                cancel: {
                    label: 'Later',
                    className: 'btn-danger',
                    callback: function () {
                    }
                }
            },
        });
    }
    else if (action == "CompleteEvaluation") {
        debugger
        $('#RenderPageId').load(base_url + "/NewAdmin/PerformanceManagement");
    }
    else if (action == "LaterEvaluation") {
        var dialog = bootbox.dialog({
            message: '<label class="text-center">You have only 72 hour to complete the evaluation please complete evaluation within 72 hour.</label>',
            closeButton: false,
            buttons: {
                confirm: {
                    label: 'Complete Now',
                    className: 'btn-success',
                    callback: function () {
                        $('#RenderPageId').load(base_url + "/NewAdmin/PerformanceManagement");
                    }
                },
                cancel: {
                    label: 'Later',
                    className: 'btn-danger',
                    callback: function () {
                    }
                }
            },
        });
    }
    NotificationRead(getNotificationId);
}
function ResultView(view, action) {
    debugger
    if (action == "View") {
        $("#myModelToVeiwDetails").modal("show");
        $("#DivForViewDetails").html(view);
    }
}
function NotificationRead(NotificationId) {
    debugger
    $.ajax({
        type: "POST",
        url: base_url + '/Notification/ReadNotification?NotificationId=' + NotificationId,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            $("#GetUnseenList").html("");
            $("#GetUnseenList").html(Data);
        },
        error: function (err) {

        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
$("#SaveNotificationAction").click(function () {
    debugger
    var FormData = $("#ScheduleMeetingForm").serialize();
    $.ajax({
        type: "POST",
        data: FormData,
        url: base_url + '/Corrective/SaveMeetingDateTime',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
           
        },
        error: function (err) {

        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
});
