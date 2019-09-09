
var Year = ''
+ '<select id="YearSelected">'
+ '<option value="2018">2019</option>'
+ '<option value="2019">2020</option>'
//+ '<option value="0">Pending</option>'
+ '</select>';
var val = '@ViewBag.LocationId';
//var locId = $("#SelectLocationId").val();
var rowId;
var assignedPercent;
var sumofAssignedPercent = 0;
var sumofAssignedAmt = 0;
var sumofRemainingAmount=0
var Year;
var BudgetAmount;
var add = 0;
var IsGridSaved = false;
$(function () {
    $("#tbl_BudgetList").jqGrid({
        url: '../GetListCostCodeForBudgetAsPerLocation?Loc=' + $_locId,
        datatype: 'json',
        type: 'GET',
        height: 'auto',
        width: 700,
        autowidth: true,
        colNames: ['Cost Code', 'Assigned %', 'Assigned Amount', 'Remaining Ammount', 'Cost Code', 'Year', 'Budget Amount', 'BCM_Id', 'CLM_Id', 'BudgetStatus', 'Action'],
        colModel: [{ name: 'Description', width: 20, sortable: true  },
        {
            name: 'AssignedPercent', width: 10, sortable: false, editable: true, editoptions: {
                dataEvents: [
                               {
                                   type: 'keyup', fn: function (e) {
                                       checkVal(this);
                                   }
                               }
                            ]
            }
        },
        { name: 'AssignedAmount', width: 20, sortable: true },
        { name: 'RemainingAmount', width: 20, sortable: true },
        { name: 'CostCode', width: 20, sortable: false, hidden: true },
        { name: 'Year', width: 20, sortable: false, hidden: true },
        { name: 'BudgetAmount', width: 0, sortable: false, hidden: true },
        { name: 'BCM_Id', width: 0, sortable: false, hidden: true },
        { name: 'CLM_Id', width: 0, sortable: false, hidden: true },
        { name: 'BudgetStatus', width: 20, sortable: false, hidden: true },
        { name: 'act', index: 'act', width: 10, sortable: false }],
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divBudgetListPager',
        sortname: 'CostCode',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        'cellEdit': true,
        'cellsubmit' : 'clientArray',
        editurl: 'clientArray',
        footerrow: true,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Driver",
        gridComplete: function () {
            $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'Description': "Total" });
            var AssignedPer = $("#tbl_BudgetList").jqGrid('getCol', 'AssignedPercent', false, 'sum');
            $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'AssignedPercent': AssignedPer });
            sumofAssignedAmt = $("#tbl_BudgetList").jqGrid('getCol', 'AssignedAmount', false, 'sum');
            $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'AssignedAmount': sumofAssignedAmt });
            sumofRemainingAmount = $("#tbl_BudgetList").jqGrid('getCol', 'RemainingAmount', false, 'sum');
            $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'RemainingAmount': sumofRemainingAmount });
            $('#edit_tbl_BudgetList').css('display', 'none');
            $('#refresh_tbl_BudgetList').css('display', 'none');
            var AllGridData = $('#tbl_BudgetList').getRowData();
            if (AllGridData.length > 0) {
                BudgetAmount = AllGridData[0].BudgetAmount;
                $("#budgetValue").val(BudgetAmount);
            }
            var BudgetVal = $("#budgetValue").val()
            if (BudgetVal == "")
            {
                $("#budgetValue").hide();
                $('#BudgetLable').hide();
            }
            else
            {
                $("#budgetValue").show();
                $('#BudgetLable').show();
            }
            //if (AllGridData[0].BudgetStatus == 'o') {
            //    var cm = $("#tbl_BudgetList").jqGrid('getColProp', 'AssignedPercent');
            //    cm.editable = false;
            //}

            $('.ui-corner-all').css("font-size", "14px");
            $('.ui-corner-all').css("font-weight", "bold");


            var ids = jQuery("#tbl_BudgetList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord AddTransferAmount" Id="' + cl + '" title="Transfer Amount" style=" float: left;margin-right: 10px;cursor:pointer;"><input type="button" class="btn btn-primary texthover-greenlight" value="Budget Tansfer" /><span class="tooltips">Transfer Amount</span></a></div>'
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                //vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
                //qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
                jQuery("#tbl_BudgetList").jqGrid('setRowData', ids[i], { act: be }); ///+ qrc 
            }
            if ($('#tbl_BudgetList').getGridParam('records') === 0) {
                $('#tbl_BudgetList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<span><div>' + Year + '&nbsp;&nbsp;&nbsp;&nbsp;<label id="BudgetLable" style="font-size:14px;">Budget Amount:&nbsp;&nbsp;</label><input id="budgetValue" class="inputSearch" type="text"><a href="javascript:void(0)">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" value="Add Budget" id="AddBudget"   class="btn btn-primary" /></a></div></span>'

    });
    //if ($("#tbl_BudgetList").getGridParam("records") > 20) {
    jQuery("#tbl_BudgetList").jqGrid('navGrid', '#divBudgetListPager', { edit: true, add: false, del: false, search: false, edittext: "Edit" });
    $('#tbl_BudgetList').navButtonAdd('#divBudgetListPager',
            {
               // buttonicon: "ui-icon-plusthick",
                //title: "<span class='ui-icon-plusthick' style='background-color:#84cfe6;'>Add Cost Code</span>",
                caption: "Add Cost Code",
                position: "last",
                buttonicon: "ui-icon-circle-plus",
                onClickButton: customButtonClicked
            });
    $('#tbl_BudgetList').navButtonAdd('#divBudgetListPager',
               {
                   //buttonicon: "ui-icon-pencil",
                   //title: "<div class='btn btn-primary'><span class='ui-icon-disk' style='background-color:#84cfe6;'>Save</span></div>",
                   caption: "Save",
                   position: "last",
                   buttonicon: "ui-icon-disk",
                   onClickButton: SaveAllGridData
               });
    //}
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
    jQuery("#tbl_BudgetList").jqGrid('setGridParam', { url: $_HostPrefix + DriverUrl + "?txtSearch=" + txtSearch.trim() + "&statusType=" + statusType + "&LocationID=" + $_locationId, page: 1 }).trigger("reloadGrid");
}

