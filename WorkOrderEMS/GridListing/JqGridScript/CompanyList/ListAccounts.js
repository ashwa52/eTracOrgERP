 

var UrlAccoutns = 'VendorManagement/ListAccounts';
var ActiveAccounts = 'VendorManagement/ActiveAccounts';
var SetPrimeryAccounts = 'VendorManagement/PrimeryAccounts';
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

                name: "IsPrimary", title: "Primary", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $customButtonForAcandDeActive = ""; 
                    if (item.IsPrimary == "Y")
                    { 
                        $customButtonForAcandDeActive = $("<input>").attr({ "type": "radio", "name": "cellRadio" }).attr({ id: "txt-edit-" + item.Id, "checked": true }).click(function (e) {
                           
                            SetPrimary(item.Id, item.Status);
                        });
                    }
                    else
                    {
                        $customButtonForAcandDeActive = $("<input>").attr({ "type": "radio", "name": "cellRadio" }).attr({ id: "txt-edit-" + item.Id }).click(function (e) {
                             
                            SetPrimary(item.Id, item.Status);
                        });

                    }
                    return  $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForAcandDeActive); 
                }
            },
            {

                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconPencilForAccountDeactivate = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconFileFordocument = $("<i>").attr({ class: "fa fa-download" }).attr({ style: "color:#0080ff;font-size: 22px;" });
                    var $iconForExpired = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:#0080ff;font-size: 22px;" }); 
                    if (item.Status == "Activated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Deactivate" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            if (item.IsPrimary == "Y")
                            {
                                alert("This account is selected as 'Primary Account'. \n Please change the primary account first to deactivate this.");
                                return false;
                            }
                            else
                            {
                                ActiveDeActiveAccount(item.Id, "N");
                            }
                            
                           
                             
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
     
});


function AddAccountDetails() {
    window.location.href = $_HostPrefix + addAccountDetails + '?id=' + $_VendorID;
};
function SetPrimary(id, Status) {
    
    if (Status == "Activated") {
        Status = "N";
    }
    else if (Status == "Deactivated") {
        Status ="Y";
    }
    else if (Status == "Expired")
    {
        Status = "Y";
    }
    $.ajax({
        type: "POST",
        url: $_HostPrefix + SetPrimeryAccounts + "?AccountsId=" + id + "&IsActive=" + Status + "&VendorId=" + $_VendorID ,
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