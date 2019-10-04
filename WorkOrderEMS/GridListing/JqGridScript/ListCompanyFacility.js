var POFacilityUrl = 'POTypeData/GetComapnyFacilityList';
var generateQRC = "eFleetVehicle/_GenerateQRCForVehicle/";
var selectData = "M:M;L:L;S:S;O:O;E:E";
var VendorId; var Location; var IsRecurring;
var ids;
var CalculateRemainingAmt; var remainingAmtAfteCal;
var timeoutHnd;
var CostCodeData; var LocationData; var Vendordata;
var flAuto = true; var arrCostData = [];

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}
//function gridReload() {
//    jQuery("#tbl_CompanyFacilityDataList").jqGrid('setGridParam', { url: $_HostPrefix + POFacilityUrl + "?VendorId=" + VendorId, page: 1 }).trigger("reloadGrid");
//}

$("#Vendor").change(function () {
    var act = '';
    VendorId = $('#Vendor').find('option:selected').val();
    Location = $("#Location option:selected").val();;
    var POType = $("#POType").val();
    $("#divheaderforPo").show();
    $("#tbl_CompanyFacilityDataList").jsGrid({
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
            var tr = $($('table#tbl_CompanyFacilityDataList .jsgrid-grid-body table tr')[e.itemIndex]);
            //jsgrid-grid-body

            var val = parseInt($(tr.find('td')[1]).find('input').val());
            var amount = parseInt($(tr.find('td')[2]).html());
            debugger
            $(tr.find('td')[3]).html(val * amount);
        },
        onItemUpdated: function (e) {
            var tr = $($('table#tbl_CompanyFacilityDataList .jsgrid-grid-body table tr')[e.itemIndex]);
            //jsgrid-grid-body

            var val = parseInt($(tr.find('td')[1]).html());
            var amount = parseInt($(tr.find('td')[2]).html());
             
            $(tr.find('td')[3]).html(val * amount);
        },
        fields: [
            { name: "COM_Facility_Desc", title: "Description", type: "text", width: 50, editing: false },
            { name: "Quantity", title: "Quantity", type: "text", width: 50, editButton: true },
            { name: "UnitPrice", title: "Unit Price", type: "text", width: 50, editing: false },
            { name: "TotalPrice", title: "Total", type: "text", width: 50, editing: false },
            { name: "Tax", title: "Tax", type: "text", width: 50, editing: false },

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


    $.ajax({
        url: $_HostPrefix + 'POTypeData/GetCompanyDetailsList',//?VendorId=' + VendorId,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ VendorId: VendorId }),
        success: function (result) {
            $('#PointOfContactName').val(result[0].PointOfContact);
            $('#PointOfContactAddress').val(result[0].Address2);
            if (result[0].InvoicingFrequency == null) {
                if (POType == 2) {
                    $('#IsReccuringAddedForVendor').show();
                }
                $('#InvoicingFrequency').val(result[0].InvoicingFrequency);
                $('#CostDuringPeriod').val(result[0].CostDuringPeriod);
                $('#PointOfContactName, #PointOfContactAddress,#CostDuringPeriod,#InvoicingFrequency').addClass('input-disabled');
                $('#lblAddress').addClass('active highlight');
                $('#lblPointOfContact,#lblInvoicingFrequency,#lblCostDuringPeriod').addClass('active highlight');
            }
            else {
                $('#InvoicingFrequency').val(result[0].InvoicingFrequency);
                $('#CostDuringPeriod').val(result[0].CostDuringPeriod);
                $('#IsReccuringAddedForVendor').hide();
                $('#PointOfContactName, #PointOfContactAddress,#CostDuringPeriod,#InvoicingFrequency').addClass('input-disabled');
                $('#lblAddress').addClass('active highlight');
                $('#lblPointOfContact,#lblInvoicingFrequency,#lblCostDuringPeriod').addClass('active highlight');
            }
            $('#PointOfContactNameHidden').val(result[0].PointOfContact);
            $('#PointOfContactAddressHidden').val(result[0].Address2);
            $(".gridShowHide").show();
        },
        error: function () { alert("Whooaaa! Something went wrong..") },
    });
    $("#tbl_CompanyFacilityDataList").on("focusout", "table tbody tr.jsgrid-edit-row input", function () {
        var tr = $(this).closest('tr.jsgrid-edit-row');
        //var val = $(this).val();
        //var quentity = $(tr.find('td')[2]).html();
        //debugger
        //$(tr.find('td')[3]).html(parseInt(val) * parseInt(quentity)) 
        tr.find('input.jsgrid-update-button[type="button"]').click();
    });
    //$.ajax({
    //    url: $_HostPrefix + POFacilityUrl + '?VendorId=' + VendorId + '&Location=' + Location,
    //    datatype: 'json',
    //    type: 'GET',
    //    success: function (result) {
    //        var arrData = [];
    //        if (result.rows.length > 0) {
    //            for (i = 0; i < result.rows.length; i++) {
    //                arrData.push({
    //                    "COM_Facility_Desc": result.rows[i].cell[0],
    //                    "Quantity": result.rows[i].cell[1],
    //                    "UnitPrice": result.rows[i].cell[2],
    //                    "TotalPrice": result.rows[i].cell[3],
    //                    "Tax": result.rows[i].cell[4],
    //                   // "Resource": result.rows[i].cell[5],
    //                    "Status": result.rows[i].cell[5],
    //                    "CostCode": result.rows[i].cell[6],
    //                    "CFM_CMP_Id": result.rows[i].cell[7],
    //                    "COM_FacilityId": result.rows[i].cell[8],
    //                    "RemainingAmt": result.rows[i].cell[9],
    //                    "LastRemainingAmount": result.rows[i].cell[10],
    //                    "StatusCalculation": result.rows[i].cell[11]
    //                });
    //            }
    //        }

    //        //jQuery('#tbl_CompanyFacilityDataList').jqGrid('clearGridData').jqGrid('setGridParam', { data: arrData, page: 1 }).trigger('loadData');
    //        // getColumnIndexByName = function (grid, columnName) {
    //        //    var cm = jQuery('#tbl_CompanyFacilityDataList').jqGrid('getGridParam', 'colModel'), i, l;
    //        //    for (i = 0, l = cm.length; i < l; i += 1) {
    //        //        if (cm[i].name === columnName) {
    //        //            return i; // return the index
    //        //        }
    //        //    }
    //        //    return true;
    //        //}




    //        //$("#tbl_CompanyFacilityDataList").jqGrid({
    //        //    datatype: "local",
    //        //    data: arrData,
    //        //    contentType: "application/json; ",//charset-utf-8
    //        //    mtype: 'GET',
    //        //    height: 'auto',
    //        //    //width: 700,
    //        //    autowidth: true,
    //        //    cellEdit: true,
    //        //    cellsubmit: 'clientArray',
    //        //    editurl: 'clientArray',
    //        //    colNames: ['Description', 'Quantity', 'Unit Price', 'Total', 'Tax', 'Status', 'CostCode', 'CFM_CMP_Id', 'COM_FacilityId', 'RemainingAmt', 'LastRemainingAmount', 'Actions'],
    //        //    colModel: [{ name: 'COM_Facility_Desc', width: 30, sortable: false },
    //        //    {
    //        //        name: 'Quantity', width: 40, sortable: false, editable: true, editoptions: {
    //        //            dataEvents: [{
    //        //                            type: 'keyup', fn: function (e) {
    //        //                                checkValOfQuantity(this);
    //        //                            }
    //        //                        }]
    //        //        }
    //        //    },
    //        //    { name: 'UnitPrice', width: 20, sortable: false },
    //        //    { name: 'TotalPrice', width: 30, sortable: false },
    //        //    { name: 'Tax', width: 40, sortable: false },
    //        //   {
    //        //       name: 'Status', width: 15, sortable: false,
    //        //        align: "center",
    //        //        editoptions: { value: "Y:X" },
    //        //        editable: false,
    //        //        edittype: 'checkbox',
    //        //        formatter: "checkbox",
    //        //        //formatter:GetValueGrid,
    //        //        search: false,
    //        //        formatoptions: {disabled: false},
    //        //        //defaultValue: "X"
    //        //   },
    //        //    { name: 'CostCode', width: 15, sortable: false, hidden:true },
    //        //    { name: 'CFM_CMP_Id', width: 15, sortable: false, hidden: true },
    //        //    { name: 'COM_FacilityId', width: 15, sortable: false, hidden: true },
    //        //    { name: 'RemainingAmt', width: 15, sortable: false, hidden: true },
    //        //    { name: 'LastRemainingAmount', width: 15, sortable: false, hidden: true },
    //        //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //        //    rownum: 10,
    //        //    rowList: [10, 20, 30],
    //        //    scrollOffset: 0,
    //        //    pager: '#divCompanyFacilityListPager',
    //        //    sortname: 'COM_Facility_Desc',
    //        //    viewrecords: true,
    //        //    gridview: true,
    //        //    loadonce: false,
    //        //    multiSort: true,
    //        //    rownumbers: true,
    //        //    emptyrecords: "No records",
    //        //    shrinkToFit: true,
    //        //    sortorder: 'asc',
    //        //    focusField: false,
    //        //    gridComplete: function () {
    //        //         ids = jQuery("#tbl_CompanyFacilityDataList").jqGrid('getDataIDs');
    //        //        jQuery("#tbl_CompanyFacilityDataList").trigger('reloadGrid');            
    //        //        for (var i = 0; i < ids.length; i++) {
    //        //            var cl = ids[i];
    //        //            be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
    //        //            de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
    //        //            vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
    //        //            qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
    //        //            jQuery("#tbl_CompanyFacilityDataList").jqGrid('setRowData', ids[i], { act: de }); ///+ qrc 
    //        //        }

    //        //        if ($("#tbl_CompanyFacilityDataList").getGridParam("records") <= 20) {
    //        //            $("#divCompanyFacilityListPager").hide();
    //        //        }
    //        //        else {
    //        //            $("#divCompanyFacilityListPager").show();
    //        //        }
    //        //        if ($('#tbl_CompanyFacilityDataList').getGridParam('records') === 0) {
    //        //            $('#tbl_CompanyFacilityDataList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        //        }
    //        //        var iCol = getColumnIndexByName($(this), 'Status'), rows = this.rows, i, c = rows.length;
    //        //        var GridData = $('#tbl_CompanyFacilityDataList').getRowData();
    //        //        var rowIdData ;
    //        //        for (i = 0; i < c; i += 1) {
    //        //            $(rows[i].cells[iCol]).click(function (e) {
    //        //                var id = $(e.target).closest('tr')[0].id,
    //        //                    isChecked = $(e.target).is(':checked');
    //        //                var rowData = jQuery('#tbl_CompanyFacilityDataList').jqGrid('getRowData', id);
    //        //                if (isChecked == true) {
    //        //                    if (rowData.Quantity > 0) {
    //        //                        CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(rowData.TotalPrice);
    //        //                        for (var i = 0; i < GridData.length; i++) {
    //        //                            rowIdData = ids[i];
    //        //                            if (GridData[i].CostCode == rowData.CostCode) {
    //        //                                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);

    //        //                            }
    //        //                        }
    //        //                    }
    //        //                    else {
    //        //                        bootbox.dialog({
    //        //                            message: "Please add Quantity.",
    //        //                            buttons: {
    //        //                                cancel: {
    //        //                                    label: "Cancel",
    //        //                                    className: "btn-default pull-right"
    //        //                                }
    //        //                            }
    //        //                        });
    //        //                    }
    //        //                }
    //        //                else {
    //        //                    if (rowData.Quantity > 0) {
    //        //                        CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(rowData.TotalPrice);
    //        //                        for (var i = 0; i < GridData.length; i++) {
    //        //                            rowIdData = ids[i];
    //        //                            if (GridData[i].CostCode == rowData.CostCode) {
    //        //                                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
    //        //                                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "Quantity", 0);
    //        //                                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "TotalPrice", 0);
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                    //else {
    //        //                    //    bootbox.dialog({
    //        //                    //        message: "Please add Quantity.",
    //        //                    //        buttons: {
    //        //                    //            cancel: {
    //        //                    //                label: "Cancel",
    //        //                    //                className: "btn-default pull-right"
    //        //                    //            }
    //        //                    //        }
    //        //                    //    });
    //        //                    //}
    //        //                }
    //        //            });
    //        //        }
    //        //    },
    //        //    caption: '<div class="header_search"><input type="text" class="form-control"></div>'
    //        //});
    //    }
    //});

});

function getVendorList(IsRecurring) {
    var Location = $("#Location").val();
    $.ajax({
        url: $_HostPrefix + 'POTypeData/GetVendorList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ Location: +Location, IsRecurring: IsRecurring }),
        success: function (result) {
            $("#Vendor").html("");
            $("#Vendor").append
                ($('<option></option>').val(null).html("---Select Vendor---"));
            for (var i = 0; i < result.length; i++) {
                $("#Vendor").append('<option value=' + result[i].CompanyId + '>' + result[i].CompanyNameLegal + '</option>');
            }
        },
        error: function () { alert(" Something went wrong..") },
    });
}

function checkValOfQuantity(data) {
    var rowId = $(data).closest('tr').attr('id');
    var amt;
    var rowData = $('#tbl_CompanyFacilityDataList').jqGrid('getRowData', rowId);
    var Quantity = data.value;
    $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "LastRemainingAmount", Quantity);
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

    LocationData = $("#Location option:selected").val();
    Vendordata = $("#Vendor option:selected").val();
    CostCodeData = rowData.CostCode;
    var PositiveAmt = Math.abs(CalculateRemainingAmt);
    var AllGridData = $('#tbl_CompanyFacilityDataList').getRowData();
    if (CalculateRemainingAmt < TotalPrice) {
        bootbox.dialog({
            message: "You cannot add amount for this cost code, please add amount less than" + " " + PositiveAmt,
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn-default pull-right"
                },
                confirm: {
                    label: "Need More Budget",
                    className: "btn btn-primary pull-left",
                    callback: function () {
                        var ISSend;
                        if (arrCostData.length == 0) {
                            $.ajax({
                                type: 'POST',
                                datatype: 'application/json',
                                contentType: 'application/json',
                                url: $_HostPrefix + 'POTypeData/SendMailForMoreBudget',
                                data: JSON.stringify({ CalculateRemainingAmt: PositiveAmt, CostCodeData: CostCodeData, LocationData: LocationData, Vendordata: Vendordata }),
                                beforesend: function () {
                                    new fn_showmaskloader('please wait...');
                                },
                                success: function (data) {
                                    toastr.success(data);
                                },
                                error: function () {
                                    alert("error:")
                                }
                            });
                        }
                        for (var i = 0; i < arrCostData.length; i++) {
                            if (arrCostData[i] == CostCodeData) {
                                bootbox.dialog({
                                    message: "Already sent this cost code for budget please add other.",
                                    buttons: {
                                        cancel: {
                                            label: "Cancel",
                                            className: "btn-default pull-right"
                                        }
                                    }
                                });
                            }
                            else {
                                //ISSend = false;
                                $.ajax({
                                    type: 'POST',
                                    datatype: 'application/json',
                                    contentType: 'application/json',
                                    url: $_HostPrefix + 'POTypeData/SendMailForMoreBudget',
                                    data: JSON.stringify({ CalculateRemainingAmt: CalculateRemainingAmt, CostCodeData: CostCodeData, LocationData: LocationData, Vendordata: Vendordata }),
                                    beforesend: function () {
                                        new fn_showmaskloader('please wait...');
                                    },
                                    success: function (data) {
                                        toastr.success(data);
                                    },
                                    error: function () {
                                        alert("error:")
                                    }
                                });
                            }
                        }

                        arrCostData.push({
                            CostCodeData
                        });

                    }
                    //callback: AddBudget(CalculateRemainingAmt, rowData.CostCode, LocationData, Vendordata)
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
        for (var i = 0; i < AllGridData.length; i++) {
            var rowIdData = ids[i];
            if (AllGridData[i].CostCode == rowData.CostCode) {
                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
            }
        }
    }
    var GridData = $('#tbl_CompanyFacilityDataList').getRowData();
    var dataGrid = $('#tbl_CompanyFacilityDataList').jqGrid('getGridParam', 'data');
    $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "TotalPrice", TotalPrice);
    var checkedStatus = rowData.Status;
    //if()
    checkedStatus = true;
    $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "Status", checkedStatus);
}

function AddBudget(CalculateRemainingAmt, CostCodeData, LocationData, Vendordata) {
    $.ajax({
        url: $_HostPrefix + ApproveUrl,
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ CalculateRemainingAmt: CalculateRemainingAmt, CostCodeData: CostCodeData, LocationData: LocationData, Vendordata: Vendordata }),
        success: function (result) {
            toastr.success("Successfully send Budget to Manager");
        },
        error: function () { alert(" Something went wrong..") },
    });
}


