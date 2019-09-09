var CountUrl = 'eCountingReport/GetVendorCountList';
var SubMiscUrl = 'Miscellaneous/GetListMiscellaneousListByMiscId';
var ApproveUrl = 'Miscellaneous/ApproveData';
var MiscViewUrl = 'Miscellaneous/ViewMiscellaneous';
var CostCodeId;
var vehcileApprovalStatus = '';
var CalculateRemainingAmt;
$(function () {
    $("#tbl_AllVendorCountList").jqGrid({
        url: $_HostPrefix + CountUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 'auto',
        width: 650,
        autowidth: true,
        colNames: ['Pending PO', 'Approve PO', 'Pending Bill', 'Approve Bill', 'Pending Payment'],
        colModel: [{ name: 'PendingPO', width: 30, sortable: true },
        { name: 'ApprovePO', width: 30, sortable: false },
        { name: 'PendingBill', width: 30, sortable: true },
        { name: 'ApproveBill', width: 30, sortable: false },
        { name: 'PendingPayment', width: 30, sortable: true }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divAllVendorCountListPager',
        sortname: 'PendingPO',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_AllVendorCountList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_AllVendorCountList").jqGrid('setRowData', ids[i], { act: be + de + qrc }); ///+ qrc 
            }
            if ($("#tbl_AllVendorCountList").getGridParam("records") <= 20) {
                $("#divAllVendorCountListPager").hide();
            }
            else {
                $("#divAllVendorCountListPager").show();
            }
            if ($('#tbl_AllVendorCountList').getGridParam('records') === 0) {
                $('#tbl_AllVendorCountList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        onCellSelect: function (rowid, iCol, id) {
            var rowData = $(this).jqGrid("getRowData", rowid);
            //var rid = jQuery('#tbl_CostCodeList').jqGrid("getGridParam", "selrow");
            var costCode = rowData.CostCode;
            var rid = $('#tbl_AllVendorCountList').jqGrid('getCell', rowid, iCol);
            if (iCol == 1) { //You clicked on admin block
                CostCodeId = id;
                var row = jQuery('#tbl_AllVendorCountList').jqGrid("getRowData", rowid);
                $.ajax({
                    type: "post",
                    url: $_HostPrefix + SubMiscUrl + '?MiscId=' + rowid,
                    datatype: 'json',
                    type: 'GET',
                    success: function (result) {
                        var arrData = [];
                        if (result.rows.length > 0) {
                            for (i = 0; i < result.rows.length; i++) {
                                arrData.push({
                                    "Status": result.rows[i].cell[0],
                                    "MISId": result.rows[i].cell[1],
                                    "LocationName": result.rows[i].cell[2],
                                    "VendorName": result.rows[i].cell[3],
                                    "UserName": result.rows[i].cell[4],
                                    "InvoiceAmount": result.rows[i].cell[5],
                                    "MISDate": result.rows[i].cell[6],
                                    "Document": result.rows[i].cell[7],
                                    "Comment": result.rows[i].cell[8],
                                    "MId": result.rows[i].cell[9]
                                });
                            }
                        }
                        jQuery('#tbl_AllVendorCountList').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 })
                               .trigger('reloadGrid');
                    }
                });
            }
        },
        caption: '<label>List of Miscellaneous</label>'
    });
    if ($("#tbl_AllVendorCountList").getGridParam("records") > 20) {
        jQuery("#tbl_AllVendorCountList").jqGrid('navGrid', '#divAllVendorCountListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {

    var txtSearch = jQuery("#SearchText").val();
    var statusType = jQuery("#vehcileStatusType :selected").val();
    jQuery("#tbl_MasterMiscellaneousList").jqGrid('setGridParam', { url: $_HostPrefix + CostCodeUrl + "?txtSearch=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
$("#btnApprove").live("click", function (event) {
    var GridData = $('#tbl_ChildMiscellaneousDetails').getRowData();
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ obj: GridData }),
        success: function (result) {
            toastr.success("Data approve successfully.");
            jQuery("#tbl_MasterMiscellaneousList").trigger("reloadGrid");
            jQuery("#tbl_ChildMiscellaneousDetails").trigger("reloadGrid");
            window.location.href = $_HostPrefix + MiscViewUrl;
        },
        error: function () { alert(" Something went wrong..") },
    });
});




