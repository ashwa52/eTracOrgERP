var HOBurl = '../ePeople/GetListLeaveManagementJSGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    var act;
    $("#tbl_AllLeaveList").jsGrid({
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
            { name: 'Id', width: 40, title: "Id", visible: false },
            { name: 'EmployeeId', width: 40, title: "Employee Id" },
            { name: 'FromDateString', width: 40, title: "From Date" },
            { name: 'ToDateString', width: 40, title: "To Date" },
            { name: 'LeaveType', width: 40, title: "Leave Type" },
            { name: 'LeaveReason', width: 90, title: "Leave Reason" },
            //{ name: 'IsActive', width: 40, type: "checkbox", title: "Actives" },
            { name: 'IsApproved', width: 40, type: "checkbox", title: "Approved" },
            { name: 'IsRejected', width: 40, type: "checkbox", title: "Rejected" },
            {
                
                name: "EmployeeId", title: "Actions", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink1 = $("<a>").attr({ href: "../ePeople/LeaveManagementAddEdit/" + item.Id });
                    var $DiagramView1 = $EditLink1.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementDelete/" + item.Id });
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView1).append($DiagramView);
                }
            },
            {
                name: "IsApproved", title: "Approved", width: 40, css: "text-center", itemTemplate: function (value, item) {
                    if (item.IsApproved == false && item.IsRejected == false) {
                        var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementApproved/" + item.Id });
                        var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-thumbs-up fa-2x" }).attr({ style: "color:green;" }));
                        return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                    }
                }
            },
            {
                name: "IsRejected", title: "Rejected", width: 35, css: "text-center", itemTemplate: function (value, item) {
                    if (item.IsRejected == false && item.IsApproved == false) {
                        return "<a><input class=\"jsgrid-button jsgrid-delete-button\" type=\"button\" data-toggle=\"modal\" title=\"Edit\" data-target=\"#myModalForAssignEmployeeData\" data-id=\"" + item.Id + "\" style=\"cursor:pointer;outline:none;\" onclick=\"OnClickValues(" + item.Id + ")\"></a>";
                        }
                },
                width: "30px",
                sortable: false,
            }
            ]
        });
})(jQuery);
function OnClickValues(id) {
    $("#IdHid").val(id);
}
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#tbl_AllLeaveList").jsGrid({
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
                    url: HOBurl + '?Search=' + $("#SearchText").val(),
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
            { name: 'Id', width: 40, title: "Id", visible: false },
            { name: 'EmployeeId', width: 40, title: "Employee Id" },
            { name: 'FromDateString', width: 40, title: "From Date" },
            { name: 'ToDateString', width: 40, title: "To Date" },
            { name: 'LeaveType', width: 40, title: "Leave Type" },
            { name: 'LeaveReason', width: 90, title: "Leave Reason" },
            //{ name: 'IsActive', width: 40, type: "checkbox", title: "Actives" },
            { name: 'IsApproved', width: 40, type: "checkbox", title: "Approved" },
            { name: 'IsRejected', width: 40, type: "checkbox", title: "Rejected" },
            {
                name: "EmployeeId", title: "Actions", width: 40, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink1 = $("<a>").attr({ href: "../ePeople/LeaveManagementAddEdit/" + item.Id });
                    var $DiagramView1 = $EditLink1.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementDelete/" + item.Id });
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView1).append($DiagramView);
                }
            },
            {
                name: "IsApproved", title: "Approved", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    if (item.IsApproved == false && item.IsRejected == false) {
                        var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementApproved/" + item.Id });
                        var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-thumbs-up fa-2x" }).attr({ style: "color:green;" }));
                        return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                    }
                }
            },
            {
                name: "IsRejected", title: "Rejected", width: 35, css: "text-center", itemTemplate: function (value, item) {
                    if (item.IsRejected == false && item.IsApproved == false) {
                        return "<a><input class=\"jsgrid-button jsgrid-delete-button\" type=\"button\" data-toggle=\"modal\" title=\"Edit\" data-target=\"#myModalForAssignEmployeeData\" data-id=\"" + item.Id + "\" style=\"cursor:pointer;outline:none;\" onclick=\"OnClickValues(" + item.Id + ")\"></a>";
                    }
                },
                width: "30px",
                sortable: false,
            }
        ]
    });
}