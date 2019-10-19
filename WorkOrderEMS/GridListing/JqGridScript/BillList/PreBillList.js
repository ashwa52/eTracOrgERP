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

    $('#myModelRejectBill').on('hidden.bs.modal', function () {
        $("#backgroundDiv").css("display", "none");
    });

    var act;
    var _searchresult = $("#searchPreBilltext").val();//searchPreBilltext
    $(function () {        
        
        $("#tbl_PreBillList").jsGrid({
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
                        url: $_HostPrefix + BillUrl + '?txtSearch=' + _searchresult + '&locationId=' + $_locationId,
                        data: filter,
                        dataType: "json"
                    });
                }
            },

            fields: [
                { name: "BillId", title: "Bill Id", type: "text", width: 50 },
                { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
                { name: "EmployeeName", title: "Employee Name", type: "text", width: 50 },
                { name: "VendorType", title: "Vendor Type", type: "text", width: 50 },
                { name: "BillDate", title: "Bill Date", type: "text", width: 50 },
                { name: "BillAmount", title: "Bill Amount", type: "text", width: 50 },
                { name: "Status", title: "Status", type: "text", width: 50 },
                { name: "Comment", title: "Comment", type: "text", width: 50, visible: false },
                { name: "BillImage", title: "Bill Image", type: "text", width: 50, visible: false },
                { name: "LBLL_Id", title: "LBLL Id", type: "text", width: 50 },
                { name: "VendorId", title: "VendorId", type: "text", width: 50, visible: false },
                {
                    name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                        var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });
                        var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "View Details" })
                            .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                ViewDetails(item);
                            }).append($iconPencil);
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                    }
                }
            ]
        });
    });
   
    $('#ViewAllLocation').change(function () {        
        ViewAllRecordsBill();
    });

    $("#searchPreBilltext").keyup(function () {
        doSearch()
    });
});

//$(document).ready(function () {
//    debugger;
   
//});

var timeoutHnd;
var flAuto = true;
function doSearch() {

    var act;
    var _searchresult = $("#searchPreBilltext").val();
    debugger;
    $("#tbl_PreBillList").jsGrid({
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
                    url: $_HostPrefix + BillUrl + '?txtSearch=' + _searchresult + '&locationId=' + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "BillId", title: "Bill Id", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "EmployeeName", title: "Employee Name", type: "text", width: 50 },
            { name: "VendorType", title: "Vendor Type", type: "text", width: 50 },
            { name: "BillDate", title: "Bill Date", type: "text", width: 50 },
            { name: "BillAmount", title: "Bill Amount", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            { name: "Comment", title: "Comment", type: "text", width: 50, visible: false},
            { name: "BillImage", title: "Bill Image", type: "text", width: 50, visible: false },
            { name: "LBLL_Id", title: "LBLL Id", type: "text", width: 50 },
            { name: "VendorId", title: "VendorId", type: "text", width: 50, visible: false },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });
                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);
                        }).append($iconPencil);
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                }
            }
        ]
    });
}

function ViewDetails(item) { 
    $("#lblBillId").html(item.BillId);
    $("#lblVendorName").html(item.VendorName);
    $("#lblVendorType").html(item.VendorType);
    $("#lblBillDate").html(item.BillDate);
    $("#lblBillAmount").html(item.BillAmount);
    $("#lblStatus").html(item.Status);
    $("#lblComment").html(item.Comment);
    $("#lblBillImage").html(item.BillImage);
    $('div #lblBillImage img').attr('width', '100px');
    $('div #lblBillImage img').attr('height', '100px');

    //if (Image == '' || Image == null || Image == "") {
    //    $("#labelMiscImage").hide();
    //    $("#lblMiscImage").hide();
    //}
    
    $('.modal-title').text("Bill Details");
    $("#myModalForPreBillData").modal('show');///myModalForPreBillData  ///myModalForBillData

    //$("#myModalForMiscellaneousData").modal('show');


}
var billStatus;
function RejectBillAfterCommentBill() {
    billStatus = "Rejected";
    
    callAjaxbill(billStatus)
}
function ApproveBill() {
    billStatus = "approved";
    $("#ApproveBill").addClass("disabled");
    callAjaxbill(billStatus)
}
function callAjaxbill(billStatus) {      //$("#ApproveBill").live("click", function (event) {
    var getdata = {};
    getdata.BillId = $("#lblBillId").html();
    getdata.VendorName = $("#lblVendorName").html();
    getdata.VendorType = $("#lblVendorType").html();
    getdata.BillDate = $("#lblBillDate").html();
    getdata.BillAmount = $("#lblBillAmount").html();
    getdata.Status = billStatus;
    getdata.Comment = $("#CommentBill").val();
    ////////$("#lblComment").html();
    debugger;
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ obj: getdata, LocationId: $_locationId, FacilityData: FacilityData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            debugger;
            //toastr.success(result);
            alert(result);
            $("#ApproveBill").removeClass("disabled");
            $("#myModalForPreBillData").modal('hide');            
        },
        error: function () { alert(" Something went wrong..") },
        complete: function () {
            
            fn_hideMaskloader();
        }
    });
}

$("#viewPreBill").on("click", function (event) {
    id = $(this).attr("vid");
    //var rowData = jQuery("#tbl_PreBillList").getRowData(id);

    var rowData = $("#tbl_PreBillList").jsGrid("option", "data");
   
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
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation1 :selected").val();
    if (locaId == 0) {
        $("#drp_MasterLocation1").hide();
    }
    else {
        $("#drp_MasterLocation1").show();
    }
    //jQuery("#tbl_PreBillList").jqGrid('setGridParam', { url: $_HostPrefix + BillUrl + '?LocationId=' + locaId, page: 1 }).trigger("reloadGrid");
    ViewAllLocation()
}

function ViewAllLocation() {
   
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation1 :selected").val();
    
    var act;
    $("#tbl_PreBillList").jsGrid({
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
                    url: $_HostPrefix + BillUrl + '?locationId=' + locaId,
                    data: filter,
                    dataType: "json"
                });
            }
        },
        fields: [
            { name: "BillId", title: "Bill Id", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "EmployeeName", title: "Employee Name", type: "text", width: 50 },
            { name: "VendorType", title: "Vendor Type", type: "text", width: 50 },
            { name: "BillDate", title: "Bill Date", type: "text", width: 50 },
            { name: "BillAmount", title: "Bill Amount", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            { name: "Comment", title: "Comment", type: "text", width: 50 , visible: false },
            { name: "BillImage", title: "Bill Image", type: "text", width: 50, visible: false },
            { name: "LBLL_Id", title: "LBLL Id", type: "text", width: 50 },
            { name: "VendorId", title: "VendorId", type: "text", width: 50, visible: false },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });
                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);
                        }).append($iconPencil);
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                }
            }
        ]
    });
}