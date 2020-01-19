var HOBurl = '../HirinngOnBoarding/GetHiringOnBoardingList';
var base_url = window.location.origin;
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

function myOpenings(PostingId) {
    $("#ListMyOpening").show();
    $("#ListMyOpening").jsGrid({
        width: "100%",
        height: "430px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();
                $.ajax({
                    url: base_url + '/NewAdmin/MyOpenings?PostingId=' + PostingId,
                    dataType: 'json',
                    success: function (data) {
                        debugger;
                        deferred.resolve(data);
                    }
                });
                return deferred.promise();
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },

        //rowRenderer : function (item) {
        //	debugger;
        //	var openings = item;
        //	var $photo = $("<div>").append($("<img>").addClass("client-photo").attr("src", openings.Image === '' || openings.Image === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : openings.Image));
        //	var $name = $("<div>").append($("<p>").append($("<strong>").text(openings.FirstName.capitalize() + " " + openings.LastName.capitalize())))
        //	return $("<tr>").append($("<td>").append($photo)).append($("<td>").append($("<p>").text(openings.FirstName.capitalize() + " " + openings.LastName.capitalize())))
        //		.append($("<td>").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
        //			.append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star")))
        //		.append($("<td>").append($("<p>").text(openings.PhoneNumber)))
        //		.append($("<td>").append($("<p>").text(openings.Email)))
        //		.append($("<td>").append($("<p>").text(openings.Status)))
        //		.append($("<td>").append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-exclamation-triangle white"))
        //			.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-gg-circle white"))
        //			.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify( openings) + ")", "href": "#" }).append("<i>").addClass("fa fa-diamond white"))));
        //},

        fields: [
			{
			    title: "Applicant Image", name: "Image", width: 30,
			    itemTemplate: function (value) {
			        return $("<div>").append($("<img>").addClass("client-photo").attr("src", value === '' || value === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : value));
			    }
			},
			{
			    title: "Applicant Name", width: 60, itemTemplate: function (v, i) {
			        return i.FirstName;// + ' ' + i.LastName;
			    }
			},
			{
			    title: "Rating", width: 40,
			    itemTemplate: function () {
			        return $("<i>").addClass("fa fa-star").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
						.append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"));
			    }
			},
			{ title: "Phone", width: 60, name: "PhoneNumber" },
			{ title: "Email", width: 60, name: "Email" },
			{ title: "Status", width: 50, name: "Status" },
			//{
			//    title: 'Action', width: 70, itemTemplate: function (value, item) {
			//        return $("<div>").addClass("actionDiv").append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-exclamation-triangle whiteR"))
			//			.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-gg-circle whiteB"))
			//			.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").attr({ "style": "color:darkblue" }).addClass("fa fa-diamond whiteGr")))
			//			.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-buysellads whiteY"))
			//			.append($("<div>").addClass("action1 inline actionboxR").append("<i>").addClass("fa fa-btc white"))
			//        .append($("<div>").addClass("action1 inline actionboxS").append("<a>").attr({ "onclick": "GoToRecruitee(" + JSON.stringify(item) + ")", "href": "#" }).addClass("fa fa-clock-o fa-2x whiteS"));
			//    }
			//},
             {
                 title: "Action", width: 70, css: "text-center", itemTemplate: function (value, item) {
                     var $iconAssessment, $iconBackground;
                     var $iconFirst = $("<i>").attr({ class: "fa fa-exclamation-triangle whiteR" });
                     var $iconSecond = $("<i>").attr({ class: "fa fa-gg-circle whiteB" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     //var $iconDiamond = $("<i>").attr({ class: "fa fa-diamond" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     var $iconDiamond = $("<i>").attr({ class: "fa fa-diamond whiteGr" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                     if (item.Status == "Shortlisted") {
                         $iconAssessment = $("<i>").attr({ class: "fa fa-buysellads whiteY" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     }
                     if (item.Status == "AssessmentPass" && item.IsActive == "Y") {
                          $iconBackground = $("<i>").attr({ class: "fa fa-btc white" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                     }
                     //var $iconGoTORecruitee = $("<i>").attr({ class: "fa fa-clock-o fa-2x whiteS" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     var $customFirstButton = $("<span>")
                           .attr({ title: jsGrid.fields.control.prototype.hiredEmployeeTooltip })
                           .attr({ id: "btn-first-" + item.ApplicantId }).click(function (e) {
                               debugger
                           }).append($iconFirst);

                     var $customSecondButton = $("<span>")
                           .attr({ title: jsGrid.fields.control.prototype.HRChallengeEmployeeTooltip })
                           .attr({ id: "btn-second-" + item.ApplicantId }).click(function (e) {
                               debugger
                           }).append($iconSecond);

                     var $customDiamondButton = $("<span>")
                             .attr({ title: jsGrid.fields.control.prototype.InterviewEmployeeTooltip })
                             .attr({ id: "btn-diamond-" + item.ApplicantId }).click(function (e) {
                                 debugger
                                 TakeInterview(item);
                             }).append($iconDiamond);

                     var $customAssessmentButton = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.AssessmentEmployeeTooltip })
                          .attr({ id: "btn-Assessment-" + item.ApplicantId }).click(function (e) {
                              debugger
                              $.ajax({
                                  type: "POST",
                                  url: base_url + '/NewAdmin/AssessmentStatusChange?Status=' + "E" + "&IsActive=" + "S" + "&ApplicantId=" + item.ApplicantId,
                                  beforeSend: function () {
                                      new fn_showMaskloader('Please wait...');
                                  },
                                  success: function (message) {
                                      if (message != null) {
                                          toastr.success(message);
                                      }
                                      else {
                                          toastr.success(message);
                                      }
                                      $("#myModalForDemotionEmployee").modal("hide");
                                  },
                                  error: function (err) {
                                  },
                                  complete: function () {
                                      fn_hideMaskloader();
                                  }
                              });
                          }).append($iconAssessment);

                     var $customBackgroundButton = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.BackgroundEmployeeTooltip })
                          .attr({ id: "btn-Background-" + item.ApplicantId }).click(function (e) {
                              debugger
                              $.ajax({
                                  type: "POST",
                                  url: base_url + '/NewAdmin/BackgroundStatusChange?Status=' + "F" + "&IsActive=" + "S" + "&ApplicantId=" + item.ApplicantId,
                                  beforeSend: function () {
                                      new fn_showMaskloader('Please wait...');
                                  },
                                  success: function (message) {
                                      if (message != null) {
                                          toastr.success(message);
                                      }
                                      else {
                                          toastr.success(message);
                                      }
                                      $("#myModalForDemotionEmployee").modal("hide");
                                  },
                                  error: function (err) {
                                  },
                                  complete: function () {
                                      fn_hideMaskloader();
                                  }
                              });
                          }).append($iconBackground);

                     //var $customRecruiteeButton = $("<span>")
                     //     .attr({ title: jsGrid.fields.control.prototype.ScheduleEmployeeTooltip })
                     //     .attr({ id: "btn-Recruitee-" + item.ApplicantId }).click(function (e) {
                     //         //window.location.href = "https://app.recruitee.com/#/settings/scheduler";
                     //         //$("#ModalScheduleInterview").modal('show');
                     //         $("#JobPostBackBtn").hide();
                     //         $("#ListMyOpening").hide();
                     //         $("#Scheduler").show();
                     //         $("#lblApplicantId").text(item.ApplicantId);
                     //         $("#lblApplicantName").text(item.FirstName);
                     //         $("#lblApplicantEmail").text(item.Email);

                     //         loadCalendar(item.ApplicantId);
                     //     }).append($iconGoTORecruitee);

                     return $("<div>").attr({ class: "btn-toolbar" }).append($customFirstButton).append($customSecondButton).append($customDiamondButton).append($customAssessmentButton).append($customBackgroundButton).append($customRecruiteeButton);
                 }
             }
        ]
    });


    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };


}
function MyInterviews() {
    $("#myinterviews").jsGrid({
        width: "100%",
        height: "300px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();

                $.ajax({
                    url: base_url + '/NewAdmin/MyInterviews',
                    dataType: 'json',
                    success: function (data) {
                        debugger;
                        deferred.resolve(data);
                    }
                });

                return deferred.promise();
            }
        },
        fields: [
			{
			    title: "Applicant Image", name: "Image", width: 60,
			    itemTemplate: function (value) {
			        return $("<div>").append($("<img>").addClass("client-photo").attr("src", value === '' || value === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : value));
			    }
			},
			{
			    title: "Applicant Name", width: 60, itemTemplate: function (v, i) {
			        return i.FirstName + ' ' + i.LastName;
			    }
			},
			{
			    title: "Rating", width: 60,
			    itemTemplate: function () {
			        return $("<i>").addClass("fa fa-star").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
						.append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"));
			    }
			},
			{ title: "Phone", width: 60, name: "PhoneNumber" },
			{ title: "Email", width: 60, name: "Email" },
			{ title: "Status", width: 60, name: "Status" },
			{
			    title: '', width: 60, itemTemplate: function (value, item) {
			        return $("<div>")
						.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").attr({ "style": "color:darkblue" }).addClass("fa fa-diamond white")));
			    }
			}
        ]
    });


    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };


}
function MyOpeningSummery() {
    $("#btnBack").hide();
    $("#ListMyOpening").hide();
    $("#MyOpeningSummery").show();
    $("#MyOpeningSummery").jsGrid({
        width: "100%",
        height: "300px",
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        controller: {
            loadData: function () {
                var d = $.Deferred();

                $.ajax({
                    url: base_url + "/NewAdmin/GetJobPostong",
                    dataType: "json"
                }).done(function (response) {
                    d.resolve(response);
                });

                return d.promise();
            }
        },

        fields: [
			{ name: "JobTitle", type: "text", width: 300 },
			{ name: "Employee", type: "text", width: 70 },
			{ name: "Applicant", type: "number", width: 70 },
			{ name: "DatePosted", type: "text", width: 150 },
			{
			    name: "Duration", type: "text", width: 60,
			    itemTemplate: function (value) {
			        return $("<span>").text(value + ' ' + 'Days');
			    }
			},
			{ name: "Status", type: "text", width: 50 },
			{
			    name: " ", width: 100, align: "center", Title: "",
                itemTemplate: function (value,item) {
                    var $iconOpenCalendar = $("<i>").attr({ class: "fa fa-clock-o fa-2x whiteS" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                    var $customOpenCalendarButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.ScheduleEmployeeTooltip })
                        .attr({ id: "btn-Recruitee-" + item.Employee }).click(function (e) {
                            e.stopPropagation();
                            //window.location.href = "https://app.recruitee.com/#/settings/scheduler";
                            //$("#ModalScheduleInterview").modal('show');
                            $("#JobPostBackBtn").hide();
                            $("#MyOpeningSummery").hide();
                            $("#Scheduler").show();
                            $("#JobId").val(item.JobPostingId);
                            //$("#lblApplicantId").text(item.Employee);
                            //$("#lblApplicantName").text(item.FirstName);
                            //$("#lblApplicantEmail").text(item.Email);
                           // getslots();
                            loadCalendar(item.Employee);
                        }).append($iconOpenCalendar);

			        return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
						.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
                        .append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"))
                        .append($customOpenCalendarButton);
			    }
			}
        ],
        rowClick: function (args) {
            console.log(args.item);
            $("#MyOpeningSummery").hide();
            $("#btnBack").show();
            myOpenings(args.item.JobPostingId);
        }
    });
}
function GetSummeryDetail(elm, postingId) {
    if ($('.text').is(':visible')) {
        $('.text').hide();
    }
    // Create chart instance
    $("#detailDiv").empty();
    var arrData = [];
    var chart = am4core.create("detailDiv", am4charts.PieChart);
    $.ajax({
        url: base_url + '/NewAdmin/GetHringChartData?postingId='+postingId,
        method: 'POST',
        success: function (response) {
            chart.data = [{
                "country": "Lithuania",
                "litres": response.Applied
            }, {
                "country": "Czech Republic",
                "litres": 301.9
            }, {
                "country": "Ireland",
                "litres": 201.1
            }, {
                "country": "Germany",
                "litres": 165.8
            }, {
                "country": "Australia",
                "litres": 139.9
            }, {
                "country": "Austria",
                "litres": 128.3
            }, {
                "country": "UK",
                "litres": 99
            }, {
                "country": "Belgium",
                "litres": 60
            }, {
                "country": "The Netherlands",
                "litres": 50
            }];
            chart.innerRadius = am4core.percent(50);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "litres";
            pieSeries.dataFields.category = "country";
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            // This creates initial animation
            pieSeries.hiddenState.properties.opacity = 1;
            pieSeries.hiddenState.properties.endAngle = -90;
            pieSeries.hiddenState.properties.startAngle = -90;
            $($(elm).parent()).prev().show()
        }
    });
   

    // Add data
    

    // Set inner radius

}
function HideDetail(elm) {
    $("#detailDiv").empty();
    $($(elm).parent()).prev().hide()
}
function JobPosting() {
    $("#ListMyOpening").hide();
    MyOpeningSummery();
}
function TakeInterview(item) {
    debugger
    $.ajax({
        url: base_url + '/NewAdmin/InfoFactSheet',
        method: 'POST',
        data: item,
        success: function (response) {
            $('#infofactsheet').empty();
            $("#infofactsheet").html(response);
        }
    });
}
function GetInterviewers(elm, applicantId) {
    $(elm).removeClass("btnNotSelected");
    $(elm).addClass("btnSelected");
    $(elm.parentElement).find('button').each(function (index, element) {
        if (element != elm && !$(element).hasClass('dropdown-toggle')) {
            $(element).removeClass("btnSelected");
            $(element).addClass("btnNotSelected");
        }
    });
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewers?applicantId=' + applicantId,
        method: 'GET',
        success: function (response) {
            $("#interviewArea").html(response);
        }
    });
}
function GetInterviewQuestions() {
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewQuestionView',
        method: 'GET',
        success: function (response) {
            debugger
            Getquestions(null);
            $("#interviewArea").html(response);
        }
    });
}

