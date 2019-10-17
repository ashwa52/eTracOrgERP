var HOBurl = '../NewAdmin/GetListOf306090ForJSGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#List306090").jsGrid({
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
                    url: HOBurl + '?locationId=' + $_LocationId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "EMP_Photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            { name: 'EmployeeName', width: 80, title: "Employee Name", },
            { name: 'JBT_JobTitle', width: 60, title: "User Type" },
            {
                name: 'Status', width: 60, title: "Status", itemTemplate: function (value, item) {
                    return $("<div>").append("Assessment Pending</div>");
                }
            },

            {
                name: 'Assesment', width: 60, title: "30-60-90", itemTemplate: function (value, item) {
                    return $("<div>").append("30</div>");
                }
            },

                    {
                        name: "UserTask", title: "User Task", width: 60,  itemTemplate: function (value, item) {
                            var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });

                            var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                  .attr({ title: jsGrid.fields.control.prototype.profileButtonTooltip })
                                  .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                                      debugger;
                                      $.ajax({
                                          type: "GET",
                                          data: { 'Id': item.EMP_EmployeeID, 'Assesment': item.Assesment},
                                          url: '../NewAdmin/userAssessmentView/',
                                          contentType: "application/json; charset=utf-8",
                                          error: function (xhr, status, error) {
                                          },
                                          success: function (result) {
                                              debugger
                                              if (result != null)
                                              {
                                                  $("#gridArea").hide();
                                                  $('#profileArea').show();
                                                  $('#profileArea').html(result);
                                              }
                                          }
                                      });
                                      
                                      
                                  }).append($iconUserView);

                            var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                                 .attr({ id: "btn-status-" + item.id }).click(function (e) {

                                     
                                 }).append($iconText);
                            return $("<div>").attr({ class: "btn-toolbar" }).append($customUserViewButton).append($customTextButton).append($customTextButton);
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