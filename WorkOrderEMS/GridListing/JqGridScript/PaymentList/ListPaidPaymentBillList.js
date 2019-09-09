var PaidUrl = 'Payment/GetPaidListByBillID';
$(function () {
    $("#tbl_PaymentPaidList").jqGrid({
        url: $_HostPrefix + PaidUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 650,
        autowidth: true,
        colNames: ['Bill No', 'Location Name', 'Vendor Name/Employee Name', 'Operating Company', 'Bill Type', 'Bill Amount', 'Bill Date', 'Grace Period', 'Payment Mode', 'Description', 'Status', 'OperatingCompanyId','LocationId'],
        colModel: [{ name: 'BillNo', width: 30, sortable: true },
        { name: 'LocationName', width: 40, sortable: false },
        { name: 'VendorName', width: 50, sortable: true },
        { name: 'OperatingCompany', width: 50, sortable: true },
        { name: 'BillType', width: 30, sortable: false },
        { name: 'BillAmount', width: 20, sortable: true },
        { name: 'BillDate', width: 20, sortable: true },
        { name: 'GracePeriod', width: 20, sortable: true },
        { name: 'PaymentMode', width: 20, sortable: true },
        { name: 'Description', width: 20, sortable: true },
        { name: 'Status', width: 20, sortable: true },
        { name: 'OperatingCompanyId', width: 20, sortable: true, hidden: true },
        { name: 'LocationId', width: 20, sortable: true, hidden: true }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPaymentPaidListPager',
        sortname: 'BillNo',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_PaymentPaidList").jqGrid('getDataIDs');
            jQuery('#tbl_PaymentPaidList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                vi = '<div><a href="javascript:void(0)" class="viewRecord" id="viewBill" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_PaymentPaidList").jqGrid('setRowData', ids[i], { act: vi }); ///+ qrc 
            }
            if ($("#tbl_PaymentPaidList").getGridParam("records") <= 20) {
                $("#divPaymentPaidListPager").hide();
            }
            else {
                $("#divPaymentPaidListPager").show();
            }
            if ($('#tbl_PaymentPaidList').getGridParam('records') === 0) {
                $('#tbl_PaymentPaidList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div><label>List of Bill</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><input type="text" class="inputSearch light-table-filter" id="searchPaymenttext" placeholder="Vendor Name"  data-table="order-table" /></span></div>'
    });
    if ($("#tbl_PaymentPaidList").getGridParam("records") > 20) {
        jQuery("#tbl_PaymentPaidList").jqGrid('navGrid', '#divPaymentPaidListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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

    jQuery("#tbl_PaymentPaidList").jqGrid('setGridParam', { url: $_HostPrefix + GetListBill + "?LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
}

function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" id="driverImage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion

