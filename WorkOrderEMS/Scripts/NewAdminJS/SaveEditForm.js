﻿function SubmitForm(element, formName) {
    debugger
    var url = '';
    var data = '';
    var isSubmit = confirm('Are you sure you want to submit');
    if (isSubmit) {
        switch (formName.id) {
            case 'depositeForm':
                url = '../EPeople/_DirectDepositeForm';
                data = $('#depositeForm').serialize();
                break;
            case 'employeeHandbook':
                url = '../EPeople/_EmployeeHandbook';
                data = $('#employeeHandbook').serialize();
                break;
        }
            debugger
            $.ajax({
                url: url,
                data: data,
                type: 'POST',
                success: function (response) {
                    debugger
                    if (response == true) {
                        // LockItem(formName.id);
                        $("#formModelEdit").modal('hide');
                    }
                    else {
                        $(element).prop('checked', false);
                        $("#formidEditInfo").html(result);
                        $("#formModelEdit").modal('show');
                    }
                },
                error: function () {
                    alert('Something went wrong!!!!');
                    $("#formModelEdit").modal('hide');
                }
            });
       
    }
}

//function LockItem(formId) {
//    debugger
//    switch (formId) {
//        case 'depositeForm':
//            var elm = $("#directdepositeicn").find('.lock i').first();
//            elm.removeClass('fa-unlock');
//            elm.addClass('fa-lock');
//            break;
//        case 'employeeHandbook':
//            var elm = $("#employeeHandbookicn").find('.lock i').first();
//            elm.removeClass('fa-unlock');
//            elm.addClass('fa-lock');
//            break;
//    }
//}