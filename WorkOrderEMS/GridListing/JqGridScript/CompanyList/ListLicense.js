var UrlLicense = 'VendorManagement/ListLicense';
var ActiveLicense = 'VendorManagement/ActiveInsurance';
var InsuranceDoc = '../VendorManagement/InsuranceDownload/';
var addDetails = 'VendorManagement/AddLicense/';
var Insurance = true;
$(function () {
    $("#tbl_LicenseList").jqGrid({
        url: $_HostPrefix + UrlLicense + '?VendorId=' + $_VendorID + "&VendorStatus=" + $_VendorStatus,
        datatype: 'json',
        type: 'GET',
        height: 300,
        width: 700,
        autowidth: true,
        colNames: ['LicenseId', 'VendorListId', 'License Name', 'License Number', 'Expiration Date', 'LicenseDocument', 'Status', 'Actions'],
        colModel: [
            { name: 'LicenseId', width: 30, sortable: true, hidden: true },
            { name: 'VendorListId', width: 30, sortable: true, hidden: true },
            { name: 'LicenseName', width: 30, sortable: true },
            { name: 'LicenseNumber', width: 30, sortable: true },
            { name: 'LicenseExpirationDate', width: 30, sortable: true },
            { name: 'LicenseDocument', width: 30, sortable: true, hidden: true },
            { name: 'Status', width: 30, sortable: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divLicenseListPager',
        sortname: 'LicenseId',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of License",
        gridComplete: function () {
            var ids = jQuery("#tbl_LicenseList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var disable = "";
                var Data = jQuery("#tbl_LicenseList").getRowData(cl);
                var LicenseFile = Data['LicenseDocument'];
                if (Data.Status == "Activated") {
                    //be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveLicense" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Click to deactivate</span></a>';
                    be = '<a href="javascript:void(0)" class="download-cloud" title="AlreadyApprove" aid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-ban fa-2x"></span><span class="tooltips" style="width:100px;">Already Approve</span></a></div>';

                }
                else if (Data.Status == "Deactivated") {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveLicense" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                var file = "";
                var ci = "";
                if (LicenseFile == null || LicenseFile == "" || LicenseFile == '' || LicenseFile == undefined) {
                }
                else {
                    file = '<a href="' + InsuranceDoc + '?Id=' + cl + '&Insurance=' + Insurance + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Receipt</span></a></div>';
                }
                if (Data.Status == "Expired")
                {
                    ci = '<a href="javascript:void(0)"  title="view" id="UpdateLicenseDetails" aid="' + cl + '" style=" float: left;cursor:pointer;"><button class="btn btn-primary" style="border-radius:25px;width:80px;">Update</button></a></div>';
                }
                jQuery("#tbl_LicenseList").jqGrid('setRowData', ids[i], { act: be + file + ci });
            }
            if ($("#tbl_LicenseList").getGridParam("records") <= 20) {
                $("#divLicenseListPager").hide();
            }
            else {
                $("#divLicenseListPager").show();
            }
            if ($('#tbl_LicenseList').getGridParam('records') === 0) {
                $('#tbl_LicenseList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddLicense"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_LicenseList").getGridParam("records") > 20) {
        jQuery("#tbl_LicenseList").jqGrid('navGrid', '#divLicenseListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});


$("#AddLicense").live("click", function (event) {
    window.location.href = $_HostPrefix + addDetails + "?VendorID=" + $_VendorID;
});
$("#UpdateLicenseDetails").live("click", function (event) {
    var id = $(this).attr("aid");
    window.location.href = $_HostPrefix + addDetails + '?id=' + id + "&VendorID=" + $_VendorID;
});

$("#ActiveLicense").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    var IsLicense = "License";
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveLicense + "?InsuranceLicenseId=" + id + "&IsActive=" + IsActive + "&IsInsuranceLicense=" + IsLicense,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_LicenseList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});