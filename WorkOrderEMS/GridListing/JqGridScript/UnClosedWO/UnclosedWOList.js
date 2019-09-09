var unclosedWOUrl = 'WorkOrder/UnclosedWorkOrder';

$(function () {
    $("#tbl_unClosedWOList").jqGrid({
        url: $_HostPrefix + unclosedWOUrl + '?LocationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Work Order', 'Assigned To', 'Start Time', 'Location Name'],
        colModel: [{ name: 'WorkOrder', width: 30, sortable: true },
        { name: 'AssignedTo', width: 40, sortable: false },
        { name: 'StartTime', width: 20, sortable: true },
        { name: 'LocationName', width: 30, sortable: true }],
        //{ name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divUnclosedWOListPager',
        sortname: 'WorkOrder',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Driver",
        gridComplete: function () {

            var ids = jQuery("#tbl_unClosedWOList").jqGrid('getDataIDs');
            //for (var i = 0; i < ids.length; i++) {
            //    var cl = ids[i];
            //    be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
            //    qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
            //    //jQuery("#tbl_unClosedWOList").jqGrid('setRowData', ids[i], { act: be + qrc }); ///+ qrc 
            //}
            if ($("#tbl_unClosedWOList").getGridParam("records") <= 20) {
                $("#divUnclosedWOListPager").hide();
            }
            else {
                $("#divUnclosedWOListPager").show();
            }
            if ($('#tbl_unClosedWOList').getGridParam('records') === 0) {
                $('#tbl_unClosedWOList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by work order" style="width: 260px;" type="text"></div>'

    });
    if ($("#tbl_unClosedWOList").getGridParam("records") > 20) {
        jQuery("#tbl_unClosedWOList").jqGrid('navGrid', '#divUnclosedWOListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});








