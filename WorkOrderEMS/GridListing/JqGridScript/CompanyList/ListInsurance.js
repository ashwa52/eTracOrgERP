var Url = 'VendorManagement/ListInsurance';
var ActiveInsurance = 'VendorManagement/ActiveInsurance';
var InsuranceDoc = '../VendorManagement/InsuranceDownload/';
var addInsuranceDetails = 'VendorManagement/AddInsurace/';
var Insurance = true;
$(function () {
    var act;
    $("#tbl_LicenseAndInsuranceList").jsGrid({
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
                    url: $_HostPrefix + Url + '?VendorId=' + $_VendorID + "&VendorStatus=" + $_VendorStatus,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [ 
            { name: "InsuranceCarries", title: "Insurance Carries", type: "text", width: 50 },
            { name: "PolicyNumber", title: "Policy Number", type: "text", width: 50 },
            { name: "DisplayLicenseExpirationDate", title: "Expiration Date", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 }, 
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check check-icon" }).attr({ style: "" });
                    var $iconPencilForAccountDeactivate = $("<i>").attr({ class: "fa fa-close close-icon" }).attr({ style: "" });
                    var $iconFileFordocument = $("<i>").attr({ class: "fa fa-download download-icon" }).attr({ style: "" });
                    var $iconForExpired = $("<i>").attr({ class: "fa fa-close close-icon" }).attr({ style: "" });
                    if (item.Status == "Activated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Deactivate" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ActiveDeActiveInsurance(item.Id, "N");
                        }).append($iconPencilForAccountApprove);
                    }
                    else if (item.Status == "Deactivated")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Active" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ActiveDeActiveInsurance(item.Id, "Y");
                        }).append($iconPencilForAccountDeactivate);


                    }
                    if (item.Status == "Expired") {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Update" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            UpdateInsurance(item.Id);

                        }).append($iconForExpired);

                    }
                    if (item.InsuranceDocument == null || item.InsuranceDocument == "" || item.InsuranceDocument == '' || item.InsuranceDocument == undefined) {
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

    //$("#tbl_LicenseAndInsuranceList").jqGrid({
    //    url: $_HostPrefix + Url + '?VendorId=' + $_VendorID + "&VendorStatus=" + $_VendorStatus,
    //    datatype: 'json',
    //    type: 'GET',
    //    height: 300,
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['InsuranceId', 'VendorListId', 'Insurance Carries', 'Policy Number', 'ExpirationDate', 'InsuranceDocument', 'Status', 'Actions'],
    //    colModel: [
    //        { name: 'InsuranceId', width: 30, sortable: true, hidden: true },
    //        { name: 'VendorListId', width: 30, sortable: true,hidden:true },
    //        { name: 'InsuranceCarries', width: 30, sortable: true },
    //        { name: 'PolicyNumber', width: 30, sortable: true },
    //        { name: 'InsuranceExpirationDate', width: 30, sortable: true },
    //        { name: 'InsuranceDocument', width: 30, sortable: true, hidden: true },
    //        { name: 'Status', width: 30, sortable: true },
    //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    scrollOffset: 0,
    //    pager: '#divLicenseAndInsuranceListPager',
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
    //        var ids = jQuery("#tbl_LicenseAndInsuranceList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            be = "";
    //            var Data = jQuery("#tbl_LicenseAndInsuranceList").getRowData(cl);
    //            var InsuranceFile = Data['InsuranceDocument'];
    //            if (Data.Status == "Activated") {
    //                be = '<a href="javascript:void(0)" class="download-cloud" title="AlreadyApprove" aid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-ban fa-2x"></span><span class="tooltips" style="width:100px;">Already Approve</span></a></div>';

    //                //be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveInsurance" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Click to deactivate</span></a>';
    //            }
    //            else if (Data.Status == "Deactivated") {
    //                be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveInsurance" iid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
    //            }
    //            var file = "";
    //            var ci = "";
    //            if (InsuranceFile == null || InsuranceFile == "" || InsuranceFile == '' || InsuranceFile == undefined) {
    //            }
    //            else {
    //                file = '<a href="' + InsuranceDoc + '?Id=' + cl + '&Insurance=' + Insurance + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Receipt</span></a></div>';
    //            }
    //            if (Data.status == "Expired") {
    //                ci = '<a href="javascript:void(0)"  title="view" id="UpdateInuranceLicense" aid="' + cl + '" style=" float: left;cursor:pointer;"><button class="btn btn-primary" style="border-radius:25px;width:80px;">Update</button></a></div>';
    //            }
    //            //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
    //            jQuery("#tbl_LicenseAndInsuranceList").jqGrid('setRowData', ids[i], { act: be + file +ci });
    //        }
    //        if ($("#tbl_LicenseAndInsuranceList").getGridParam("records") <= 20) {
    //            $("#divLicenseAndInsuranceListPager").hide();
    //        }
    //        else {
    //            $("#divLicenseAndInsuranceListPager").show();
    //        }
    //        if ($('#tbl_LicenseAndInsuranceList').getGridParam('records') === 0) {
    //            $('#tbl_LicenseAndInsuranceList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //    caption: '<div id="AddInsurance"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    //});
    //if ($("#tbl_LicenseAndInsuranceList").getGridParam("records") > 20) {
    //    jQuery("#tbl_LicenseAndInsuranceList").jqGrid('navGrid', '#divLicenseAndInsuranceListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});


function AddInsurance() {
    window.location.href = $_HostPrefix + addInsuranceDetails + '?VendorID=' + $_VendorID;
};
function UpdateInsurance(id) {
    
    window.location.href = $_HostPrefix + addDetails + '?id=' + id + "&VendorID=" + $_VendorID;;
};

function ActiveDeActiveInsurance(id, Status)
{
     
    var IsInsurance = "Insurance";
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveInsurance + "?InsuranceLicenseId=" + id + "&IsActive=" + Status + "&IsInsuranceLicense=" + IsInsurance,
        ///data:{},
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
          
            $("#tbl_LicenseAndInsuranceList").jsGrid("loadData"); 
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
};