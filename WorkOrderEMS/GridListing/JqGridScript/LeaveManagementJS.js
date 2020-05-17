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
            { name: 'Id', width: 10, title: "Id", visible: false },
            { name: 'EmployeeId', width: 10, title: "Employee Id", visible: false },
            { name: 'EmployeeName', width: 50, title: "Employee Name" },
            { name: 'FromDateString', width: 30, title: "From Date" },
            { name: 'ToDateString', width: 30, title: "To Date" },
            { name: 'LeaveType', width: 40, title: "Leave Type" },
            { name: 'LeaveReason', width: 100, title: "Leave Reason" },
            {
                name: 'Status', width: 30, title: "Status", itemTemplate: function (value, item) {
                    if (item.Status == "Rejected") {
                        return "<div style='color:red'><b>" + value + "</b></div>";
                    }
                    if (item.Status == "Approved") {
                        return "<div style='color:green'><b>" + value + "</b></div>";
                    }
                    return value;
                } },
            //{ name: 'IsActive', width: 40, type: "checkbox", title: "Actives" },
            //{ name: 'IsApproved', width: 40, type: "checkbox", title: "Approved" },
            //{ name: 'IsRejected', width: 40, type: "checkbox", title: "Rejected" },
            //{
            //    name: "EmployeeId", title: "Actions", width: 35, css: "text-center", itemTemplate: function (value, item) {
            //        var $EditLink1 = $("<a>").attr({ href: "../ePeople/LeaveManagementAddEdit/" + item.Id });
            //        var $DiagramView1 = $EditLink1.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
            //        var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementDelete/" + item.Id });
            //        var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));
            //        return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView1).append($DiagramView);
            //    }
            //},
            {
                name: "act", type: "control", items: act, title: "Action", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-edit" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForDelete = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:green;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 20px 0 10px;'>")
                        .attr({ title: "Edit" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            EditItem(item.Id)
                        }).append($iconPencilForEdit);
                    var $customDeleteButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "Delete" })
                        .attr({ id: "btn-delete-" + item.Id }).click(function (e) {
                            DeleteItem(item.Id)
                        }
                        ).append($iconPencilForDelete);
                    if (item.Status == "Pendding") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton);
                    }
                    return $("<div>").attr({ class: "btn-toolbar" });
                }
            }
            //{
            //    name: "IsApproved", title: "Approved", width: 40, css: "text-center", itemTemplate: function (value, item) {
            //        if (item.IsApproved == false && item.IsRejected == false) {
            //            var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementApproved/" + item.Id });
            //            var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-thumbs-up fa-2x" }).attr({ style: "color:green;" }));
            //            return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
            //        }
            //    }
            //},
            //{
            //    name: "IsRejected", title: "Rejected", width: 35, css: "text-center", itemTemplate: function (value, item) {
            //        if (item.IsRejected == false && item.IsApproved == false) {
            //            return "<a><input class=\"jsgrid-button jsgrid-delete-button\" type=\"button\" data-toggle=\"modal\" title=\"Edit\" data-target=\"#myModalForAssignEmployeeData\" data-id=\"" + item.Id + "\" style=\"cursor:pointer;outline:none;\" onclick=\"OnClickValues(" + item.Id + ")\"></a>";
            //            }
            //    },
            //    width: "30px",
            //    sortable: false,
            //}
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
            { name: 'EmployeeId', width: 40, title: "Employee Id", visible: false },
            { name: 'EmployeeName', width: 40, title: "Employee Name" },
            { name: 'FromDateString', width: 40, title: "From Date" },
            { name: 'ToDateString', width: 40, title: "To Date" },
            { name: 'LeaveType', width: 40, title: "Leave Type" },
            { name: 'LeaveReason', width: 90, title: "Leave Reason" },
            {
                name: 'Status', width: 30, title: "Status", itemTemplate: function (value, item) {
                    if (item.Status == "Rejected") {
                        return "<div style='color:red'><b>" + value + "</b></div>";
                    }
                    if (item.Status == "Approved") {
                        return "<div style='color:green'><b>" + value + "</b></div>";
                    }
                    return value;
                }
            },
            //{ name: 'IsActive', width: 40, type: "checkbox", title: "Actives" },
            //{ name: 'IsApproved', width: 40, type: "checkbox", title: "Approved" },
            //{ name: 'IsRejected', width: 40, type: "checkbox", title: "Rejected" },
            {
                name: "EmployeeId", title: "Actions", width: 40, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink1 = $("<a>").attr({ href: "../ePeople/LeaveManagementAddEdit/" + item.Id });
                    var $DiagramView1 = $EditLink1.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementDelete/" + item.Id });
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));
                    if (item.Status == "Pendding") {
                        return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView1).append($DiagramView);
                    }
                    return $("<div>").attr({ class: "btn-toolbar" });
                }
            }
            //{
            //    name: "IsApproved", title: "Approved", width: 30, css: "text-center", itemTemplate: function (value, item) {
            //        if (item.IsApproved == false && item.IsRejected == false) {
            //            var $EditLink = $("<a>").attr({ href: "../ePeople/LeavemanagementApproved/" + item.Id });
            //            var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-thumbs-up fa-2x" }).attr({ style: "color:green;" }));
            //            return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
            //        }
            //    }
            //},
            //{
            //    name: "IsRejected", title: "Rejected", width: 35, css: "text-center", itemTemplate: function (value, item) {
            //        if (item.IsRejected == false && item.IsApproved == false) {
            //            return "<a><input class=\"jsgrid-button jsgrid-delete-button\" type=\"button\" data-toggle=\"modal\" title=\"Edit\" data-target=\"#myModalForAssignEmployeeData\" data-id=\"" + item.Id + "\" style=\"cursor:pointer;outline:none;\" onclick=\"OnClickValues(" + item.Id + ")\"></a>";
            //        }
            //    },
            //    width: "30px",
            //    sortable: false,
            //}
        ]
    });
}

function EditItem(Id) {
    event.preventDefault();
    var addNewUrl = "/EPeople/LeaveManagementAddEdit?Id=" + Id;
    $('#RenderPageId').load(addNewUrl);
}

function DeleteItem(Id) {
    var addNewUrl = "/EPeople/LeavemanagementDelete?Id=" + Id;
    $('#RenderPageId').load(addNewUrl);
}