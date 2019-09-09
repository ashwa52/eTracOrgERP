var PaymentTermsUrl = 'PaymentTerms/GetPaymentTerms';
var ActivatePaymentTerm = 'PaymentTerms/ActivePaymentTerm';

$(function () {
    $("#tbl_PaymentTermList").jqGrid({
        url: $_HostPrefix + PaymentTermsUrl + '?LocationID=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['PayTerm Code', 'Grace Period','Active', 'Actions'],
        colModel: [{ name: 'PTM_Term', width: 30, sortable: true },
        { name: 'PTM_GracePeriod', width: 40, sortable: false },
        { name: 'PTM_IsActive', width: 40, sortable: false, hidden:true },
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPaymentTermListPager',
        sortname: 'PTM_Term',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Payment Terms",
        gridComplete: function () {
            var ids = jQuery("#tbl_PaymentTermList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_PaymentTermList").getRowData(cl);
                if (Data.PTM_IsActive == "Y")
                {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="ActivePaymentMode" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else
                {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="ActivePaymentMode" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips">Click to Active</span></a>';
                }
                jQuery("#tbl_PaymentTermList").jqGrid('setRowData', ids[i], { act: be });
            }
            if ($("#tbl_PaymentTermList").getGridParam("records") <= 20) {
                $("#divPaymentTermListPager").hide();
            }
            else {
                $("#divPaymentTermListPager").show();
            }
            if ($('#tbl_PaymentTermList').getGridParam('records') === 0) {
                $('#tbl_PaymentTermList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddPaymentTerms"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_PaymentTermList").getGridParam("records") > 20) {
        jQuery("#tbl_PaymentTermList").jqGrid('navGrid', '#divPaymentTermListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_PaymentTermList").jqGrid('setGridParam', { url: $_HostPrefix + PaymentTermsUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddPaymentTerms").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: { },
        url: '../PaymentTerms/AddPaymentTerms/',
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
        url: $_HostPrefix + ActivatePaymentTerm + "?PaymentTermId=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_PaymentTermList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});