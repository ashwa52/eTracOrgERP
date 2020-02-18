
var details = [];
function OpenForm(formname, elm) {
    var geturl = '';
    var isLocked = false;
    $(elm).find('i').each(function (i, ielm) {
        if ($(ielm).hasClass('fa-lock'))
            isLocked = true;
    });
    if (isLocked) {
        return;
    }
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
        case 'EmergencyContactForm':
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
    debugger
    var url = '';
    var data = '';
    var isDirectDepositeSaved = false;
    var isContactSaved = false;
    var isBackgroundCheck = false;
    //var isSubmit = confirm('Are you sure you want to submit');
    debugger
    //if (isSubmit) {
        debugger
        switch (formName.id) {
            case 'depositeForm':
                url = '/Guest/_DirectDepositeForm';
                data = $('#depositeForm').serialize();
                isDirectDepositeSaved = true;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'employeeHandbook':
                url = '/Guest/_EmployeeHandbook';
                data = $('#employeeHandbook').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'photoreleaseform':
                url = '/Guest/_photoreleaseform';
                data = $('#photoreleaseform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'emergencycontactform':
                url = '/Guest/_emergencyContactForm';
                data = $('#emergencycontactform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'confidentialityagreementform':
                url = '/Guest/_ConfidentialityAgreementForm';
                data = $('#confidentialityagreementform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'educationverificationform':
                url = '/Guest/_EducationVarificationForm';
                data = $('#educationverificationform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'w4form':
                url = '/Guest/_W4Form';
                data = $('#w4form').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'I9Form':
                debugger
                url = '/Guest/_I9Form';
                data = $('#I9Form').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                break;
            case 'ContactSavedForm':
                debugger
                url = '/Guest/_ContactSavedForm';
                data = $('#ContactSavedForm').serialize();
                isDirectDepositeSaved = false;
                isBackgroundCheck = false;
                isContactSaved = true;
                //element.preventDefault();
                break;
            case 'BackGroundCheckForm':
                debugger
                url = '/Guest/_BackGroundCheckForm';
                data = $('#BackGroundCheckForm').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = true;
                //element.preventDefault();
                break;
                
        }
        if (isContactSaved == false) {
            PostForm(url, data, isDirectDepositeSaved,isBackgroundCheck, function (successResponse) {
                if (successResponse == true) {
                    LockItem(formName.id);
                    $("#formModel").modal('hide');
                    location.href = '/Guest/PersonalFile';
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
        else {
            debugger
            SaveContact(data, details, url, function (errorCallback) {
                alert('Something went wrong!!!!');
                $("#formModel").modal('hide');
            });
        }
    //}

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
$(".isCheckedContact").click(function () {
    debugger
    $_this = this;
    if ($_this.checked == true) {
        var getVal = $($_this).attr("value");
        details.push({  ContactId: getVal,IsChecked:"Y" });
    } else {
        var getVal = $($_this).attr("value");
        details.push({  ContactId: getVal,IsChecked:"N" });
        }
})
function PostForm(url, data,isDirectDepositeSaved,isBackgroundCheck, successCallback, errorCallback) {
    debugger
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        //success: successCallback,
        success:function (Data) {
            debugger
            var base_url = window.location.origin;
            if (isDirectDepositeSaved == true) {
                
                $.ajax({
                    type: "GET",
                    url: base_url + "/Guest/_ContactInfo",
                    success: function (data) {
                        debugger
                        $("#openConatctModal").html(data);
                        //$('#myModelForContactDetails').modal('show', { backdrop: 'static', keyboard: false });
                        $("#myModelForContactDetails").modal("show");
                    },
                    error: function () {

                    }
                });
                isDirectDepositeSaved = false;
            }
            else if (isBackgroundCheck == true) {
                //$.ajax({
                //    type: "GET",
                //    url: base_url + "/Guest/_ContactInfo",
                //    success: function (data) {
                //        debugger
                        //$("#openModalForDocument").html(data);
                        $('#myModalToAddDocumentUpload').modal('show', { backdrop: 'static', keyboard: false });
                        //$("#myModalToAddDocumentUpload").modal("show");
                //    },
                //    error: function () 
                //    }
                //});
            }
            else {
                //var url = "../Guest/_W4Form";
                $("#RenderPageId").html(Data);
            }
        },
        error: errorCallback
    });
}
function SaveContact(data, details, url, errorCallback) {
    debugger
    $.ajax({
        type: "POST",
        url: url,
        data: { model: data, lstModel: details },
        success: function (Data) {
            debugger
            //$('#myModelForContactDetails').modal('hide', { backdrop: '', keyboard: true });
            
            $('#myModelForContactDetails').modal('hide');          
            $("#RenderPageId").html(Data);
            $('.fade').removeClass('modal-backdrop');
            $('.fade').removeClass('show');
        },
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
            var elm = $("#directdepositeicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'employeeHandbook':
            var elm = $("#employeehandbookicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#employeehandbookicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'photoreleaseform':
            var elm = $("#photoreleaseicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#photoreleaseicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'emergencycontactform':
            var elm = $("#emergencycontactformicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#emergencycontactformicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'confidentialityagreementform':
            var elm = $("#confidentialityagreementicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#confidentialityagreementicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'educationverificationform':
            var elm = $("#educationverificationformicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#educationverificationformicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
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
function blink() {
    $(".blink").fadeOut('slow', function () {
        $(this).fadeIn('slow', function () {
            blink(this.id);
        });
    });
}
function unblink(id) {
    $("#" + id).stop().fadeIn();
    NextBlink(id);
}
//blink('w4formicn');
function NextBlink(id) {

    var siblingid = $("#" + id).next()[0].id;
    blink(siblingid);
}
function SetFormStatus() {
    $.ajax({
        url: '/Guest/GetFormsStatus',
        method: 'GET',
        success: function (response) {
            debugger;
            if (response.W4Status == 'Y') {
                $("#w4formicn").removeClass('blink');
                $($("#w4formicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('w4form');

            }
            if (response.EmergencyContactFormStatus == 'Y') {
                $("#emergencycontactformicn").removeClass('blink');
                $($("#emergencycontactformicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('emergencycontactform');

            }
            if (response.EmployeeHandbook == 'Y') {
                $("#employeehandbookicn").removeClass('blink');
                $($("#employeehandbookicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('employeehandbook');
            }
            if (response.DirectDepositForm == 'Y') {
                $("#directdepositeicn").removeClass('blink');
                $($("#directdepositeicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('depositeForm');

            }
            if (response.PhotoReleaseForm == 'Y') {
                $("#photoreleaseicn").removeClass('blink');
                $($("#photoreleaseicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn')

            }
            if (response.ConfidentialityAgreement == 'Y') {
                $("#confidentialityagreementicn").removeClass('blink');
                $($("#confidentialityagreementicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn')

            }
            if (response.EducationVerificationForm == 'Y') {
                $("#educationverificationformicn").removeClass('blink');
                $($("#educationverificationformicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('educationverificationform');

            }
            var $div2blink = $(".blink"); // Save reference, only look this item up once, then save
            var backgroundInterval = setInterval(function () {
                $div2blink.toggleClass("backgroundRed");
            }, 1500)
        }
    });
}

//////=========================Next From Background check============================//////
//=========================================================================================
$("#AddSignatureForBackground").click(function () {

})
$("#UploadDocApplicant").click(function () {
    debugger
    var url = window.location.origin;
    var fileUploadLicense = $("#myLicensefileUpload").get(0);   
    var filesLicense = fileUploadLicense.files;    
    // Create FormData object  
    var fileDataLicense = new FormData();
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < filesLicense.length; i++) {
        fileDataLicense.append(filesLicense[i].name, filesLicense[i]);
    }   
    $.ajax({
        url: url + '/Guest/UploadFilesApplicant',
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data:   fileData ,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            debugger
            var fileUploadSSN = $("#mySSNfileUpload").get(0);
            var filesSSN = fileUploadSSN.files;
            var fileDataSSN = new FormData();
            for (var i = 0; i < filesSSN.length; i++) {
                fileDataSSN.append(filesSSN[i].name, filesSSN[i]);
            }
            $.ajax({
                url: url + '/Guest/UploadFilesApplicant',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (result) {
                    debugger
                },
                error: function (err) {

                },
                complete: function () {
                    fn_hideMaskloader();
                }
            });
            $("#myModalToAddDocumentUpload").modal('hide');
            //alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
});
///============================End Code======================================================
//===========================================================================================