var MiscUrl = 'Miscellaneous/GetListMasterMiscellaneous';
var SubMiscUrl = 'Miscellaneous/GetListMiscellaneousListByMiscId';
var ApproveUrl = 'Miscellaneous/ApproveData';
var MiscViewUrl = 'Miscellaneous/ViewMiscellaneous';
var CostCodeId;
var vehcileApprovalStatus = '';
var CalculateRemainingAmt;
var allLocation = '';

//if ($_userType == "1" || $_userType == "5" || $_userType == "6") {
allLocation = '<div class="onoffswitch2" style="margin-left: 860px;"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'
//}


$(function () {
    var act;
    var _searchresult = $("#SearchText").val();
    $("#tbl_MasterMiscellaneousList").jsGrid({
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
                    url: $_HostPrefix + MiscUrl + '?txtSearch' + _searchresult + '&LocationId=' + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "MISId", title: "Miscellaneous ID", type: "text", width: 50 },
            { name: "LocationName", title: "Location Name", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "UserName", title: "User Name", type: "text", width: 50 },
            { name: "InvoiceAmount", title: "Invoice Amount", type: "text", width: 50 },
            { name: "MISDate", title: "MIS Date", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 }


        ],
        rowClick: function (args) {
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];

            $.each(keys, function (idx, value) {
                if (value == "MISId") {
                    $("#MISId").val(getData[value]);
                }
                //text.push(value + " : " + getData[value])
            });
            getListMiscellaneousByMiscId();
        }
    });
});



function doSearch() {
    var act;
    var _searchresult = $("#SearchText").val();
    $("#tbl_MasterMiscellaneousList").jsGrid({
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
                    url: $_HostPrefix + MiscUrl + '?txtSearch=' + _searchresult + '&LocationId=' + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "MISId", title: "Miscellaneous ID", type: "text", width: 50 },
            { name: "LocationName", title: "Location Name", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "UserName", title: "User Name", type: "text", width: 50 },
            { name: "InvoiceAmount", title: "Invoice Amount", type: "text", width: 50 },
            { name: "MISDate", title: "MIS Date", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 }
        ],

    });
}


function ViewDetails(item) {

    
    VendorId = item.MISId;



    $("#lblMiscId").html(item.MISId);
    $("#lblLocationName").html(item.LocationName);
    $("#lblVendorName").html(item.VendorName);
    $("#lblUserName").html(item.UserName);
    $("#lblInvoiceAmount").html(item.InvoiceAmount);
    $("#lblUserName").html(item.UserName);
    $("#lblMiscDate").html(item.MISDate);
    $("#lblComment").html(item.Comment);
    /*
    $("#lblMiscImage").html(Image);
    $('div #lblMiscImage img').attr('width', '100px');
    $('div #lblMiscImage img').attr('height', '100px');
    if (Image == '' || Image == null || Image == "") {
        $("#labelMiscImage").hide();
        $("#lblMiscImage").hide();
    }*/
    $('.modal-title').text("Miscellaneous Details");
    $("#myModalForMiscellaneousData").modal('show');



    //$("#myModalForMiscellaneousData").modal('show');

  
}


$(document).ready(function () {
    $("#miscDetailList").hide();
    $("#SearchText").keyup(function () {
        doSearch()
    });

    $('#ViewAllLocation').change(function () {
        ViewAllRecordsMiscellaneous();
    });

});

function getListMiscellaneousByMiscId() {
    var act;
    $("#miscDetailList").show();
    $("#tbl_MasterMiscellaneousListdetail").jsGrid({
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
                    url: $_HostPrefix + SubMiscUrl + '?MiscId=' + $("#MISId").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            
            {
                name: "MiscStatus", title: "Status", align: "center",
                itemTemplate: function (value, item) {
                    return $("<input>").attr("type", "checkbox")
                        .attr("checked", value || item.Checked)
                        .on("change", function () {
                            item.Checked = $(this).is(":checked");
                        });
                }
            },
            

            { name: "MISId", title: "MISId", type: "text", width: 30 },
            { name: "LocationName", title: "LocationName", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor/Employee Name", type: "text", width: 70 },
            { name: "InvoiceAmount", title: "InvoiceAmount", type: "text", width: 50 },
            { name: "MISDate", title: "MISDate", type: "text", width: 50 },
            { name: "Document", title: "Image", type: "text", width: 50 },
            { name: "MISDate", title: "MISDate", type: "text", width: 50 },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {                    
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list list-icon" }).attr({ style: "" });

                    var $customEditButton = $("<span class='view-detail-icon'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);                            
                        }).append($iconPencil);


                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                    
                }
            }

        ],

    });
}

