var FormUrl = 'PDFForm/GetPDFFormDetailsList';
var ActiveFormUrl = 'PDFForm/ActiveForm';
var formReceiptDoc = '../PDFForm/FormDownload/';

$(function () {
    $("#tbl_PDFFormList").jqGrid({
        url: $_HostPrefix + FormUrl + '?locationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: [ 'Form Name','Is Active','Download', 'Actions'],
        colModel: [{ name: 'FormName', width: 30, sortable: true},
        { name: 'IsActive', width: 30, sortable: true},
        { name: 'FormPath', width: 30, sortable: true,hidden:true},
        { name: 'act', index: 'act', width: 30, sortable: false }],
        //rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divPDFFormListPager',
        sortname: 'FormType',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Forms",
        gridComplete: function () {
            var ids = jQuery("#tbl_PDFFormList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                var Data = jQuery("#tbl_PDFFormList").getRowData(cl);
                if (Data.IsActive == "Active") {
                    be = '<a href="javascript:void(0)" class="Assign" title="Active" id="ActiveForm" IsActive="N" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                }
                else {
                    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" id="ActiveForm" IsActive="Y" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                }
                var formfile = "";
                if (Data.FormPath == null || Data.FormPath == "" || Data.FormPath == '' || Data.FormPath == undefined) {
                }
                else {
                    formfile = '<a href="' + formReceiptDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Form</span></a></div>';
                }
                jQuery("#tbl_PDFFormList").jqGrid('setRowData', ids[i], { act: be, formfile  });
                //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
                //jQuery("#tbl_VendorTypeList").jqGrid('setRowData', ids[i], { act: de });
            }
            if ($("#tbl_PDFFormList").getGridParam("records") <= 20) {
                $("#divPDFFormListPager").hide();
            }
            else {
                $("#divPDFFormListPager").show();
            }
            if ($('#tbl_PDFFormList').getGridParam('records') == 0) {
                $('#tbl_PDFFormList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddForm"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_PDFFormList").getGridParam("records") > 20) {
        jQuery("#tbl_PDFFormList").jqGrid('navGrid', '#divVendorTypeListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}

function gridReload() {
    var txtSearch = jQuery("#SearchText").val();
    jQuery("#tbl_PDFFormList").jqGrid('setGridParam', { url: $_HostPrefix + VendorTypeUrl + "?txtSearch=" + txtSearch.trim(), page: 1 }).trigger("reloadGrid");
}

$("#AddForm").live("click", function (event) {
    $("#myModalForPDFForm").modal('show');
});

$("#ActiveForm").live("click", function (event) {
    var id = $(this).attr("cid");
    var IsActive = $(this).attr("IsActive");
    $.ajax({
        type: "POST",
        url: $_HostPrefix + ActiveFormUrl + "?Id=" + id + "&IsActive=" + IsActive,
        contentType: "application/json; charset=utf-8",
        beforesend: function () {
            new fn_showmaskloader('please wait...');
        },
        success: function (result) {
            $('#tbl_PDFFormList').trigger("reloadGrid");
            toastr.success(result);
        },
        error: function (xhr, status, error) {
        },
    });
});
