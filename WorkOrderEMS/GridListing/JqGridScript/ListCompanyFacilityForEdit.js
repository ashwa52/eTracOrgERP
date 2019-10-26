var POurl = '/POTypeData/GetPOFacilityListForEditByPOId/';
var POId = $('#POId').val();
var ids; var arrData = [];

var act = '';
var CalculateRemainingAmt; var remainingAmtAfteCal; var getColumnIndexByName;
$(function (event) {
    var  VendorId = $('#SelectedVendor').find('option:selected').val();
  
   var Location = $("#Location option:selected").val();
    $("#tbl_CompanyFacilityDataEditList").jsGrid({
        height: "100%!important",
        width: "100%",
        filtering: false,
        editing: true,
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
                    data: filter,
                    url: $_HostPrefix + POFacilityUrl + '?VendorId=' + VendorId + '&Location=' + Location,
                    dataType: "json"
                });
            }
        },
        onItemUpdating: function (e) {

            var grid = $("#tbl_CompanyFacilityDataEditList").data("JSGrid");
            var _qval = grid.data[e.itemIndex];
            Lastquantity = _qval.Quantity;
            //$("table#tbl_CompanyFacilityDataList .jsgrid-grid-body table .jsgrid-update-button").hide();

        },
        onItemUpdated: function (e) {

            var _rowdata = e.grid.data[e.itemIndex];
            var tr = $($('table#tbl_CompanyFacilityDataEditList .jsgrid-grid-body table tr')[e.itemIndex]);
            //jsgrid-grid-body 

            var _val = parseInt($(tr.find('td .jsgrid-cell')[1]).find('input').val());

            if (CheckStatus != "chk") {
                checkValOfQuantityForEdit(_rowdata, e.itemIndex);
            }
        },
        fields: [
            { name: "COM_Facility_Desc", title: "Description", type: "text", width: 50, editing: false },
            {
                name: "Quantity", title: "Quantity", type: "text", width: 50, editButton: true

            },
            { name: "UnitPrice", title: "Unit Price", type: "text", width: 50, editing: false },
            { name: "TotalPrice", title: "Total", type: "text", width: 50, editing: false },

            { name: "Tax", title: "Tax", type: "text", width: 50, editing: false },
            {
                name: "Status", title: "Status", width: 50, itemTemplate: function (value1, item) {
                    return $("<input>").attr("type", "checkbox")
                        .attr("checked", value1 === 'S')
                        .on("change", function () {

                            var GridData = $("#tbl_CompanyFacilityDataEditList").data("JSGrid");
                            if ($(this).is(":checked")) {
                                if (item.Quantity > 0) {
                                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(item.TotalPrice);
                                    for (var i = 0; i < GridData.data.length; i++) {
                                        rowIdData = ids[i];
                                        if (GridData[i].CostCode == item.CostCode) {
                                            item.RemainingAmt = CalculateRemainingAmt;
                                            item.Status = 'S';
                                            CheckStatus = "chk";
                                            $("#tbl_CompanyFacilityDataEditList").jsGrid("updateItem", item).done(function () {

                                            });


                                        }
                                    }
                                }
                                else {
                                    bootbox.dialog({
                                        message: "Please add Quantity.",
                                        buttons: {
                                            cancel: {
                                                label: "Cancel",
                                                className: "btn-default pull-right",
                                                callback: function () {
                                                    item.Status = 'S';
                                                    CheckStatus = "chk";
                                                    $("#tbl_CompanyFacilityDataEditList").jsGrid("updateItem", item).done(function () {

                                                    });
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                            else {
                                if (item.Quantity > 0) {
                                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(item.TotalPrice);
                                    for (var i = 0; i < GridData.data.length; i++) {
                                        if (GridData.data[i].CostCode == item.CostCode) {
                                            item.Quantity = 0;
                                            item.RemainingAmt = CalculateRemainingAmt;
                                            item.TotalPrice = 0;
                                            item.Status = 'X';
                                            CheckStatus = "chk";
                                            $("#tbl_CompanyFacilityDataEditList").jsGrid("updateItem", item).done(function () {

                                            });

                                        }
                                    }
                                }
                                else {

                                    bootbox.dialog({
                                        message: "Please add Quantity.",
                                        buttons: {
                                            cancel: {
                                                label: "Cancel",
                                                className: "btn-default pull-right",
                                                callback: function () {
                                                    item.Status = 'X';
                                                    CheckStatus = "chk";
                                                    $("#tbl_CompanyFacilityDataEditList").jsGrid("updateItem", item).done(function () {

                                                    });
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        });
                     

                }
            },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilFordelete = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:green;font-size: 22px;" });

                    $customButtonForDelete = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "delete" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {

                    }).append($iconPencilFordelete);



                    return $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForDelete);

                }
            }
        ]
    });


    //$("#tbl_CompanyFacilityDataEditList").jsGrid({
    //    height: "100%!important",
    //    width: "100%",
    //    filtering: false,
    //    editing: false,
    //    inserting: false,
    //    sorting: false,
    //    paging: true,
    //    autoload: true,
    //    pageSize: 10,
    //    pageButtonCount: 5,

    //    controller: {
    //        loadData: function(filter) {
    //            return $.ajax({
    //                type: "GET",
    //                data: filter,
    //                url: $_HostPrefix + POurl + '?POId=' + POId,
    //                dataType: "json"
    //            });
    //        }
    //    },

    //    fields: [
    //        { name: "COM_Facility_Desc", title: "Description", type: "text", width: 50 },
    //        { name: "Quantity", title: "Quantity", type: "text", width: 50 },
    //        { name: "UnitPrice", title: "Unit Price", type: "text", width: 50 },
    //        { name: "TotalPrice", title: "Total", type: "text", width: 50 },
    //        { name: "Tax", title: "Tax", type: "text", width: 50 },
    //        { name: 'RemainingAmt', width: 50, title: "RemainingAmt", type: "text", },
    //    ]
    //});
});

$("#tbl_CompanyFacilityDataEditList").on("focusout", "table tbody tr.jsgrid-edit-row input", function (data) {
    CheckStatus = " ";
    var tr = $(this).closest('tr.jsgrid-edit-row');
    var val = $(this).val();
    //var quentity = $(tr.find('td')[2]).html();
    //debugger
    //$(tr.find('td')[3]).html(parseInt(val) * parseInt(quentity)) 
    tr.find('input.jsgrid-update-button[type="button"]').click();


});

//$("#tbl_CompanyFacilityDataEditList").jqGrid({
//    datatype: "json",
//    //data:arrData,
//    url: POurl + '?POId=' + POId,
//    contentType: "application/json; charset-utf-8",
//    mtype: 'GET',
//    height: 'auto',
//    width: 700,
//    autowidth: true,
//    cellEdit: true,
//    cellsubmit: 'clientArray',
//    editurl: 'clientArray',
//    colNames: ['Description', 'Quantity', 'Unit Price', 'Total', 'Tax', 'Status', 'CostCode', 'CFM_CMP_Id', 'COM_FacilityId', 'RemainingAmt', 'LastRemainingAmount','FacId' ,'Actions'],
//    colModel: [{ name: 'COM_Facility_Desc', width: 30, sortable: true },
//    {
//        name: 'Quantity', width: 40, sortable: false, editable: true, editoptions: {
//            dataEvents: [{
//                type: 'keyup', fn: function (e) {
//                    ChangeValOfQuantity(this);
//                }
//            }]
//        }
//    },
//    { name: 'UnitPrice', width: 20, sortable: true },
//    { name: 'TotalPrice', width: 30, sortable: true },
//    { name: 'Tax', width: 40, sortable: false },
//   {
//       name: 'Status', width: 15, sortable: false,
//       align: "center",
//       editoptions: { value: "Y:X" },
//       editable: false,
//       edittype: 'checkbox',
//       formatter: "checkbox",
//       search: false,
//       formatoptions: { disabled: false },
//   },
//    { name: 'CostCode', width: 15, sortable: false, hidden: true },
//    { name: 'CFM_CMP_Id', width: 15, sortable: false, hidden: true },
//    { name: 'COM_FacilityId', width: 15, sortable: false, hidden: true },
//    { name: 'RemainingAmt', width: 15, sortable: false },
//    { name: 'LastRemainingAmount', width: 15, sortable: false, hidden: true },
//    { name: 'POId', width: 15, sortable: false, hidden: true },
//    { name: 'act', index: 'act', width: 30, sortable: false, hidden: true }],
//    rownum: 20,
//    recordtext: "View {0} - {1} of {2}",
//    loadtext: "Loading...",
//    pgtext : "Page {0} of {1}",
//    rowList: [10, 20, 30],
//    scrollOffset: 0,
//    pager: '#divCompanyFacilityEditListPager',
//    sortname: 'COM_Facility_Desc',
//    viewrecords: true,
//    gridview: true,
//    loadonce: true,
//    multiSort: true,
//    rownumbers: true,
//    emptyrecords: "No records",
//    shrinkToFit: true,
//    sortorder: 'asc',
//    gridComplete: function () {
//        ids = jQuery("#tbl_CompanyFacilityDataEditList").jqGrid('getDataIDs');
//        for (var i = 0; i < ids.length; i++) {
//            var cl = ids[i];
//            be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
//            de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
//            vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
//            qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
//            //jQuery("#tbl_CompanyFacilityDataList").jqGrid('setRowData', ids[i], { act: de }); ///+ qrc 
//        }
//        if ($("#tbl_CompanyFacilityDataEditList").getGridParam("records") <= 20) {
//            $("#divCompanyFacilityEditListPager").show();
//        }
//        else {
//            $("#divCompanyFacilityEditListPager").show();
//        }
//        if ($('#tbl_CompanyFacilityDataEditList').getGridParam('records') === 0) {
//            $('#tbl_CompanyFacilityDataEditList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
//        }
//        getColumnIndexByName = function (grid, columnName) {
//            var cm = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getGridParam', 'colModel'), i, l;
//            for (i = 0, l = cm.length; i < l; i += 1) {
//                if (cm[i].name === columnName) {
//                    return i; // return the index
//                }
//            }
//        }
//        var iCol = getColumnIndexByName($(this), 'Status'), rows = this.rows, i, c = rows.length;
//        var GridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
//        var data = $('#tbl_CompanyFacilityDataEditList').getGridParam('data')
//        var rowIdData;
//        for (var i = 0; i < GridData.length; ++i) {
//            var rowId = ids[i];
//            var Data = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', rowId);
//            var iid = $("#tbl_CompanyFacilityDataEditList tr").attr('id');
//            //$("#tbl_CompanyFacilityDataEditList input[value='Y']").attr('checked', false);
//            if (GridData[i].Quantity > 0 || GridData[i].Quantity != null) {
//                TotalPrice = 0; CalculateRemainingAmt = 0;
//                TotalPrice = (GridData[i].Quantity * GridData[i].UnitPrice);
//                CalculateRemainingAmt = parseInt(GridData[i].RemainingAmt) - TotalPrice;
//                //CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(TotalPrice);
//                var iid = $("#tbl_CompanyFacilityDataEditList tr").attr('id');
//                //$('#cb_' + yourgridid).attr('checked', false);
//                if (GridData[i].Status == 'Y')
//                {
//                    //$("#" + ids[i] + " input[type='checkbox']").attr('checked', true);
//                    $(" input[type='checkbox'] input[value='Y']").attr('checked', true);
//                }
//                else
//                {
//                    $("input[value='Y']").attr('checked', false);
//                    //$("#" + ids[i] + " input[type='checkbox']").attr('checked', false);
//                }
//                for (var l = 0; l < GridData.length; l++) {
//                    rowIdData = ids[l];

//                    if (GridData[l].CostCode == Data.CostCode) {
//                        $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
//                    }
//                }
//            }
//        }
//        for (i = 0; i < c; i += 1) {            
//            $(rows[i].cells[iCol]).click(function (e) {
//                var id = $(e.target).closest('tr')[0].id,
//                    isChecked = $(e.target).is(':checked');
//                var rowData = jQuery('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', id);
//                if (isChecked == true) {
//                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(rowData.TotalPrice);
//                    for (var j = 0; j < GridData.length; j++) {
//                        rowIdData = ids[j];
//                        if (GridData[j].CostCode == rowData.CostCode) {
//                            $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
//                        }
//                    }
//                }
//                else {
//                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(rowData.TotalPrice);
//                    for (var j = 0; j < GridData.length; j++) {
//                        rowIdData = ids[j];
//                        if (GridData[j].CostCode == rowData.CostCode) {
//                            $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
//                        }
//                    }
//                }
//            });
//        }
//    },
//    caption: '<div class="header_search"><input type="text" class="form-control"></div>'
//});

function checkValOfQuantityForEdit(data, _itemIndexVal) {
    var grid = $("#tbl_CompanyFacilityDataEditList").data("JSGrid");
    var tr = $($('table#tbl_CompanyFacilityDataEditList .jsgrid-grid-body table tr')[_itemIndexVal]);
    var _row = grid.data;
    var rowData = data; //$('#tbl_CompanyFacilityDataList').jqGrid('getRowData', rowId);


    data.LastRemainingAmount = Lastquantity;
    //$("#tbl_CompanyFacilityDataList").jsGrid("setCell", rowId, "LastRemainingAmount", Quantity);

    var UnitPrice = rowData.UnitPrice;
    var TotalPrice;
    var Calculation;
    if (rowData.LastRemainingAmount == 0) {
        TotalPrice = (rowData.Quantity * UnitPrice);
        CalculateRemainingAmt = rowData.RemainingAmt - TotalPrice;
    }
    else if (rowData.LastRemainingAmount < rowData.Quantity) {
        TotalPrice = (rowData.Quantity * UnitPrice);
        Calculation = parseInt(TotalPrice) - parseInt(rowData.TotalPrice);
        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) - parseInt(Calculation);
        CalculateRemainingAmt = remainingAmtAfteCal;
    }
    else if (rowData.LastRemainingAmount > rowData.Quantity) {
        TotalPrice = (rowData.Quantity * UnitPrice);
        Calculation = parseInt(rowData.TotalPrice) - parseInt(TotalPrice);
        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) + parseInt(Calculation);
        CalculateRemainingAmt = remainingAmtAfteCal;
    }

    LocationData = $("#Location option:selected").val();
    Vendordata = $("#Vendor option:selected").val();
    CostCodeData = rowData.CostCode;
    var PositiveAmt = Math.abs(CalculateRemainingAmt);
    var AllGridData = $("#tbl_CompanyFacilityDataEditList").data("JSGrid");
    if (CalculateRemainingAmt < TotalPrice) {
        bootbox.dialog({
            message: "You cannot add amount for this cost code, please add amount less than" + " " + PositiveAmt,
            buttons: {
                cancel:
                {
                    label: "Cancel",
                    className: "btn-default pull-right",
                    callback: function () {
                        $("#tbl_CompanyFacilityDataEditList").jsGrid("loadData");
                    }
                },
                confirm: {
                    label: "Need More Budget",
                    className: "btn btn-primary pull-left", 
                }
            },
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                    $("#tbl_CompanyFacilityDataEditList").jsGrid("loadData");
                }
            }
        });
    }
    else {
        for (var i = 0; i < AllGridData.length; i++) {
            var rowIdData = ids[i];
            if (AllGridData[i].CostCode == rowData.CostCode) {
                rowData.RemainingAmt = CalculateRemainingAmt;
            }
        }
    } 
    $(tr.find('td')[3]).html(TotalPrice); 
    var checkedStatus = rowData.Status;
    //$(tr.find('td')[5]).html(true); 
    //if()
   /// checkedStatus = true;
    // $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "Status", checkedStatus);
}

//function ChangeValOfQuantity(data) {
//    var rowId = $(data).closest('tr').attr('id');
//    var amt;
//    var rowData = $('#tbl_CompanyFacilityDataEditList').jqGrid('getRowData', rowId);
//    var Quantity = data.value;
//    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowId, "LastRemainingAmount", Quantity);
//    var UnitPrice = rowData.UnitPrice;
//    var TotalPrice; var Calculation;
//    if (rowData.LastRemainingAmount == 0) {
//        TotalPrice = (Quantity * UnitPrice);
//        CalculateRemainingAmt = rowData.RemainingAmt - TotalPrice;
//    }
//    else if (rowData.LastRemainingAmount < data.value) {
//        TotalPrice = (Quantity * UnitPrice);
//        Calculation = parseInt(TotalPrice) - parseInt(rowData.TotalPrice);
//        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) - parseInt(Calculation);
//        CalculateRemainingAmt = remainingAmtAfteCal;
//    }
//    else if (rowData.LastRemainingAmount > data.value) {
//        TotalPrice = (Quantity * UnitPrice);
//        Calculation = parseInt(rowData.TotalPrice) - parseInt(TotalPrice);
//        remainingAmtAfteCal = parseInt(rowData.RemainingAmt) + parseInt(Calculation);
//        CalculateRemainingAmt = remainingAmtAfteCal;
//    }
//    //var lastremainingAmt = rowData.LastRemainingAmount;
//    //$("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "LastRemainingAmount", CalculateRemainingAmt);

//    var PositiveAmt = Math.abs(CalculateRemainingAmt); //CalculateRemainingAmt.replace("-", "")
//    var AllGridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
//    if (CalculateRemainingAmt < TotalPrice) {
//        bootbox.dialog({
//            message: "You cannot add amount for this cost code, please add amount less than" + PositiveAmt,
//            buttons: {
//                cancel: {
//                    label: "Cancel",
//                    className: "btn-default pull-right"
//                },
//                confirm: {
//                    label: "Need More Budget",
//                    className: "btn btn-Primary pull-right"
//                }
//            },
//            danger: {
//                label: "cancel",
//                classname: "btn btn-primary",
//                callback: function () {
//                }
//            }
//        });
//    }
//    else {
//        if (rowData.RemainingAmt > 0) {
//            for (var i = 0; i < AllGridData.length; i++) {
//                var rowIdData = ids[i];
//                if (AllGridData[i].CostCode == rowData.CostCode) {
//                    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
//                }
//            }
//        }
//    }
//    var GridData = $('#tbl_CompanyFacilityDataEditList').getRowData();
//    var dataGrid = $('#tbl_CompanyFacilityDataEditList').jqGrid('getGridParam', 'data');
//    $("#tbl_CompanyFacilityDataEditList").jqGrid("setCell", rowId, "TotalPrice", TotalPrice);
//}