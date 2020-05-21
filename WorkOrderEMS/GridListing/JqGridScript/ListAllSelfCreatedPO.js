var POSelfurl = 'POTypeData/GetAllSelfCreatedPOList';
var POType = ''
+ '<select id="ApproveDataSelf" class="getselfvalue">'
+ '<option value="W">Not Approved PO</option>'
+ '<option value="Y">Approved PO</option>'
+ '</select>';
var LocationId; var POApproveRemoveId;
var ProductList = []; var DataLists;
var status = "W";

$(function () {
    var act;
    $("#tbl_AllSelfPOList").jsGrid({
        height: "100%!important",
        width: "100%",
        filtering: false,
        editing: false,
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
                    url: $_HostPrefix + POSelfurl + '?LocationId=' + $_locationId + '&status=' + status,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "DisplayLogPOId", title: "PO Id", type: "text", width: 50 },
            { name: "POType", title: "PO Type", type: "text", width: 50 },
            { name: "CompanyName", title: "Company Name", type: "text", width: 50 },
            { name: "LocationName", title: "Location Name", type: "text", width: 50 },
            { name: "UserName", title: "Waiting Approval", type: "text", width: 50 }, 
            { name: "DisplayPODate", title: "PO Date", type: "text", width: 50 },
            { name: "DisplayDeliveryDate", title: "Delivery Date", type: "text", width: 50 },

            { name: "POStatusToDisplay", title: "PO Status", type: "text", width: 50 },
            { name: "Total", title: "Total Amount", type: "text", width: 50 },
            {

                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {

                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check check-icon" }).attr({ style: "" });

                    var $customButtonForEdit = $("<i>").attr({ class: "fa fa-pencil pencil-icon" }).attr({ style: "" });

                    var $iconPencil = $("<i>").attr({ class: "fa fa-list list-icon" }).attr({ style: "" });
                    var $customButtonForAcandDeActive = "";

                    if (item.POStatus == "Y")
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Already Approve" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {

                        }).append($iconPencilForAccountApprove);
                    }
                    else
                    {
                        $customButtonForAcandDeActive = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "Click to Edit" }).attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            Edit(item.id);
                        }).append($customButtonForEdit);
                    }
                    var $customViewButton = $("<span style='padding: 0 5px 0 0;'>").attr({ title: "View Details" }).attr({ id: "btn-edit-" + item.id }).click(function (e) {
                        ViewDetails(item);
                    }).append($iconPencil);
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForAcandDeActive).append($customViewButton);

                }
            }
        ]
    });






    //$("#tbl_AllSelfPOList").jqGrid({
    //    url: $_HostPrefix + POSelfurl + '?LocationId=' + $_locationId + '&status=' + status,
    //    datatype: 'json',
    //    type: 'GET',
    //    height: 400,
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['PO Id', 'PO Type', 'Company Name', 'Location Name','Waiting Approval', 'PO Date', 'Delivery Date', 'POStatusForCondition', 'PO Status', 'LogId', 'Total Amount', 'Actions'],
    //    colModel: [{ name: 'LogPOId', width: 30,  sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'POType', width: 40, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'CompanyName', width: 20, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'LocationName', width: 30, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'UserName', width: 30, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'PODate', width: 40, sortable: false },
    //    { name: 'DeliveryDate', width: 15, sortable: false },
    //    { name: 'POStatus', width: 15, sortable: false, hidden: true },
    //     { name: 'POStatusToDisplay', width: 15, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
    //    { name: 'LogId', width: 15, sortable: false, hidden: true },
    //    { name: 'Total', width: 15, sortable: false },
    //    { name: 'act', index: 'act', width: 30, sortable: false }],
    //    viewrecords: true,
    //    loadonce: true,
    //    gridview: true,
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    pager: $("#divAllSelfPOListPager"),
    //    rownumbers: true,
    //    emptyrecords: "No records to display",
    //    sortorder: 'asc',
    //    gridComplete: function () {
    //        jQuery('#tbl_AllSelfPOList').addClass('Selforder-table');
    //        var ids = jQuery("#tbl_AllSelfPOList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            var data = jQuery("#tbl_AllSelfPOList").getRowData(cl);
    //            be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
    //            //de = '<a href="javascript:void(0)" class="deleteRecord" id="RemovePO" title="delete" did="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-remove fa-2x texthover-bluelight"></span><span class="tooltips">Remove</span></a>';
    //            ai = '<a href="javascript:void(0)" id="ApproveSelfPOId" class="Assign" ApproveId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-list fa-2x texthover-yellowlight"></span><span class="tooltips">View</span></a></div>';
    //            var disable = "";
    //            if (data.POStatus == 'Y') {
    //                disable = '<a href="javascript:void(0)" class="download-cloud" title="AlreadyApprove" aid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-ban fa-2x"></span><span class="tooltips">Already Approve</span></a></div>';
    //                jQuery("#tbl_AllSelfPOList").jqGrid('setRowData', ids[i], { act: ai });
    //            }
    //            else {
    //                jQuery("#tbl_AllSelfPOList").jqGrid('setRowData', ids[i], { act: ai + be }); ///+ qrc 
    //            }
    //        }
    //        //if ($("#tbl_AllSelfPOList").getGridParam("records") <= 20) {
    //        //    $("#divAllSelfPOListPager").hide();
    //        //}
    //        //else {
    //        $("#divAllSelfPOListPager").show();
    //        $("input[name='DeliveryDate").hide();
    //        $("input[name='PODate").hide();
    //        $("input[name='act").hide();
    //        $("input[name='Total").hide();
    //        $("input[name='POType").hide();
    //        $("input[name='CompanyName").hide();
    //        $("input[name='LocationName").hide();
    //        $("input[name='POStatusToDisplay").hide(); 
    //        $("input[name='UserName").hide();
    //        //}
    //        if ($('#tbl_AllSelfPOList').getGridParam('records') === 0) {
    //            $('#tbl_AllSelfPOList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //    caption: ' <div><label>User Created PO List</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' +'&nbsp;<span><input type="text" class="inputSearch Selflight-table-filter" id="searchSelfCreatedPOtext" placeholder="PO Number"  Selfdata-table="Selforder-table" /></span></div>' //<span class="header_search"><input id="SearchText" class="inputSearch" placeholder="Serach By PO Number" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text">
    //});
    ////$("#tbl_AllSelfPOList").jqGrid('filterToolbar', { searchoperators: true });
    //if ($("#tbl_AllSelfPOList").getGridParam("records") > 20) {
    //    jQuery("#tbl_AllSelfPOList").jqGrid('navGrid', '#divAllSelfPOListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});

