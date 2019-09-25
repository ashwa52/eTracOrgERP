var HOBurl = '../GlobalAdmin/GetListITAdministratorForJSGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#ListEmployeeManagement").jsGrid({
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
                name: "ProfileImage", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            { name: 'Name', width: 80, title: "Employee Name", },//visible: true
            { name: 'UserType', width: 60, title: "User Type" },
                    {
                        name: "UserTask", title: "User Task", width: 60,  itemTemplate: function (value, item) {
                            var $iconFolder = $("<span>").append('<i class= "fa fa-folder-open fa-2x" style="color:yellow;margin-left: 6px;margin-top: 4px;" ></i>');// $("<span>").attr({ class: "fa fa-folder-open fa-2x" }).attr({ style: "color:yellow;background-color:green;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconUserView = $("<span>").append('<i class= "fa fa-user fa-2x" style="color:black;margin-left: 6px;margin-top: 4px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconTransfer = $("<span>").append('<i class= "fa fa-file fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;"></i>');//attr({ class: "fa fa-file fa-2x" }).attr({ style: "color:white;background-color:#D26C36;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconText = $("<span>").append('<i class= "fa fa-file-text fa-2x" style="color:white;margin-left: 6px;margin-top: 4px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $customEditButton = $("<span style='background: green; width: 35px; height: 35px;border-radius: 35px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.listEmployeeTooltip })
                                .attr({ id: "btn-file-" + item.id }).click(function (e) {
                                    $("#myModalForAddEmployee").modal("show");
                                    e.stopPropagation();
                                }).append($iconFolder);
                            var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                  .attr({ title: jsGrid.fields.control.prototype.profileButtonTooltip })
                                  .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                                      
                                      $.ajax({
                                          type: "GET",
                                          data: { 'Id': item.id, 'LocationId': $("#drp_MasterLocation1 option:selected").val() },
                                          url: '../EPeople/GetUserHeirachyList/',
                                          contentType: "application/json; charset=utf-8",
                                          error: function (xhr, status, error) {
                                          },
                                          success: function (result) {
                                              debugger
                                              if (result != null)
                                              {
                                                  var bodyId = $('#viewProfileData').append('<div class=div_' + result + '><img src='+result.ProfileImage+'/></div><br>');
                                                  if(result.ChildrenList.legth > 0)
                                                  {
                                                      for(var i=0;i<result.ChildrenList.legth;i++){
                                                          bodyId.append('<span><img src='+result.ChildrenList[i].ProfileImage+'</span>')
                                                      }
                                                     
                                                  }
                                                  $("#ModalProfilePreview").modal('show');
                                              }
                                          }
                                      });
                                      
                                      
                                  }).append($iconUserView);
                            var $customTransferButton = $("<span style='background: #D26C36; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                 .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                                 }).append($iconTransfer);
                            var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                                 .attr({ id: "btn-status-" + item.id }).click(function (e) {

                                     
                                 }).append($iconText);
                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customUserViewButton).append($customTransferButton).append($customTextButton).append($customTextButton);
                        }
                    },
                       {
                           name: "ListView", title: "List View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                               var $ListView = $("<span>").attr({ class: "fa fa-list-ol fa-2x" }).attr({ style: "color:gray;" });
                               
                               var $customEditButton = $("<span>")
                                   .attr({ title: jsGrid.fields.control.prototype.listEmployeeTooltip })
                                   .attr({ id: "btn-list-" + item.Id }).click(function (e) {
                                       $("#myModalForAddEmployee").modal("show");
                                       e.stopPropagation();
                                   }).append($ListView);
                               return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                           }
                       },
                       {
                           name: "DiagramView", title: "Diagram View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                               var $DiagramView = $("<span>").attr({ class: "fa fa-sitemap fa-2x" }).attr({ style: "color:green;" });
                               
                               var $customEditButton = $("<span>")
                                   .attr({ title: jsGrid.fields.control.prototype.listEmployeeTooltip })
                                   .attr({ id: "btn-diagram-" + item.Id }).click(function (e) {
                                       $("#myModalForAddEmployee").modal("show");
                                       e.stopPropagation();
                                   }).append($DiagramView);
                               return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                           }
                       },
                        {
                            name: "UserView", title: "User View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                                var $UserView = $("<span>").attr({ class: "fa fa-user-circle fa-2x" }).attr({ style: "color:black;"});
                            
                                var $customEditButton = $("<span>")
                                    .attr({ title: jsGrid.fields.control.prototype.userEmployeeTooltip })
                                    .attr({ id: "btn-user-view" + item.Id }).click(function (e) {
                                        $("#myModalForAddEmployee").modal("show");
                                        e.stopPropagation();
                                    }).append($UserView);
                                return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
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