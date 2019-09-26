var UnApprovedCompanyurl = 'VendorManagement/GetAllUnApprovedVendorList';
var AddCompany = 'VendorManagement/VendorManagementSetup/';
var editCompany = 'VendorManagement/EditVendor/';
var addAccountDetails = 'VendorManagement/AddAccountDetails/';
var GridListLicense = 'VendorManagement/ListInsuranceLicenseView';
var veiwVedorDetails = 'VendorManagement/GetAllVendorDataToView/';
var ListAccountDetails = 'VendorManagement/ListAccountOfVendor/';

var LocationId; var VendorId;

$(function () {
    var act;
    $("#jsGrid-basic").jsGrid({
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
                    url: '/VendorManagement/GetAllUnApprovedVendorList',
                    data: filter,
                    dataType: "json"
                });
            }
        },
      
        fields: [
            { name: "VendorId", title: "Vendor Id",  type: "text", width: 50 },
            { name: "CompanyNameLegal", title: "Vendor Name", type: "text", width: 50 },
            { name: "Address1", title: "Address", type: "text", width: 50 },
            { name: "Phone1", title: "Phone Number", type: "text", width: 50 },
            { name: "PointOfContact", title: "Point Of Contact", type: "text", width: 50 },
            { name: "VendorTypeData", title: "Vendor Type", type: "text", width: 50 }, 
            { name: "Status", title: "Status", type: "text", width: 50 }, 
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" }); 

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title:"View Details"})
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            
                            ViewDetails(item);
                            //var addNewUrl = "../GlobalAdmin/EditLocationSetup?loc=" + item.Id;
                            //$('#RenderPageId').load(addNewUrl);
                            //e.stopPropagation();
                        }).append($iconPencil);
                    

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);

                    //var ed = "<a href='javascript:void(0)' class='EditRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-pencil fa-1x' style='color:yellow;'></span></a>";
                    //var de = "<a href='javascript:void(0)' class='DeleteRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-delete fa-1x' style='color:black;'></span></a>";
                    //var vi = "<a href='javascript:void(0)' class='ViewRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-eye fa-1x' style='color:black'></span></a>";
                    ////return $("<span>").attr("class", ed);
                    //var alldiv = "<span>" + ed + "</span>" + "<span>" + de + "</span>" + "<span>" + vi + "</span>";
                    //return $("<div>").html(alldiv);
                } }
        ]
    });
   

});
$(function () {
    //$("#tbl_AllUnApprovedList").jqGrid({
    //    url: $_HostPrefix + UnApprovedCompanyurl + '?LocationId=' + $_locationId + "&VendorStatus=" + $_VendorStatus,
    //    datatype: 'json',
    //    type: 'GET',
    //    height: 400,
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['Vendor Id', 'Vendor Name', 'Address', 'Phone Number', 'Point Of Contact', 'Vendor Type', 'Status', 'Actions'],
    //    colModel: [{ name: 'VendorId', width: 30, sortable: true, hidden: true },
    //    { name: 'CompanyNameLegal', width: 40, sortable: false },
    //    { name: 'Address1', width: 20, sortable: true },
    //    { name: 'Phone1', width: 40, sortable: false },
    //    { name: 'PointOfContact', width: 20, sortable: true },
    //    { name: 'VendorTypeData', width: 20, sortable: true },
    //    { name: 'Status', width: 20, sortable: true, hidden: true },
    //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    scrollOffset: 0,
    //    pager: '#divAllUnApprovedListPager',
    //    sortname: 'VendorId',
    //    viewrecords: true,
    //    gridview: true,
    //    loadonce: false,
    //    multiSort: true,
    //    rownumbers: true,
    //    emptyrecords: "No records to display",
    //    shrinkToFit: true,
    //    sortorder: 'asc',
    //    gridComplete: function () {
    //        jQuery('#tbl_AllUnApprovedList').addClass('order-table');
    //        var ids = jQuery("#tbl_AllUnApprovedList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            var data = jQuery("#tbl_AllUnApprovedList").getRowData(cl);
    //            be = '<div><a href="javascript:void(0)" class="EditRecord" eid="' + cl + '" title="edit" style="float: left;margin-right:10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>';
    //            ad = '<a href="javascript:void(0)" class="EditRecord" id="addAccountdetails" aid="' + cl + '" title="edit" style="float:left;margin-right: 10px;cursor:pointer;"><span class="fa fa-university fa-2x texthover-greenlight"></span><span class="tooltips">List Account</span></a>';
    //            vi = '<a href="javascript:void(0)" class="viewRecord" id="ViewUnApprovedVendorData" title="view" vid="' + cl + '" style="float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk fa-2x"></span><span class="tooltips">View</span></a></div>';
    //            ai = '<a href="javascript:void(0)" id="AddInsurance" class="Assign" InsuraceVendorId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-medkit fa-2x texthover-yellowlight"></span><span class="tooltips">List Insurance/License</span></a></div>';
               
    //            jQuery("#tbl_AllUnApprovedList").jqGrid('setRowData', ids[i], { act: vi }); ///+ qrc + vi + ad + ai  + be 
    //        }
           



    //        if ($("#tbl_AllUnApprovedList").getGridParam("records") <= 20) {
    //            $("#divAllUnApprovedListPager").hide();
    //        }
    //        else {
    //            $("#divAllUnApprovedListPager").show();
    //        }
    //        if ($('#tbl_AllUnApprovedList').getGridParam('records') === 0) {
    //            $('#tbl_AllUnApprovedList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //    caption: ' <div><a href="javascript:void(0)"><i id="AddCompany" class="fa fa-plus-square" style="font-size:36px;"></i>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a><span><input type="text" class="inputSearch light-table-filter" id="searchUnApprovedtext" placeholder="Vendor Name"  data-table="order-table" /></span></div>' //<span class="header_search"><input id="SearchText" class="inputSearch" placeholder="Serach By PO Number" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text">

    //});
    //if ($("#tbl_AllUnApprovedList").getGridParam("records") > 20) {
    //    jQuery("#tbl_AllUnApprovedList").jqGrid('navGrid', '#divAllUnApprovedListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});
function ViewDetails(item) {
     
    VendorId = item.VendorId;
    
    $.ajax({
        type: "POST",
        url: $_HostPrefix + veiwVedorDetails + '?VendorId=' + item.VendorId,
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
            if (item.Status == "Rejected") {
                $("#btnApproveData").hide();
                $("#btnRejectPO").hide();
            }
            else {
                $("#btnApproveData").show();
                $("#btnRejectPO").show();
            }
            if (result.LocationAssignedModel != null) {
                if (result.LocationAssignedModel.length > 0) {
                    var arr = [];
                    var llcmArr = [];
                    for (i = 0; i < result.LocationAssignedModel.length; i++) {
                        arr.push(result.LocationAssignedModel[i].LocationName);
                        llcmArr.push(result.LocationAssignedModel[i].LLCM_Id)
                    }
                    var loc = arr.join(',');
                    var llcm = llcmArr.join(',');
                    $("#lblSelectedLocation").html(loc);
                    $('#LLCM_Id').val(llcm);
                }
            }
            if (result.VendorFacilityModel != null) {
                $('#records_table').html('');
                var arrData = [];
                $('#UnVendorFacility_table').html("");
                $('#UnVendorFacility_table tbody').empty();
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;"><th>Cost Code</th><th>Facility Type</th><th>Description</th><th>Unit Price</th><th>Tax</th></tr>';
                $('#UnVendorFacility_table').append(thHTML);
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

                        $('#UnVendorFacility_table').append(trHTML);
                    }
                }
            }
            $('.modal-title').text("Vendor All Details");
            $("#myModalForGetUnApprovedVendorDetails").modal('show');
        }
    });
    // $("#lblPOStatus").html(POStatus);
   
}
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#jsGrid-basic").jsGrid({
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
                    url: $_HostPrefix + UnApprovedCompanyurl + '?_search=' + $("#SearchText").val(),//'/VendorManagement/GetAllUnApprovedVendorList',
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "VendorId", title: "Vendor Id", type: "text", width: 50 },
            { name: "CompanyNameLegal", title: "Vendor Name", type: "text", width: 50 },
            { name: "Address1", title: "Address", type: "text", width: 50 },
            { name: "Phone1", title: "Phone Number", type: "text", width: 50 },
            { name: "PointOfContact", title: "Point Of Contact", type: "text", width: 50 },
            { name: "VendorTypeData", title: "Vendor Type", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {

                            ViewDetails(item);
                            //var addNewUrl = "../GlobalAdmin/EditLocationSetup?loc=" + item.Id;
                            //$('#RenderPageId').load(addNewUrl);
                            //e.stopPropagation();
                        }).append($iconPencil);


                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);

                    //var ed = "<a href='javascript:void(0)' class='EditRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-pencil fa-1x' style='color:yellow;'></span></a>";
                    //var de = "<a href='javascript:void(0)' class='DeleteRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-delete fa-1x' style='color:black;'></span></a>";
                    //var vi = "<a href='javascript:void(0)' class='ViewRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-eye fa-1x' style='color:black'></span></a>";
                    ////return $("<span>").attr("class", ed);
                    //var alldiv = "<span>" + ed + "</span>" + "<span>" + de + "</span>" + "<span>" + vi + "</span>";
                    //return $("<div>").html(alldiv);
                }
            }
        ]
    });
}
function filter(args) {
    debugger
   
} 
//function gridReload() {

