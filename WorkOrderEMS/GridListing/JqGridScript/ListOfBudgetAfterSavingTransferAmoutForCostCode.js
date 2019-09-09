
$(function () {
    $("#tbl_BudgetListOfTransfer").jqGrid({
        url: '../GetListCostCodeForTranferBudgetAsPerLocation?Loc=' + $_locId,
        //data:{LocationID:$_locId,RemainingAmt:1000},
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Cost Code', 'Assigned Percent%', 'Assigned Amount', 'Remaining Amount', 'CostCode', 'Year', 'BudgetAmount', 'BCM_Id', 'CLM_Id', 'BudgetStatus'],
        colModel: [{ name: 'Description', width: 60, sortable: true },
        { name: 'AssignedPercent', width: 30, sortable: true },
        { name: 'AssignedAmount', width: 30, sortable: false },
        { name: 'RemainingAmount', width: 30, sortable: false},
        { name: 'CostCode', width: 0, sortable: false, hidden: true },
        { name: 'Year', width: 0, sortable: false, hidden: true },
        { name: 'BudgetAmount', width: 0, sortable: false, hidden: true },
        { name: 'BCM_Id', width: 0, sortable: false, hidden: true },
        { name: 'CLM_Id', width: 0, sortable: false, hidden: true },
        { name: 'BudgetStatus', width: 0, sortable: false, hidden: true }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divBudgetListOfTransferPager',
        sortname: 'CostCode',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {

            var ids = jQuery("#tbl_BudgetListOfTransfer").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                tr = '<div><a href="javascript:void(0)" class="qrc AddAmountForCostCode" title="Transfer Amount" Id="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-exchange texthover-bluelight"></span><span class="tooltips">Transfer Amount</span></a></div>';
                jQuery("#tbl_BudgetListOfTransfer").jqGrid('setRowData', ids[i], { act: tr }); ///+ qrc 
            }
            if ($("#tbl_BudgetListOfTransfer").getGridParam("records") <= 20) {
                $("#divBudgetListOfTransferPager").hide();
            }
            else {
                $("#divBudgetListOfTransferPager").show();
            }
            if ($('#tbl_BudgetListOfTransfer').getGridParam('records') === 0) {
                $('#tbl_BudgetListOfTransfer tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Cost Code" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text"></div>'

    });
    if ($("#tbl_BudgetListOfTransfer").getGridParam("records") > 20) {
        jQuery("#tbl_BudgetListOfTransfer").jqGrid('navGrid', '#divBudgetListOfTransferPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}