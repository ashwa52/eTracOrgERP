var fuelurl = 'fueling/GetFuelingList';
var fuelingReceiptDoc = '../fueling/ReceiptDownload/';

$(function () {
    $("#tbl_AllPDFFormList").jqGrid({
        url: $_HostPrefix + fuelurl,
        datatype: 'json',
        type: 'GET',
        height: 400,
        width: 700,
        autowidth: true,
        colNames: ['Form Name', 'Submitted By', 'Created Date', 'Status', 'Actions'],
        colModel: [{ name: 'FormName', width: 30, sortable: true },
        { name: 'CreatedBy', width: 40, sortable: true },
        { name: 'CreateDate', width: 40, sortable: false },
        { name: 'IsActive', width: 40, sortable: true },      
        { name: 'act', index: 'act', width: 30, sortable: false }],
        rownum: 10,
        rowList: [10, 20, 30],
        scrollOffset: 0,
        pager: '#divAllPDFFormListPager',
        sortname: 'FuelingDate',
        viewrecords: true,
        gridview: true,
        loadonce: false,
        multiSort: true,
        rownumbers: true,
        //emptyrecords: "No records to display",
        shrinkToFit: true,
        sortorder: 'asc',
        gridComplete: function () {
            var ids = jQuery("#tbl_AllPDFFormList").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                var rowData = jQuery("#tbl_AllPDFFormList").getRowData(cl);
                var FuelStatusFile = rowData['FuelReceiptImage'];
                vi = '<a href="javascript:void(0)" class="qrc" title="Detail" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';

                // vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" style=" float: left;margin-right: 3px;cursor:pointer;">view<span class=" icon-cog fa-2x texthover-bluelight"></span><span class="tooltips">View</span></a></div>';
                var fuelfile = "";
                if (FuelStatusFile == null || FuelStatusFile == "" || FuelStatusFile == '' || FuelStatusFile == undefined) {
                }
                else {
                    fuelfile = '<a href="' + fuelingReceiptDoc + '?Id=' + cl + '" class="download-cloud" title="Receipt file" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Receipt</span></a></div>';
                }
                $("#tbl_AllPDFFormList").jqGrid('setRowData', ids[i], { act: vi + fuelfile }); //+ qrc 
            }

            if ($("#tbl_AllPDFFormList").getGridParam("records") <= 20) {
                $("#divAllPDFFormListPager").hide();
            }
            else {
                $("#divAllPDFFormListPager").show();
            }

            if ($('#tbl_AllPDFFormList').getGridParam('records') === 0) {
                $('#tbl_AllPDFFormList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
            }
        },
        //caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by vehicle no" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;<input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" >&nbsp;<input type="button" value="Show Fueling" class="ViewAllButton" onclick="ViewFuelDetails();" title="Click to view"/></div>'
        caption: '<div id="openFormLink"><a href="javascript:void(0)" style="cursor:pointer;"><i class="fa fa-plus-square" style="font-size:36px;"></i></a></div>'
        //  caption: '<div class="header_search"><input id="SearchText" class="inputSearch" placeholder="Search by vehicle no" onkeydown="doSearch(arguments[0]||event)" type="text">&nbsp;' + vehcileApprovalStatus + '&nbsp;</div>'
    });
    if ($("#tbl_AllPDFFormList").getGridParam("records") > 20) {
        jQuery("#tbl_AllPDFFormList").jqGrid('navGrid', '#divAllPDFFormListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    }
});
$("#openFormLink").live("click", function (event) {
    $.ajax({
        type: "GET",
        data: {},
        url: 'GetPDFFormName/PDFData',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            if (result > 0) {
                for (var i = 0; i < result.length; i++) {
                    var number = i + 1;
                    $("#divPDFDetailPreview").append("<ll style='display:inline;'><a href=" + result[i].FormPath + ">" + number + "&nbsp;)&nbsp;" + result[i].FormName + "</a> </li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                }
            }
            else {
                $("#divPDFDetailPreview").append("<ll style='display:inline;'>No PDF Found </li>");
            }

            $("#myModalForPDFFormData").modal('show');
        }
    });
});