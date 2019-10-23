var RuleUrl = 'Rule/GetRuleList';
var ActiveContractType = 'ContractType/ActiveContractType';
var editRule = 'Rule/editRule';
$(function () {
    $("#tbl_RuleList").jqGrid({
        url: $_HostPrefix + RuleUrl + '?locationId=' + $_locationId,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Rule Name','Module Name', 'Level', 'Date', 'Slab From', 'Slab To', 'IsActive', 'Actions'],
        colModel: [{ name: 'RuleName', width: 30, sortable: true },
        { name: 'ModuleName', width: 40, sortable: false },
        { name: 'Level', width: 40, sortable: false },
        { name: 'Date', width: 40, sortable: false},
        { name: 'SlabFrom', width: 40, sortable: false },
        { name: 'SlabTo', width: 40, sortable: false },
        { name: 'IsActive', width: 40, sortable: false },
        { name: 'act', index: 'act', width: 30, sortable: false }], 
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divRuleListPager',
        sortname: 'RuleName',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        caption: "List of Rule",
        gridComplete: function () {
            var ids = jQuery("#tbl_RuleList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "";
                de = "";
               // var Data = jQuery("#tbl_RuleList").getRowData(cl);
                //if (Data.IsActive == "Y") {
                //    be = '<a href="javascript:void(0)" class="Assign" title="Active" IsActive="N" id="RuleId" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-yellowlight"></span><span class="tooltips">Activated</span></a>';
                //}
                //else {
                //    be = '<a href="javascript:void(0)" class="deleteRecord" title="delete" IsActive="Y" id="RuleId" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-check fa-2x texthover-bluelight"></span><span class="tooltips" style="width:100px;">Click to Active</span></a>';
                //   }
                var de = '<div><a href="javascript:void(0)" class="EditRecord" id="EditRule" rid="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a></div>';

                jQuery("#tbl_RuleList").jqGrid('setRowData', ids[i], { act: de });
            }
            if ($("#tbl_RuleList").getGridParam("records") <= 20) {
                $("#divRuleListPager").hide();
            }
            else {
                $("#divRuleListPager").show();
            }
            if ($('#tbl_RuleList').getGridParam('records') === 0) {
                $('#tbl_RuleList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        caption: '<div id="AddRule"><a href="javascript:void(0)"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
    });
    if ($("#tbl_RuleList").getGridParam("records") > 20) {
        jQuery("#tbl_RuleList").jqGrid('navGrid', '#divRuleListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});

$("#EditRule").live("click", function (event) {
    var id = $(this).attr("rid");
    window.location.href = $_HostPrefix + editRule + '?id=' + id;
    //$("#largeeditpopup").load('../StaffUser/EditStaffUser/' + id);
});

$("#AddRule").live("click", function (event) {    
    $("#myModalForRule").modal('show');
    //$.ajax({
    //    type: "GET",
    //    data: {},
    //    url: '../ContractType/AddContractType/',
    //    contentType: "application/json; charset=utf-8",
    //    error: function (xhr, status, error) {
    //    },
    //    success: function (result) {
    //        $('.modal-title').text("Add Control Type");
    //        $("#largeeditpopup").html(result);
    //        $("#myModallarge").modal('show');
    //    }
    //});
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