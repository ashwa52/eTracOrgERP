var POurl = 'POTypeData/GetAllPOList';
var AddPO = 'POTypeData/Index/';
var ApprovePO = 'POTypeData/Index/';
var EditPO = 'POTypeData/EditPOByPOId/';
var POTypeList = ''
+ '<select id="ApproveData">'
+ '<option value="W">Not Approved</option>'
+ '<option value="Y">Approved PO</option>'
+ '</select>';
var LocationId; var POApproveRemoveId;
var ProductList = []; var DataLists;
var status = "W";
var searchColumn;
var allLocationPO = '';

//if ($_userType == "1" || $_userType == "5" || $_userType == "6") {
allLocationPO = '<div class="onoffswitch2" style="margin-left: 650px;"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'
//}
$(function () {
    $("#tbl_AllPOList").jqGrid({
        url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status,
        datatype: 'json',
        type: 'GET',
        height: 300,
        width: 700,
        autowidth: true,
        colNames: ['PO Id', 'PO Type', 'Company Name', 'Location Name', 'PO Date', 'Delivery Date', 'POStatusForCondition', 'PO Status', 'LogId', 'Total Amount', 'CreatedBy', 'Actions'],
        colModel: [{ name: 'LogPOId', width: 30, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
        { name: 'POType', width: 40, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
        { name: 'CompanyName', width: 20, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
        { name: 'LocationName', width: 30, sorttype: 'text', searchoptions: { sopt: ['cn'] } },
        { name: 'PODate', width: 40},
        { name: 'DeliveryDate', width: 15 },
        { name: 'POStatus', width: 15, sorttype: 'text', hidden: true },
         { name: 'POStatusToDisplay', width: 15,  searchoptions: { sopt: ['cn'] } },
        { name: 'LogId', width: 15,  hidden: true },
        { name: 'Total', width: 15 },
        { name: 'CreatedBy', width: 15, sorttype: 'text', hidden: true },
        { name: 'act', index: 'act', width: 30 }],
        viewrecords: true,
        loadonce: false,
        gridview: true,
        rownum: 10,
        rowList: [10, 20, 30],
        pager: $("#divAllPOListPager"),
        rownumbers: true,
        emptyrecords: "No records to display",
        sortorder: 'asc',
        gridComplete: function () {
            searchColumn = jQuery("#tbl_AllPOList").jqGrid('getCol', 'LogPOId', true)
            var ids = jQuery("#tbl_AllPOList").jqGrid('getDataIDs');
            //jQuery("#tbl_AllPOList").addCSS("order-table");
            jQuery('#tbl_AllPOList').addClass('order-table');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var data = jQuery("#tbl_AllPOList").getRowData(cl);
                be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                //de = '<a href="javascript:void(0)" class="deleteRecord" id="RemovePO" title="delete" did="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-remove fa-2x texthover-bluelight"></span><span class="tooltips">Remove</span></a>';
                ai = '<a href="javascript:void(0)" id="ApprovePOId" class="Assign" ApproveId="' + cl + '" title="assign" style=" float: left;margin-right: 3px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">ViewPO</span></a></div>';
                var disable = "";                
                if (data.POStatus == 'Y') {
                    disable = '<a href="javascript:void(0)" class="download-cloud" title="AlreadyApprove" aid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-ban fa-2x"></span><span class="tooltips">Already Approve</span></a></div>';
                    jQuery("#tbl_AllPOList").jqGrid('setRowData', ids[i], { act: disable + ai });
                }
                else {
                    jQuery("#tbl_AllPOList").jqGrid('setRowData', ids[i], { act: be + ai }); ///+ qrc 
                }
            }
            $("input[name='DeliveryDate").hide(); 
            $("input[name='PODate").hide(); 
            $("input[name='act").hide(); 
            $("input[name='Total").hide(); 
            $("input[name='POType").hide(); 
            $("input[name='CompanyName").hide(); 
            $("input[name='LocationName").hide(); 
            $("input[name='POStatusToDisplay").hide();
            //if ($("#tbl_AllPOList").getGridParam("records") <= 20) {
            //    $("#divAllPOListPager").hide();
            //}
            //else {
                $("#divAllPOListPager").show();
            //}
            if ($('#tbl_AllPOList').getGridParam('records') === 0) {
                $('#tbl_AllPOList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: ' <div><span><a href="javascript:void(0)"><i id="AddPO" class="fa fa-plus-square" style="font-size:36px;"></i></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + POTypeList + '&nbsp;<input type="text" class="inputSearch light-table-filter" id="searchtext" placeholder="PO Number"  data-table="order-table" /></span> ' + allLocationPO + '</div>' //<span class="header_search"><input id="SearchText" class="inputSearch" placeholder="Serach By PO Number" style="width: 260px;" onkeydown="doSearch(arguments[0]||event)" type="text">

    });
    $('#ViewAllLocation').change(function () {
        ViewAllRecordsPO();
    });
    ///$("#tbl_AllPOList").jqGrid('filterToolbar', { searchoperators: true });
    
    //$('#tbl_AllPOList').jqGrid('filterToolbar', { searchOnEnter: true, enableClear: false });
    //if ($("#tbl_AllPOList").getGridParam("records") > 20) {

    jQuery("#tbl_AllPOList").jqGrid('navGrid', '#divAllPOListPager', { edit: false, add: false, del: false, search: true, edittext: "Edit", searchtext: "Search PO", refresh: true });
    //}
});



var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}
//function gridReload() {

//    var txtSearch = jQuery("#SearchText").val();
//    jQuery("#tbl_AllPOList").jqGrid('setGridParam', { url: $_HostPrefix + DriverUrl + "?POType=" + POType + "&LocationId=" + $_locationId, page: 1 }).trigger("reloadGrid");
//}
$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    window.location.href = $_HostPrefix + EditPO + '?POId=' + id + '&LocationId=' + $_locationId;
});

$("#AddPO").live("click", function (event) {
    window.location.href = $_HostPrefix + AddPO;
});

function RejectPO()
{
    $("#myModelApproveReject").modal('show');
}
function AppprovePO() {
    $("#btnApproveData").addClass("disabled");
    $("#btnRejectPO").addClass("disabled");
    callAjaxPO();
}
function RejectPOAfterCommentPO() {
    $("#btnApproveData").addClass("disabled");
    $("#btnRejectPO").addClass("disabled");   
    callAjaxPO();
}

function callAjaxPO()
{
    var objData = new Object();
    objData.LocationId = $_locationId; 
    debugger;
    var data = jQuery("#tbl_AllPOList").getRowData(POApproveRemoveId);
    objData.POApproveRemoveId = POApproveRemoveId;
    for (var j = 0; j < DataLists.length; j++)
    {
        ProductList.push({"COM_FacilityId": DataLists[j].cell[0] ,
            "CostCode": DataLists[j].cell[1],
            "FacilityType": DataLists[j].cell[2],
            "COM_Facility_Desc": DataLists[j].cell[3],
            "UnitPrice": DataLists[j].cell[4],
            "Tax": DataLists[j].cell[5]
        });
        
    }
    objData.POId = data.LogId;
    objData.Comment = $("#CommentPO").val();
    $.ajax({
        url: '../POTypeData/ApprovePO',
        type: 'POST',       
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objPOApproveRejectModel: objData, objListData: data, ProductListData: ProductList }),
        beforesend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForGetPODetails").modal('hide');
            $("#btnApproveData").removeClass("disabled");
            $("#btnRejectPO").removeClass("disabled");
            $("#tbl_AllPOList").trigger("reloadGrid");
        },
        error: function () { toastr.success(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
 //   $("#tbl_AllPOList").trigger("reloadGrid");
 //   $("#tbl_AllPOList").jqGrid("setGridParam", { datatype: "json" })
 //.trigger("reloadGrid", [{ current: true }]);
}

$("#ApprovePOId").live("click", function (event) {
    POApproveRemoveId = $(this).attr("ApproveId");
    var rowData = jQuery("#tbl_AllPOList").getRowData(POApproveRemoveId);
    if (rowData.POStatus == 'Y' || rowData.POStatus == 'R')
    {
        $('#btnApproveData').hide();
        $('#btnRejectPO').hide();
    }
    else {
        $('#btnApproveData').show();
        $('#btnRejectPO').show();
    }
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
    $("#lblPODate").html(PODate);
    $("#lblDeliveryDate").html(DeliveryDate);
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
                       '<tr><td>' +  result.rows[i].cell[9] +
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
});

(function (document) {
    'use strict';
    var LightTableFilter = (function (Arr) {
        var _input;
        function _onInputEvent(e) {
            _input = e.target;
            var tables = document.getElementsByClassName(_input.getAttribute('data-table'));
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
                var inputs = document.getElementsByClassName('light-table-filter');
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
    $('#ApproveData').change(function () {
        status = $("#ApproveData :selected").val();
        jQuery("#tbl_AllPOList").jqGrid('setGridParam', {
            url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status, datatype: 'json',type: 'GET', page: 1}).trigger("reloadGrid");
    });
});
$(document).ready(function () {
    
    $('#ss').keyup(function () {
        if (this.value.length) {
            var searchString = jQuery(this).val().toLowerCase()
         
        }
    });
})
function ViewAllRecordsPO() {
    var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation :selected").val();
    if (locaId == 0) {
        $("#drp_MasterLocation").hide();
    }
    else {
        $("#drp_MasterLocation").show();
    }
    jQuery("#tbl_AllPOList").jqGrid('setGridParam', { url: $_HostPrefix + POurl + '?LocationId=' + locaId + '&status=' + status, page: 1 }).trigger("reloadGrid");
}