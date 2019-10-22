var RequisitonList = '../NewAdmin/RequisitionList';

(function ($) {
    'use strict'
    var data;
    $("#ListRquisitionData").jsGrid({
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
                    url: RequisitonList + '?LocationId=' + $("#drp_MasterLocation1 option:selected").val(),
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
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