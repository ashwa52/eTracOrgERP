var pturl = 'passenger/GetPassengerCountList';
var serviceTypeDDL = ''
    + '<select id="serviceTypeDDL" class="" onchange="doSearch(arguments[0]||event);">'
    + '<option value="0">All</option>'
    + '<option value="464">Regular</option>'
    + '<option value="465">Event</option>'
+ '</select>';
$(function () {
    $("#tbl_PassengerTrackingCountList").jqGrid({
        url: $_HostPrefix + pturl,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Route Name', 'Service Type', 'Vehicle Number', 'PickUp Point', 'Drop Point', 'Count', 'Submitted By', 'Submitted Date'],
        colModel: [{ name: 'RouteName', width: 30, sortable: true },
        { name: 'ServiceTypeName', width: 40, sortable: true },
        { name: 'VehicleNumber', width: 40, sortable: false },
        { name: 'PickUpPoint', width: 40, sortable: true },
        { name: 'DropPoint', width: 40, sortable: true },
        { name: 'PassengerCount', width: 30, sortable: true },
        { name: 'EmployeeName', width: 30, sortable: true },
        { name: 'CreatedDate', width: 30, sortable: true }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPTListPager',
        sortname: 'CreatedDate',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Passenger count",
        gridComplete: function () {
            var ids = jQuery("#tbl_PassengerTrackingCountList").jqGrid('getDataIDs');
            //for (var i = 0; i < ids.length; i++) {
            //    var cl = ids[i];
            //    be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
            //    de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a></div>';
            //    jQuery("#tbl_PassengerTrackingCountList").jqGrid('setRowData', ids[i], { act: be + de }); //+ qrc 
            //}
            if ($("#tbl_PassengerTrackingCountList").getGridParam("records") <= 20) {
                $("#divPTListPager").hide();
            }
            else {
                $("#divPTListPager").show();
            }
            if ($('#tbl_PassengerTrackingCountList').getGridParam('records') === 0) {
                $('#tbl_PassengerTrackingCountList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Pickup point" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + serviceTypeDDL + '&nbsp;</div>'
    });
    if ($("#tbl_PassengerTrackingCountList").getGridParam("records") > 20) {
        jQuery("#tbl_PassengerTrackingCountList").jqGrid('navGrid', '#divPTListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    var statusType = jQuery("#serviceTypeDDL :selected").val();
    jQuery("#tbl_PassengerTrackingCountList").jqGrid('setGridParam', { url: $_HostPrefix + pturl + "?SearchText=" + txtSearch.trim() + "&statusType=" + statusType, page: 1 }).trigger("reloadGrid");
}