/*
$(function () {
    $("#tbl_MasterMiscellaneousList").jqGrid({
        url: $_HostPrefix + MiscUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 'auto',
        width: 650,
        autowidth: true,
        colNames: ['Miscellaneous ID', 'Location', 'Vendor/Employee Name', 'User Name', 'Invoice Amount', 'Miscellaneous Date', 'Status'],
        colModel: [{ name: 'MISId', width: 30, sortable: true },
        { name: 'LocationName', width: 40, sortable: false },
        { name: 'VendorName', width: 20, sortable: true },
        { name: 'UserName', width: 40, sortable: false ,hidden:true},
        { name: 'InvoiceAmount', width: 20, sortable: true },
        { name: 'MISDate', width: 20, sortable: true },
         { name: 'Status', width: 20, sortable: true }],
        //{ name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divMasterMiscellaneousListPager',
        sortname: 'MiscNumber',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
           
            var ids = jQuery("#tbl_MasterMiscellaneousList").jqGrid('getDataIDs');
            jQuery('#tbl_MasterMiscellaneousList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_MasterMiscellaneousList").jqGrid('setRowData', ids[i], { act: be + de + qrc }); ///+ qrc 
            }
            if ($("#tbl_MasterMiscellaneousList").getGridParam("records") <= 20) {
                $("#divMasterMiscellaneousListPager").hide();
            }
            else {
                $("#divMasterMiscellaneousListPager").show();
            }
            if ($('#tbl_MasterMiscellaneousList').getGridParam('records') === 0) {
                $('#tbl_MasterMiscellaneousList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        onCellSelect: function (rowid, iCol, id) {
            var rowData = $(this).jqGrid("getRowData", rowid);
            //var rid = jQuery('#tbl_CostCodeList').jqGrid("getGridParam", "selrow");
            var costCode = rowData.CostCode;
            var rid = $('#tbl_MasterMiscellaneousList').jqGrid('getCell', rowid, iCol);
            if (iCol == 1) { //You clicked on admin block
                CostCodeId = id;
                var row = jQuery('#tbl_MasterMiscellaneousList').jqGrid("getRowData", rowid);
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
                                    "Status":result.rows[i].cell[0],
                                    "MISId": result.rows[i].cell[1],
                                    "LocationName": result.rows[i].cell[2],
                                    "VendorName": result.rows[i].cell[3],
                                    "UserName": result.rows[i].cell[4],
                                    "InvoiceAmount": result.rows[i].cell[5],
                                    "MISDate": result.rows[i].cell[6],
                                    "Document": result.rows[i].cell[7],
                                    "Comment": result.rows[i].cell[8], 
                                    "MId": result.rows[i].cell[9],
                                    "LocationId": result.rows[i].cell[10],
                                    "Vendor": result.rows[i].cell[11]
                                });
                            }
                        }
                        jQuery('#tbl_ChildMiscellaneousDetails').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 })
                               .trigger('reloadGrid');
                        getColumnIndexByName = function (grid, columnName) {
                            var cm = jQuery('#tbl_ChildMiscellaneousDetails').jqGrid('getGridParam', 'colModel'), i, l;
                            for (i = 0, l = cm.length; i < l; i += 1) {
                                if (cm[i].name === columnName) {
                                    return i; // return the index
                                }
                            }
                            return true;
                        }
                        $("#tbl_ChildMiscellaneousDetails").jqGrid({
                            datatype: "local",
                            data: arrData,
                            contentType: "application/json; charset-utf-8",
                            mtype: 'GET',
                            height: 'auto',
                            width: 700,
                            autowidth: true,
                            cellEdit: true,
                            cellsubmit: 'clientArray',
                            editurl: 'clientArray',
                            colNames: ['Status', 'MIS Id', 'Location Name', 'Vendor/Employee Name', 'User Name', 'Invoice Amount', 'MIS Date', 'Image', 'Comment', 'MId','LocationId','Vendor', 'Action'],
                            colModel: [{
                                name: 'Status', width: 15, sortable: false,
                                align: "center",
                                editoptions: { value: "Y:N" },
                                editable: false,
                                edittype: 'checkbox',
                                formatter: "checkbox",
                                search: false,
                                formatoptions: { disabled: false },
                            },
                            { name: 'MISId', width: 30, sortable: true },
                            { name: 'LocationName', width: 30, sortable: true },
                            { name: 'VendorName', width: 40, sortable: false },
                            { name: 'UserName', width: 30, sortable: true,hidden:true },
                            { name: 'InvoiceAmount', width: 40, sortable: false },
                            { name: 'MISDate', width: 40, sortable: false },
                            { name: 'Document', width: 30, sortable: false, formatter: imageFormat },
                            { name: 'Comment', width: 40, sortable: false ,editable:true},
                            { name: 'MId', width: 40, sortable: false, hidden: true, },
                            { name: 'LocationId', width: 40, sortable: false, hidden: true, },
                            { name: 'Vendor', width: 40, sortable: false, hidden: true, },
                            { name: 'act', index: 'act', width: 30, sortable: false }],
                            rownum: 10,
                            rowList: [10, 20, 30],
                            scrollOffset: 0,
                            pager: '#divChildMiscellaneousDetailsListPager',
                            sortname: 'MiscNumber',
                            viewrecords: true,
                            gridview: true,
                            loadonce: false,
                            multiSort: true,
                            rownumbers: true,
                            emptyrecords: "No records to display",
                            shrinkToFit: true,
                            sortorder: 'asc',
                            gridComplete: function () {
                                if (row.Status == "Approved") {
                                    $('#btnApprove').attr('disabled', 'disabled');
                                    jQuery('#tbl_ChildMiscellaneousDetails').setColProp('Comment',{editable:false});
                                 }
                                else {
                                    jQuery('#tbl_ChildMiscellaneousDetails').setColProp('Comment', { editable: true });   
                                }
                                var ids = jQuery("#tbl_ChildMiscellaneousDetails").jqGrid('getDataIDs');
                                for (var i = 0; i < ids.length; i++) {
                                    var cl = ids[i];
                                    be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                                    de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                                    vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="MiscView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                                    qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                                    jQuery("#tbl_ChildMiscellaneousDetails").jqGrid('setRowData', ids[i], { act: vi}); ///+ qrc 
                                }
                                if ($("#tbl_ChildMiscellaneousDetails").getGridParam("records") <= 20) {
                                    $("#divChildMiscellaneousDetailsListPager").hide();
                                }
                                else {
                                    $("#divChildMiscellaneousDetailsListPager").show();
                                }
                                if ($('#tbl_ChildMiscellaneousDetails').getGridParam('records') === 0) {
                                    $('#tbl_ChildMiscellaneousDetails tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
                                }
                                var gridmodelData = $('#tbl_ChildMiscellaneousDetails').jqGrid('getGridParam', 'data');
                                var iCol = getColumnIndexByName($(this), 'Status'), rows = this.rows, i, c = rows.length;
                                for (i = 0; i < c; i += 1) {
                                    var tt = i + 1;
                                    if (tt <= c - 1) {
                                        if (gridmodelData[i].Status == "N") {                                           
                                            var iid = $("#tbl_ChildMiscellaneousDetails tr").attr('id');
                                            $("input[value='n']").attr('checked', false);
                                            $("#" + tt, "#tbl_ChildMiscellaneousDetails").css("background-color", "#FB7869");
                                            $("#" + tt, "#tbl_ChildMiscellaneousDetails").removeClass("ui-widget-content jqgrow ui-row-ltr ui-state-hover ui-state-highlight");
                                            $("#" + tt, "#tbl_ChildMiscellaneousDetails").addClass("jqgrow");
                                        }
                                    }
                                    $(rows[i].cells[iCol]).click(function (e) {
                                        
                                        var id = $(e.target).closest('tr')[0].id,
                                            isChecked = $(e.target).is(':checked');
                                        var rowData = jQuery('#tbl_ChildMiscellaneousDetails').jqGrid('getRowData', id);
                                        //if (isChecked == true) {
                                        //    CalculateRemainingAmt += parseInt(rowData.InvoiceAmount);
                                        //}
                                        //else {
                                        //    CalculateRemainingAmt -= parseInt(rowData.InvoiceAmount);
                                        //}
                                    });
                                }
                            },
                            caption: '<span><button id="btnApprove" style="margin-left:2%;width:200%;border-radius: 20px;" class="btn btn-success">Approve</button></span>'

                        });
                    }
                });
            }
        },
        caption: '<div><span><input type="text" class="inputSearch light-table-filter" id="searchMiscellaneoutext" placeholder="MISC Number"  data-table="order-table" /></span> '+ allLocation +'</div>'
    });
    $('#ViewAllLocation').change(function () {
        ViewAllRecordsMiscellaneous();
    });
    if ($("#tbl_MasterMiscellaneousList").getGridParam("records") > 20) {
        jQuery("#tbl_MasterMiscellaneousList").jqGrid('navGrid', '#divMasterMiscellaneousListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

*/

