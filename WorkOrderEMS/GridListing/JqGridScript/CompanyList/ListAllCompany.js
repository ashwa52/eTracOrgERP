var Companyurl = 'VendorManagement/GetAllCompanyListList';
var AddCompany = 'VendorManagement/VendorManagementSetup/';
var editCompany = 'VendorManagement/EditVendor/';
var addAccountDetails = 'VendorManagement/AddAccountDetails/';
var GridListLicense = 'VendorManagement/ListInsuranceLicenseView';
var addFacilityDetails = 'VendorManagement/AddFacilityDetails/';

var ListAccountDetails = 'VendorManagement/ListAccountOfVendor/';
//var EditPO = 'POTypeData/EditPOByPOId/';
//var POType = ''
//+ '<select id="ApproveData" class="" onchange="doSearch(arguments[0]||event);">'
//+ '<option value="Approve">Approved PO</option>'
//+ '<option value="NotApprove">Not Approved</option>'
//+ '</select>';
var LocationId; var VendorId;

$(function () {
    $("#tbl_AllCompanyDataList").jqGrid({
        url: $_HostPrefix + Companyurl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Vendor Id', 'Vendor Name', 'Address','Phone Number','Point Of Contact','Vendor Type','Status','Account','Insurance','Licesne', 'Actions'],
        colModel: [{ name: 'VendorId', width: 30, sortable: true, hidden:true },
        { name: 'CompanyNameLegal', width: 40, sortable: false },
        { name: 'Address1', width: 20, sortable: true },
        { name: 'Phone1', width: 40, sortable: false },
        { name: 'PointOfContact', width: 20, sortable: true },
        { name: 'VendorTypeData', width: 20, sortable: true },
        { name: 'Status', width: 20, sortable: true, hidden: true },
        { name: 'AccountStatus', width: 20, sortable: true, hidden: true },
        { name: 'InsuranceStatus', width: 20, sortable: true, hidden: true },
        { name: 'LicenseStatus', width: 20, sortable: true, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divAllCompanyDataListPager',
        sortname: 'VendorId',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_AllCompanyDataList").jqGrid('getDataIDs');
            jQuery('#tbl_AllCompanyDataList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];               
                be = '<a href="javascript:void(0)" class="EditRecord" eid="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>';
                ad = '<a href="javascript:void(0)" class="EditRecord" id="addAccountdetails" aid="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-university fa-2x texthover-greenlight"></span><span class="tooltips">List Account</span></a>';
                fcl = '<a href="javascript:void(0)" class="Assign" id="FacilityListVendorData" title="Add Facility" fid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-list fa-2x texthover-bluelight"></span><span class="tooltips">Facility List</span></a>';
                vi = '<a href="javascript:void(0)" class="Assign" id="ViewVendorData" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-list fa-2x texthover-bluelight"></span><span class="tooltips">View</span></a>';
                ai = '<a href="javascript:void(0)" id="AddInsurance" class="Assign" InsuraceVendorId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-medkit fa-2x texthover-yellowlight"></span><span class="tooltips">List Insurance/License</span></a>';
                jQuery("#tbl_AllCompanyDataList").jqGrid('setRowData', ids[i], { act: be + vi + ad + ai + fcl }); ///+ qrc 

            }
            var data = jQuery("#tbl_AllCompanyDataList").getRowData();
            var ids = jQuery("#tbl_AllCompanyDataList").jqGrid('getDataIDs');
            var rows = this.rows, c = rows.length;
            for (var j = 0; j < ids.length; j++) {
                var tt = ids[j];
                if (data[j].AccountStatus == "W") {
                    var AccId = $('tr[id^="' + tt + '"] td').find('a').eq(3).attr('aid');;
                    //$('tr[id^="' + tt + '"]').css("background-color", "#74ED33");
                   // //$("tr #" + tt).css("background-color", "#FB7869");
                   // $('tr[id^="' + tt + '"]').removeClass("ui-widget-content jqgrow ui-row-ltr ui-state-hover ui-state-highlight");
                    //$('tr[id^="' + tt + '"]').addClass("jqgrow");
                    $('a[aid^="' + tt + '"]').addClass("BlinkData");
                    
                }
                else if (data[j].InsuranceStatus == "W") {
                    var licenseId = $('tr[id^="' + tt + '"] td').find('a').eq(3).attr('InsuraceVendorId');;
                    //$('tr[id^="' + tt + '"]').css("background-color", "#74ED33");
                    ////$("tr #" + tt).css("background-color", "#FB7869");
                    //$('tr[id^="' + tt + '"]').removeClass("ui-widget-content jqgrow ui-row-ltr ui-state-hover ui-state-highlight");
                    //$('tr[id^="' + tt + '"]').addClass("jqgrow");
                    $('a[InsuraceVendorId^="' + licenseId + '"]').addClass("BlinkData");
                }
                else if (data[j].LicenseStatus == "W") {

                    var licenseId = $('tr[id^="' + tt + '"] td').find('a').eq(3).attr('InsuraceVendorId');;
                    $('tr[id^="' + tt + '"]').css("background-color", "#74ED33");
                    //$("tr #" + tt).css("background-color", "#FB7869");
                    $('tr[id^="' + tt + '"]').removeClass("ui-widget-content jqgrow ui-row-ltr ui-state-hover ui-state-highlight");
                    $('tr[id^="' + tt + '"]').addClass("jqgrow");
                    $('a[InsuraceVendorId^="' + licenseId + '"]').addClass("BlinkData");
                }
            }
            if ($("#tbl_AllCompanyDataList").getGridParam("records") <= 20) {
                $("#divAllCompanyDataListPager").hide();
            }
            else {
                $("#divAllCompanyDataListPager").show();
            }
            if ($('#tbl_AllCompanyDataList').getGridParam('records') === 0) {
                $('#tbl_AllCompanyDataList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: ' <div><a href="javascript:void(0)"></a><input type="text" class="inputSearch light-table-filter" id="searchtext" placeholder="Vendor Name"  data-table="order-table" /></span></div>' //<span class="header_search"><input id="SearchText" class="inputSearch" placeholder="Serach By PO Number" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text">

    });
    if ($("#tbl_AllCompanyDataList").getGridParam("records") > 20) {
        jQuery("#tbl_AllCompanyDataList").jqGrid('navGrid', '#divAllCompanyDataListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
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
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("eid");
    window.location.href = $_HostPrefix + editCompany + '?id=' + id;
});
$("#FacilityListVendorData").live("click", function (event) {
    var fid = $(this).attr("fid");
    window.location.href = $_HostPrefix + addFacilityDetails + '?id=' + fid ;
});
$("#addAccountdetails").live("click", function (event) {
    var id = $(this).attr("aid");
    window.location.href = $_HostPrefix + ListAccountDetails + '?VendorId=' + id;
});

$("#AddInsurance").live("click", function (event) {
    var id = $(this).attr("InsuraceVendorId");
    //window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id, '?VendorStatus=' + $_VendorStatus;
    window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id + "&VendorStatus=" + $_VendorStatus;
    //window.location.href = $_HostPrefix + addInsuranceAndLicenseDetails + '?id=' + id;
});

$("#AddCompany").live("click", function (event) {
    window.location.href = $_HostPrefix + AddCompany;
});


$("#ViewVendorData").live("click", function (event) {
    VendorId = $(this).attr("vid");
    var rowData = jQuery("#tbl_AllCompanyDataList").getRowData(VendorId);
    //var VendorName = rowData['CompanyNameLegal'];
    //$("#lblVendorName").html(VendorName);
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    if (rowData.Status == "Y")
    {
        $("#btnApproveData").hide();
        $('#btnRejectPO').hide();
    }
    else
    {
        $("#btnApproveData").show();
        $('#btnRejectPO').show();
    }
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
            $("#lblCardHolderName").html(result.CardHolderName); $("#lblExpirationDate").html(result.CardHolderName);
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
});

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