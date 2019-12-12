var HOBurl = '../NewAdmin/GetListOf306090ForJSGrid';
var QExpectationsUrl = '../NewAdmin/GetListOfQExpectationsForJSGrid';
var QEvaluationsUrl = '../NewAdmin/GetListOfQEvaluationsForJSGrid';
var MeetingUrl = '../NewAdmin/GetMeetingList';


var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#List306090").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: HOBurl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        //      rowRenderer: function (item) {
        //         var user = item;
        //          //<tr > <td ><img src="http://localhost:57572/Content/Images/ProfilePic/4_636464971121344334.png" style="height: 50px; width: 50px; border-radius: 50px;"></td>
        //          //<td class="jsgrid-cell" style="width: 80px;">George  T  Smith</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;"></td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">Review Submitted</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">Q4</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;">2019</td>
        //          //    <td class="jsgrid-cell" style="width: 60px;"><div class="btn-toolbar"><span style="background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Assessment" id="btn-profile-undefined"><span><i class="fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;"></i></span></span><span style="background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Notification" id="btn-status-undefined"><span><i class="fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;"></i></span></span></div></td></tr>

        //         // var $photo = $("<div>").addClass("client-photo").append($("<img>").attr("src", user.picture.large));
        //          var $info = $('<tr class="jsgrid-row">')
        //              .append($("<img>").attr("src", user.EMP_Photo).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {
        //              })).append()
        //              .append('<td class="jsgrid-cell">' + user.EmployeeName)
        //              .append('<td class="jsgrid-cell">' + user.JBT_JobTitle)
        //              .append('<td class="jsgrid-cell">' + user.Status)
        //              .append('<td class="jsgrid-cell">' + user.Assesment)
        //.append('<td class="jsgrid-cell" style="width: 60px;"><div class="btn-toolbar"><span style="background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Assessment" id="btn-profile-undefined"><span><i class="fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;"></i></span></span><span style="background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;" title="Notification" id="btn-status-undefined"><span><i class="fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;"></i></span></span></div></td>');



        //              //.append($("<p>").text("Location: " + user.location.city.capitalize() + ", " + user.location.street))
        //              //.append($("<p>").text("Email: " + user.email))
        //              //.append($("<p>").text("Phone: " + user.phone))
        //              //.append($("<p>").text("Cell: " + user.cell));

        //          return $info;
        //      },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Assesment', width: 60, title: "30-60-90"
            }
            ,

            {
                name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Assessment" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment},
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                url: '../NewAdmin/userAssessmentView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($iconUserView);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment },
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },

                                url: '../NewAdmin/userEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });
                        }).append($evaluationText);
                    if (item.Status == "Assessment Submitted") {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton).append($evaluationTextButton);
                    } else {
                        return $("<div>").attr({ class: "btn-toolbar", id: "Action_" + item.EMP_EmployeeID }).append($customUserViewButton).append($customTextButton).append($customTextButton)
                    }
                }
            },


        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }

    });
    $("#ListQExpectations").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QExpectationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,

            {
                name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Assessment" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType ,'FinYear':item.FinYear,'FinQuarter':item.Expectation},
                                url: '../NewAdmin/userExpectationsView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($iconUserView);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment },
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'Department': item.DepartmentName, 'LocationName': item.LocationName },

                                url: '../NewAdmin/userEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });
                        }).append($evaluationText);
                    if (item.Status == "Expectations Submitted" || item.EMP_EmployeeID == $("#LoggedInUser").val()) {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customUserViewButton).append($customTextButton).append($customTextButton);
                    } else {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customUserViewButton).append($customTextButton).append($customTextButton)
                    }
                }
            },


        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListQEvaluations").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QEvaluationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,
            {
                name: 'MeetingDateTime', width: 60, title: "Meeting Date TIme", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.MeetingDate + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.MeetingTime + "</span></div>");
                }
            },

            {
                name: "UserTask", title: "User Task", width: 70, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeeting = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconPiP = $("<span>").append('<i class="fa fa-user-circle-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeetingComplete = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:green;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/QEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($evaluationText);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Notification" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $customMeetingButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Setup Meeting" })
                        .attr({ id: "btn-status" }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#SetUpMeetingModal").modal('show');
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);

                        }).append($iconMeeting);

                    var $customMeetingSuccessButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Meeting Completed" })
                        .attr({ id: "btn-status" }).click(function (e) {
                        }).append($iconMeetingComplete);

                    var $customPiPButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "PIP" })
                        .attr({ id: "btn-status" }).click(function (e) {

                        }).append($iconPiP);


                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);
                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/GetMeetingDetail/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $.ajax({
                                            type: "POST",
                                            data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                            url: '../NewAdmin/QEvaluationView/',
                                            error: function (xhr, status, error) {
                                            },
                                            success: function (data) {

                                                if (data != null) {
                                                    $("#gridArea").hide();
                                                    $('#profileArea').show();
                                                    $('#profileArea').html(data);
                                                }
                                                debugger;
                                                if (result == "MEETINGNOTFOUND") {
                                                    $("#selfAssessmentTable :input").attr("disabled", true);
                                                    $("#labelAddCalendar").html('Please complete the meeting before submitting the Evaluation <span title="Setup meeting"><i onclick=showMeetingPopUp() class="fa fa-calendar-check-o fa-2x" style="margin-left:6px; margin-top:4px;"></i ></span>');
                                                    $("#MeetingNotDoneModal").modal('show');
                                                } else if (result == "MEETINGNOTCOMPLETED") {
                                                    $("#selfAssessmentTable :input").attr("disabled", true);
                                                    $("#labelAddCalendar").html('Meeting is scheduled but not completed');
                                                    $("#MeetingNotDoneModal").modal('show');
                                                }




                                            }
                                        });
                                    }
                                    //else {
                                    //    $("#MeetingNotDoneModal").modal('show');
                                    //}
                                }
                            });
                        }).append($evaluationText);
                    debugger
                    if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted" && item.PRMeetingStatus != "MEETINGCOMPLETED") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingButton).append($customPiPButton).append($evaluationTextButton);
                    } else if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted" && item.PRMeetingStatus == "MEETINGCOMPLETED" || item.PRMeetingStatus == "MEETINGNOTCOMPLETED") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingSuccessButton).append($customPiPButton).append($evaluationTextButton);
                    } else {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customTextButton);
                    }
                }
            },
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListCorrectiveAction").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: QEvaluationsUrl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.JBT_JobTitle + "</span></div>");
                }
            },
            //{ name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status"

            },

            {
                name: 'Expectation', width: 60, title: "Expectation"
            },
            {
                name: 'FinYear', width: 60, title: "Financial Year"
            }
            ,

            {
                name: "UserTask", title: "User Task", width: 70, itemTemplate: function (value, item) {
                    var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconText = $("<span>").append('<i class= "fa fa-plus-square fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $evaluationText = $("<span>").append('<i class= "fa fa-calendar-check-o fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconMeeting = $("<span>").append('<i class="fa fa-clock-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconPiP = $("<span>").append('<i class="fa fa-user-circle-o fa-2x " style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                    var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {

                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                //data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/QEvaluationView/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null) {
                                        $("#gridArea").hide();
                                        $('#profileArea').show();
                                        $('#profileArea').html(result);
                                    }
                                }
                            });


                        }).append($evaluationText);

                    var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Issue Corrective" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                        }).append($iconText);

                    var $customMeetingButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Setup Meeting" })
                        .attr({ id: "btn-status" }).click(function (e) {
                            $("#EmailTo").val(item.EmployeeName);
                            $("#ReceipientEmailId").val(item.EMP_EmployeeID);
                            $("#SetUpMeetingModal").modal('show');
                            $("#FinYear").val(item.FinYear);
                            $("#FinQrtr").val(item.Expectation);

                        }).append($iconMeeting);

                    var $customPiPButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "PIP" })
                        .attr({ id: "btn-status" }).click(function (e) {

                        }).append($iconPiP);


                    var $evaluationTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                        .attr({ title: "Evaluation" })
                        .attr({ id: "btn-status-" + item.id }).click(function (e) {
                            $.ajax({
                                type: "POST",
                                data: { 'Id': item.EMP_EmployeeID, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation },
                                url: '../NewAdmin/GetMeetingDetail/',
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {

                                    if (result != null && result == true) {
                                        $.ajax({
                                            type: "POST",
                                            data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.AssessmentType, 'Name': item.EmployeeName, 'Image': item.EMP_Photo, 'JobTitle': item.JBT_JobTitle, 'FinYear': item.FinYear, 'FinQuarter': item.Expectation, 'Department': item.DepartmentName, 'LocationName': item.LocationName },
                                            url: '../NewAdmin/QEvaluationView/',
                                            error: function (xhr, status, error) {
                                            },
                                            success: function (result) {

                                                if (result != null) {
                                                    $("#gridArea").hide();
                                                    $('#profileArea').show();
                                                    $('#profileArea').html(result);
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        $("#MeetingNotDoneModal").modal('show');
                                    }
                                }
                            });


                        }).append($evaluationText);
                    //if (item.Status == "Expectations Submitted" || item.Status == "Evaluation Submitted") {
                    //    return $("<div>").attr({ class: "btn-toolbar" }).append($evaluationTextButton).append($customTextButton).append($customMeetingButton).append($customPiPButton).append($evaluationTextButton);
                    //} else {
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customTextButton);
                    //}
                }
            },


        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
    $("#ListScheduledMeeting").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: MeetingUrl,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "ManagerPhoto", title: "Manager Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'ManagerName', width: 80, title: "Manager Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.ManagerName + "<br><span style='font-size:10px;color:black;font-style:italic;'></span></div>");
                }
            },
            {
                name: "EmployeePhoto", title: "Employee Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            {
                name: 'EmployeeName', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.EmployeeName + "<br><span style='font-size:10px;color:black;font-style:italic;'></span></div>");
                }
            },
            {
                name: 'MeetingDateTime', width: 60, title: "Meeting Date TIme", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.MeetingDate + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.MeetingTime + "</span></div>");
                }
            },

        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
})(jQuery);
$(document).ready(function () {
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();
        $("#ListQRC").jsGrid("loadData");
    })

});
function showMeetingPopUp() {
    $("#MeetingNotDoneModal").modal('hide');
    $("#SetUpMeetingModal").modal('show');

}