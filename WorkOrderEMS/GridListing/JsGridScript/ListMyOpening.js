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
					return $("<div>").append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-exclamation-triangle white"))
						.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-gg-circle white"))
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
					return $("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn")
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
			$('#epeople').hide();
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
function RecordYes() {
	$("#selectAns").val('Y');
	$("#commentbox").prop('required', true);
}
function RecordNo() {
	$("#selectAns").val('N');
}
function SaveAndNext() {
	debugger;
	var qus_num = $("#hdn_qusnum").val();
	if (qus_num >= 0) {
		var opt_val = $("#selectAns").val();
		if (opt_val == undefined || opt_val == '' || opt_val == null) {
			alert('Please select an option');
			return;
		}
	}
	if ($("#commentbox").prop('required')) {
		var value = $("#commentbox").val();
		if (value == '' || value == undefined) {
			return;
		}
	}
	if (id != null)
		id += 1;

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

	Getquestions(null);
}