function OpenForm(formname) {
	var geturl = '';

	switch (formname) {
		case 'directdeposite':
			geturl = '/Guest/_DirectDepositeForm';
			break;
		case 'employeehandbook':
			geturl = '/Guest/_EmployeeHandbook';
			break;
		case 'W4Form':
			geturl = '/Guest/_W4Form';
			break;
		case 'I9Form':
			geturl = '/Guest/_I9Form';
			break;
		case 'EmergencyContectForm':
			geturl = '/Guest/_EmergencyContectForm';
			break;
		case 'photorelease':
			geturl = '/Guest/_PhotoReleaseForm';
			break;
		case 'confidentiality':
			geturl = '/Guest/_ConfidentialityAgreementForm';
			break;
		case 'educationVerificationForm':
			geturl = '/Guest/_EducationVarificationForm';
			break;
		case 'creditCardAuthorizationForm':
			geturl = '/Guest/_CreditCardAuthorizationForm';
			break;
		case 'previousEmployeement':
			geturl = '/Guest/_PreviousEmployeement';
			break;
	}
	GetForm(geturl, function (successRes) {
		$("#formid").html(successRes);
	}, function (errorRes) {
		alert('Something went wrong!!!');
	});
	
	$("#formModel").modal('show');
}
function closeModel() {
	$("#formModel").modal('hide');
}
function SubmitForm(element, formName) {
	var url = '';
	var data = '';
	var isSubmit = confirm('Are you sure you want to submit');
	if (isSubmit) {
		switch (formName.id) {
			case 'depositeForm':
				url = '/Guest/_DirectDepositeForm';
				data = $('#depositeForm').serialize();
				break;
			case 'employeeHandbook':
				url = '/Guest/_EmployeeHandbook';
				data = $('#employeeHandbook').serialize();
				break;
		}
		PostForm(url, data, function (successResponse) {
			if (successResponse == true) {
				LockItem(formName.id);
				$("#formModel").modal('hide');
			}
			else {
				$(element).prop('checked', false);
				$("#formid").html(result);
				$("#formModel").modal('show');
			}
		}, function (errorResponse) {
			alert('Something went wrong!!!!');
			$("#formModel").modal('hide');
		});
	}

	//if (isSubmit) {
	//	$.ajax({
	//		type: "POST",
	//		url: '/Guest/_DirectDepositeForm',
	//		data: $('#depositeForm').serialize(),
	//		success: function (result) {
	//			if (result == true) {
	//				location.href = '/Guest/PersonalFile?isSaved=true';
	//			} else {
	//				$("#formid").html(result);
	//				$("#formModel").modal('show');
	//			}
	//		},
	//		error: function () {

	//		}
	//	});
	//}
}
function PostForm(url, data, successCallback, errorCallback) {
	$.ajax({
		type: "POST",
		url: url,
		data: data,
		success: successCallback,
		error: errorCallback
	});
}
function GetForm(url, successCallback, errorCallback) {
	$.ajax({
		type: "GET",
		url: url,
		success: successCallback,
		error: errorCallback
	});
}

function ThankYou() {
	location.href = '/Guest/ThankYou';
}
function LockItem(formId) {
	switch (formId) {
		case 'depositeForm':
			var elm = $("#directdepositeicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
		case 'employeeHandbook':
			var elm = $("#employeeHandbookicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
	}
}