var timeoutHnd;
var flAuto = true;
//function doSearch(ev) {
//    if (timeoutHnd)
//        clearTimeout(timeoutHnd)
//    timeoutHnd = setTimeout(gridReload, 500)
//}

function gridReload() {

    var txtSearch = jQuery("#SearchText").val();
    var statusType = jQuery("#vehcileStatusType :selected").val();
    jQuery("#tbl_MasterMiscellaneousList").jqGrid('setGridParam', { url: $_HostPrefix + CostCodeUrl + "?txtSearch=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
$("#btnApprove").on("click", function (event) {
    var GridData = $('#tbl_ChildMiscellaneousDetails').getRowData();
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ obj: GridData, LocationId: $_locationId }),
        beforesend: function () {
            new fn_showMaskloader('Please wait...Loading');
        },
        success: function (result) {
            toastr.success("Data approve successfully.");
            jQuery("#tbl_MasterMiscellaneousList").trigger("reloadGrid");
            jQuery("#tbl_ChildMiscellaneousDetails").trigger("reloadGrid");
            window.location.href = $_HostPrefix + MiscViewUrl;
        },
        error: function () { alert(" Something went wrong..") },
        complete: function () {
            new fn_hideMaskloader();
        }
    });
});

$(".EditRecord").on("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});

$("#MiscView").on("click", function (event) {
    var id = $(this).attr("vid");
    var rowData = jQuery("#tbl_ChildMiscellaneousDetails").getRowData(id);
    var MISId = rowData['MISId'];
    var LocationName = rowData['LocationName'];
    var VendorName = rowData['VendorName'];
    var UserName = rowData['UserName'];
    var InvoiceAmount = rowData['InvoiceAmount'];
    var MISDate = rowData['MISDate'];
    var Comment = rowData['Comment'];
    //var StorageAddress = rowData['StorageAddress'];
    var Image = rowData['Document'];
    $("#lblMiscId").html(MISId);
    $("#lblLocationName").html(LocationName);
    $("#lblVendorName").html(VendorName);
    $("#lblUserName").html(UserName);
    $("#lblInvoiceAmount").html(InvoiceAmount);
    $("#lblUserName").html(UserName);
    $("#lblMiscDate").html(MISDate);
    $("#lblComment").html(Comment);
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    //$("#lblFuelType").html(FuelType);
    //$("#lblGVWR").html(GVWR);
    //$("#lblStorageAddress").html(StorageAddress);
    $("#lblMiscImage").html(Image);
    $('div #lblMiscImage img').attr('width', '100px');
    $('div #lblMiscImage img').attr('height', '100px');
    if (Image == '' || Image == null || Image == "") {
        $("#labelMiscImage").hide();
        $("#lblMiscImage").hide();
    }
    $('.modal-title').text("Miscellaneous Details");
    $("#myModalForMiscellaneousData").modal('show');
});

$(".deleteRecord").on("click", function (event) {

    var id = $(this).attr("cid");
    bootbox.dialog({
        message: "are you sure you want to delete this Driver?",
        buttons: {
            success: {
                label: "delete",
                classname: "btn btn-primary",
                callback: function () {
                    $.ajax({
                        type: "post",
                        url: '../eFleetDriver/DeleteDriver' + '?DriverID=' + id,
                        //data: "{'VehicleID':'" + id + "'}",// { VehicleID: + id +   },
                        //$(event).attr("id")
                        beforesend: function () {
                            new fn_showmaskloader('please wait...');
                        },
                        success: function (data) {
                            toastr.success(data.message);
                            $('#tbl_DriverList').trigger('reloadgrid');
                            gridReload();
                        },
                        error: function () {
                            alert("error:")
                        }
                        //complete: function () {
                        //    fn_hidemaskloader();
                        //}
                    });
                }
            },
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {

                }
            }
        }
    })
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
    if (cellvalue == "") { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" id="driverImage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion
//#region toggle location
function ViewAllRecordsMiscellaneous() {
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation1 :selected").val();
    if (locaId == 0) {
        $("#drp_MasterLocation1").hide();
    }
    else {
        $("#drp_MasterLocation1").show();
    }

    ///jQuery("#tbl_MasterMiscellaneousList").jqGrid('setGridParam', { url: $_HostPrefix + MiscUrl + '?locationId=' + locaId, page: 1 }).trigger("reloadGrid");
    ViewAllLocation()
}
//#end region toggle location 


function ViewAllLocation() {
    var act;
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation1 :selected").val();
    $("#tbl_MasterMiscellaneousList").jsGrid({
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
                    url: $_HostPrefix + MiscUrl + '?LocationId=' + locaId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "MISId", title: "Miscellaneous ID", type: "text", width: 50 },
            { name: "LocationName", title: "Location Name", type: "text", width: 50 },
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "UserName", title: "User Name", type: "text", width: 50 },
            { name: "InvoiceAmount", title: "Invoice Amount", type: "text", width: 50 },
            { name: "MISDate", title: "MIS Date", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 }


        ]
    });
}

