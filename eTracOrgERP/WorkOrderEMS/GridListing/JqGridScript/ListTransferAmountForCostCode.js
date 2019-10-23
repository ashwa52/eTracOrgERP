var CostCodeName;
var CostCodeId;
var RemainingAmount;
var BCM_Id;
var CLM_Id;
$(function () {
    $("#tbl_TransferAmountList").jqGrid({
        url: '../GlobalAdmin/GetListOFTransferCostCodeForLocation/?Loc=' + $_locId + '&RemainingAmt=' + 1000 + '&CLM_Id=' + $_CostIdToTranfer,
        //data: objBudgetModel,//{LocationID:$_locId,RemainingAmt:1000},
        datatype: 'json',
        //contentType: 'application/json; charset=utf-8',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Cost Code', 'Remaining Amount', 'CostCode','Year', 'BCM_Id', 'CLM_Id', 'BudgetStatus', 'Actions'],
        colModel: [{ name: 'Description', width: 30, sortable: true },
        { name: 'RemainingAmount', width: 30, sortable: true },   
        { name: 'CostCode', width: 0, sortable: false , hidden:true},
        { name: 'Year', width: 0, sortable: false, hidden: true },
        { name: 'BCM_Id', width: 0, sortable: false, hidden: true },
        { name: 'CLM_Id', width: 0, sortable: false, hidden: true },
        { name: 'BudgetStatus', width: 0, sortable: false, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divTransferAmountListPager',
        sortname: 'CostCode',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        //caption: "List of Driver",
        gridComplete: function () {

            var ids = jQuery("#tbl_TransferAmountList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                tr = '<div><a href="javascript:void(0)" class="qrc AddAmountForCostCode" title="Transfer Amount" Id="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-exchange texthover-bluelight"></span><span class="tooltips">Transfer Amount</span></a></div>';
                jQuery("#tbl_TransferAmountList").jqGrid('setRowData', ids[i], { act: tr }); ///+ qrc 
            }
            if ($("#tbl_TransferAmountList").getGridParam("records") <= 20) {
                $("#divTransferAmountListPager").hide();
            }
            else {
                $("#divTransferAmountListPager").show();
            }
            if ($('#tbl_TransferAmountList').getGridParam('records') === 0) {
                $('#tbl_TransferAmountList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Cost Code" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'

    });
    if ($("#tbl_TransferAmountList").getGridParam("records") > 20) {
        jQuery("#tbl_TransferAmountList").jqGrid('navGrid', '#divTransferAmountListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}
//function gridReload() {
//    jQuery("#tbl_TransferAmountList").jqGrid('setGridParam', { url: '../GetListOFTransferCostCodeForLocation/?LocationID=' +$_locId + '?RemainingAmt=' +1000 }).trigger("reloadGrid");
//}

$(".AddAmountForCostCode").live("click", function (event) {
    var id = $(this).attr("Id");
    var rowData = jQuery("#tbl_TransferAmountList").getRowData(id);
    CostCodeId = rowData.CostCode;
    RemainingAmount = rowData.RemainingAmount;
    BCM_Id = rowData.BCM_Id;
    CLM_Id = rowData.CLM_Id;
    $("#myModalForCostCodeBudgetAmount").modal('show');
});
function GetCostCodeData() {
    CostCodeName = $("#CostCodeForLocation option:selected").text();
    CostCodeId = $('#CostCodeForLocation').find('option:selected').val();
    RemainingAmount = $("#CostCodeForLocation option:selected").attr("RemainingAmount");
    BCM_Id = $("#CostCodeForLocation option:selected").attr("BCM_Id");
    CLM_Id = $("#CostCodeForLocation option:selected").attr("CLM_Id");
}
$("#btnCostCode").live("click", function (event) {
   var amountVal = $("#TransferAmountForSame").val();
    var a1 =parseInt(amountVal)
   var r1 =parseInt(RemainingAmount)
        if (a1 > r1)
            {
            $('#errormessage').show();            
        }
        else {
            $('#myModalForCostCodeBudgetAmount').modal('hide');
        }

});
$("#btnSaveTransferBudgetForCostCode").live("click", function (event) {
    event.preventDefault();
    var BCM_CLM_TransferId = CLM_Id;
    var SourceVal = $("#TransferMode").val();
    var BudgetSource;
    var amount;
    var locationId;
    if (SourceVal == "TransferFromOtherLocation")
    {
        amount = $("#TransferBudgetAmt").val();
        BudgetSource = "TRANSFERED";
        locationId = $('#Location').find('option:selected').val();
    }
    else if (SourceVal == "TransferFromOtherCostCode")
    {
        BudgetSource = "INTRA";
        amount = $("#TransferAmountForSame").val();
        locationId  =$("#LocationIdToTransfer").val();
    }
    else {
        BudgetSource = "OVERDUE";
        amount = $("#NewAllocationId").val();
        BCM_CLM_TransferId = null;
        locationId = $("#LocationIdToTransfer").val();
    }
    var AssignedPercent = $("#AssignedPercent").val();
   
    
    var objBudgetForLocationModel = new Object();
    objBudgetForLocationModel.AssignedPercent = AssignedPercent;
    objBudgetForLocationModel.BCM_CLM_TransferId = BCM_CLM_TransferId;
    objBudgetForLocationModel.BCM_Id = $("#BCM_Id").val();
    objBudgetForLocationModel.BudgetAmount = amount;
    objBudgetForLocationModel.BudgetSource = BudgetSource;
    objBudgetForLocationModel.CLM_Id = $("#CLM_Id").val();
    objBudgetForLocationModel.locationId = locationId;
    objBudgetForLocationModel.Year = $("#Year").val();
    
    //objBudgetForLocationModel=JSON.stringify(objBudgetForLocationModel)
   // BudgetModel.BCM_CLM_TransferId = BCM_CLM_TransferId''
    $("#TransferAmt").val("");
    $.ajax({
        type: 'POST',
        url: '../GlobalAdmin/SavingTransferAmountOfCostCode/',
        //data: {objBudgetForLocationModel: objBudgetForLocationModel },
        data: JSON.stringify(objBudgetForLocationModel),
        //data: { BudgetAmount: amount, Location: $_locId, Year: 2018, BCM_Id: BCM_Id, CLM_Id: CLM_Id, BCM_CLM_TransferId: BCM_CLM_TransferId, AssignedPercent :AssignedPercent,BudgetSource:BudgetSource},
        contentType: 'application/json; charset=utf-8',
        datatype: 'json',
        success: function (result) {
            //window.location.href = '@Url.Action("BudgetAllocation","GlobalAdmin",new { loc = '+$_locId+'})';
            window.location.replace('../GlobalAdmin/BudgetAllocation/?loc=' + $_locId);
           // window.location.href = '../GlobalAdmin/BudgetAllocation/?loc=' + $_locId;
            toastr.success(result);
        },
        error: function (result) {
            alert('Fail ');
        }
    });
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



//#endregion



