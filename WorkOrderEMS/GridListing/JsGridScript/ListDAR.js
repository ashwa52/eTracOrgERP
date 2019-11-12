var DARurl = '../NewAdmin/GeDARList';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
var FromDate = "", ToDate = "", FromTime = "", ToTime = "", TastType="";
$("#ShowDAR").click(function () {
    debugger
    FromDate = $("#FromDate").val();
    ToDate = $("#ToDate").val();
    FromTime = $("#FromTime").val();
    ToTime = $("#ToTime").val();
    $("#ListDAR").jsGrid("loadData");
});
(function ($) {
    'use strict'
    var data;
    $("#ListDAR").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        inserting: true,
        editing: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        loadMessage: "Please, wait...",
        controller: {
            loadData: function (filter) {
                debugger
                return $.ajax({
                    type: "GET",
                    url: DARurl + '?LocationId=' + $("#drp_MasterLocation1 option:selected").val() + '&TastType=' + TastType + '&FromDate=' + FromDate + '&ToDate=' + ToDate + '&FromTime=' + FromTime + '&ToTime=' + ToTime,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        //onDataLoading: function (args) {
        //    return $.ajax({
        //        type: "GET",
        //        url: DARurl + '?LocationId=' + $_LocationId + '&TastType=' + 0 + '&FromDate' + FromDate + '&ToDate' + ToDate + '&FromTime' + FromTime + '&ToTime' + ToTime,
        //        datatype: 'json',
        //        contentType: "application/json",
        //    });
        //},
        //data: response,
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            { name: 'EmployeeName', width: 160, title: "Employee Name", css: "text-center" },//visible: true
            { name: 'ActivityDetails', width: 210, title: "Activity Details" },
                    { name: "TaskType", width: 150, title: "Work Request Type" },
                    { name: "StartTime", width: 120, title: "Start Time" },
                    { name: "EndTime", width: 120, title: "End Time" },
                     { name: "CreatedOn", width: 120, title: "Submitted On" }
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });
})(jQuery);
$(document).ready(function () {
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();
        $("#ListQRC").jsGrid("loadData");
    })
});