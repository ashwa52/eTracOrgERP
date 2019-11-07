var HOBurl = '../HRMS/GetListShiftJSGrid';
var Search = '../HRMS/GetListShiftJSGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;
(function ($) {
    'use strict'
    var data;
    $("#Tbl_HRMS_Shift").jsGrid({
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

                return $.ajax({
                    type: "GET",
                    url: HOBurl,
                    datatype: 'json',
                    contentType: "application/json"
                    
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            { name: 'Id', width: 10, title: "Id", },//visible: true
            { name: 'ShiftCode', width: 40, title: "Shift Code" },
            { name: 'ShiftName', width: 100, title: "Shift Name" },
            { name: 'Description', width: 100, title: "Description" },
            { name: 'StartTime', width: 30, title: "StartTime" },
            { name: 'EndTime', width: 30, title: "EndTime" },
            { name: 'IsActive', width: 30, title: "Active", type: "checkbox" },
            {
                name: "FinYear", title: "Action", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../HRMS/ShiftMasterAddEdit/" + item.Id })
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            },
        ]
    });
})(jQuery);

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#Tbl_HRMS_Shift").jsGrid({
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
                return $.ajax({
                    type: "GET",
                    url: HOBurl,
                    url: Search + '?Search=' + $("#SearchText").val(),
                    datatype: 'json',
                    contentType: "application/json"
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            { name: 'Id', width: 80, title: "Id", },//visible: true
            { name: 'ShiftCode', width: 60, title: "Shift Code" },
            { name: 'ShiftName', width: 60, title: "Shift Name" },
            { name: 'Description', width: 60, title: "Description" },
            { name: 'StartTime', width: 30, title: "StartTime" },
            { name: 'EndTime', width: 30, title: "EndTime" },
            { name: 'IsActive', width: 60, title: "Active", type: "checkbox" },
            {
                name: "FinYear", title: "Action", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../HRMS/ShiftMasterAddEdit/" + item.Id })
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            },
        ]
    });
}