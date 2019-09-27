var UrlAccoutns = 'VendorManagement/ListAccounts';
var ActiveAccounts = 'VendorManagement/ActiveAccounts';
var AccountsDoc = 'VendorManagement/AccountDocDownload/';
var addAccountDetails = 'VendorManagement/AddAccountDetails/';
var Insurance = true;
$(function () {
    var act;
    $("#tbl_AccountList").jsGrid({
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
                    url: $_HostPrefix + UrlAccoutns + '?VendorId=' + $_VendorID,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            // { name: "VendorId", title: "Vendor Id", type: "text", width: 50 ,hidden:true },

            { name: "BankName", title: "Bank Name", type: "text", width: 50 },
            { name: "BankLocation", title: "Bank Location", type: "text", width: 50 },
            { name: "AccountNumber", title: "Account Number", type: "text", width: 50 },
            { name: "CardNumber", title: "Card Number", type: "text", width: 50 },
            { name: "IFSCCode", title: "IFSCCode", type: "text", width: 50 },
            { name: "SwiftOICCode", title: "Swift OIC Code", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
             
            {

                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconPencilForAccountDeactivate = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconFileFordocument = $("<i>").attr({ class: "fa fa-download" }).attr({ style: "color:#0080ff;font-size: 22px;" });
                    var $iconForExpired = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:#0080ff;font-size: 22px;" }); 
                    if (item.Status == "Activated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Deactivate" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                              ActiveDeActiveAccount(item.Id,"N"); 
                        }).append($iconPencilForAccountApprove);
                    }
                    else if (item.Status == "Deactivated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Active" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ActiveDeActiveAccount(item.Id, "Y");
                        }).append($iconPencilForAccountDeactivate);
                         

                    }
                    if (item.Status == "Expired")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Update" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                          
                        }).append($iconForExpired);
                        
                    } 
                    if (item.AccountDocuments == null || item.AccountDocuments == "" || item.AccountDocuments == '' || item.AccountDocuments == undefined) {
                    }
                    else
                    {
                        var $customButtonForFile = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Receipt file" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            window.location.href = $_HostPrefix + AccountsDoc + '?Id=' + item.Id;
                       }).append($iconFileFordocument); 
                    }
                    
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForAcandDeActive).append($customButtonForFile); 
                }
            }
            


        ]
    });
    //$("#tbl_AccountList").jqGrid({
    //    url: $_HostPrefix + UrlAccoutns + '?VendorId=' + $_VendorID,
    //    datatype: 'json',
    //    type: 'GET',
    //    height: 300,
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['AccountID', 'Bank Name', 'Bank Location', 'Account Number', 'Card Number', 'IFSCCode', 'Swift OIC Code', 'InsuranceDocument', 'Status', 'Actions'],
    //    colModel: [
    //        { name: 'AccountID', width: 30, sortable: true, hidden: true },
    //        { name: 'BankName', width: 30, sortable: true},
    //        { name: 'BankLocation', width: 30, sortable: true },
    //        { name: 'AccountNumber', width: 30, sortable: true },
    //        { name: 'CardNumber', width: 30, sortable: true },
    //        { name: 'IFSCCode', width: 30, sortable: true },
    //        { name: 'SwiftOICCode', width: 30, sortable: true },
    //        { name: 'AccountDocuments', width: 30, sortable: true, hidden: true },
    //        { name: 'Status', width: 30, sortable: true },
    //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    scrollOffset: 0,
    //    pager: '#divAccountListPager',
    //    sortname: 'InsuranceId',
    //    viewrecords: true,
    //    gridview: true,
    //    loadonce: false,
    //    multiSort: true,
    //    rownumbers: true,
    //    emptyrecords: "No records to display",
    //    shrinkToFit: true,
    //    sortorder: 'asc',
    //    caption: "List of Payment Mode",
    //    gridComplete: function () {
    //        var ids = jQuery("#tbl_AccountList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            be = "";
    //            var Data = jQuery("#tbl_AccountList").getRowData(cl);
    //            var AccountsFile = Data['AccountDocuments'];
    //            if (Data.Status == "Activated") {
    //                be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveAccount" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Click to deactivate</span></a>';
    //            }
    //            else if (Data.Status == "Deactivated") 
    //    {
    //                be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveAccount" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
    //            }
    //            var file = "";
    //            var ci = "";
    //            if (AccountsFile == null || AccountsFile == "" || AccountsFile == '' || AccountsFile == undefined) {
    //            }
    //            else {
    //                file = '<a href="' + $_HostPrefix + AccountsDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Document</span></a></div>';
    //            }
    //            if (Data.status == "Expired") {
    //                ci = '<a href="javascript:void(0)"  title="view" id="UpdateInuranceLicense" aid="' + cl + '" style=" float: left;cursor:pointer;"><button class="btn btn-primary" style="border-radius:25px;width:80px;">Update</button></a></div>';
    //            }
    //            //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
    //            jQuery("#tbl_AccountList").jqGrid('setRowData', ids[i], { act: be + file + ci });
    //        }
    //        if ($("#tbl_AccountList").getGridParam("records") <= 20) {
    //            $("#divAccountListPager").hide();
    //        }
    //        else {
    //            $("#divAccountListPager").show();
    //        }
    //        if ($('#tbl_AccountList').getGridParam('records') === 0) {
    //            $('#tbl_AccountList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //    caption: '<div id="AddAccountsDetails"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    //});
    //if ($("#tbl_AccountList").getGridParam("records") > 20) {
    //    jQuery("#tbl_AccountList").jqGrid('navGrid', '#divAccountListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});


function AddAccountDetails() {
    window.location.href = $_HostPrefix + addAccountDetails + '?id=' + $_VendorID;
};

function ActiveDeActiveAccount(id, Status) { 
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveAccounts + "?AccountsId=" + id + "&IsActive=" + Status ,
        ///data:{},
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $("#tbl_AccountList").jsGrid("loadData"); 
           // $('#tbl_AccountList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
};