var BillUrl = 'Bill/GetListPreBill';
var ApproveUrl = 'Bill/ApproveBillData/';
var BillFacilityListUrl = 'Bill/BillFacilityListData/';
var CostCodeId; var id;
var vehcileApprovalStatus = '';
var FacilityData;
var allLocationBill = '';
//if ($_userType == "1" || $_userType == "5" || $_userType == "6") {
allLocationBill = '<div class="onoffswitch2" style="margin-left:750px;"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'
//}
$(function () {
    $("#tbl_PreBillList").jqGrid({
        url: $_HostPrefix + BillUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 430,
        width: 650,
        autowidth: true,
        colNames: ['Bill Id', 'Vendor Name', 'Employee Name', 'Vendor Type', 'Bill Date', 'Bill Amount', 'Status', 'Comment', 'LBLL_Id', 'VendorId', 'Bill Image', 'Action'],
        colModel: [{ name: 'BillId', width: 30, sortable: true },
        { name: 'VendorName', width: 40, sortable: false },
        { name: 'EmployeeName', width: 20, sortable: true },
        { name: 'VendorType', width: 20, sortable: true },
        { name: 'BillDate', width: 40, sortable: false },
        { name: 'BillAmount', width: 20, sortable: true },
        { name: 'Status', width: 20, sortable: true, hidden: true },
        { name: 'Comment', width: 20, sortable: true, hidden: true },
        { name: 'BillImage', width: 20, sortable: true, formatter: imageFormat },
        { name: 'LBLL_Id', width: 20, sortable: true, hidden: true },
        { name: 'VendorId', width: 20, sortable: true, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPreBillListPager',
        sortname: 'BillId',
        viewrecords: true,
        gridview: true,
        //loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_PreBillList").jqGrid('getDataIDs');
            jQuery('#tbl_PreBillList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                vi = '<div><a href="javascript:void(0)" class="viewRecord" id="viewPreBill" title="view" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_PreBillList").jqGrid('setRowData', ids[i], { act: vi }); ///+ qrc 
            }
            if ($("#tbl_PreBillList").getGridParam("records") <= 20) {
                $("#divPreBillListPager").hide();
            }
            else {
                $("#divPreBillListPager").show();
            }
            if ($('#tbl_PreBillList').getGridParam('records') === 0) {
                $('#tbl_PreBillList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div><label>List of Bill</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span><input type="text" class="inputSearch light-table-filter" id="searchPreBilltext" placeholder="Bill No"  data-table="order-table" /></span>' + allLocationBill + '</div>'
    });
    $('#ViewAllLocation').change(function () {
        ViewAllRecordsBill();
    });
    if ($("#tbl_PreBillList").getGridParam("records") > 20) {
        jQuery("#tbl_PreBillList").jqGrid('navGrid', '#divPreBillListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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

    jQuery("#tbl_PreBillList").jqGrid('setGridParam', { url: $_HostPrefix + GetListBill + "?LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
function RejectBillAfterCommentBill() {
    callAjaxbill()
}
function ApproveBill() {
    $("#ApproveBill").addClass("disabled");
    callAjaxbill()
}
function callAjaxbill() {      //$("#ApproveBill").live("click", function (event) {
    var GridData = $('#tbl_PreBillList').getRowData(id);
    GridData.Comment = $("#CommentBill").val();
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ obj: GridData, LocationId: $_locationId, FacilityData: FacilityData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#ApproveBill").removeClass("disabled");
            $("#myModalForPreBillData").modal('hide');
            jQuery("#tbl_PreBillList").trigger("reloadGrid");
        },
        error: function () { alert(" Something went wrong..") },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

$("#viewPreBill").live("click", function (event) {
    id = $(this).attr("vid");
    var rowData = jQuery("#tbl_PreBillList").getRowData(id);
   
    if (rowData.Status == "Y") {
        $('#ApproveBill').hide();
        $('#RejectBill').hide();
    }
    else {
        $('#ApproveBill').show();
        $('#RejectBill').show();
    }
    $.ajax({
        url: $_HostPrefix + BillFacilityListUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ BillId: id }),
        success: function (result) {
            FacilityData = result;
            if (result.length > 0) {
                $('#Billrecords_table').html('');
                var arrData = [];
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;"><th>Bill Facility Id</th><th>Cost Code</th><th>Facility Type</th><th>Description</th><th>Unit Price</th><th>Tax</th></tr>';
                $('#Billrecords_table').append(thHTML);
                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        var trHTML = '';
                        trHTML +=
                           '<tr><td>' + result[i].BillFacilityId +
                           '</td><td>' + result[i].CostCoseDescription +
                           '</td><td>' + result[i].FacilityType +
                           '</td><td>' + result[i].Description +
                           '</td><td>' + result[i].Amount +
                           '</td><td>' + result[i].Tax +
                           '</td></tr>';

                        $('#Billrecords_table').append(trHTML);
                    }
                }
            }
                var BillId = rowData['BillId'];
                var VendorName = rowData['VendorName'];
                var VendorType = rowData['VendorType'];
                var BillDate = rowData['BillDate'];
                var BillAmount = rowData['BillAmount'];
                var Status = rowData['Status'];
                var Comment = rowData['Comment'];
                $("#lblBillId").html(BillId);
                $("#lblVendorName").html(VendorName);
                $("#lblVendorType").html(VendorType);
                $("#lblBillDate").html(BillDate);
                $("#lblBillAmount").html(BillAmount);
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
        },
        error: function () { alert(" Something went wrong..") },
    });
    $('.modal-title').text("Bill Details");
    $("#myModalForPreBillData").modal('show');
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
function ViewAllRecordsBill() {
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();
    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }
    jQuery("#tbl_PreBillList").jqGrid('setGridParam', { url: $_HostPrefix + BillUrl + '?LocationId=' + locaId, page: 1 }).trigger("reloadGrid");
}