function Getquestions(id) {
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewQuestions',
        method: 'POST',
        data: { id: id },
        success: function (innerResponse) {
            $("#questionArea").empty();
            $("#questionArea").html(innerResponse);
            $("#qusOpt").val('')
        }
    });
}
function RecordYes(isFinal) {
	if (!isFinal) {
		$("#selectAns").val('Y');
		document.querySelector('#commentbox').required = false;
		$("#lblmsg").hide();
	}
	else {
		$("#selectAns").val('Y');
		document.querySelector('#commentboxlast').required = true;
	}
	//SaveAnswer(function (data) { });

}
function RecordNo(isFinal) {
	if (!isFinal) {
		$("#selectAns").val('N');
		$("#commentbox").prop('required', true);
	} else {
		$("#selectAns").val('N');
		document.querySelector('#commentboxlast').required = false;
		$("#lblmsglast").hide();
	}
	//SaveAnswer(function (data) { });
}
function SaveAndNext() {
    debugger;
    if (CheckIfOptionNotSelected())
        return;
    if ($("#commentbox").prop('required')) {
        var value = $("#commentbox").val();
        if (value == '' || value == undefined) {
            $("#lblmsg").show();
            $("#lblmsg").css("display", "block");
            return;
        }
    }
    if (SaveAnswer(function (data) {
		if (data) {
			$("#commentbox").val('');
			var INA_API_ApplicantId = $("#applicant_id").val();
			CheckIfAllResponded(INA_API_ApplicantId, $("#q_id").val(), function (responseForNextQuestion) {
				if (responseForNextQuestion) {
					Getquestions(null);
    }
    else {
					alert("All interviewers are not responded yet!!!");
					return;
    }
    })

    }
    else {
			alert('Something went wrong please try again!!! ');
			return;
    }


    }));



}
function SaveAnswer(callback) {
    var INA_INQ_Id = $("#q_id").val();
    var INA_API_ApplicantId = $("#applicant_id").val();
    var INA_EMP_EmployeeID = '';
    var INA_Answer = $("#selectAns").val();
    var INA_Comments = $("#commentbox").val();
    console.log(INA_INQ_Id);
    console.log(INA_API_ApplicantId);
    console.log(INA_EMP_EmployeeID);
    console.log(INA_Answer);
    console.log(INA_Comments);
    $.ajax({
        url: base_url + '/NewAdmin/SaveAnswers',
        method: 'POST',
        async: false,
        cache: false,
        data: { QusId: INA_INQ_Id, ApplicantId: INA_API_ApplicantId, Answer: INA_Answer, Comment: INA_Comments },
        success: callback
    });
}
function GoOnline(elm, empid) {

    var ApplicantId = $("#applicant_id").val();;
    var comment = '';
    var IsAvailable = 'Y';
    $.ajax({
        url: base_url + '/NewAdmin/CanInterviewerIsOnline',
        method: 'POST',
        data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
        success: function (response) {
            if (response == true) {
                $("#icnOnline").hide();
                if ($(elm).is(':checked')) {
                    debugger;
                    console.log('This is firing');
                    $('.toggle-off').removeClass('active');
                    $('.toggle-on').addClass('active');
                    $("#" + empid).css("background", "#43D84A");
                } else {
                    $('.toggle-off').addClass('active');
                    $('.toggle-on').removeClass('active');
                    $("#" + empid).css("background", "#fff");
                }
            }
            else {
                $('.toggle-off').addClass('active');
                $('.toggle-on').removeClass('active');
                $("#" + empid).css("background", "#fff");
            }
        }
    });
}
function CheckIfOptionNotSelected() {
    var qus_num = $("#hdn_qusnum").val();
    if (qus_num >= 0) {
        var opt_val = $("#selectAns").val();
        if (opt_val == undefined || opt_val == '' || opt_val == null) {
            alert('Please select an option');
            return true;
        }
    }
}
function GetScore(callback) {
    var ApplicantId = $("#applicant_id").val();
    $.ajax({
        url: base_url + '/NewAdmin/GetScore?ApplicantId=' + ApplicantId,
        method: 'GET',
        success: callback
    });
}
function FinishInterview() {
    if (CheckIfOptionNotSelected())
        return;

    if ($("#commentboxlast").prop('required')) {
        var isCommentRequired = $("#commentboxlast").val();
        if (isCommentRequired == '' || isCommentRequired == undefined || isCommentRequired == null) {
            $("#lblmsglast").show();
            $("#lblmsglast").css("display", "block");
            return;
        }
    }
    if (SaveAnswer(function (data) {
		GetScore(function (response) {
			$("#startInterview").empty();
			$("#startInterview").html("<div class='col-md-12' style='text-align:center'><div class='card cardscore'><label style='color:green'>Total Score:</label><label>" + response + "</label></div><div><label style='font-size:20px;color:white;'>Thanks For The Interview</label><a style='margin:20px;' href='/NewAdmin#' class='btn btn-default btnfinish'>Exit</a></div></div>");
    });
    }));
}
function CheckIfAllResponded(applicantId, qusid, callback) {
    $.ajax({
        url: base_url + '/NewAdmin/CheckNextQuestion?ApplicantId=' + applicantId + "&QusId=" + qusid,
        method: 'GET',
        success: callback
    });
}
function MarkAbsent() {

}


