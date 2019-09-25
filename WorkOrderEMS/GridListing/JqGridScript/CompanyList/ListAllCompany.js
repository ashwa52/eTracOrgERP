var Companyurl = 'VendorManagement/GetAllCompanyListList';
var AddCompany = 'VendorManagement/VendorManagementSetup/';
var editCompany = 'VendorManagement/EditVendor/';
var addAccountDetails = 'VendorManagement/AddAccountDetails/';
var GridListLicense = 'VendorManagement/ListInsuranceLicenseView';
var addFacilityDetails = 'VendorManagement/AddFacilityDetails/';
var addFileImport = 'VendorManagement/FileImport/';

var ListAccountDetails = 'VendorManagement/ListAccountOfVendor/';
//var EditPO = 'POTypeData/EditPOByPOId/';
//var POType = ''
//+ '<select id="ApproveData" class="" onchange="doSearch(arguments[0]||event);">'
//+ '<option value="Approve">Approved PO</option>'
//+ '<option value="NotApprove">Not Approved</option>'
//+ '</select>';
var LocationId; var VendorId;

$(function () {
    var act;
    $("#tbl_AllCompanyDataList").jsGrid({
        height: "170%",
        width: "100%",
        filtering: false,
        editing: false,
        inserting: false,
        sorting: false,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: $_HostPrefix + Companyurl + '?LocationId=' + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            //{ name: "VendorId", title: "Vendor Id", type: "text", width: 50 ,hidden:true },
            { name: "CompanyNameLegal", title: "Vendor Name", type: "text", width: 50 },
            { name: "Address1", title: "Address", type: "text", width: 50 },
            { name: "Phone1", title: "Phone Number", type: "text", width: 50 },
            { name: "PointOfContact", title: "Point Of Contact", type: "text", width: 50 },
            { name: "VendorTypeData", title: "Vendor Type", type: "text", width: 50 },
            //{ name: "Status", title: "Status", type: "text", width: 50, hidden: true},
            {
               
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item)
                { 
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style:"color:green;font-size: 22px;" });
                    var $iconPencilForAccount = $("<i>").attr({ class: "fa fa-university" }).attr({ style:"color:#ee82ee;font-size: 22px;" });
                    var $iconPencilForFacility = $("<i>").attr({ class: "fa fa-list" }).attr({ style:"color:#3cb371;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style:"color:bluelight;font-size: 22px;" });
                    var $iconPencilForInsurance = $("<i>").attr({ class: "fa fa-medkit" }).attr({ style: "color:#ffa500;font-size: 22px;" });
                    var $iconPencilForImport = $("<i>").attr({ class: "fa fa-upload" }).attr({ style: "color:#0080ff;font-size: 22px;" });
                     
                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Edit" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {                   
                        window.location.href = $_HostPrefix + editCompany + '?id=' + item.id;
                    }).append($iconPencilForEdit);

                    var $customButtonForAccount = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Account Details" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        window.location.href = $_HostPrefix + ListAccountDetails + '?VendorId=' + item.id;
                    }).append($iconPencilForAccount);

                    var $customButtonForFacility = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Facility Details" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        window.location.href = $_HostPrefix + addFacilityDetails + '?id=' + item.id;
                    }).append($iconPencilForFacility);

                    var $customButtonForView = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "View " }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        ViewVendorDetails(item.VendorId);
                    }).append($iconPencilForView);

                    var $customButtonForInsurance = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Insurance Details" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + item.id + "&VendorStatus=" + $_VendorStatus;;
                    }).append($iconPencilForInsurance);

                    var $customButtonFileImport = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "File Import" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        window.location.href = $_HostPrefix + addFileImport + '?id=' + item.id;
                    }).append($iconPencilForImport);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customButtonForAccount).append($customButtonForFacility).append($customButtonForView).append($customButtonForInsurance).append($customButtonFileImport);
                  }
            }
        ]
    });
});

var timeoutHnd;
var flAuto = true;
//function doSearch(ev) {
//    if (timeoutHnd)
//        clearTimeout(timeoutHnd)
//    timeoutHnd = setTimeout(gridReload, 500)
//}
//function gridReload() {

//    var txtSearch = jQuery("#SearchText").val();
//    jQuery("#tbl_AllCompanyDataList").jqGrid('setGridParam', { url: $_HostPrefix + Companyurl + "?&LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
//}
$(".EditRecord").on("click", function (event) {
   
});
$("#FacilityListVendorData").on("click", function (event) {
    var fid = $(this).attr("fid");
    window.location.href = $_HostPrefix + addFacilityDetails + '?id=' + fid ;
});
$("#addAccountdetails").on("click", function (event) {
    var id = $(this).attr("aid");
    window.location.href = $_HostPrefix + ListAccountDetails + '?VendorId=' + id;
});

$("#AddInsurance").on("click", function (event) {
    var id = $(this).attr("InsuraceVendorId");
    //window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id, '?VendorStatus=' + $_VendorStatus;
    window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id + "&VendorStatus=" + $_VendorStatus;
    //window.location.href = $_HostPrefix + addInsuranceAndLicenseDetails + '?id=' + id;
});

$("#AddCompany").on("click", function (event) {
    window.location.href = $_HostPrefix + AddCompany;
});


