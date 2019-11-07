var HOBurl = '../ePeople/GetListHolidayJSGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#tbl_AllHolidayList").jsGrid({
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
            { name: 'SrNo', width: 40, title: "SrNo" },
            { name: 'Id', width: 40, title: "Id", visible: false },
            {
                name: 'HolidayDateString', width: 60,title: "Holiday Date",
                //{ name: 'TESTANDO_DATA', index: 'TESTANDO_DATA', width: 100, formatter: 'date', formatoptions: { srcformat: "Y-m-d", newformat: "d/m/Y" } }
                //itemTemplate: function (val, item) {
                //    return $("<label>" + val + "<br/>" + item.Description + "</label>");
                //}
            },
            { name: 'HolidayName', width: 80, title: "Holiday Name" },
            { name: 'Description', width: 100, title: "Description" },
            { name: 'IsActive', width: 60, type: "checkbox", title: "Actives" },
            {
                name: "FinYear", title: "Edit", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../ePeople/HolidayMasterAddEdit/" + item.Id });
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            },
            {
                name: "HolidayName", title: "Delete", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../ePeople/HolidayMasterAddDelete/" + item.Id });
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));

                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            }
        ]
    });
})(jQuery);

var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#tbl_AllHolidayList").jsGrid({
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
            { name: 'SrNo', width: 40, title: "SrNo" },
            { name: 'Id', width: 40, title: "Id", visible: false },
            {
                name: "HolidayDateString", width: 80, title: "Holiday Date",
                //itemTemplate: function (val, item) {
                //    return $("<label>" + val + "<br/>" + item.Description + "</label>");
                //}
            },
            { name: 'HolidayName', width: 80, title: "Holiday Name" },
            { name: 'Description', width: 100, title: "Description" },
            { name: 'IsActive', width: 60, type: "checkbox", title: "Actives" },
            {
                name: "FinYear", title: "Action", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../ePeople/HolidayMasterAddEdit/" + item.Id })
                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-edit fa-2x" }).attr({ style: "color:green;" }));
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            },
            {
                name: "HolidayName", title: "Action", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    var $EditLink = $("<a>").attr({ href: "../ePeople/HolidayMasterAddDelete/" + item.Id })

                    var $DiagramView = $EditLink.append($("<i>").attr({ class: "fa fa-trash fa-2x" }).attr({ style: "color:green;" }));

                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                }
            }
        ]
    });
}