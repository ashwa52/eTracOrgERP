var $userType;
var $locationId;
var $hiddenAction = true;
var ListUserUrl = '../GridListing/JqGridHandler/DARDetailsList.ashx';

function _bind_DatePickers() {
    $("#fromDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '-3m' });
    $("#toDateDAR").datepicker({ dateFormat: 'mm/dd/yy', endDate: new Date, startDate: '0d' });

    var startDate = new Date();
    var FromEndDate = new Date();
    var ToEndDate = new Date();
    ToEndDate.setDate(ToEndDate.getDate());

    $('#fromDateDAR').datepicker({
        weekStart: 1,
        startDate: FromEndDate.setDate(-30),
        endDate: FromEndDate,
        autoclose: true
        })
        .on('changeDate', function (selected) {
            startDate = new Date(selected.date.valueOf());
            startDate.setDate(startDate.getDate(new Date(selected.date.valueOf())));
            $('#toDateDAR').datepicker('setStartDate', startDate);
            $('#fromDateDAR').datepicker('hide');
        });

    $('#toDateDAR')
        .datepicker({
            weekStart: 1,
            startDate: startDate,
            endDate: ToEndDate,
            autoclose: true
        })
        .on('changeDate', function (selected) {
            FromEndDate = new Date(selected.date.valueOf());
            FromEndDate.setDate(FromEndDate.getDate(new Date(selected.date.valueOf())));
            $('#fromDateDAR').datepicker('setEndDate', FromEndDate);
            $('#toDateDAR').datepicker('hide');
        });
};

var EmployeeDDL = '<select id="Employeeddl">'
    + '<option value="0" >All Employee</option>';

$(window).load(function () {
    if ($userType == "1" || $userType == "5" || $userType == "6" || $userType == "2" ) { //Global Admin +IT Administrator
        $hiddenAction = false;
    }
    else {
        $hiddenAction = true;
    }
    var LocationId = $("#drp_MasterLocation").val();

    $.ajax({
        //url: '../GlobalAdmin/GetListITAdministrator',
        //data: { UserType: "All Users" },//JSON.stringify({ UserType :"All Users" }),
        url: '../DropDown/GetEmployeeByLocation',
        data: JSON.stringify({ LocationId: LocationId }),
        async: false,
        type: 'POST',
        contentType: "application/json",
        success: function (result) {
            $.each(result, function (i, v) {
                EmployeeDDL = EmployeeDDL + '<option value="' + v.Value + '">' + v.Text + '</option>';
            });
            EmployeeDDL = EmployeeDDL + '</select>';
        },
        error: function (er) {
        }
    });

    var TaskTypeDll = '<select id="TaskTypeDll">'
        + '<option value="0" >All Work Request Type</option>';
    var downloadDisclaimerDoc = '../eMaintenanceDisclaimer/DARDisclaimerDownload/';
    $(function () {
        $.ajax({
            url: '../Common/GetTaskType',
            data: JSON.stringify({ taskType: "TASKTYPECATEGORY" }),
            async: false,
            type: 'POST',
            contentType: "application/json",
            success: function (result) {
                $.each(result, function (i, v) {
                    TaskTypeDll = TaskTypeDll + '<option value="' + v.Value + '">' + v.Text + '</option>';
                });
                TaskTypeDll = TaskTypeDll + '</select>';
            },
            error: function (er) {
            }
        });

        $("#tbl_DARList").jqGrid({
            url: ListUserUrl,
            datatype: 'json',
            height: 400,
            mtype: 'POST',
            width: 700,
            autowidth: true,
            //colNames: ['Employee Name', 'Activity Details', 'Work Requst Type', 'Start Time', 'End Time', 'Start Time Image', 'End Time Image', 'Submitted On', 'Actions'],
            colNames: ['Employee Name', 'Activity Details', 'Work Request Type', 'Start Time', 'End Time', 'Submitted On','DisclaimerFormFile', 'Actions'],
            colModel: [
                { name: 'Employee Name', width: 65, sortable: false },
                { name: 'Activity Details', width: 180, sortable: false },
                { name: 'Work Requst Type', width: 80, sortable: false },
                { name: 'Start Time', width: 40, sortable: false },
                { name: 'End Time', width: 40, sortable: false },
                //{ name: 'Start Time Image', width: 30, sortable: false, formatter: imageFormat },
                //{ name: 'End Time Image', width: 25, sortable: false, formatter: imageFormat },
                { name: 'Submitted On', width: 65, sortable: false },
                { name: 'DisclaimerFormFile', width: 40, sortable: false, hidden: true}, 
                { name: 'act', index: 'act', width: 35, sortable: false, hidden: $hiddenAction }],
            rownum: 10,
            rowList: [10, 20, 30],
            scrollOffset: 0,
            pager: '#divDARListPager',
            sortname: 'CreatedOn',
            viewrecords: true,
            sortorder: 'desc',
            caption: "List of DAR",
            loadError: function (data) { $('#message').html(JSON.stringify(data) + 'No records found.'); },
            gridComplete: function () {
                $('.clockpicker').each(function () {
                    $(this).clockpicker();
                });
                _bind_DatePickers();
                $('#message').html('');
                var ids = jQuery("#tbl_DARList").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    var rowData = jQuery("#tbl_DARList").getRowData(cl);
                    var DisclaimerFormFile = rowData['DisclaimerFormFile'];
                    var df = "";
                    
                    if (DisclaimerFormFile != null && DisclaimerFormFile != "" && DisclaimerFormFile != '' && DisclaimerFormFile != undefined) {
                        df = '<a href="' + downloadDisclaimerDoc + '?Id=' + cl + '" class="download-cloud" title="Disclaimer" vid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="fa fa-cloud-download fa-2x"></span><span class="tooltips">Disclaimer</span></a></div>';
                    }
                    be = '<div><a href="javascript:void(0)" data-toggle = "modal" data-target = "#myModalmedium" class="EditRecord"  Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
                    
                    if (df != "")
                    {
                        jQuery("#tbl_DARList").jqGrid('setRowData', ids[i], { act: be + df });
                    }
                    else {
                        jQuery("#tbl_DARList").jqGrid('setRowData', ids[i], { act: be });
                    }

                  
                }
                $("#fromDate").datepicker({ dateFormat: 'mm/dd/y', minDate: '-30', maxDate: new Date });
                $("#toDate").datepicker({ dateFormat: 'mm/dd/y', minDate: new Date, maxDate: new Date });
                //Added By Bhushan Dod on 30/03/2015 if record < 20 then pager become hide 
                if ($("#tbl_DARList").getGridParam("records") <= 20) {
                    $("#divDARListPager").hide();
                }
                else {
                    $("#divDARListPager").show();
                }
                if ($('#tbl_DARList').getGridParam('records') === 0) {
                    $('#tbl_DARList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
                }

                if (!($('#txtFromTime').length > 0)) {
                    var label = $("<input type=\"text\" placeholder=\"From Time\" id=\"txtFromTime\" style=\"width:95px;height: 30px; margin - right: 5px; cursor: pointer;\" data-role=\"datebox\" data-options=\'{\"mode\":\"durationflipbox\",\"useFocus\":true,\"overrideDurationOrder\":[\"h\",\"i\"],\"overrideDurationFormat\": \"%Dl:%DM\" }\' readonly=\"readonly\" />");
                    $("#dynamicFromTime").append(label).trigger("create"); // for dymanic creation
                }
                if (!($('#txtToTime').length > 0)) {
                    var label = $("<input type=\"text\" placeholder=\"To Time\" id=\"txtToTime\" style=\"width:90px;height: 30px; margin - right: 5px; cursor: pointer;\" data-role=\"datebox\" data-options=\'{\"mode\":\"durationflipbox\",\"useFocus\":true,\"overrideDurationOrder\":[\"h\",\"i\"],\"overrideDurationFormat\": \"%Dl:%DM\" }\' readonly=\"readonly\" />");
                    $("#dynamicToTime").append(label).trigger("create"); // for dymanic creation
                }
            },
           // caption: '<div class="header_search"><input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><div id="dynamicFromTime" class="dynamic-input"></div><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" ><div id="dynamicToTime"></div>' + TaskTypeDll + '' + EmployeeDDL + '&nbsp;<input type="button" value="Show DAR" class="ViewAllButton" onclick="ViewDARDetails();" title="Click to view user on All Locations."/></div>'
            caption: '<div class="header_search"><input id="fromDateDAR" class="" type="text" value="" style="width:110px;  margin-right: 5px; cursor: pointer;" name="startDate_Check" placeholder="From Date" readonly="readonly"><input type="text" class="clockpicker" id="txtFromTime" placeholder="From Time" style="width:95px;height:30px; margin-right: 5px; cursor: pointer;" data-placement="bottom" data-align="top" data-autoclose="true" readonly="readonly" /><input id="toDateDAR" class="" type="text" value="" style="width:110px; margin-right: 5px; cursor: pointer;" name="endDate_Check" placeholder="To Date" readonly="readonly" ><input type="text" class="clockpicker" id="txtToTime" placeholder="To Time" style="width:90px;height:30px; margin-right: 5px; cursor: pointer;" data-placement="bottom" data-align="top" data-autoclose="true" readonly="readonly" />' + TaskTypeDll + '' + EmployeeDDL + '&nbsp;<input type="button" value="Show DAR" class="ViewAllButton" onclick="ViewDARDetails();" title="Click to view user on All Locations."/>&nbsp;&nbsp;&nbsp;<input type="button" value="Export to Excel" id="export" class="exportToexcelButton" onclick="ViewDARDetailstwo()"/> </div>'

        });
        
        //$("#export").click(function (e) {
        //    debugger
        //    var row = '<tr><th>val</th><th>val2</th></tr>';
        //    var tabledata = jQuery("#tbl_DARList").html().toString();
        //    var $row = $(this).closest('table').children('tr:first');
        //    $(tabledata).append(row);                      
        //    $(tabledata).table2excel({
        //      headings: true,
        //      filename : "DARExcel"
        //    });           
        //   // window.open('data:application/vnd.ms-excel,' + fullData);  //$('#tbl_DARList').html()
        //        //e.preventDefault();
        //})
        //$("#export").on("click", function () {
        //    debugger
        //    $("#tbl_DARList").jqGrid("exportToExcel", {
        //        includeLabels: true,
        //        includeGroupHeader: true,
        //        includeFooter: true,
        //        fileName: "jqGridExport.xlsx",
        //        maxlength: 40 // maxlength for visible string data 
        //    })
        //})
        if ($("#tbl_DARList").getGridParam("records") > 20) {
            jQuery("#tbl_DARList").jqGrid('navGrid', '#divDARListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
        }

    });
});



//#region Image
function imageFormat(cellvalue, options, rowObject) {

    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" onclick="EnlargeImageView(this);"/>';
    }
}
//**********************************************************************************
var names = ["id", "thingy", "blank", "number", "status"];
var mydata = [];
var data = $("#tbl_DARList").jqGrid('getRowData');
for (var i = 0; i < data.length; i++) {
    mydata[i] = {};
    for (var j = 0; j < data[i].length; j++) {
        mydata[i][names[j]] = data[i][j];
    }
}

for (var i = 0; i <= mydata.length; i++) {
    $("#tbl_DARList").jqGrid('addRowData', i + 1, mydata[i]);
}

/*
$("#grid").jqGrid('setGridParam', {onSelectRow: function(rowid,iRow,iCol,e){alert('row clicked');}});
*/
$("#tbl_DARList").jqGrid('setGridParam', { ondblClickRow: function (rowid, iRow, iCol, e) { alert('double clicked'); } });

console.log($("#tbl_DARList").jqGrid('getRowData'));

console.log($("#tbl_DARList").jqGrid('getRowData'));
console.log(JSON.stringify($("#tbl_DARList").jqGrid('getRowData')));

function JSONToCSVConvertor(JSONData, ReportTitle, ShowLabel) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;

    var CSV = '';
    //Set Report title in first row or line

    //CSV += ReportTitle //+ '\r\n\n';

    //This condition will generate the Label/Header
    if (ShowLabel) {
        var row = "";
        //This loop will extract the label from 1st index of on array
        for (var index in arrData[0]) {

            //Now convert each value to string and comma-seprated
            if (index == "act" || index == "DisclaimerFormFile")
            { }
            else
            {
                row += index + ',';                               
            }
        }
        
        row = row.slice(0, -1);
       
        //append Label row with line break
        CSV += row + '\r\n';
    }

    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";

        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            if (index == "act" || index == "DisclaimerFormFile")
            { }
            else
            {               
                row += '"' + arrData[i][index] + '",';
            }
        }

        row.slice(0, row.length - 1);

        //add a line break after each row
        CSV += row ;
    }

    if (CSV == '') {
        alert("Invalid data");
        return;
    }

    //Generate a file name
    var fileName = "MyReport_";
    //this will remove the blank-spaces from the title and replace it with an underscore
    fileName += ReportTitle.replace(/ /g, "_");

    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + escape(CSV);

    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    

    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");
    link.href = uri;

    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";

    //this part will append the anchor tag and remove it after automatic click
    console.log(document.body);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

    function ViewDARDetailstwo()
    {
    console.log('test');
    JSONToCSVConvertor(JSON.stringify($('#tbl_DARList').jqGrid('getRowData')), 'DAR', true);
};
//*************************************************************************************
//#endregion
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    if (timeoutHnd)
        clearTimeout(timeoutHnd)
    timeoutHnd = setTimeout(gridReload, 500)
}
function gridReload() {
    var fromDate = $("#fromDateDAR").val() + " " + $("#txtFromTime").val();
    var toDate = jQuery("#toDateDAR").val() + " " + $("#txtToTime").val();
    var empId = $("#Employeeddl").val();
    var taskType = $("#TaskTypeDll").val();
    jQuery("#tbl_DARList").jqGrid('setGridParam', { url: ListUserUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&taskType=" + taskType + "&empId=" + empId, page: 1 }).trigger("reloadGrid");
}
//function ExportExcel() {
//    debugger
//    $("#jqGrid").jqGrid("exportToExcel", {
//        includeLabels: true,
//        includeGroupHeader: true,
//        includeFooter: true,
//        fileName: "jqGridExport.xlsx",
//        maxlength: 40 // maxlength for visible string data 
//    })
//}
//#endregion
function EnlargeImageView(value) {
    $("#myAssignedWorkOrder").modal('show');
    $("#myAssignedWorkOrder").find("#myModalLabel").html("");
    var path = $(value).attr("src");
    var data = "<img src='" + path + "' style='width:100%;height:100%;' />";
    $("#myAssignedWorkOrder").find("#AssignedWorkorderBody").html(data);
}
function ViewDARDetails() {
    var fromDate = $("#fromDateDAR").val() + " " + $("#txtFromTime").val();;
    var toDate = jQuery("#toDateDAR").val() + " " + $("#txtToTime").val();;
    var empId = $("#Employeeddl").val();
    var taskType = $("#TaskTypeDll").val();
    jQuery("#tbl_DARList").jqGrid('setGridParam', { url: ListUserUrl + "?fromDate=" + fromDate + "&toDate=" + toDate + "&taskType=" + taskType + "&empId=" + empId, page: 1 }).trigger("reloadGrid");
}

$(".EditRecord").live("click", function (event) {
    var id = $(this).attr("Id");
    $('#mediumeditpopup').load("../DAR/EditDARDetail", { 'id': id }
        , function (e) {
            $('.modal-title').text("Edit DAR Details");
            //window.location.href="../"

        });
});

function success(e) {
    $("#myModalmedium").modal("hide");
    ViewDARDetails();
}