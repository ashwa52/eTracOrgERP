var HOBurl = '../HirinngOnBoarding/GetHiringOnBoardingList';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

let details = [
        { "EmployeeName": "Otto Clay", "MobileNo": "8665543669", "EmailId": "OttoClay@yopmail.com", "Date": "24/09/2019" },
        { "EmployeeName": "Connor Johnston", "MobileNo": "8665543669", "EmailId": "ConnorJohnston@yopmail.com", "Date": "24/09/2019" },
        { "EmployeeName": "Lacey Hess", "MobileNo": "8665543669", "EmailId": "LaceyHess@yopmail.com", "Date": "24/09/2019" },
        { "EmployeeName": "Timothy Henson", "MobileNo": "8665543669", "EmailId": "TimothyHenson@yopmail.com", "Date": "24/09/2019" },
        { "EmployeeName": "Thomas Hardy", "MobileNo": "8665543669", "EmailId": "ThomasHardy@yopmail.com", "Date": "24/09/2019" },
];
(function ($) {
    'use strict'
    var data;
    $("#ListOnBoarding").jsGrid({
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
        //controller: {
        //    loadData: function (filter) {
        //        debugger
        //        return $.ajax({
        //            type: "GET",
        //            url: HOBurl + '?locationId=' + $_LocationId ,
        //            datatype: 'json',
        //            contentType: "application/json",
        //        });
        //    }
        //},
        //onDataLoading: function (args) {
        //    return $.ajax({
        //        type: "GET",
        //        url: QRCurl + '?locationId=' + $_LocationId ,
        //        datatype: 'json',
        //        contentType: "application/json",
        //    });
        //},
        data: details,
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            { name: 'EmployeeName', width: 160, title: "Employee Name", css: "text-center" },//visible: true
            { name: 'MobileNo', width: 150, title: "Mobile No" },
                    { name: "EmailId", width: 150, title: "E-mail" },
                    { name: "Date", width: 150, title: "Date" },
                    {
                        name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                            
                            var $iconPencil = $("<input>").attr({ type: "button" }).attr({ class: "btn btn-primary" }).attr({ value: "Add", id: "AddEmployee" }).attr({ style: "color:white;background-color:green;margin-left:20px;border-radius:35px;" });
                            var $iconTrash = $("<input>").attr({ type: "button" }).attr({ class: "btn btn-primary" }).attr({ value: "Verify" }).attr({ style: "color:white;background-color:gray;margin-left:20px;border-radius:35px;" });;
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                    $("#myModalForAddEmployee").modal("show");
                                    e.stopPropagation();
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                  .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                  .attr({ id: "btn-delete-" + item.Id }).click(function (e) {
                                      $.ajax({
                                          type: "POST",
                                          url: "../GlobalAdmin/DeleteLocation?id=" + item.Id,
                                          success: function (Data) {
                                              //$("#jsGrid-basic").jsGrid("loadData");
                                              var addNewUrl = "../GlobalAdmin/ListLocation";
                                              $('#RenderPageId').load(addNewUrl);
                                          },
                                          error: function (err) {
                                          }

                                      });

                                      e.stopPropagation();
                                  }).append($iconTrash);

                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton);
                        }
                    },
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