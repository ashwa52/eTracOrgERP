var fuelurl = 'fueling/GetFuelingList';
var fuelingReceiptDoc = '../fueling/ReceiptDownload/';
var vehcileApprovalStatus = ''
//+ '<select id="vehcileStatusType" class="" onchange="doSearch(arguments[0]||event);">'
//+ '<option value="0">All Approved</option>'
//+ '<option value="244">Approved By Manager</option>'
////+ '<option value="0">Pending</option>'
//+ '</select>';
function _bind_DatePickers() {
    $("#fromDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '-3m' });
    $("#toDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '0d' });

    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    ToEndDate.setDate(ToEndDate.getDate());

    $('#fromDateDAR').datepicker({
        weekStart: 1,
        startDate: FromEndDate.setDate(-30),
        endDate: FromEndDate,
        autoclose: true
    })
        .on('changeDate', function (selected) {
            startDate = new Date(selected.date.valueOf());
            startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
            $('#toDateDAR').datepicker('setStartDate', startDate);
            $('#fromDateDAR').datepicker('hide');
        });

    $('#toDateDAR')
        .datepicker({
            weekStart: 1,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        })
        .on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('#fromDateDAR').datepicker('setEndDate', FromEndDate);
            $('#toDateDAR').datepicker('hide');
        });
};

$(function () {
    $("#tbl_FuelingList").jqGrid({
        url: $_HostPrefix + fuelurl,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Vehicle Number', 'QRCodeID', 'Mileage', 'Fuel Type', 'Fueling Date', 'Gallons', 'PricePerGallon', 'Total', 'Gas Station Name', 'Driver Name', 'FuelReceiptImage', 'Actions'],
        colModel: [{ name: 'VehicleNumber', width: 30, sortable: true },
        { name: 'QRCodeID', width: 40, sortable: true },
        { name: 'Mileage', width: 40, sortable: false },
        { name: 'FuelTypeName', width: 40, sortable: true },
        { name: 'FuelingDate', width: 30, sortable: false },
        { name: 'Gallons', width: 30, sortable: true },
        { name: 'PricePerGallon', width: 30, sortable: true },
        { name: 'Total', width: 30, sortable: true },
        { name: 'GasStatioName', width: 30, sortable: true },
        { name: 'DriverName', width: 30, sortable: true },
        { name: 'FuelReceiptImage', width: 5, sortable: false, hidden: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divFuelingListPager',
        sortname: 'FuelingDate',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Fueling",
        gridComplete: function () {
            _bind_DatePickers();
            $('#message').html('');
            var ids = jQuery("#tbl_FuelingList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_FuelingList").getRowData(cl);
                var FuelStatusFile = rowData['FuelReceiptImage'];
                vi = '<a href="javascript:void(0)" class="qrc" title="Detail" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';

                // vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 3px;cursor:pointer;">view<span class=" icon-cog fa-2x texthover-bluelight"></span><span class="tooltips">View</span></a></div>';
                var fuelfile = "";
                if (FuelStatusFile == null || FuelStatusFile == "" || FuelStatusFile == '' || FuelStatusFile == undefined) {
                }
                else {
                    fuelfile = '<a href="' + fuelingReceiptDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Receipt</span></a></div>';
                }
                $("#tbl_FuelingList").jqGrid('setRowData', ids[i], { act: vi + fuelfile }); //+ qrc 
            }

            $("#fromDate").datepicker({ dateFormat: 'mm/dd/y', minDate: '-30', maxDate: new Date });
            $("#toDate").datepicker({ dateFormat: 'mm/dd/y', minDate: new Date, maxDate: new Date });
     
            if ($("#tbl_FuelingList").getGridParam("records") <= 20) {
                $("#divFuelingListPager").hide();
            }
            else {
                $("#divFuelingListPager").show();
            }
          
            if ($('#tbl_FuelingList').getGridParam('records') === 0) {
                $('#tbl_FuelingList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by vehicle no" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" >&nbsp;<input type="button" value="Show Fueling" class="ViewAllButton" onclick="ViewFuelDetails();" title="Click to view"/></div>'

        //  caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by vehicle no" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + vehcileApprovalStatus + '&nbsp;</div>'
    });
    if ($("#tbl_FuelingList").getGridParam("records") > 20) {
        jQuery("#tbl_FuelingList").jqGrid('navGrid', '#divFuelingListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function ViewFuelDetails() {
  
    var txtSearch = jQuery("#SearchText").val();
    var fromDate = $("#fromDateDAR").val();
    var toDate = jQuery("#toDateDAR").val();
    jQuery("#tbl_FuelingList").jqGrid('setGridParam', { url: $_HostPrefix + fuelurl + "?txtSearch=" + txtSearch.trim() + "&frmDate=" + fromDate + "&todate=" + toDate, page: 1 }).trigger("reloadGrid");
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    var fromDate = $("#fromDateDAR").val();
    var toDate = jQuery("#toDateDAR").val();
    //var statusType = jQuery("#serviceTypeStatus :selected").val();
    jQuery("#tbl_FuelingList").jqGrid('setGridParam', { url: $_HostPrefix + fuelurl + "?txtSearch=" + txtSearch.trim() + "&frmDate=" + fromDate + "&todate=" + toDate, page: 1 }).trigger("reloadGrid");
}

$(".qrc").live("click", function (event) {
  
    var id = $(this).attr("vid");
    var rowData = jQuery("#tbl_FuelingList").getRowData(id);
    var VehicleNumber = rowData['VehicleNumber'];
    var QRCodeID = rowData['QRCodeID'];
    var Mileage = rowData['Mileage'];
    var FuelTypeName = rowData['FuelTypeName'];
    var FuelingDate = rowData['FuelingDate'];
    var Gallons = rowData['Gallons'];
    var PricePerGallon = rowData['PricePerGallon'];
    var Total = rowData['Total'];
    var GasStatioName = rowData['GasStatioName'];
    var DriverName = rowData['DriverName'];
    $('#lblVehicleNumber').html(VehicleNumber);
    $('#lblQRCodeID').html(QRCodeID);
    $('#lblMileage').html(Mileage);
    $('#lblSubmittedOn').html(FuelTypeName);
    $('#lblFuelTypeName').html(FuelTypeName);
    $('#lblFuelingDate').html(FuelingDate);
    $('#lblGallons').html(Gallons);
    $('#lblPricePerGallon').html(PricePerGallon);
    $('#lblTotal').html(Total);
    $('#lblGasStationName').html(GasStatioName);
    $('#lblDriverName').html(DriverName);

    //$('#').html();
    //$('#').html();
    $("#FuelingModalDetailPreview").modal('show');

});