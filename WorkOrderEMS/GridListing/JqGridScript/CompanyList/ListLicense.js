var UrlLicense = 'VendorManagement/ListLicense';
var ActiveLicense = 'VendorManagement/ActiveInsurance';
var InsuranceDoc = '../VendorManagement/InsuranceDownload/';
var addDetails = 'VendorManagement/AddLicense/';
var Insurance = true;
$(function () { 
    var act;
    $("#tbl_LicenseList").jsGrid({
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
                    url: $_HostPrefix + UrlLicense + '?VendorId=' + $_VendorID + "&VendorStatus=" + $_VendorStatus,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "LicenseName", title: "License Name", type: "text", width: 50 },
            { name: "LicenseNumber", title: "License Number", type: "text", width: 50 },
            { name: "DisplayLicenseExpirationDate", title: "Expiration Date", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check check-icon" }).attr({ style: "" });
                    var $iconPencilForAccountDeactivate = $("<i>").attr({ class: "fa fa-close close-icon" }).attr({ style: "" });
                    var $iconFileFordocument = $("<i>").attr({ class: "fa fa-download download-icon" }).attr({ style: "" });
                    var $iconForExpired = $("<i>").attr({ class: "fa fa-close close-icon" }).attr({ style: "" });
                  var  $customButtonForAcandDeActive = "";
                    if (item.Status == "Activated") {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Deactivate" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ActiveDeActiveLicense(item.Id, "N");
                        }).append($iconPencilForAccountApprove);
                    }
                    else if (item.Status == "Deactivated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Active" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ActiveDeActiveLicense(item.Id, "Y");
                        }).append($iconPencilForAccountDeactivate);


                    }
                    if (item.Status == "Expired") {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Update" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            UpdateLicense(item.Id);

                        }).append($iconForExpired);

                    }
                    if (item.LicenseDocument == null || item.LicenseDocument == "" || item.LicenseDocument == '' || item.LicenseDocument == undefined) {
                    }
                    else {
                        var $customButtonForFile = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Receipt file" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            window.location.href = $_HostPrefix + InsuranceDoc + '?Id=' + item.Id + '&Insurance=' + Insurance;
                        }).append($iconFileFordocument);
                    }

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForAcandDeActive).append($customButtonForFile);
                }
            }
        ]
    });

    //$("#tbl_LicenseList").jqGrid({
    //    url: $_HostPrefix + UrlLicense + '?VendorId=' + $_VendorID + "&VendorStatus=" + $_VendorStatus,
    //    datatype: 'json',
    //    type: 'GET',
    //    height: 300,
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['LicenseId', 'VendorListId', 'License Name', 'License Number', 'Expiration Date', 'LicenseDocument', 'Status', 'Actions'],
    //    colModel: [
    //        { name: 'LicenseId', width: 30, sortable: true, hidden: true },
    //        { name: 'VendorListId', width: 30, sortable: true, hidden: true },
    //        { name: 'LicenseName', width: 30, sortable: true },
    //        { name: 'LicenseNumber', width: 30, sortable: true },
    //        { name: 'LicenseExpirationDate', width: 30, sortable: true },
    //        { name: 'LicenseDocument', width: 30, sortable: true, hidden: true },
    //        { name: 'Status', width: 30, sortable: true },
    //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    scrollOffset: 0,
    //    pager: '#divLicenseListPager',
    //    sortname: 'LicenseId',
    //    viewrecords: true,
    //    gridview: true,
    //    loadonce: false,
    //    multiSort: true,
    //    rownumbers: true,
    //    emptyrecords: "No records to display",
    //    shrinkToFit: true,
    //    sortorder: 'asc',
    //    caption: "List of License",
    //    gridComplete: function () {
    //        var ids = jQuery("#tbl_LicenseList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            be = "";
    //            var disable = "";
    //            var Data = jQuery("#tbl_LicenseList").getRowData(cl);
    //            var LicenseFile = Data['LicenseDocument'];
    //            if (Data.Status == "Activated") {
    //                //be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveLicense" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Click to deactivate</span></a>';
    //                be = '<a href="javascript:void(0)" class="download-cloud" title="AlreadyApprove" aid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-ban fa-2x"></span><span class="tooltips" style="width:100px;">Already Approve</span></a></div>';

    //            }
    //            else if (Data.Status == "Deactivated") {
    //                be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveLicense" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
    //            }
    //            var file = "";
    //            var ci = "";
    //            if (LicenseFile == null || LicenseFile == "" || LicenseFile == '' || LicenseFile == undefined) {
    //            }
    //            else {
    //                file = '<a href="' + InsuranceDoc + '?Id=' + cl + '&Insurance=' + Insurance + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Receipt</span></a></div>';
    //            }
    //            if (Data.Status == "Expired")
    //            {
    //                ci = '<a href="javascript:void(0)"  title="view" id="UpdateLicenseDetails" aid="' + cl + '" style=" float: left;cursor:pointer;"><button class="btn btn-primary" style="border-radius:25px;width:80px;">Update</button></a></div>';
    //            }
    //            jQuery("#tbl_LicenseList").jqGrid('setRowData', ids[i], { act: be + file + ci });
    //        }
    //        if ($("#tbl_LicenseList").getGridParam("records") <= 20) {
    //            $("#divLicenseListPager").hide();
    //        }
    //        else {
    //            $("#divLicenseListPager").show();
    //        }
    //        if ($('#tbl_LicenseList').getGridParam('records') === 0) {
    //            $('#tbl_LicenseList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //    caption: '<div id="AddLicense"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    //});
    //if ($("#tbl_LicenseList").getGridParam("records") > 20) {
    //    jQuery("#tbl_LicenseList").jqGrid('navGrid', '#divLicenseListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});


function AddLicense(event) {
    window.location.href = $_HostPrefix + addDetails + "?VendorID=" + $_VendorID;
};
function UpdateLicense(id) {
  
    window.location.href = $_HostPrefix + addDetails + '?id=' + id + "&VendorID=" + $_VendorID;
};

function ActiveDeActiveLicense(id, Status) { 
    debugger;
    var IsLicense = "License";
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveLicense + "?InsuranceLicenseId=" + id + "&IsActive=" + Status + "&IsInsuranceLicense=" + IsLicense,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) { 
            $("#tbl_LicenseList").jsGrid("loadData"); 
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
};