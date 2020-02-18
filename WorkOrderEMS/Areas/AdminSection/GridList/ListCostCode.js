var CostCodeUrl = 'CostCode/GetListCostCode';
var SubCostCodeUrl = 'CostCode/GetListOfSubCostCode';
var editurl = 'eFleetDriver/EditDriver/';
var activeURL = 'CostCode/ActiveCostCode/';
var generateQRC = "eFleetVehicle/_GenerateQRCForVehicle/";
var CostCodeListUrl = 'CostCode/ListCostCode/';
var CostCodeId; var activeMasterId;
var vehcileApprovalStatus = ''
//+ '<select id="vehcileStatusType" class="" onchange="doSearch(arguments[0]||event);">'
//+ '<option value="0">All Approved</option>'
//+ '<option value="244">Approved By Manager</option>'
////+ '<option value="0">Pending</option>'
//+ '</select>';
$(function () {
    $("#tbl_CostCodeList").jqGrid({
        url: $_HostPrefix + CostCodeUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Cost Code', 'Description'],// 'Actions''IsActive'],
        colModel: [{ name: 'CostCode', width: 30, sortable: true },
        { name: 'Description', width: 40, sortable: false }],
       // { name: 'IsActive', width: 20, sortable: true }],
        //{ name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 20,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divCostCodeListPager',
        sortname: 'CostCodeNumber',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Cost Code",
        gridComplete: function () {
            var ids = jQuery("#tbl_CostCodeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                
            }
            if ($("#tbl_CostCodeList").getGridParam("records") <= 20) {
                $("#divCostCodeListPager").hide();
            }
            else {
                $("#divCostCodeListPager").show();
            }
            if ($('#tbl_CostCodeList').getGridParam('records') === 0) {
                $('#tbl_CostCodeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        onCellSelect: function (rowid, iCol, id) {
            var rowData = $(this).jqGrid("getRowData", rowid);
            //var rid = jQuery('#tbl_CostCodeList').jqGrid("getGridParam", "selrow");
            var costCode = rowData.CostCode;
             var rid = $('#tbl_CostCodeList').jqGrid('getCell', rowid, iCol);
             if (iCol == 1) { //You clicked on admin block
                 CostCodeId = id;
                 var row = jQuery('#tbl_CostCodeList').jqGrid("getRowData", rowid);
                 activeMasterId = rowid;
                 GridReloadData(activeMasterId);
            }
        },
        caption: '<div id="AddCostCode"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_CostCodeList").getGridParam("records") > 20) {
        jQuery("#tbl_CostCodeList").jqGrid('navGrid', '#divCostCodeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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

    var statusType = jQuery("#vehcileStatusType :selected").val();
    jQuery("#tbl_CostCodeList").jqGrid('setGridParam', { url: $_HostPrefix + 'CostCode/GetListOfSubCostCode?txtSearch=' , page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});
$('#ActiveCostCode').live("click", function (event) {

    id = $(this).attr("ActiveId");
    var IsActive = $(this).attr("IsActive");
    var rowData = jQuery("#tbl_SubCostCodeDetails").getRowData(id);
    id = rowData.CostCode;
    $.ajax({
        type: "POST",
        url: $_HostPrefix + activeURL + '?CostCodeId=' + id + '&IsActive=' + IsActive,
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (data) {
            debugger
            jQuery("#tbl_SubCostCodeDetails").jqGrid('setGridParam', { url: $_HostPrefix + 'CostCode/GetListOfSubCostCode?id=' + activeMasterId, type: "GET", page: 1 }).trigger("reloadGrid");
            $('#tbl_SubCostCodeDetails').trigger('reloadGrid');
            GridReloadData(activeMasterId);
            // $('#tbl_SubCostCodeDetails').trigger("reloadGrid");
           // jQuery('#tbl_SubCostCodeDetails').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 })
           //.trigger('reloadGrid');
            
            toastr.success(data);
            //$('#tbl_SubCostCodeDetails').trigger("reloadGrid");
            //$('#tbl_CostCodeList').trigger('reloadGrid');
            //window.location.href = $_HostPrefix + CostCodeListUrl;
        },
        error: function () {
            alert("error:")
        }
    });
});

function GridReloadData(rowid)
{
    debugger
    $.ajax({
        //type: "post",
        url: $_HostPrefix + 'CostCode/GetListOfSubCostCode' + '?id=' + rowid,
        datatype: 'json',
        type: 'GET',
        success: function (result) {
            var arrData = [];
            if (result.rows.length > 0) {
                for (i = 0; i < result.rows.length; i++) {
                    arrData.push({
                        "CostCode": result.rows[i].cell[0],
                        "Description": result.rows[i].cell[1],
                        "IsActive": result.rows[i].cell[2],
                    });
                }
            }
            jQuery('#tbl_SubCostCodeDetails').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 })
                   .trigger('reloadGrid');
            $("#tbl_SubCostCodeDetails").jqGrid({
                datatype: "local",
                data: arrData,
                contentType: "application/json; charset-utf-8",
                mtype: 'GET',
                height: 400,
                width: 700,
                autowidth: true,
                colNames: ['Sub Cost Code', 'Description', 'Active', 'Actions'],
                colModel: [{ name: 'CostCode', width: 30, sortable: true },
                { name: 'Description', width: 30, sortable: true },
                { name: 'IsActive', width: 40, sortable: false },
                { name: 'act', index: 'act', width: 30, sortable: false }],
                rownum: 10,
                rowList: [10, 20, 30],
                scrollOffset: 0,
                pager: '#divSubCostDetailsListPager',
                sortname: 'CostCodeNumber',
                viewrecords: true,
                gridview: true,
                loadonce: false,
                multiSort: true,
                rownumbers: true,
                emptyrecords: "No records to display",
                shrinkToFit: true,
                sortorder: 'asc',
                caption: "List of Cost Code",
                gridComplete: function () {
                    var ids = jQuery("#tbl_SubCostCodeDetails").jqGrid('getDataIDs');
                    for (var i = 0; i < ids.length; i++) {
                        var cl = ids[i];
                        var Data = jQuery("#tbl_SubCostCodeDetails").getRowData(cl);
                        be = "";
                        if (Data.IsActive == "Active") {
                            be = '<div><a href="javascript:void(0)" id="ActiveCostCode" IsActive="N" class="Assign" ActiveId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Actived</span></a></div>';
                        }
                        else {
                            be = '<div><a href="javascript:void(0)" id="ActiveCostCode" IsActive="Y" class="deleteRecord" ActiveId="' + cl + '" title="delete" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Please Active</span></a></div>';
                        }
                        jQuery("#tbl_SubCostCodeDetails").jqGrid('setRowData', ids[i], { act: be });
                    }
                    if ($("#tbl_SubCostCodeDetails").getGridParam("records") <= 20) {
                        $("#divSubCostDetailsListPager").hide();
                    }
                    else {
                        $("#divSubCostDetailsListPager").show();
                    }
                    if ($('#tbl_SubCostCodeDetails').getGridParam('records') === 0) {
                        $('#tbl_SubCostCodeDetails tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
                    }
                },
                caption: '<span id="AddSubCostCode"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></span>'

            });
        }
    });
}


$("#AddCostCode").live("click", function (event) {
    $("#myModalForCostCode").modal('show');
});
$("#AddSubCostCode").live("click", function (event) {
    $('#CostCode').val(CostCodeId);
    $("#myModalForSubCostCode").modal('show');
});
$("#QRCGenerate").live("click", function (event) {
    var id = $(this).attr("data-value");
    var rowData = jQuery("#tbl_DriverList").getRowData(id);
    var DriverName = rowData['EmployeeNameList'];
    var DriverLicenseNo = rowData['DriverLicenseNo'];
    var CDLType = rowData['CDLType'];
    var CDLExpiration = rowData['CDLExpiration'];
    var MVRExpiration = rowData['MVRExpiration'];
    //var FuelType = rowData['ListFuelType'];
    //var GVWR = rowData['GVWR'];
    //var StorageAddress = rowData['StorageAddress'];
    var DriverImage = rowData['DriverImage'];
    $("#lblDriverName").html(DriverName);
    $("#lblDriverLicenseNo").html(DriverLicenseNo);
    $("#lblCDLType").html(CDLType);
    $("#lblCDLExpiration").html(CDLExpiration);
    $("#lblMVRExpiration").html(MVRExpiration);
    $("#lblDriverImage").html(DriverImage);
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    //$("#lblFuelType").html(FuelType);
    //$("#lblGVWR").html(GVWR);
    //$("#lblStorageAddress").html(StorageAddress);
    //$("#lblVehicleImage").html(VehicleImage);
    $('div #lblDriverImage img').attr('width', '100px');
    $('div #lblDriverImage img').attr('height', '100px');
    if (DriverImage == '' || DriverImage == null || DriverImage == "") {
        $("#labelDriverImage").hide();
        $("#lblDriverImage").hide();
    }
    $('.modal-title').text("eFleet Driver Details");
    $("#myModalFOReFleetDriverQRC").modal('show');
});


