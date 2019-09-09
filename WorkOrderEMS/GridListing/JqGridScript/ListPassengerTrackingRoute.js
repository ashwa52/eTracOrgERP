var pturl = 'passenger/GetPassengerRouteList';
var editurl = 'passenger/edit/';
var serviceTypeDDL = ''
    + '<select id="serviceTypeDDL" class="" onchange="doSearch(arguments[0]||event);">'
    + '<option value="0">All</option>'
    + '<option value="464">Regular</option>'
    + '<option value="465">Event</option>'
+ '</select>';
$(function () {
    $("#tbl_PassengerTrackingRouteList").jqGrid({
        url: $_HostPrefix + pturl,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Route Name', 'Service Type', 'Start Date', 'End Date', 'PickUp Point', 'Drop Point', 'Actions'],
        colModel: [{ name: 'RouteName', width: 30, sortable: true },
        { name: 'ServiceType', width: 40, sortable: true },
        { name: 'StartDate', width: 40, sortable: false },
        { name: 'EndDate', width: 40, sortable: true },
        { name: 'PickUpPoint', width: 40, sortable: true },
        { name: 'DropPoint', width: 30, sortable: true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPTListPager',
        sortname: 'StartDate',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Passenger route",
        gridComplete: function () {
            var ids = jQuery("#tbl_PassengerTrackingRouteList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a></div>';
                jQuery("#tbl_PassengerTrackingRouteList").jqGrid('setRowData', ids[i], { act: be + de }); //+ qrc 
            }           
            if ($("#tbl_PassengerTrackingRouteList").getGridParam("records") <= 20) {
                $("#divPTListPager").hide();
            }
            else {
                $("#divPTListPager").show();
            }
            if ($('#tbl_PassengerTrackingRouteList').getGridParam('records') === 0) {
                $('#tbl_PassengerTrackingRouteList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by Pickup point" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + serviceTypeDDL + '&nbsp;</div>'
    });
    if ($("#tbl_PassengerTrackingRouteList").getGridParam("records") > 20) {
        jQuery("#tbl_PassengerTrackingRouteList").jqGrid('navGrid', '#divPTListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
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
    jQuery("#tbl_PassengerTrackingRouteList").jqGrid('setGridParam', { url: $_HostPrefix + pturl + "?SearchText=" + txtSearch.trim() + "&statusType=" + statusType, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + editurl + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});

$(".deleteRecord").live("click", function (event) {
    
    var id = $(this).attr("cid");
    bootbox.dialog({
        message: "Are you sure you want to delete this route?",
        buttons: {
            success: {
                label: "delete",
                classname: "btn btn-primary",
                callback: function () {
                    $.ajax({
                        type: "post",
                        url: '../passenger/delete/' + '?id=' + id,
                        beforesend: function () {
                            new fn_showmaskloader('please wait...');
                        },
                        success: function (data) {
                            
                            toastr.success(data.Message);
                            $('#tbl_PassengerTrackingRouteList').trigger('reloadgrid');
                            gridReload();
                        },
                        error: function () {
                            alert("error:")
                        }
                        //complete: function () {
                        //    fn_hidemaskloader();
                        //}
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