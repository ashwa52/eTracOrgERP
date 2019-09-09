var POurl = '/POTypeData/GetPOFacilityListForEditByPOId/';
var POId = $('#POId').val();
var ids; var arrData = [];
var CalculateRemainingAmt; var remainingAmtAfteCal; var getColumnIndexByName;
//$.ajax({
//    url:  POurl + '?POId=' + POId,
//    datatype: 'json',
//    type: 'GET',
//    success: function (result) {        
//        if (result.rows.length > 0) {
//            for (i = 0; i < result.rows.length; i++) {
//                arrData.push({
//                    "COM_Facility_Desc": result.rows[i].cell[0],
//                    "Quantity": result.rows[i].cell[1],
//                    "UnitPrice": result.rows[i].cell[2],
//                    "TotalPrice": result.rows[i].cell[3],
//                    "Tax": result.rows[i].cell[4],
//                    "Status": result.rows[i].cell[5],
//                    "CostCode": result.rows[i].cell[6],
//                    "CFM_CMP_Id": result.rows[i].cell[7],
//                    "COM_FacilityId": result.rows[i].cell[8],
//                    "RemainingAmt": result.rows[i].cell[9],
//                    "LastRemainingAmount": result.rows[i].cell[10],
//                    "StatusCalculation": result.rows[i].cell[11]
//                });
//            }
//            jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 })
//               .trigger('reloadGrid');
//        }       
//    }
//});
$("#tbl_CompanyFacilityDataEditList").jqGrid({
    datatype: "json",
    //data:arrData,
    url: POurl + '?POId=' + POId,
    contentType: "application/json; charset-utf-8",
    mtype: 'GET',
    height: 'auto',
    width: 700,
    autowidth: true,
    cellEdit: true,
    cellsubmit: 'clientArray',
    editurl: 'clientArray',
    colNames: ['Description', 'Quantity', 'Unit Price', 'Total', 'Tax', 'Status', 'CostCode', 'CFM_CMP_Id', 'COM_FacilityId', 'RemainingAmt', 'LastRemainingAmount','FacId' ,'Actions'],
    colModel: [{ name: 'COM_Facility_Desc', width: 30, sortable: true },
    {
        name: 'Quantity', width: 40, sortable: false, editable: true, editoptions: {
            dataEvents: [{
                type: 'keyup', fn: function (e) {
                    ChangeValOfQuantity(this);
                }
            }]
        }
    },
    { name: 'UnitPrice', width: 20, sortable: true },
    { name: 'TotalPrice', width: 30, sortable: true },
    { name: 'Tax', width: 40, sortable: false },
   {
       name: 'Status', width: 15, sortable: false,
       align: "center",
       editoptions: { value: "Y:X" },
       editable: false,
       edittype: 'checkbox',
       formatter: "checkbox",
       search: false,
       formatoptions: { disabled: false },
   },
    { name: 'CostCode', width: 15, sortable: false, hidden: true },
    { name: 'CFM_CMP_Id', width: 15, sortable: false, hidden: true },
    { name: 'COM_FacilityId', width: 15, sortable: false, hidden: true },
    { name: 'RemainingAmt', width: 15, sortable: false },
    { name: 'LastRemainingAmount', width: 15, sortable: false, hidden: true },
    { name: 'POId', width: 15, sortable: false, hidden: true },
    { name: 'act', index: 'act', width: 30, sortable: false, hidden: true }],
    rownum: 20,
    recordtext: "View {0} - {1} of {2}",
    loadtext: "Loading...",
    pgtext : "Page {0} of {1}",
    rowList: [10, 20, 30],
    scrollOffset: 0,
    pager: '#divCompanyFacilityEditListPager',
    sortname: 'COM_Facility_Desc',
    viewrecords: true,
    gridview: true,
    loadonce: true,
    multiSort: true,
    rownumbers: true,
    emptyrecords: "No records",
    shrinkToFit: true,
    sortorder: 'asc',
    gridComplete: function () {
        ids = jQuery("#tbl_CompanyFacilityDataEditList").jqGrid('getDataIDs');
        for (var i = 0; i < ids.length; i++) {
            var cl = ids[i];
            be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
            de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
            vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
            qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
            //jQuery("#tbl_CompanyFacilityDataList").jqGrid('setRowData', ids[i], { act: de }); ///+ qrc 
        }
        if ($("#tbl_CompanyFacilityDataEditList").getGridParam("records") <= 20) {
            $("#divCompanyFacilityEditListPager").show();
        }
        else {
            $("#divCompanyFacilityEditListPager").show();
        }
        if ($('#tbl_CompanyFacilityDataEditList').getGridParam('records') === 0) {
            $('#tbl_CompanyFacilityDataEditList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
        }
        getColumnIndexByName = function (grid, columnName) {
            var cm = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getGridParam', 'colModel'), i, l;
            for (i = 0, l = cm.length; i < l; i += 1) {
                if (cm[i].name === columnName) {
                    return i; // return the index
                }
            }
        }
        var iCol = getColumnIndexByName($(this), 'Status'), rows = this.rows, i, c = rows.length;
        var GridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
        var data = $('#tbl_CompanyFacilityDataEditList').getGridParam('data')
        var rowIdData;
        for (var i = 0; i < GridData.length; ++i) {
            var rowId = ids[i];
            var Data = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', rowId);
            var iid = $("#tbl_CompanyFacilityDataEditList tr").attr('id');
            //$("#tbl_CompanyFacilityDataEditList input[value='Y']").attr('checked', false);
            if (GridData[i].Quantity > 0 || GridData[i].Quantity != null) {
                TotalPrice = 0; CalculateRemainingAmt = 0;
                TotalPrice = (GridData[i].Quantity * GridData[i].UnitPrice);
                CalculateRemainingAmt = parseInt(GridData[i].RemainingAmt) - TotalPrice;
                //CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(TotalPrice);
                var iid = $("#tbl_CompanyFacilityDataEditList tr").attr('id');
                //$('#cb_' + yourgridid).attr('checked', false);
                if (GridData[i].Status == 'Y')
                {
                    //$("#" + ids[i] + " input[type='checkbox']").attr('checked', true);
                    $(" input[type='checkbox'] input[value='Y']").attr('checked', true);
                }
                else
                {
                    $("input[value='Y']").attr('checked', false);
                    //$("#" + ids[i] + " input[type='checkbox']").attr('checked', false);
                }
                for (var l = 0; l < GridData.length; l++) {
                    rowIdData = ids[l];

                    if (GridData[l].CostCode == Data.CostCode) {
                        $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
                    }
                }
            }
        }
        for (i = 0; i < c; i += 1) {            
            $(rows[i].cells[iCol]).click(function (e) {
                var id = $(e.target).closest('tr')[0].id,
                    isChecked = $(e.target).is(':checked');
                var rowData = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', id);
                if (isChecked == true) {
                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(rowData.TotalPrice);
                    for (var j = 0; j < GridData.length; j++) {
                        rowIdData = ids[j];
                        if (GridData[j].CostCode == rowData.CostCode) {
                            $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
                        }
                    }
                }
                else {
                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(rowData.TotalPrice);
                    for (var j = 0; j < GridData.length; j++) {
                        rowIdData = ids[j];
                        if (GridData[j].CostCode == rowData.CostCode) {
                            $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
                        }
                    }
                }
            });
        }
    },
    caption: '<div class="header_search"><input type="text" class="form-control"></div>'
});



function ChangeValOfQuantity(data) {
    var rowId = $(data).closest('tr').attr('id');
    var amt;
    var rowData = $('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', rowId);
    var Quantity = data.value;
    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowId, "LastRemainingAmount", Quantity);
    var UnitPrice = rowData.UnitPrice;
    var TotalPrice; var Calculation;
    if (rowData.LastRemainingAmount == 0) {
        TotalPrice = (Quantity * UnitPrice);
        CalculateRemainingAmt = rowData.RemainingAmt - TotalPrice;
    }
    else if (rowData.LastRemainingAmount < data.value) {
        TotalPrice = (Quantity * UnitPrice);
        Calculation = parseInt(TotalPrice) - parseInt(rowData.TotalPrice);
        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) - parseInt(Calculation);
        CalculateRemainingAmt = remainingAmtAfteCal;
    }
    else if (rowData.LastRemainingAmount > data.value) {
        TotalPrice = (Quantity * UnitPrice);
        Calculation = parseInt(rowData.TotalPrice) - parseInt(TotalPrice);
        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) + parseInt(Calculation);
        CalculateRemainingAmt = remainingAmtAfteCal;
    }
    //var lastremainingAmt = rowData.LastRemainingAmount;
    //$("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "LastRemainingAmount", CalculateRemainingAmt);

    var PositiveAmt = Math.abs(CalculateRemainingAmt); //CalculateRemainingAmt.replace("-", "")
    var AllGridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
    if (CalculateRemainingAmt < TotalPrice) {
        bootbox.dialog({
            message: "You cannot add amount for this cost code, please add amount less than" + PositiveAmt,
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn-default pull-right"
                },
                confirm: {
                    label: "Need More Budget",
                    className: "btn btn-Primary pull-right"
                }
            },
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                }
            }
        });
    }
    else {
        if (rowData.RemainingAmt > 0) {
            for (var i = 0; i < AllGridData.length; i++) {
                var rowIdData = ids[i];
                if (AllGridData[i].CostCode == rowData.CostCode) {
                    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
                }
            }
        }
    }
    var GridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
    var dataGrid = $('#tbl_CompanyFacilityDataEditList').jqGrid('getGridParam', 'data');
    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowId, "TotalPrice", TotalPrice);
}