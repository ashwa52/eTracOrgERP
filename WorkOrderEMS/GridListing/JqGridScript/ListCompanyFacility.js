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
var Lastquantity;
var CheckStatus;
$("#Vendor").change(function () {
    CheckStatus = "";
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

            var grid = $("#tbl_CompanyFacilityDataList").data("JSGrid");
            var _qval = grid.data[e.itemIndex];
            Lastquantity = _qval.Quantity;
            //$("table#tbl_CompanyFacilityDataList .jsgrid-grid-body table .jsgrid-update-button").hide();

        },
        onItemUpdated: function (e) {
         
            var _rowdata = e.grid.data[e.itemIndex];
            var tr = $($('table#tbl_CompanyFacilityDataList .jsgrid-grid-body table tr')[e.itemIndex]);
            //jsgrid-grid-body 
            
            var _val = parseInt($(tr.find('td .jsgrid-cell')[1]).find('input').val());
           
            if (CheckStatus != "chk") {
                checkValOfQuantity(_rowdata, e.itemIndex);
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
                name: "Status", title: "Status",  width: 50, itemTemplate: function (value1, item) { 
                    return $("<input>").attr("type", "checkbox")
                        .attr("checked", value1 === 'S')
                        .on("change", function () {
                              
                            var GridData = $("#tbl_CompanyFacilityDataList").data("JSGrid");
                            if ($(this).is(":checked")) {
                                if (item.Quantity > 0) {
                                    CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(item.TotalPrice);
                                    for (var i = 0; i < GridData.data.length; i++) {
                                        rowIdData = ids[i];
                                        if (GridData[i].CostCode == item.CostCode) {
                                            item.RemainingAmt = CalculateRemainingAmt;
                                            item.Status = 'S';
                                            CheckStatus = "chk";
                                            $("#tbl_CompanyFacilityDataList").jsGrid("updateItem", item).done(function () {

                                            });
                                            

                                        }
                                    }
                                }
                                else
                                {
                                    bootbox.dialog({
                                        message: "Please add Quantity.",
                                        buttons: {
                                            cancel: {
                                                label: "Cancel",
                                                className: "btn-default pull-right",
                                                callback: function () {
                                                    item.Status = 'S';
                                                    CheckStatus = "chk";
                                                    $("#tbl_CompanyFacilityDataList").jsGrid("updateItem", item).done(function () {

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
                                            $("#tbl_CompanyFacilityDataList").jsGrid("updateItem", item).done(function () {
                                               
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
                                                    $("#tbl_CompanyFacilityDataList").jsGrid("updateItem", item).done(function () {

                                                    });
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        });
                    //isChecked = $(e.target).is(':checked');
                    //if (isChecked == true) {
                    //    if (rowData.Quantity > 0) {
                    //        CalculateRemainingAmt = parseInt(CalculateRemainingAmt) - parseInt(rowData.TotalPrice);
                    //        for (var i = 0; i < GridData.length; i++) {
                    //            rowIdData = ids[i];
                    //            if (GridData[i].CostCode == rowData.CostCode) {
                    //                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);

                    //            }
                    //        }
                    //    }
                    //    else {
                    //        bootbox.dialog({
                    //            message: "Please add Quantity.",
                    //            buttons: {
                    //                cancel: {
                    //                    label: "Cancel",
                    //                    className: "btn-default pull-right"
                    //                }
                    //            }
                    //        });
                    //    }
                    //}
                    //else {
                    //    if (rowData.Quantity > 0) {
                    //        CalculateRemainingAmt = parseInt(CalculateRemainingAmt) + parseInt(rowData.TotalPrice);
                    //        for (var i = 0; i < GridData.length; i++) {
                    //            rowIdData = ids[i];
                    //            if (GridData[i].CostCode == rowData.CostCode) {
                    //                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "RemainingAmt", CalculateRemainingAmt);
                    //                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "Quantity", 0);
                    //                $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowIdData, "TotalPrice", 0);
                    //            }
                    //        }
                    //    }
                    //    //else {
                    //    //    bootbox.dialog({
                    //    //        message: "Please add Quantity.",
                    //    //        buttons: {
                    //    //            cancel: {
                    //    //                label: "Cancel",
                    //    //                className: "btn-default pull-right"
                    //    //            }
                    //    //        }
                    //    //    });
                    //    //}
                    //}
                    //return $("<input>").attr("type", "checkbox")
                    //    .attr("checked", value || item.Checked)
                    //    .on("change", function () {
                    //        item.Checked = $(this).is(":checked");
                    //    });

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
    $("#tbl_CompanyFacilityDataList").on("focusout", "table tbody tr.jsgrid-edit-row input", function (data) {
        CheckStatus = " ";
        var tr = $(this).closest('tr.jsgrid-edit-row');
        var val = $(this).val();
        //var quentity = $(tr.find('td')[2]).html();
        //debugger
        //$(tr.find('td')[3]).html(parseInt(val) * parseInt(quentity)) 
        tr.find('input.jsgrid-update-button[type="button"]').click();


    });


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

function checkValOfQuantity(data, _itemIndexVal) {
    var grid = $("#tbl_CompanyFacilityDataList").data("JSGrid");
    var tr = $($('table#tbl_CompanyFacilityDataList .jsgrid-grid-body table tr')[_itemIndexVal]);
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
    var AllGridData = $("#tbl_CompanyFacilityDataList").data("JSGrid");
    if (CalculateRemainingAmt < TotalPrice) {
        bootbox.dialog({
            message: "You cannot add amount for this cost code, please add amount less than" + " " + PositiveAmt,
            buttons: {
                cancel: {
                    label: "Cancel",
                    className: "btn-default pull-right",
                    callback: function () {
                        $("#tbl_CompanyFacilityDataList").jsGrid("loadData");
                    }
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
                                    $("#tbl_CompanyFacilityDataList").jsGrid("loadData");
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
                                            className: "btn-default pull-right",
                                            callback: function () {
                                                $("#tbl_CompanyFacilityDataList").jsGrid("loadData");
                                            }
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
                    $("#tbl_CompanyFacilityDataList").jsGrid("loadData"); 
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

    ////var GridData = $('#tbl_CompanyFacilityDataList').getRowData();
    ////var dataGrid = $('#tbl_CompanyFacilityDataList').jqGrid('getGridParam', 'data');

    $(tr.find('td')[3]).html(TotalPrice);
    //  $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "TotalPrice", TotalPrice);
    var checkedStatus = rowData.Status;
  
    //if()
    checkedStatus = true;
    // $("#tbl_CompanyFacilityDataList").jqGrid("setCell", rowId, "Status", checkedStatus);
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


