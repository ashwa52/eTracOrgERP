var CostCodeUrl = 'AccountSection/GetListCostCode';
var SubCostCodeUrl = 'AccountSection/GetListOfSubCostCode';
var editurl = 'eFleetDriver/EditDriver/';
var deleteURL = 'eFleetDriver/DeleteDriver/';
var generateQRC = "eFleetVehicle/_GenerateQRCForVehicle/";
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
        colNames: ['Cost Code', 'Description', 'IsActive', 'Actions'],
        colModel: [{ name: 'CostCode', width: 30, sortable: true },
        { name: 'Description', width: 40, sortable: false },
        { name: 'IsActive', width: 20, sortable: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        //rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divCostCodeListPager',
        sortname: 'CostCodeNumber',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        paging: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
       // caption: "List of Cost Code",
        gridComplete: function () {

            var ids = jQuery("#tbl_CostCodeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_DriverList").jqGrid('setRowData', ids[i], { act: be + de + qrc }); ///+ qrc 
            }
            $("#divCostCodeListPager").show();
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
                var row = jQuery('#tbl_CostCodeList').jqGrid("getRowData", rowid);
                $.ajax({
                    type: "post",
                    url: '../AccountSection/GetListOfSubCostCode' + '?id=' + rowid,
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
                        $("#tbl_SubCostCodeDetails").jqGrid({
                            datatype: "local",
                            data: arrData,
                            contentType: "application/json; charset-utf-8",
                            mtype: 'GET',
                            height: 400,
                            width: 700,
                            autowidth: true,
                            colNames: ['Sub Cost Code','Cost Code', 'Description', 'Created By', 'Created Date', 'Actions'],
                            colModel: [{ name: 'SubCostCode', width: 30, sortable: true },
                            { name: 'CostCode', width: 30, sortable: true },
                            { name: 'Description', width: 40, sortable: false },
                            { name: 'UserName', width: 20, sortable: true },
                            { name: 'CreatedDate', width: 30, sortable: true },
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
                                    be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                                    de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                                    vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                                    qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                                    jQuery("#tbl_SubCostCodeDetails").jqGrid('setRowData', ids[i], { act: be + de + qrc }); ///+ qrc 
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

    var txtSearch = jQuery("#SearchText").val();
    var statusType = jQuery("#vehcileStatusType :selected").val();
    jQuery("#tbl_CostCodeList").jqGrid('setGridParam', { url: $_HostPrefix + CostCodeUrl + "?txtSearch=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});
$("#AddCostCode").live("click", function (event) {
    $("#myModalForCostCode").modal('show');
});
$("#AddSubCostCode").live("click", function (event) {
    ('#CostCode').val(CostCodeId);
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

$(".deleteRecord").live("click", function (event) {
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
    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" id="driverImage" onclick="EnlargeImageView(this);"/>';
    }
}
//#endregion

