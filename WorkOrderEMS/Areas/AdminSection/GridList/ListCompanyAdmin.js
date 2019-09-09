var Companyurl = 'AdminCompany/GetAllCompanyAdminList';
var AddAdminCompany = 'AdminCompany/Index';
var LicenseDownloadDoc = '../AdminCompany/DownloadStateLicense/';
var AddAccount = 'AdminCompany/AddAccount';
var LocationId; var VendorId;

$(function () {
    $("#tbl_CompanyHolderList").jqGrid({
        url: $_HostPrefix + Companyurl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Vendor Id', 'Company Name', 'Company type','Address','Tax Id No', 'State Lic Exp Date','LicenseDoc', 'Actions'],
        colModel: [{ name: 'VendorId', width: 30, sortable: true, hidden: true },
        { name: 'CompanyNameLegal', width: 40, sortable: false },
        { name: 'CompanyType', width: 40, sortable: false },
        { name: 'Address', width: 20, sortable: true },
        { name: 'TaxIdNo', width: 20, sortable: true },
        { name: 'StateLicExpDate', width: 20, sortable: true },
        { name: 'StateLicDoc', width: 20, sortable: true, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divCompanyHolderListPager',
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
            var ids = jQuery("#tbl_CompanyHolderList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var data = jQuery("#tbl_CompanyHolderList").getRowData(cl);
                //be = '<div><a href="javascript:void(0)" class="EditRecord" eid="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>';
                ad = '<a href="javascript:void(0)" class="EditRecord" id="addAccountdetails" aid="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-university fa-2x texthover-greenlight"></span><span class="tooltips">Add Account</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" id="ViewVendorData" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk fa-2x"></span><span class="tooltips">View</span></a>';
                //vi = '<a href="javascript:void(0)" class="viewRecord" id="ViewVendorData" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;">view<span class="ui-icon ui-icon-disk fa-2x"></span><span class="tooltips">Add Account</span></a>';
                ai = '<a href="javascript:void(0)" id="ViewCompanyDetails" class="Assign" InsuraceVendorId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-eye fa-2x texthover-yellowlight"></span><span class="tooltips">View</span></a></div>';
                var Licensefile = "";
                if (data.StateLicDoc == null || data.StateLicDoc == "" || data.StateLicDoc == '' || data.StateLicDoc == undefined) {
                }
                else {
                    Licensefile = '<a href="' + LicenseDownloadDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">State License</span></a></div>';
                }
                jQuery("#tbl_CompanyHolderList").jqGrid('setRowData', ids[i], { act: ai + ad + Licensefile }); ///+ qrc 
            }
            if ($("#tbl_CompanyHolderList").getGridParam("records") <= 20) {
                $("#divCompanyHolderListPager").hide();
            }
            else {
                $("#divCompanyHolderListPager").show();
            }
            if ($('#tbl_CompanyHolderList').getGridParam('records') === 0) {
                $('#tbl_CompanyHolderList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: ' <div><a href="javascript:void(0)"><i id="AddAdminCompany" class="fa fa-plus-square" style="font-size:36px;"></i></a></div>' //<span class="header_search"><input id="SearchText" class="inputSearch" placeholder="Serach By PO Number" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text">

    });
    if ($("#tbl_CompanyHolderList").getGridParam("records") > 20) {
        jQuery("#tbl_CompanyHolderList").jqGrid('navGrid', '#divCompanyHolderListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});
var timeoutHnd;
var flAuto = true;

$("#AddAdminCompany").live("click", function (event) {
    window.location.href = $_HostPrefix + AddAdminCompany;
});

$("#ViewCompanyDetails").live("click", function (event) {
    var idCompany = $(this).attr("InsuraceVendorId");
    $.ajax({
        type: "POST",
        url: $_HostPrefix+'VendorManagement/GetAllVendorDataToView' + '?VendorId=' + idCompany,
        datatype: 'json',
        success: function (result) {
            $("#lblVendorNameLegal").html(result.CompanyNameLegal); $("#lblVendorNameDBA").html(result.CompanyNameDBA);
            $("#lblVendorType").html(result.VendorTypeData); $("#lblPointOfContact").html(result.PointOfContact);
            $("#lblAddress").html(result.Address1); $("#lblPhone1").html(result.Phone1);
            $("#lblPhone2").html(result.Phone2); $("#lblEmail").html(result.Email);
            $("#lblWebsite").html(result.Website); $("#lblLicenseName").html(result.CompanyNameLegal);
            $("#lblLicenseNumber").html(result.LicenseNumber); $("#lblLicenseExpirationDate").html(result.LicenseExpirationDate);
            $("#lblInsuranceCarries").html(result.InsuranceCarries); $("#lblPolicyNumber").html(result.PolicyNumber);
            $("#lblInsuranceExpirationDate").html(result.InsuranceExpirationDate); $("#lblPayMode").html(result.PrimaryPaymentMode);
            $("#lblBankName").html(result.BankName); $("#lblBankLocation").html(result.BankLocation);
            $("#lblAccountNumber").html(result.AccountNumber); $("#lblIFSCCode").html(result.IFSCCode);
            $("#lblSwiftOICCode").html(result.SwiftOICCode); $("#lblCardNumber").html(result.CardNumber);
            $("#lblCardHolderName").html(result.CardHolderName); $("#lblExpirationDate").html(result.CardHolderName);
            $("#lblPolicyNumber").html(result.PolicyNumberAccount);
            $('.modal-title').text("Company All Details");
            $("#myModalForGetCompanyDetails").modal('show');
        }
    });
});

$('#addAccountdetails').live("click", function (event) {
    var idCompany = $(this).attr("aid");
    window.location.href = $_HostPrefix + AddAccount +'?CompanyId=' + idCompany;
})