//    var txtSearch = jQuery("#SearchText").val();
//    jQuery("#tbl_AllCompanyDataList").jqGrid('setGridParam', { url: $_HostPrefix + Companyurl + "?&LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
//}
$(".EditRecord").on("click", function (event) {
    var id = $(this).attr("eid");
    window.location.href = $_HostPrefix + editCompany + '?id=' + id;
});
//$("#addAccountdetails").on("click", function (event) {
//    var id = $(this).attr("aid");
//    window.location.href = $_HostPrefix + addAccountDetails + '?id=' + id;
//});
$("#addAccountdetails").on("click", function (event) {
    var id = $(this).attr("aid");
    window.location.href = $_HostPrefix + ListAccountDetails + '?VendorId=' + id;
});

$("#AddInsurance").on("click", function (event) {
    var id = $(this).attr("InsuraceVendorId");
    //window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id, '?VendorStatus=' + false;
    window.location.href = $_HostPrefix + GridListLicense + '?Vendorid=' + id + "&VendorStatus=" + $_VendorStatus;
    //window.location.href = $_HostPrefix + addInsuranceAndLicenseDetails + '?id=' + id;
});

$("#AddCompany").on("click", function (event) {
    window.location.href = $_HostPrefix + AddCompany;
});


$("#ViewUnApprovedVendorData").on("click", function (event) {
    VendorId = $(this).attr("vid");
    var rowData = jQuery("#tbl_AllUnApprovedList").getRowData(VendorId);
    if (rowData.Status == "Y") {
        $("#btnApproveData").hide();
        $('#btnRejectPO').hide();
    }
    else {
        $("#btnApproveData").show();
        $('#btnRejectPO').show();
    }

    $.ajax({
        type: "POST",
        url: $_HostPrefix + veiwVedorDetails + '?VendorId=' + VendorId,
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
                    var llcmArr = [];
                    for (i = 0; i < result.LocationAssignedModel.length; i++) {
                        arr.push(result.LocationAssignedModel[i].LocationName);
                        llcmArr.push(result.LocationAssignedModel[i].LLCM_Id)
                    }
                    var loc = arr.join(',');
                    var llcm = llcmArr.join(',');
                    $("#lblSelectedLocation").html(loc);
                    $('#LLCM_Id').val(llcm);
                }
            }
            if (result.VendorFacilityModel != null) {
                $('#records_table').html('');
                var arrData = [];
                $('#UnVendorFacility_table tbody').empty();
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;"><th>Cost Code</th><th>Facility Type</th><th>Description</th><th>Unit Price</th><th>Tax</th></tr>';
                $('#UnVendorFacility_table').append(thHTML);
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

                        $('#UnVendorFacility_table').append(trHTML);
                    }
                }
            }
        }
    });
    // $("#lblPOStatus").html(POStatus);
    $('.modal-title').text("Vendor All Details");
    $("#myModalForGetUnApprovedVendorDetails").modal('show');
});

function RejectVendor() {
 
    $("#myModelApproveRejectVendor").modal('show');
}
function AppproveVendor() {
    $("#CommentVendor").val("");
    $("#btnApproveData").addClass("disabled");
    callAjaxVendor();
}
function RejectVendorAfterComment() {
    
    
    if ($("#Comment").val()!="") {
       
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
    objData.LLCM_Id = $('#LLCM_Id').val();
    $.ajax({
        url: '../VendorManagement/ApproveVendor',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objVendorApproveRejectModel: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) { 
            toastr.success(result);
            $("#myModalForGetUnApprovedVendorDetails").modal('hide');
            //$("#myModelApproveRejectVendor").modal('hide');
            $("#btnApproveData").removeClass("disabled"); 
            $("#jsGrid-basic").jsGrid("loadData"); 
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}