function OpenForm(formname,elm) {
	var geturl = '';
	var isLocked = false;
	$(elm).find('i').each(function (i, ielm) {
		if ($(ielm).hasClass('fa-lock'))
			isLocked=true;
	});
	if (isLocked)
		return;
	unblink('w4formicn');
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
			geturl = '/Guest/_emergencyContactForm';
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
			case 'photoreleaseform':
				url = '/Guest/_photoreleaseform';
				data = $('#photoreleaseform').serialize();
				break;
			case 'emergencycontactform':
				url = '/Guest/_emergencyContactForm';
				data = $('#emergencycontactform').serialize();
				break;
			case 'emergencycontactform':
				url = '/Guest/_emergencyContactForm';
				data = $('#emergencycontactform').serialize();
				break;
			case 'confidentialityagreementform':
				url = '/Guest/_ConfidentialityAgreementForm';
				data = $('#confidentialityagreementform').serialize();
				break;
			case 'educationverificationform':
				url = '/Guest/_EducationVarificationForm';
				data = $('#educationverificationform').serialize();
				break;
			case 'w4form':
				url = '/Guest/_W4Form';
				data = $('#w4form').serialize();
				break;
		}
		PostForm(url, data, function (successResponse) {
			if (successResponse == true) {
				LockItem(formName.id);
				$("#formModel").modal('hide');
			}
			else {
				
				$("#formid").html(successResponse);
				$("#formModel").modal('show');
				$(element).prop('checked', false);
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
		case 'photoreleaseform':
			var elm = $("#photoreleaseicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
		case 'emergencycontactform':
			var elm = $("#emergencycontactformicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
		case 'confidentialityagreementform':
			var elm = $("#confidentialityagreementicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
		case 'educationverificationform':
			var elm = $("#educationverificationformicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			break;
		case 'w4form':
			var elm = $("#w4formicn").find('.lock i').first();
			elm.removeClass('fa-unlock');
			elm.addClass('fa-lock');
			var elm = $("#w4formicn").find('.bluesky').first();
			elm.removeClass('bluesky');
			elm.addClass('grn-icn');
			break;
	}
}
function blink(id) {
	$("#"+id).fadeOut('slow', function () {
		$(this).fadeIn('slow', function () {
			blink(this.id);
		});
	});
}
function unblink(id) {
	$("#" + id).stop().fadeIn();
	NextBlink(id);
} 
blink('w4formicn');
function NextBlink(id) {
	var siblingid = $("#" + id).siblings()[0].id;
	blink(siblingid);
}