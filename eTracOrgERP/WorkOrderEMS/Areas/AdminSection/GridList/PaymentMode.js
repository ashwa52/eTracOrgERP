var PaymentModeUrl = 'PaymentMode/GetPaymentMode';
var ActivePaymentMode = 'PaymentMode/ActivePaymentMode';

$(function () {
    $("#tbl_PaymentModeList").jqGrid({
        url: $_HostPrefix + PaymentModeUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Payment Mode','IsActive', 'Actions'],
        colModel: [{ name: 'PMD_PaymentMode', width: 30, sortable: true },
            { name: 'PMD_IsActive', width: 30, sortable: true ,hidden:true},
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPaymentModeListPager',
        sortname: 'PMD_PaymentMode',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Payment Mode",
        gridComplete: function () {
            var ids = jQuery("#tbl_PaymentModeList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_PaymentModeList").getRowData(cl);
                if (Data.PMD_IsActive == "Y") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActivePaymentMode" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActivePaymentMode" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                jQuery("#tbl_PaymentModeList").jqGrid('setRowData', ids[i], { act: be });
            }
            if ($("#tbl_PaymentModeList").getGridParam("records") <= 20) {
                $("#divPaymentModeListPager").hide();
            }
            else {
                $("#divPaymentModeListPager").show();
            }
            if ($('#tbl_PaymentModeList').getGridParam('records') === 0) {
                $('#tbl_PaymentModeList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddPaymentMode"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_PaymentModeList").getGridParam("records") > 20) {
        jQuery("#tbl_PaymentModeList").jqGrid('navGrid', '#divPaymentModeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_PaymentModeList").jqGrid('setGridParam', { url: $_HostPrefix + PaymentModeUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddPaymentMode").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: {},
        url: '../PaymentMode/AddPaymentMode/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            $('.modal-title').text("Add Payment Terms");
            $("#largeeditpopup").html(result);
            $("#myModallarge").modal('show');
        }
    });
});

$("#ActivePaymentMode").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActivePaymentMode + "?PaymentModeId=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_PaymentModeList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});