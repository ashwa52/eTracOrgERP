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
function SubmitForm() {

	var isSubmit = confirm('Are you sure you want to submit');

	if (isSubmit) {
		$.ajax({
			type: "POST",
			url: '/Guest/_DirectDepositeForm',
			data: $('#depositeForm').serialize(),
			success: function (result) {
				if (result == true) {
					location.href = '/Guest/PersonalFile?isSaved=true';
				} else {
					$("#formid").html(result);
					$("#formModel").modal('show');
				}
			},
			error: function () {

			}
		});
	}
}

function ThankYou() {
	location.href = '/Guest/ThankYou';
}