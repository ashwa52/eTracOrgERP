var POTypeUrl = 'POType/GetPOTypes';
var ActivePOTypeUrl = 'POType/ActivePOType';
$(function () {
    $("#tbl_POTypeList").jqGrid({
        url: $_HostPrefix + POTypeUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['PO Name', 'POT_IsActive', 'Actions'],
        colModel: [{ name: 'POT_POName', width: 30, sortable: true }, 
            { name: 'POT_IsActive', width: 30, sortable: true, hidden:true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPOTypeListPager',
        sortname: 'POT_POName',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of PO Type",
        gridComplete: function () {
            var ids = jQuery("#tbl_POTypeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_POTypeList").getRowData(cl);
                if (Data.POT_IsActive == "Y") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="POTypeId" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="POTypeId" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                jQuery("#tbl_POTypeList").jqGrid('setRowData', ids[i], { act: be });
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                //jQuery("#tbl_POTypeList").jqGrid('setRowData', ids[i], { act: de });
            }
            if ($("#tbl_POTypeList").getGridParam("records") <= 20) {
                $("#divPOTypeListPager").hide();
            }
            else {
                $("#divPOTypeListPager").show();
            }
            if ($('#tbl_POTypeList').getGridParam('records') === 0) {
                $('#tbl_POTypeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddPOType"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_POTypeList").getGridParam("records") > 20) {
        jQuery("#tbl_POTypeList").jqGrid('navGrid', '#divPOTypeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_POTypeList").jqGrid('setGridParam', { url: $_HostPrefix + POTypeUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddPOType").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: {},
        url: '../POType/AddPOType/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $('.modal-title').text("Add PO Type");
            $("#largeeditpopup").html(result);
            $("#myModallarge").modal('show');
        }
    });
});

$("#POTypeId").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActivePOTypeUrl + "?POTypeId=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_POTypeList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});