function ViewVendorDetails(VendorId) { 
   // alert(VendorId);
    //var VendorName = rowData['CompanyNameLegal'];
    //$("#lblVendorName").html(VendorName);
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    //if (rowData.Status == "Y")
    //{
    //    $("#btnApproveData").hide();
    //    $('#btnRejectPO').hide();
    //}
    //else
    //{
    //    $("#btnApproveData").show();
    //    $('#btnRejectPO').show();
    //}
    $.ajax({
        type: "post",
        url: '../VendorManagement/GetAllVendorDataToView' + '?VendorId=' + VendorId,
        datatype: 'json',
        success: function (result) {
            $("#lblVendorNameLegal").html(result.CompanyNameLegal); $("#lblVendorNameDBA").html(result.CompanyNameDBA);
            $("#lblVendorType").html(result.VendorTypeData); $("#lblPointOfContact").html(result.PointOfContact);
            $("#lblAddress").html(result.Address1); $("#lblPhone1").html(result.Phone1);
            $("#lblPhone2").html(result.Phone2); $("#lblEmail").html(result.Email);
            $("#lblWebsite").html(result.Website); $("#lblLicenseName").html(result.CompanyNameLegal);
            $("#lblLicenseNumber").html(result.LicenseNumber); $("#lblLicenseExpirationDate").html(result.LicenseExpirationDate);
            $("#lblInsuranceCarries").html(result.InsuranceCarries); $("#lblPolicyNumber").html(result.PolicyNumber);
            $("#lblInsuranceExpirationDate").html(result.InsuranceExpirationDate); $("#lblFirstCompany").html(result.CompanyNameLegal);
            $("#lblSecondaryCompany").html(result.SecondaryCompany); $("#lblVendorTypeContract").html(result.VendorTypeData);
            $("#lblContractType").html(result.ContractType); $("#lblContractissuedby").html(result.ContractIssuedBy);
            $("#lblContractexecutedby").html(result.ContractExecutedBy);
            $("#lblPrimaryPaymentMode").html(result.PrimaryPaymentMode); $("#lblPaymentTerm").html(result.PaymentTerm);
            $("#lblGracePeriod").html(result.GracePeriod); $("#lblInvoicingFrequency").html(result.InvoicingFrequecy);
            $("#lblStartDate").html(result.StartDate);
            $("#lblEndDate").html(result.EndDate); $("#lblCostDuringPeriod").html(result.CostDuringPeriod);
            $("#lblAnnualValueOfAggriment").html(result.AnnualValueOfAggriment); $("#lblMinimumBillAmount").html(result.MinimumBillAmount);
            $("#lblBillDueDate").html(result.BillDueDate);
            $("#lblLateFineFee").html(result.LateFine); $("#lblPayMode").html(result.PrimaryPaymentMode);
            $("#lblBankName").html(result.BankName); $("#lblBankLocation").html(result.BankLocation);
            $("#lblAccountNumber").html(result.AccountNumber); $("#lblIFSCCode").html(result.IFSCCode);
            $("#lblSwiftOICCode").html(result.SwiftOICCode); $("#lblCardNumber").html(result.CardNumber);
            $("#lblCardHolderName").html(result.CardHolderName);
            $("#lblExpirationDate").html(result.ExpirationDate);
            $("#lblPolicyNumber").html(result.PolicyNumberAccount); 
            if (result.LocationAssignedModel != null) {
                if (result.LocationAssignedModel.length > 0) {
                    var arr = [];
                    //var llcmArr = [];
                    for (i = 0; i < result.LocationAssignedModel.length; i++) {
                        arr.push(result.LocationAssignedModel[i].LocationName);
                        //llcmArr.push(result.LocationAssignedModel[i].LLCM_Id)
                    }
                    var loc = arr.join(',');
                    //var llcm = llcmArr.join(',');
                    $("#lblSelectedLocation").html(loc);
                    //$('#LLCM_Id').html(llcm);
                }
            }
            if (result.VendorFacilityModel != null) {
                $('#records_table').html('');
                var arrData = [];
                var thHTML = '';
                $('#VendorFacility_table').empty();
                thHTML += '<tr style="background-color:#0792bc;"><th>Cost Code</th><th>Facility Type</th><th>Description</th><th>Unit Price</th><th>Tax</th></tr>';
                $('#VendorFacility_table').append(thHTML);
                if (result.VendorFacilityModel.length > 0) {
                    for (i = 0; i < result.VendorFacilityModel.length; i++) {
                        var trHTML = '';
                        trHTML +=
                           '<tr><td>' + result.VendorFacilityModel[i].Costcode +
                           '</td><td>' + result.VendorFacilityModel[i].ProductServiceType +
                           '</td><td>' + result.VendorFacilityModel[i].ProductServiceName +
                           '</td><td>' + result.VendorFacilityModel[i].UnitCost +
                           '</td><td>' + result.VendorFacilityModel[i].Tax +
                           '</td></tr>';

                        $('#VendorFacility_table').append(trHTML);
                    }
                }
            }
        }
    });
   // $("#lblPOStatus").html(POStatus);
    $('.modal-title').text("Vendor All Details");
    $("#myModalForGetVendorDetails").modal('show');
};

function RejectVendor() {
    $("#myModelApproveRejectVendor").modal('show');
}
function AppproveVendor() {
    callAjaxVendor();
}
function RejectVendorAfterComment() {
    if ($("#msform").valid())
    {
        callAjaxVendor();
    }
    else {
        return false;
    }
    
}
function callAjaxVendor() {
    var objData = new Object();
    objData.VendorId = VendorId;
    objData.LocationId = $_locationId;
    objData.Comment = $("#CommentVendor").val();
    $.ajax({
        url: '../VendorManagement/ApproveVendor',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objVendorApproveRejectModel: objData }),
        //data: { "onjFormData": values, "obj":GridData },
        success: function (result) {
            toastr.success(result);
            $("#myModalForGetVendorDetails").modal('hide');
            $("#myModelApproveRejectVendor").modal('hide');
            jQuery("#tbl_AllCompanyDataList").trigger("reloadGrid")
        },
        error: function () { toastr.error(result); },
    });
}