var timeoutHnd;
var flAuto = true;
function doSearchforsecondList() {
    $("#tbl_AllSelfPOList").jsGrid({
        controller: {
            loadData: function(filter) {
                return $.ajax({
                    type: "GET",
                    url: $_HostPrefix + POSelfurl + '?LocationId=' + $_locationId + '&status=' + status + '&txtSearch=' + $("#SearchTextForList2").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        }
    });
}
//function doSearch(ev) {
//    if (timeoutHnd)
//        clearTimeout(timeoutHnd)
//    timeoutHnd = setTimeout(gridReload, 500)
//}
//function gridReload() {

//    var txtSearch = jQuery("#SearchText").val();
//    jQuery("#tbl_AllSelfPOList").jqGrid('setGridParam', { url: $_HostPrefix + DriverUrl + "?POType=" + POType + "&LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
//}

function ViewDetails(event) {
    
    POApproveRemoveId = event.id;
    var rowData = event;
    $('#btnApproveData').hide();
    $('#btnRejectPO').hide();
    var POId = rowData['LogPOId'];
    var POType = rowData['POType'];
    var CompanyName = rowData['CompanyName'];
    var LocationName = rowData['LocationName'];
    var PODate = rowData['PODate'];
    var POStatus = rowData['POStatusToDisplay'];
    var DeliveryDate = rowData['DeliveryDate'];
    var Total = rowData['Total'];
    $("#lblPOId").html(POId);
    $("#lblPOType").html(POType);
    $("#lblCompanyName").html(CompanyName);
    $("#lblLocationName").html(LocationName);
    $("#lblPODate").html(rowData.DisplayPODate);
    $("#lblDeliveryDate").html(rowData.DisplayDeliveryDate);
    $('#lblTotal').html(Total)
    // $("#labellWorkRequestStatus").show();
    //$("#lblWorkRequestStatus").show();
    $.ajax({
        type: "post",
        url: '../POTypeData/GetAllPOFacilityByPOIdList' + '?POId=' + POApproveRemoveId,
        datatype: 'json',
        type: 'GET',
        success: function (result) {
            $('#records_table').html('');
            var arrData = [];
            var thHTML = '';
            var GrandTotal;
            thHTML += '<tr style="background-color:#0792bc;"><th>Cost Code</th><th>Description</th><th>Unit Price</th><th>Quantity</th><th>Total</th><th>Tax</th></tr>';
            $('#records_table').append(thHTML);
            if (result.rows.length > 0) {
                for (i = 0; i < result.rows.length; i++) {
                     
                    DataLists = result.rows;
                    GrandTotal = result.rows[i].cell[7]
                    var trHTML = '';
                    trHTML +=
                       '<tr><td>' + result.rows[i].cell[9] +
                       '</td><td>' + result.rows[i].cell[3] +
                       '</td><td>' + result.rows[i].cell[4] +
                       '</td><td>' + result.rows[i].cell[6] +
                       '</td><td>' + result.rows[i].cell[8] +
                       '</td><td>' + result.rows[i].cell[5] +

                       '</td></tr>';
                    $('#records_table').append(trHTML);
                }
            }
        }
    });
    $("#lblPOStatus").html(POStatus);
    $('.modal-title').text("PO Details");
    $("#myModalForGetPODetails").modal('show');
};

(function (document) {
    'use strict';
    var LightTableFilter = (function (Arr) {
        var _input;
        function _onInputEvent(e) {
            _input = e.target;
            var tables = document.getElementsByClassName(_input.getAttribute('Selfdata-table'));
            Arr.forEach.call(tables, function (table) {
                Arr.forEach.call(table.tBodies, function (tbody) {
                    Arr.forEach.call(tbody.rows, _filter);
                });
            });
        }
        //To hide row  add css display : none
        function _filter(row) {
            var text = row.textContent.toLowerCase(), val = _input.value.toLowerCase();
            row.style.display = text.indexOf(val) === -1 ? 'none' : '';
            $('.jqgfirstrow').css("display", "");
        }
        return {
            init: function () {
                var inputs = document.getElementsByClassName('Selflight-table-filter');
                Arr.forEach.call(inputs, function (input) {
                    input.oninput = _onInputEvent;
                });
            }
        };
    })(Array.prototype);
    document.addEventListener('readystatechange', function () {
        if (document.readyState === 'complete') {
            LightTableFilter.init();
        }
    });

})(document);
$(function () {
    $('#ApproveDataSelf').change(function () {
        //status = $("#ApproveDataSelf :selected").val();
        //jQuery("#tbl_AllSelfPOList").jqGrid('setGridParam', { url: $_HostPrefix + POSelfurl + '?LocationId=' + $_locationId + '&status=' + status, page: 1 }).trigger("reloadGrid");
    });
});
