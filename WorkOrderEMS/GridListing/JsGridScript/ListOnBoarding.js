var base_url = window.location.origin;
var HOBurl = '../HirinngOnBoarding/GetHiringOnBoardingList';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

var API_id;
let details = [];

var datadetails = "";
console.log(datadetails);

(function ($) {
    'use strict'

})(jQuery);
$(document).ready(function () {
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();

        $("#ListQRC").jsGrid("loadData");
    })
});

function addNewOnboardingDetail(item) {
	debugger;
	getStateList();
	var date = item.API_DateOfJoining.replace('/Date(', '').replace(')', '').replace('/', '');
	var dateVal = new Date(parseInt(date)).toLocaleDateString();
	$("#htn_applicationId").val(item.API_ApplicantId);
	$('#onboardFirstName').val(item.API_FirstName);
	$('#onboardMobile').val(item.API_PhoneNumber);
	$('#onboardEmail').val(item.API_Email);
	$("#onboardMiddleName").val(item.API_MiddleName);
	$("#onboardLastName").val(item.API_LastName);
	$("#job_title").val(item.API_JobTitleID);
	$("#doj").val(dateVal);
	$("#myModalForAddEmployee").modal("show");
    e.stopPropagation();
}
$('#SaveVCSChartForm1').validate({
    
    rules: {
        onboardFirstName: {
            required: true
        },
        onboardLastName: {
            required: true
        },
        onboardMiddleName: {
            required: true
        },
        onboardAddress1: {
            required: true
        },
        onboardAddress2: {
            required: true
        },
        onboardCity: {
            required: true
        },
        onboardState: {
            required: true
        },
        onboardMobile: {
            required: true
        },
        onboardEmail: {
            required: true
        }
    },
    submitHandler: function (Form) {
        debugger// for demo
        return false;
    }
});

function saveApplicantInfo() {
    var isValid = $('#SaveVCSChartForm1').valid();
    if (isValid) {
        //alert(isValid);
        var myData = {
			FirstName: $("#onboardFirstName").val(),
			MiddleName: $("#onboardMobile").val(),
			LastName: $("#onboardLastName").val(),
			StreetAddress1: $("#onboardAddress1").val(),
			StreetAddress2: $("#onboardAddress2").val(),
			City: $("#onboardCity").val(),
			State: $("#onboardState").val(),
			MobileNumber: $("#onboardMobile").val(),
			EmailId: $("#onboardEmail").val(),
			EmpId: $("#empid").val(),
			App_Id: $("#htn_applicationId").val(),
			API_DateOfJoining: $("#doj").val(),
			API_JobTitleID: $("#job_title").val()
        }
        $.ajax({
            url: base_url+'/NewAdmin/SaveApplicantInfo',
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
			data: JSON.stringify(myData),
			success: function (response) {
				if (response == true) {
					alert("Employee information is saved successfully");
					$("#myModalForAddEmployee").modal("hide");
				}
				else {
					alert("Something went wrong try again!!!");
					$("#myModalForAddEmployee").modal("hide");
				}
			}
        });
    }
    else {
        alert("Invalid");
    }


}
function getStateList() {
	$('#onboardState').empty()
	$.ajax({
		type: "GET",
		url: base_url+"/NewAdmin/GetStateList",
		success: function (data) {
				$('#onboardState').append('<option value="-1">--Select--</option>');
			// Use jQuery's each to iterate over the opts value
			$.each(data, function (i, d) {
				// You will need to alter the below to get the right values from your json object.  Guessing that d.id / d.modelName are columns in your carModels data
				$('#onboardState').append('<option value="' + d.StateId + '">' + d.StateName + '</option>');
			});
		}
	});
}

function getApplicantInfo() {
    $.ajax({
        type: 'GET',
        url: base_url+"/NewAdmin/GetApplicantInfo",
        success: function (data) {
            datadetails = data;
            //var data;
            $("#ListOnBoarding").jsGrid({
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
                data: data,
                onRefreshed: function (args) {
                    $(".jsgrid-insert-row").hide();
                    $(".jsgrid-filter-row").hide()
                    $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

                },
                fields: [
                    { name: 'API_FirstName', width: 160, title: "Employee Name", css: "text-center" },//visible: true
                    { name: 'API_PhoneNumber', width: 150, title: "Mobile No" },
                    { name: "API_Email", width: 150, title: "E-mail" },
                    { name: "API_Date", width: 150, title: "Date" },
                    {
                        name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                            debugger
                            var spanq = $("<span>").attr({ class: "mdi mdi-power" });
                            var $iconPencil = $("<button>").attr({ class: "btn btn-primary" }).attr({ value: "Add", id: "AddEmployee" }).attr({ style: "color:white;background-color:green;border-radius:35px;" }).append(spanq);
                            var $iconTrash = $("<input>").attr({ type: "button" }).attr({ class: "btn btn-primary" }).attr({ value: "Verify" }).attr({ style: "color:white;background-color:gray;border-radius:35px;" });;
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                    //call function to render the page
                                    debugger;
                                    API_id = item.API_ApplicantId;
                                    addNewOnboardingDetail(item);
                                    
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                .attr({ id: "btn-delete-" + item.Id }).click(function (e) {
                                    API_id = item.API_ApplicantId;
                                    $("#myModalToShowLocation").modal("show");                                   
                                    e.stopPropagation();
                                }).append($iconTrash);

                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton);
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
            return details;
        }
    });
    return details;
}

function checkempID() {
	var empId = $("#empid").val();
	if (empId == '')
		return;
    $.ajax({
        url: base_url+'/NewAdmin/ValidateEmployeeID?empId=' + empId,
        type: 'GET',
        success: function (data) {
			if (data == true) {
				$("#empid").val("");
				$("#msg").text("Employee Id Already Exists");
				$("#msg").show();
				$("#btnGenerate").hide();
			}
			else {
				$("#btnGenerate").show();
				$("#msg").hide();
			}
        }
    });
}

function VerifyEmployee() {
    debugger
    var object = new Object();
    object.Status = "I";
    object.App_Id = API_id;
    object.LocationId = $("#ddl_LocationForEmployee option:selected").val();
    $.ajax({
        type: "POST",
        url: base_url + "/NewAdmin/VerifyEmployeeAfterGenerate",
        data: { onboardingDetailRequestModel: object },
        success: function (Data) {
            debugger
            $("#myModalToShowLocation").modal("hide");
        },
        error: function (err) {
        }
    });

}
