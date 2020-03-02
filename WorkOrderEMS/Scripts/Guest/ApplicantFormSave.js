
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
    var isBenifitSectionPopUpOpen = false;
    //var isSubmit = confirm('Are you sure you want to submit');
    
    //if (isSubmit) {
    if (formName.id == undefined || formName.id == null) {
        formName.id = formName[0].id;
    }
        switch (formName.id) {
            case 'depositeForm':
                url = '/Guest/_DirectDepositeForm';
                data = $('#depositeForm').serialize();
                isDirectDepositeSaved = true;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'employeeHandbook':
                url = '/Guest/_EmployeeHandbook';
                data = $('#employeeHandbook').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'photoreleaseform':
                url = '/Guest/_photoreleaseform';
                data = $('#photoreleaseform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'emergencycontactform':
                url = '/Guest/_emergencyContactForm';
                data = $('#emergencycontactform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'confidentialityagreementform':
                url = '/Guest/_ConfidentialityAgreementForm';
                data = $('#confidentialityagreementform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'educationverificationform':
                url = '/Guest/_EducationVarificationForm';
                data = $('#educationverificationform').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'w4form':
                url = '/Guest/_W4Form';
                data = $('#w4form').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'I9Form':
                
                url = '/Guest/_I9Form';
                data = $('#I9Form').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false
                break;
            case 'ContactSavedForm':
                
                url = '/Guest/_ContactSavedForm';
                data = $('#ContactSavedForm').serialize();
                isDirectDepositeSaved = false;
                isBackgroundCheck = false;
                isContactSaved = true;
                isBenifitSectionPopUpOpen = false
                //element.preventDefault();
                break;
            case 'BackGroundCheckForm':                
                url = '/Guest/_BackGroundCheckForm';
                data = $('#BackGroundCheckForm').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = true;
                isBenifitSectionPopUpOpen = false
                //element.preventDefault();
                break;
            case 'SaveBenifit':
                url = '/Guest/BenifitSection';
                data = $('#SaveBenifit').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = true;
                //element.preventDefault();
                break;
            case 'selfIdentificationForm':
                url = '/Guest/SaveSelfIdentificationForm';
                data = $('#selfIdentificationForm').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false;
                //element.preventDefault();
                break;
            case 'ApplicantFunFactForm':
                url = '/Guest/ApplicantFunFact';
                data = $('#ApplicantFunFactForm').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false;
                //element.preventDefault();
                break;
            case 'RateOfPay':
                url = '/Guest/_RateOfPay';
                data = $('#RateOfPay').serialize();
                isDirectDepositeSaved = false;
                isContactSaved = false;
                isBackgroundCheck = false;
                isBenifitSectionPopUpOpen = false;
                //element.preventDefault();
                break;
                
        }
        if (isContactSaved == false) {
            PostForm(url, data, isDirectDepositeSaved,isBackgroundCheck,isBenifitSectionPopUpOpen, function (successResponse) {
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
    
    $_this = this;
    var getVal;
    if ($_this.checked == true) {
         getVal = $($_this).attr("value");
        details.push({  ContactId: getVal,IsChecked:"Y" });
    } else {
         getVal = $($_this).attr("value");
        details.push({  ContactId: getVal,IsChecked:"N" });
        }
})
function PostForm(url, data,isDirectDepositeSaved,isBackgroundCheck,isBenifitSectionPopUpOpen, successCallback, errorCallback) {
    
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
        success:function (Data) {
            debugger
            var base_url = window.location.origin;
            //this is to open popup when direct deposite form save so will not open anything jsut open contact info popup
            if (isDirectDepositeSaved == true) {
                
                $.ajax({
                    type: "GET",
                    url: base_url + "/Guest/_ContactInfo",
                    success: function (data) {                       
                        $("#openConatctModal").html(data);
                        $("#myModelForContactDetails").modal("show");
                    },
                    error: function () {
                    }
                });
                isDirectDepositeSaved = false;
            }
                //once backgroud save the open popup for document saved
            else if (isBackgroundCheck == true && Data == true) {
                        $('#myModalToAddDocumentUpload').modal('show', { backdrop: 'static', keyboard: false });
            }
                //afetr finished all the process all the form need to sign by the employee so after signed background form need to redirect to education verification form
            //else if (isBackgroundCheck == true && Data == false) {
            //    debugger
            //    $.ajax({
            //        type: "GET",
            //        url: base_url + "/Guest/_EducationVarificationForm",
            //        success: function (data) {
            //            debugger
            //            $("#RenderPageId").html(data);
            //        },
            //        error: function (err) {
            //            alert(err)
            //        }
            //    });
                //isBackgroundCheck = false;
            //}
                //once benifit saved then open popup to show disclamer of benifit
            else if (isBenifitSectionPopUpOpen == true) {
                $("#myModelForDesclaimerEEO").modal('show');
            }
            else {
                $("#RenderPageId").html(Data);
            }
        },
        error: errorCallback,
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function SaveContact(data, details, url, errorCallback) {
    
    $.ajax({
        type: "POST",
        url: url,
        data: { model: data, lstModel: details },
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            $('#myModelForContactDetails').modal('hide');          
            $("#RenderPageId").html(Data);
            $('.fade').removeClass('modal-backdrop');
            $('.fade').removeClass('show');
        },
        error: errorCallback,
        complete: function () {
            fn_hideMaskloader();
        }
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
            elm = $("#directdepositeicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'employeeHandbook':
            var elm = $("#employeehandbookicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#employeehandbookicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'photoreleaseform':
            var elm = $("#photoreleaseicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#photoreleaseicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'emergencycontactform':
            var elm = $("#emergencycontactformicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
             elm = $("#emergencycontactformicn").find('.bluesky').first();
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
             elm = $("#educationverificationformicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'w4form':
            var elm = $("#w4formicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
             elm = $("#w4formicn").find('.bluesky').first();
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
var signatureName="" ; 
$("#AddSignature").click(function (e) {
    
    var url = window.location.origin;
    $.ajax({
        url: url + '/Guest/GetSignature',
        type: "POST",      
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {            
            if(result != null && result != false){
                //$("#myModalForSignatureApplicant").modal('show');
                $("#openItForSignature").attr("src", result.imagePath);
                $("#getSignName").val(result.name);
                $("#ShowHideSignatureSpan,#AddSignature").hide();
                $("#openItForSignature").show();
                signatureName = name;
            }
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }        
    });    
})
$('.savesignaturInfo').click(function(){
    var url = window.location.origin;
    $_this = this;
    var isUpdate = $($_this).attr("is-update");
    $.ajax({
        url: url + '/Guest/SaveSignature?isUpdate='+isUpdate,
        type: "POST",      
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            
            if(result != null && result != false){
                $("#myModalForSignatureApplicant").modal('show');
                $("#openItForSignature").attr("src",result.imagePath);
                $("#ShowHideSignatureSpan").hide();
                $("#openItForSignature").show();
                signatureName = name;
            }
        },
        error: function (err) {

        },
        complete: function () {
            fn_hideMaskloader();
        }        
    });  
});
$("#UploadDocApplicant").click(function () {
    debugger
    var url = window.location.origin;
    //get(0) its return the first element of jquery object(jquery object is also an array), first file to be upload
    var fileUploadLicense = $("#myLicensefileUpload").get(0);   
    var filesLicense = fileUploadLicense.files;    
    // Create FormData object  
    // FormData interface enables appending File objects to XHR-requests (Ajax-requests).
    var fileData = new FormData();
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < filesLicense.length; i++) {
        fileData.append(filesLicense[i].name, filesLicense[i]);
    }   
    $.ajax({
        url: url + '/Guest/UploadFilesApplicant?isLicense='+true,
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data:   fileData ,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            
            var fileUploadSSN = $("#mySSNfileUpload").get(0);
            var filesSSN = fileUploadSSN.files;
            fileData = new FormData();
            for (var i = 0; i < filesSSN.length; i++) {
                fileData.append(filesSSN[i].name, filesSSN[i]);
            }
            $.ajax({
                url: url + '/Guest/UploadFilesApplicant?isLicense='+false,
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (result) {
                    $("#RenderPageId").html(result);
                    $("#myModalToAddDocumentUpload").hide();
                    $('.fade').removeClass('modal-backdrop');
                    $('.fade').removeClass('show');
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

function GotoNextForm(istrue) {
    debugger
    var url = window.location.origin;
    if (istrue) {
         url +=  '/Guest/SelfIdentificationForm';
    } else {
        url += '/Guest/ApplicantFunFacts';
    }
    $.ajax({
        url: url,
        type: "GET",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            $("#RenderPageId").html(result);
            $("#myModelForDesclaimerEEO").hide();
            $('.fade').removeClass('modal-backdrop');
            $('.fade').removeClass('show');
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}


$("#AddSignatureTranslator").click(function () {
    debugger
    $("#myModalForSignatureApplicant").modal('show');
})

$(function () {
    debugger
    $('#colors_sketchApplicant').sketch();
    $(".tools a").eq(0).attr("style", "color:#000");
    $(".tools a").click(function () {
        $(".tools a").removeAttr("style");
        $(this).attr("style", "color:#000");
    });
    $("#btnSaveTranslator").bind("click", function () {
        debugger
        var base64 = $('#colors_sketchApplicant')[0].toDataURL();
        $("#openForTranslator").attr("src", base64);
        $("#openForTranslator").show();
        $("#ShowHideSignatureTranslatorSpan, #AddSignatureTranslator").hide();
        $("#SignatureImageBase").val(base64);
    });
});
$("#AddSignatureManager").click(function () {
    debugger
    $("#myModalForSignatureManager").modal('show');
})

$(function () {
    debugger
    $('#colors_sketchManager').sketch();
    $(".tools a").eq(0).attr("style", "color:#000");
    $(".tools a").click(function () {
        $(".tools a").removeAttr("style");
        $(this).attr("style", "color:#000");
    });
    $("#btnSaveManager").bind("click", function () {
        debugger
        var base64 = $('#colors_sketchApplicant')[0].toDataURL();
        $("#openItForSignatureManager").attr("src", base64);
        $("#openItForSignatureManager").show();
        $("#ShowHideSignatureSpanMan, #AddSignatureManager").hide();
        $("#SignatureImageBase").val(base64);
    });
});

//$("#").
///============================End Code======================================================
//===========================================================================================