function GetCompanyOpening() {
    var records = [
        { "JobTitle": "Otto Clay", "PositionCount": 1, "ApplicantCount": 1, "JobPostingDate": "28/12/2019", "Duration": "30", "Status": true}
    ];
	$("#companyOpening").jsGrid({
		width: "100%",
		height: "300px",
		sorting: true,
		paging: true,
		autoload: true,
		pageSize: 10,
		pageButtonCount: 5,
		controller: {
			loadData: function () {
				var d = $.Deferred();

				$.ajax({
					url: "/NewAdmin/GetCompanyOpening",
					dataType: "json"
				}).done(function (response) {
					d.resolve(response);
				});

				return d.promise();
			}
		},
		fields: [
			{
				title: "Job Title", width: 100, itemTemplate: function (v, i) {
				    return i.JobTitle;
				}
			},
			{
				title: "Position", width: 60, itemTemplate: function (v, i) {
				    return i.PositionCount;
				}
			},
			{
				title: "Applicant", width: 60, itemTemplate: function (v, i) {
				    return i.ApplicantCount;
				}
			},
			{
				title: "Date Posted", width: 60, itemTemplate: function (v, i) {
				    return i.JobPostingDate;
				}
			},
			{
				title: "Duration", width: 60, itemTemplate: function (v, i) {
				    return i.Duration;
				}
			},
			{ name: "Status", type: "text", width: 50 },
			{
			    name: " ", width: 100, align: "center", Title: "",
			    itemTemplate: function (value,item) {
			        return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this," + item.JPS_JobPostingId + ");' onmouseout='HideDetail(this)'>"))
						.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
						.append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"));
			    }
			}
			
		],
		rowClick: function (args) {
			console.log(args.item);
			$("#MyOpeningSummery").hide();
			$("#btnBack").show();
			myOpenings(args.item.JobPostingId);
		}
	});
}

function GoToRecruitee(item) {
    debugger
    
}

   function ToAcceptRejectAfterInterview(data) {
        debugger
        var getAppId = $(data).attr("applicantid");
        var getVal = $("#ToAcceptRejectAfterInterview option:selected").val();
        $.ajax({
            url: base_url + '/NewAdmin/InterviewAcceptCancel?status=' + getVal + "&ApplicantId=" + getAppId,
            method: 'POST',
            //data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
            success: function (response) {
               
            }
        });
    }
function getslots(date) {
    $.ajax({
        url: base_url + '/NewAdmin/GetSlotTimings',
        data: {"date":date},
        method: 'POST',
        success: function (optionList) {
            $("#eventTime").html('');
            var combo = $("<select class='form-control'></select>");
            combo.append("<option id='" + 0 + "'>--Select Time Slot--</option>");
            $.each(optionList, function (i, el) {
                combo.append("<option id='" + el.SLT_Id + "'>" + el.SLT_fromTime + "</option>");
                console.log(el.SLT_fromTime );
            });
            console.log(combo);
            $("#eventTime").append(combo);
        }
    });

}