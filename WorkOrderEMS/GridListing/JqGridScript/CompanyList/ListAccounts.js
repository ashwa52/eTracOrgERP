var UrlAccoutns = 'VendorManagement/ListAccounts';
var ActiveAccounts = 'VendorManagement/ActiveAccounts';
var AccountsDoc = 'VendorManagement/AccountDocDownload/';
var addAccountDetails = 'VendorManagement/AddAccountDetails/';
var Insurance = true;
$(function () {
    $("#tbl_AccountList").jqGrid({
        url: $_HostPrefix + UrlAccoutns + '?VendorId=' + $_VendorID,
        datatype: 'json',
        type: 'GET',
        height: 300,
        width: 700,
        autowidth: true,
        colNames: ['AccountID', 'Bank Name', 'Bank Location', 'Account Number', 'Card Number', 'IFSCCode', 'Swift OIC Code', 'InsuranceDocument', 'Status', 'Actions'],
        colModel: [
            { name: 'AccountID', width: 30, sortable: true, hidden: true },
            { name: 'BankName', width: 30, sortable: true},
            { name: 'BankLocation', width: 30, sortable: true },
            { name: 'AccountNumber', width: 30, sortable: true },
            { name: 'CardNumber', width: 30, sortable: true },
            { name: 'IFSCCode', width: 30, sortable: true },
            { name: 'SwiftOICCode', width: 30, sortable: true },
            { name: 'AccountDocuments', width: 30, sortable: true, hidden: true },
            { name: 'Status', width: 30, sortable: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divAccountListPager',
        sortname: 'InsuranceId',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Payment Mode",
        gridComplete: function () {
            var ids = jQuery("#tbl_AccountList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_AccountList").getRowData(cl);
                var AccountsFile = Data['AccountDocuments'];
                if (Data.Status == "Activated") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveAccount" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Click to deactivate</span></a>';
                }
                else if (Data.Status == "Deactivated") {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveAccount" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                var file = "";
                var ci = "";
                if (AccountsFile == null || AccountsFile == "" || AccountsFile == '' || AccountsFile == undefined) {
                }
                else {
                    file = '<a href="' + $_HostPrefix + AccountsDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Document</span></a></div>';
                }
                if (Data.status == "Expired") {
                    ci = '<a href="javascript:void(0)"  title="view" id="UpdateInuranceLicense" aid="' + cl + '" style=" float: left;cursor:pointer;"><button class="btn btn-primary" style="border-radius:25px;width:80px;">Update</button></a></div>';
                }
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                jQuery("#tbl_AccountList").jqGrid('setRowData', ids[i], { act: be + file + ci });
            }
            if ($("#tbl_AccountList").getGridParam("records") <= 20) {
                $("#divAccountListPager").hide();
            }
            else {
                $("#divAccountListPager").show();
            }
            if ($('#tbl_AccountList').getGridParam('records') === 0) {
                $('#tbl_AccountList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddAccountsDetails"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_AccountList").getGridParam("records") > 20) {
        jQuery("#tbl_AccountList").jqGrid('navGrid', '#divAccountListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});


$("#AddAccountsDetails").live("click", function (event) {
    window.location.href = $_HostPrefix + addAccountDetails + '?id=' + $_VendorID;
});

$("#ActiveAccount").live("click", function (event) {
    var id = $(this).attr("iid");
    var IsActive = $(this).attr("IsActive");
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveAccounts + "?AccountsId=" + id + "&IsActive=" + IsActive ,
        ///data:{},
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_AccountList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});