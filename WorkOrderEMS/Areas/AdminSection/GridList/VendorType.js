var VendorTypeUrl = 'VendorType/GetVendorTypes';
var DeleteVendorTypeUrl = 'VendorType/DeleteVendorType';
var ActiveVendorTypeUrl = 'VendorType/ActiveVendorType';

$(function () {
    $("#tbl_VendorTypeList").jqGrid({
        url: $_HostPrefix + VendorTypeUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Vendor Type', 'Vendor_IsActive', 'Actions'],
        colModel: [{ name: 'CTT_VendorType', width: 30, sortable: true },
        { name: 'Vendor_IsActive', width: 30, sortable: true , hidden:true},
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divVendorTypeListPager',
        sortname: 'CTT_VendorType',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Vendor Type",
        gridComplete: function () {
            var ids = jQuery("#tbl_VendorTypeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_VendorTypeList").getRowData(cl);
                if (Data.Vendor_IsActive == "Y") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" id="ActiveVendorType" IsActive="N" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" id="ActiveVendorType" IsActive="Y" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                jQuery("#tbl_VendorTypeList").jqGrid('setRowData', ids[i], { act: be });
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                //jQuery("#tbl_VendorTypeList").jqGrid('setRowData', ids[i], { act: de });
            }
            if ($("#tbl_VendorTypeList").getGridParam("records") <= 20) {
                $("#divVendorTypeListPager").hide();
            }
            else {
                $("#divVendorTypeListPager").show();
            }
            if ($('#tbl_VendorTypeList').getGridParam('records') === 0) {
                $('#tbl_VendorTypeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddVendorType"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_VendorTypeList").getGridParam("records") > 20) {
        jQuery("#tbl_VendorTypeList").jqGrid('navGrid', '#divVendorTypeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_VendorTypeList").jqGrid('setGridParam', { url: $_HostPrefix + VendorTypeUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddVendorType").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: {},
        url: '../VendorType/AddVendorType/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $('.modal-title').text("Add Vendor Type");
            $("#largeeditpopup").html(result);
            $("#myModallarge").modal('show');
        }
    });
});

$("#ActiveVendorType").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveVendorTypeUrl + "?vendorTypeId=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_VendorTypeList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});
