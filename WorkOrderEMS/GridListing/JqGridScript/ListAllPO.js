var POurl = 'POTypeData/GetAllPOList';
var AddPO = 'POTypeData/Index/';
var ApprovePO = 'POTypeData/Index/';
var EditPO = 'POTypeData/EditPOByPOId/';
var POTypeList = ''
+ '<select id="ApproveData">'
+ '<option value="W">Not Approved</option>'
+ '<option value="Y">Approved PO</option>'
+ '</select>';
var LocationId;
var POApproveRemoveId;
var ProductList = []; var DataLists;
var status = "W";
var searchColumn;
var _currentrowdata = '';
var allLocationPO = '';

//if ($_userType == "1" || $_userType == "5" || $_userType == "6") {
allLocationPO = '<div class="onoffswitch2" style="margin-left: 650px;"><input type="checkbox" name="onoffswitch2" class="onoffswitch2-checkbox" id="ViewAllLocation"><label for="ViewAllLocation" class="onoffswitch2-label"><span class="onoffswitch2-inner"></span><span class="onoffswitch2-switch"></span></label></div>'

$(function () {
    var act;
    $("#tbl_AllPOList").jsGrid({
        height: "170%",
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
                    url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status,
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
            { name: "DisplayPODate", title: "PO Date", type: "text", width: 50 },
            { name: "DisplayDeliveryDate", title: "Delivery Date", type: "text", width: 50 },
            { name: "POStatusToDisplay", title: "PO Status", type: "text", width: 50 },
            { name: "Total", title: "Total Amount", type: "text", width: 50 },
            {
               
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    
                    var $iconPencilForAccountApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                   
                    var $customButtonForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:green;font-size: 22px;" });
                   
                    var $iconPencil = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });
                    var $customButtonForAcandDeActive = "";
                   
                    if (item.POStatusToDisplay == "Approved")
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
                        ViewDetailsForPO(item.id, item); 
                        }).append($iconPencil); 
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customButtonForAcandDeActive).append($customViewButton);
                
            } 
            }
        ]
    });


});
 
var timeoutHnd;
var flAuto = true;
function doSearch() {
    $("#tbl_AllPOList").jsGrid({
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status + '&txtSearch=' + $("#SearchText").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        }
    });
}
 
  function Edit(id) {
   
    window.location.href = $_HostPrefix + EditPO + '?POId=' + id + '&LocationId=' + $_locationId;
};

$("#AddPO").on("click", function (event) {
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
    var data = _currentrowdata;
    objData.POApproveRemoveId = POApproveRemoveId;
    if (DataLists != undefined)
    {
        for (var j = 0; j < DataLists.length; j++) {
            ProductList.push({
                "COM_FacilityId": DataLists[j].cell[0],
                "CostCode": DataLists[j].cell[1],
                "FacilityType": DataLists[j].cell[2],
                "COM_Facility_Desc": DataLists[j].cell[3],
                "UnitPrice": DataLists[j].cell[4],
                "Tax": DataLists[j].cell[5]
            });

        }
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
            $("#tbl_AllPOList").jsGrid("loadData"); 
            $("#tbl_AllSelfPOList").jsGrid("loadData"); 

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

function ViewDetailsForPO(POApproveRemoveId1, item) { 
    _currentrowdata = item;
    POApproveRemoveId = POApproveRemoveId1;
   
    var rowData = item ;
    if (rowData.POStatus == 'Y' || rowData.POStatus == 'R')
    {
        $('#btnApproveData').hide();
        $('#btnRejectPO').hide();
    }
    else {
        $('#btnApproveData').show();
        $('#btnRejectPO').show();
    } 
    $("#lblPOId").html(rowData.DisplayLogPOId);
    $("#lblPOType").html(rowData.POType);
    $("#lblCompanyName").html(rowData.CompanyName);
    $("#lblLocationName").html(rowData.LocationName);
    $("#lblPODate").html(rowData.DisplayPODate);
    $("#lblDeliveryDate").html(rowData.DisplayDeliveryDate);
    $('#lblTotal').html(rowData.Total);
    
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
            if (result.rows.length > 0)
            {
                for (i = 0; i < result.rows.length; i++)
                {
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
    $("#lblPOStatus").html(rowData.POStatus);
    
    $("#myModalForGetPODetails").modal('show');
};

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
    $('#ViewAllLocation').change(function () {
        ViewAllRecordsPO();
    });
    $('#ApproveData').change(function () {
        status = $("#ApproveData :selected").val();
        $("#tbl_AllPOList").jsGrid({
            controller: {
                loadData: function (filter) {
                    return $.ajax({
                        type: "GET",
                        url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status,
                        data: filter,
                        dataType: "json"
                    });
                }
            }
            //   url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status, datatype: 'json',type: 'GET', page: 1}).trigger("reloadGrid");
        });
    });
    $(document).ready(function () {

        $('#ss').keyup(function () {
            if (this.value.length) {
                var searchString = jQuery(this).val().toLowerCase()

            }
        });
    })
});
    function ViewAllRecordsPO() {
        var locaId = $('#ViewAllLocation').prop('checked') == true ? 0 : $("#drp_MasterLocation1 :selected").val();
        if (locaId == 0) {
            $("#drp_MasterLocation1").hide();
        }
        else {
            $("#drp_MasterLocation1").show();
        }
        $("#tbl_AllPOList").jsGrid({
            controller: {
                loadData: function (filter) {
                    return $.ajax({
                        type: "GET",
                        url: $_HostPrefix + POurl + '?LocationId=' + locaId + '&status=' + status,page: 1,
                        data: filter,
                        dataType: "json"
                    });
                }
            }
            //   url: $_HostPrefix + POurl + '?LocationId=' + $_locationId + '&status=' + status, datatype: 'json',type: 'GET', page: 1}).trigger("reloadGrid");
        });
        
    }
 