
var ClientUrl = 'Client/GetListClient';
var editurl = 'Client/EditClient/';
//var editurl = 'Client/Create/';
var deleteURL = 'eFleetDriver/DeleteDriver/';
var generateQRC = "eFleetVehicle/_GenerateQRCForVehicle/";
var vehcileApprovalStatus = ''

$(function () {
    $("#tbl_ClientList").jqGrid({
        url: $_HostPrefix + ClientUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 430,
        width: 650,
        autowidth: true,
        colNames: ['Name', 'Email', 'User Type', 'Locatio', 'Profile Image', 'Actions'],
        colModel: [{ name: 'UserName', width: 80, sortable: true },
                  { name: 'UserEmail', width: 100, sortable: false },
                  { name: 'UserTypeView', width: 100, sortable: false, },
                  { name: 'LocationIds', width: 100, sortable: false, },
                  { name: 'ProfileImage', width: 40, sortable: false, title: false, formatter: imageFormat },
                  { name: 'act', index: 'act', width: 50, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divClientListPager',
        sortname: 'UserId',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_ClientList").jqGrid('getDataIDs');
            jQuery('#tbl_ClientList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                vi = '<div><a href="javascript:void(0)" class="qrc" id="viewClient"  vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a></div>';
                ed = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                jQuery("#tbl_ClientList").jqGrid('setRowData', ids[i], { act:ed + vi }); ///+ qrc 
            }
            if ($("#tbl_ClientList").getGridParam("records") <= 20) {
                $("#divClientListPager").hide();
            }
            else {
                $("#divClientListPager").show();
            }
            if ($('#tbl_ClientList').getGridParam('records') === 0) {
                $('#tbl_ClientList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div><span><a href="javascript:void(0)"><i id="AddClient" class="fa fa-plus-square" style="font-size:36px;"></i></a></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" class="inputSearch light-table-filter" id="searchPreBilltext" placeholder="Vendor Name"  data-table="order-table" /></span></div>'
    });
    if($("#tbl_ClientList").getGridParam("records") > 20) {
        $("#tbl_ClientList").jqGrid('navGrid', '#divClientListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_ClientList").jqGrid('setGridParam', { url: $_HostPrefix + DriverUrl + "?txtSearch=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});
$("#AddClient").live("click", function (event) {
    window.location.href = '../Client/Create';
})

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
        message: "are you sure you want to delete this Client?",
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


