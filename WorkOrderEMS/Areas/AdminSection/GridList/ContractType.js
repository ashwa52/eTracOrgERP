var ContractTypeUrl = 'ContractType/GetContractTypes';
var ActiveContractType = 'ContractType/ActiveContractType';
$(function () {
    $("#tbl_ContractTypeList").jqGrid({
        url: $_HostPrefix + ContractTypeUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Contract Type', 'Description', 'CTT_IsActive', 'Actions'],
        colModel: [{ name: 'CTT_ContractType', width: 30, sortable: true },
        { name: 'CTT_Discription', width: 40, sortable: false },
        { name: 'CTT_IsActive', width: 40, sortable: false,hidden:true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divContractTypeListPager',
        sortname: 'CTT_ContractType',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Contract Type",
        gridComplete: function () {
            var ids = jQuery("#tbl_ContractTypeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_ContractTypeList").getRowData(cl);
                if (Data.CTT_IsActive == "Y") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActiveContractType" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActiveContractType" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                jQuery("#tbl_ContractTypeList").jqGrid('setRowData', ids[i], { act: be });
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                //jQuery("#tbl_ContractTypeList").jqGrid('setRowData', ids[i], { act: de });
            }
            if ($("#tbl_ContractTypeList").getGridParam("records") <= 20) {
                $("#divContractTypeListPager").hide();
            }
            else {
                $("#divContractTypeListPager").show();
            }
            if ($('#tbl_ContractTypeList').getGridParam('records') === 0) {
                $('#tbl_ContractTypeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddContractType"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_ContractTypeList").getGridParam("records") > 20) {
        jQuery("#tbl_ContractTypeList").jqGrid('navGrid', '#divContractTypeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_ContractTypeList").jqGrid('setGridParam', { url: $_HostPrefix + ContractTypeUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddContractType").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: {},
        url: '../ContractType/AddContractType/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $('.modal-title').text("Add Control Type");
            $("#largeeditpopup").html(result);
            $("#myModallarge").modal('show');
        }
    });
});

$("#ActiveContractType").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveContractType + "?ContractTypeId=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_ContractTypeList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});