var HOBurl = '../HirinngOnBoarding/GetHiringOnBoardingList';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

function myOpenings() {
	$("#ListMyOpening").show();
	$("#ListMyOpening").jsGrid({
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
					url: '/NewAdmin/MyOpenings',
					dataType: 'json',
					success: function (data) {
						debugger;
						deferred.resolve(data);
					}
				});

				return deferred.promise();
			}
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
			        return $("<div>").addClass("actionDiv").append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-exclamation-triangle whiteR"))
						.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-gg-circle whiteB"))
						.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").addClass("fa fa-diamond whiteGr")))
						.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-check-circle whiteY"))
						.append($("<div>").addClass("action1 inline actionboxR").append("<i>").addClass("fa fa-check white"));
			    }
			}, 
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
					url: '/NewAdmin/MyInterviews',
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
						.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").addClass("fa fa-diamond white")));
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
					url: "/NewAdmin/GetJobPostong",
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
				itemTemplate: function (value) {
					return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
						.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
						.append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"));
				}
			}
		],
		rowClick: function (args) {
			console.log(args.item);
			$("#MyOpeningSummery").hide();
			$("#btnBack").show();
			myOpenings();
		}
	});
}
function GetSummeryDetail(elm) {
	debugger;
	if ($('.text').is(':visible')) {
		$('.text').hide();
	}
	// Create chart instance
	$("#detailDiv").empty();
	var chart = am4core.create("detailDiv", am4charts.PieChart);

	// Add data
	chart.data = [{
		"country": "Lithuania",
		"litres": 501.9
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

	// Set inner radius
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
function HideDetail(elm) {
	$("#detailDiv").empty();
	$($(elm).parent()).prev().hide()
}
function JobPosting() {
	$("#ListMyOpening").hide();
	MyOpeningSummery();
}
function TakeInterview(item) {
	debugger;
	$.ajax({
		url: '/NewAdmin/InfoFactSheet',
		method: 'POST',
		data: item,
		success: function (response) {
		    $('#infofactsheet').empty();
		    $("#infofactsheet").html(response);
		}
	});
}
function GetInterviewers(elm, applicantId) {
	debugger;
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
		url: '/NewAdmin/GetInterviewers?applicantId=' + applicantId,
		method: 'GET',
		success: function (response) {
			$("#interviewArea").html(response);
		}
	});
}
function GetInterviewQuestions() {
	$("#interviewArea").empty();
	$.ajax({
		url: '/NewAdmin/GetInterviewQuestionView',
		method: 'GET',
		success: function (response) {
			Getquestions(null);
			$("#interviewArea").html(response);
		}
	});
}

function Getquestions(id) {
	$.ajax({
		url: '/NewAdmin/GetInterviewQuestions',
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
}
function SaveAndNext() {
	debugger;
	if (CheckIfOptionNotSelected())
		return;
	if ($("#commentbox").prop('required')) {
		var value = $("#commentbox").val();
		if (value == '' || value == undefined) {
			$("#lblmsg").show();
			$("#lblmsg").css("display","block");
			return;
		}
	}
	if (SaveAnswer(function (data) {
		if (data) {
			$("#commentbox").val('');
			var INA_API_ApplicantId = $("#applicant_id").val();
			CheckIfAllResponded(INA_API_ApplicantId, function (responseForNextQuestion) {
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
		url: '/NewAdmin/SaveAnswers',
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
		url: '/NewAdmin/CanInterviewerIsOnline',
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
					$("#"+empid).css("background", "#fff");
				}
			}
			else {
				$('.toggle-off').addClass('active');
			$('.toggle-on').removeClass('active');
				$("#"+empid).css("background", "#fff");
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
		url: '/NewAdmin/GetScore?ApplicantId=' + ApplicantId,
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
function CheckIfAllResponded(applicantId, callback) {
	$.ajax({
		url: '/NewAdmin/CheckNextQuestion?ApplicantId=' + applicantId,
		method: 'GET',
		success: callback
	});
}
function MarkAbsent() {

}