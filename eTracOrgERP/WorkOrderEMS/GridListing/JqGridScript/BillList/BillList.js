var BillUrl = 'Bill/GetListBill';
var ApproveUrl = 'Bill/ApproveBillData/';
var CostCodeId; var id;
var vehcileApprovalStatus = '';

$(function () {
    $("#tbl_BillList").jqGrid({
        url: $_HostPrefix + BillUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 430,
        width: 650,
        autowidth: true,
        colNames: ['Bill Id', 'Vendor Name', 'Vendor Type', 'Bill Date', 'Bill Amount', 'Invoice Date', 'Status','Comment','Bill Image','LBLL_ID','Action'],
        colModel: [{ name: 'BillId', width: 30, sortable: true },
        { name: 'VendorName', width: 40, sortable: false },
        { name: 'VendorType', width: 20, sortable: true },
        { name: 'BillDate', width: 40, sortable: false },
        { name: 'BillAmount', width: 20, sortable: true },
        { name: 'InvoiceDate', width: 20, sortable: true },
        //{ name: 'BillType', width: 20, sortable: true },
        { name: 'Status', width: 20, sortable: true, hidden: true },
        { name: 'Comment', width: 20, sortable: true, hidden: true },
        { name: 'BillImage', width: 20, sortable: true, formatter: imageFormat },
        { name: 'LBLL_Id', width: 20, sortable: true, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divBillListPager',
        sortname: 'BillId',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_BillList").jqGrid('getDataIDs');
            jQuery('#tbl_BillList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                vi = '<div><a href="javascript:void(0)" class="viewRecord" id="viewBill" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a></div>';           
                jQuery("#tbl_BillList").jqGrid('setRowData', ids[i], { act: vi}); ///+ qrc 
            }
            if ($("#tbl_BillList").getGridParam("records") <= 20) {
                $("#divBillListPager").hide();
            }
            else {
                $("#divBillListPager").show();
            }
            if ($('#tbl_BillList').getGridParam('records') === 0) {
                $('#tbl_BillList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div><label>List of Bill</label><span><input type="text" class="inputSearch light-table-filter" id="searchBilltext" placeholder="Vendor Name"  data-table="order-table" /></div>'
    });
    if ($("#tbl_BillList").getGridParam("records") > 20) {
        jQuery("#tbl_BillList").jqGrid('navGrid', '#divBillListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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

    jQuery("#tbl_BillList").jqGrid('setGridParam', { url: $_HostPrefix + GetListBill + "?LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
function RejectBillAfterCommentBill()
{
    callAjaxbill()
}
function ApproveBill()
{
    callAjaxbill()
}
function callAjaxbill() {      //$("#ApproveBill").live("click", function (event) {
    var GridData = $('#tbl_BillList').getRowData(id);
    GridData.Comment = $("#CommentBill").val();
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ obj: GridData, LocationId: $_locationId }),
        success: function (result) {
            toastr.success(result);
            $("#myModalForBillData").modal('hide');
            jQuery("#tbl_BillList").trigger("reloadGrid");
        },
        error: function () { alert(" Something went wrong..") },
    });
}

$("#viewBill").live("click", function (event) {
    id = $(this).attr("vid");
    var rowData = jQuery("#tbl_BillList").getRowData(id);
    if (rowData.Status != "M")
    {
        $('#ApproveBill').hide();
        $('#RejectBill').hide();
    }
    else
    {
        $('#ApproveBill').show();
        $('#RejectBill').show();
    }
    var BillId = rowData['BillId'];
    var VendorName = rowData['VendorName'];
    var VendorType = rowData['VendorType'];
    var BillDate = rowData['BillDate'];
    var BillAmount = rowData['BillAmount'];
    var InvoiceDate = rowData['InvoiceDate'];
    var Status = rowData['Status'];
    var Comment = rowData['Comment'];
    $("#lblBillId").html(BillId);
    $("#lblVendorName").html(VendorName);
    $("#lblVendorType").html(VendorType);
    $("#lblBillDate").html(BillDate);
    $("#lblBillAmount").html(BillAmount);
    $("#lblInvoiceDate").html(InvoiceDate);
    $("#lblStatus").html(Status);
    $("#lblComment").html(Comment);
    var BillImage = rowData['BillImage'];
  
    $("#lblBillImage").html(BillImage);
    $('div #lblBillImage img').attr('width', '100px');
    $('div #lblBillImage img').attr('height', '100px');
    if (BillImage == '' || BillImage == null || BillImage == "") {
        $("#labelBillImage").hide();
        $("#lblBillImage").hide();
    }
    $('.modal-title').text("Bill Details");
    $("#myModalForBillData").modal('show');
});

function loadpreview(result) {
    var isanyfieldempty = false;
    var QRCType;
    var errorMessage;
    $_QRCIDNumber = result.data.QRCID.toString();
    generateqrcodeByVJ();
    if ($("#QRCType").val() != "36") {
        $(".VehicleTypeDisplay").css('display', 'none');
    }
    else {
        $(".VehicleTypeDisplay").css('display', '');
    }
    $('#myModalFORQRC').modal('show');
    $("#ModalConfirumationPreview").modal('show');
    return !isanyfieldempty;
}
//#region Image
function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" id="driverImage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion

