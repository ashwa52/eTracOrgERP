function OpenForm(formname) {
	if (formname === 'directdeposite') {
		$.ajax({
			url: '/Guest/_DirectDepositeForm',
			method: 'GET',
			success: function (response) {
				$("#formid").html(response);
			}
		});
	}
	if (formname === 'employeehandbook') {
		$.ajax({
			url: '/Guest/_EmployeeHandbook',
			method: 'GET',
			success: function (response) {
				$("#formid").html(response);
			}
		});
	}
	$("#formModel").modal('show');
}
function closeModel() {
	$("#formModel").modal('hide');
}
function SubmitForm(element,formName) {
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