function customButtonClicked() {
    if (IsGridSaved == false && sumofAssignedAmt > 0) {
        bootbox.dialog({
            message: "Please Save Budget, Otherwise Your data will lost..",
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                }
            }
        });
    }
    else{
        window.location.href = '../TreeView/?loc=' + $_locId;
    }
}
function SaveAllGridData()
{
    if (add > 100) {
        bootbox.dialog({
            message: "Total of Assigned % not be Greater than 100.",
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                }
            }
        });
    }
    else {
        var GridData = $('#tbl_BudgetList').getRowData();
        var selectedYearForGrid = $("#YearSelected option:selected").val();
        $.ajax({
            type: 'POST',
            url: '../SaveAllBudgetGridData/',
            data: { obj: GridData, LocationId: $_locId, Year: selectedYearForGrid },
            //contentType: 'application/json; charset=utf-8',
            datatype: 'json',
            success: function (result) {
                $("#tbl_BudgetList").trigger('reloadGrid');
                toastr.success(result);
                IsGridSaved = true;
            },
            error: function (result) {
                alert('Fail ');
            }
        });
    }
}

$(".AddTransferAmount").live("click", function (event) {
var id = $(this).attr("Id");
var rowData = jQuery("#tbl_BudgetList").getRowData(id);
var CostCodeDetailsModel = new Object();
CostCodeDetailsModel.Location = $_locId;
CostCodeDetailsModel.CostCodeId = id;
///JSON.stringify(CostCodeDetailsModel)
    //window.location.href = '../TransferAmountForCostCode/?CostCodeId=' + id + '?LocationId=' + $_locId;
window.location.href = "../TransferAmountForCostCode?CostCodeId=" + id + "&LocationId=" + $_locId;
});

$("#AddBudget").live("click", function (event) {
    $("#myModalForBudgetAmount").modal('show');
});
function checkVal(data) {
    var addData=0;
    IsGridSaved = false;
    var rowId = $(data).closest('tr').attr('id');;
    var rowData = $('#tbl_BudgetList').jqGrid('getRowData', rowId);
    assignedPercent = data.value;
    var txtval = $("#budgetValue").val();
    var GridData = $('#tbl_BudgetList').getRowData();
    var dataGrid = $('#tbl_BudgetList').jqGrid('getGridParam', 'data');
    for (var i = 0; i < GridData.length; i++) {
        addData = GridData[i].AssignedPercent;
        var n =addData.lastIndexOf('>');
        var result = addData.substring(n + 1);
        var a1 = parseInt(result)
        if (isNaN(a1) && i>0) {
            a1 = 0;
            var tt = GridData[i - 1].AssignedPercent;
            var a3 = parseInt(tt)
            var a2 = parseInt(assignedPercent)
            add;
        }
        else if (i == 0) {
            var a2 = parseInt(assignedPercent)
            add = a1 + a2;
        }
        else {
            add += a1;
        }
    }
    //$('#tbl_BudgetList').jqGrid('setRowData', rowId, rowData);
    //$("#tbl_BudgetList").jqGrid("setCell", rowId, "AssignedPercent", assignedPercent);
   // var AssignedPer = $("#tbl_BudgetList").jqGrid('getCol', 'AssignedPercent', false, 'sum');
    if (add > 100) {
        bootbox.dialog({
            message: "Total of Assigned % not be Greater than 100.",
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                }
            }
        });
    }
    else {
        var calculateAssignedAmt = txtval * (assignedPercent / 100)
        rowData.AssignedAmount = calculateAssignedAmt;
        $("#tbl_BudgetList").jqGrid("setCell", rowId, "AssignedAmount", calculateAssignedAmt);
        $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'AssignedPercent': add });
        sumofAssignedAmt = $("#tbl_BudgetList").jqGrid('getCol', 'AssignedAmount', false, 'sum');
        $('#tbl_BudgetList').jqGrid('footerData', 'set', { 'AssignedAmount': sumofAssignedAmt });
    }
}
$("#btnSaveForCostCode").live("click", function (event) {
    var txtValue = 0; var txtPreviousBudgetAmt=0;
    txtValue = $("#BudgetAmount").val();
    txtPreviousBudgetAmt = $("#budgetValue").val(txtValue);
    var addValue = txtValue + txtPreviousBudgetAmt;
    $("#budgetValue").val(txtValue);
    var selectedYear = $("#YearSelected option:selected").val();
    $("#myModalForBudgetAmount").modal('hide');
    $.ajax({
        type: 'POST',
        url: '../SaveBudgetAmountForLocation/',
        data: { BudgetAmount: txtValue, locationId: $_locId, BudgetYear: selectedYear },
        //contentType: 'application/json; charset=utf-8',
         datatype: 'json',
         success: function (result) {
             $("#tbl_BudgetList").trigger('reloadGrid');
             toastr.success(result);
             IsGridSaved = false;
        },
        error: function (result) {
            alert('Fail ');
        }
    });
});
$("#QRCGenerate").live("click", function (event) {
    var id = $(this).attr("data-value");
    var rowData = jQuery("#tbl_BudgetList").getRowData(id);
    var DriverName = rowData['EmployeeNameList'];
    $("#lblDriverName").html(DriverName);
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