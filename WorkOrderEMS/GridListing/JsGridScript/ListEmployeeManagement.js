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
                            var $iconFolder = $("<span>").append('<i class= "fa fa-folder-open" style="color:yellow;margin-left: 11px;margin-top: 6px;" ></i>');// $("<span>").attr({ class: "fa fa-folder-open fa-2x" }).attr({ style: "color:yellow;background-color:green;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconUserView = $("<span>").append('<i class= "fa fa-user" style="color:black;margin-left: 11px;margin-top: 6px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconTransfer = $("<span>").append('<i class= "fa fa-file" style="color:white;margin-left: 11px;margin-top: 6px;"></i>');//attr({ class: "fa fa-file fa-2x" }).attr({ style: "color:white;background-color:#D26C36;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconText = $("<span>").append('<i class= "fa fa-file-text" style="color:white;margin-left: 11px;margin-top: 6px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $customEditButton = $("<span style='background: green; width: 35px; height: 35px;border-radius: 35px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.listEmployeeTooltip })
                                .attr({ id: "btn-file-" + item.id }).click(function (e) {
                                    $("#myModalForAddEmployee").modal("show");
                                    e.stopPropagation();
                                }).append($iconFolder);
                            var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                  .attr({ title: jsGrid.fields.control.prototype.profileButtonTooltip })
                                  .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                                      
                                      //$.ajax({
                                      //    type: "GET",
                                      //    data: { 'Id': item.id, 'LocationId': $("#drp_MasterLocation1 option:selected").val() },
                                      //    url: '../EPeople/GetUserHeirachyList/',
                                      //    contentType: "application/json; charset=utf-8",
                                      //    error: function (xhr, status, error) {
                                      //    },
                                      //    success: function (result) {
                                      //        debugger
                                      //        if (result != null)
                                      //        {
                                      //            var bodyId = $('#viewProfileData').append('<div class=div_' + result + '><img src='+result.ProfileImage+'/></div><br>');
                                      //            if(result.ChildrenList.legth > 0)
                                      //            {
                                      //                for(var i=0;i<result.ChildrenList.legth;i++){
                                      //                    bodyId.append('<span><img src='+result.ChildrenList[i].ProfileImage+'</span>')
                                      //                }
                                                     
                                      //            }
                                      //            $("#ModalProfilePreview").modal('show');
                                      //        }
                                      //    }
                                      //});
                                      $("#ModalProfilePreview").modal('show');
                                      
                                  }).append($iconUserView);
                            var $customTransferButton = $("<span style='background: #D26C36; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                 .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                                 }).append($iconTransfer);
                            var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                                 .attr({ id: "btn-status-" + item.id }).click(function (e) {
                                     debugger
                                     $("#myModalForChangeStatusData").modal('show');
                                     
                                 }).append($iconText);

                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customUserViewButton).append($customTransferButton).append($customTextButton).append($customTextButton);
                        }
                    },
                       {
                           name: "ListView", title: "List View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                               var $ListView = $("<span>").attr({ class: "fa fa-list-ol fa-2x" }).attr({ style: "color:gray;" });
                               
                               var $customEditButton = $("<span>")
                                   .attr({ title: jsGrid.fields.control.prototype.listEmployeeTooltip })
                                   .attr({ id: "btn-list-" + item.id }).click(function (e) {
                                       debugger
                                       $.ajax({
                                          type: "GET",
                                          data: { 'Id': item.id, 'LocationId': $("#drp_MasterLocation1 option:selected").val() },
                                          url: '../EPeople/GetUserListByUserId/',
                                          contentType: "application/json; charset=utf-8",
                                          error: function (xhr, status, error) {
                                          },
                                          success: function (result) {
                                              debugger
                                              if (result != null)
                                              {
                                                  $("#ViewUserListDetails").html(result);
                                                  $("#myModalForUserList").modal("show");
                                                  //var bodyId = $('#viewProfileData').append('<div class=div_' + result + '><img src='+result.ProfileImage+'/></div><br>');
                                                  //if(result.ChildrenList.legth > 0)
                                                  //{
                                                  //    for(var i=0;i<result.ChildrenList.legth;i++){
                                                  //        bodyId.append('<span><img src='+result.ChildrenList[i].ProfileImage+'</span>')
                                                  //    }                                                     
                                                  //}
                                                  //$("#ModalProfilePreview").modal('show');
                                              }
                                          }
                                      });
                                       
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
                                   .attr({ id: "btn-diagram-" + item.id }).click(function (e) {
                                       debugger
                                       //$("#myModalForViewVCS").modal("show");
                                       var ChartElement = document.getElementById("containerForViewChart");
                                       $.ajax({
                                           type: "POST",
                                           url: "../AdminSection/AdminDashboard/GetChartDisplayData",
                                           success: function (listData) {
                                               debugger
                                               var tabledata = [];
                                               if (listData != null) {
                                                   for (var i = 0; i < listData.length; i++) {
                                                       var data = {};
                                                       if (i == 0) {
                                                           listData[i].parentId = null;
                                                       }
                                                       data.id = listData[i].Id;
                                                       data.parentId = listData[i].parentId;
                                                       data.SeatingName = listData[i].SeatingName;
                                                       data.JobDescription = listData[i].JobDescription;
                                                       data.DepartmentName = listData[i].DepartmentName;
                                                       tabledata.push(data);
                                                   }
                                                   var orgChart = new getOrgChart(ChartElement, {
                                                       primaryFields: ["SeatingName", "JobDescription", "DepartmentName"],
                                                       dataSource: tabledata,
                                                   });
                                                   $(".get-text-0").attr("y", "30"); $(".get-text-0").attr("y", "30");
                                                   $(".get-text-1").attr("y", "65"); $(".get-text-2").attr("y", "90");
                                                   $(".get-text-3").attr("y", "120"); $(".get-text-4").attr("y", "150");
                                                   $(".get-text-4").attr("y", "180"); $(".get-text-6").attr("y", "210");
                                                   $(".get-text-7").attr("y", "240"); $(".get-text-8").attr("y", "270");
                                                   $(".get-text-9").attr("y", "300"); $(".get-text-10").attr("y", "330");
                                               }
                                               debugger
                                               $.ajax({
                                                   type: "POST",
                                                   url: "../EPeople/GetVehicleSeatingChartPositionedUser?Id=" + item.id,
                                                   success: function (data) {
                                                       debugger
                                                       $("#myModalForViewVCSData").modal("show");
                                                       if (data != null) {

                                                       }
                                                   },
                                                   error: function (err) {
                                                   }
                                               });
                                           },
                                           error: function (err) {
                                           }
                                       });
                                   }).append($DiagramView);
                               return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                           }
                       },
                        {
                            name: "UserView", title: "User View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                                var $UserView = $("<span>").attr({ class: "fa fa-user-circle fa-2x" }).attr({ style: "color:black;"});
                            
                                var $customEditButton = $("<span>")
                                    .attr({ title: jsGrid.fields.control.prototype.userEmployeeTooltip })
                                    .attr({ id: "btn-user-view" + item.id }).click(function (e) {
                                        debugger
                                        ViewTreeUserData(item.id);
                                       
                                        //$("#myModalForViewUserChart").modal("show");
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
function ViewTreeUserData(id) {
    debugger
    $.ajax({
        type: "POST",
        data: { 'Id': id },
        url: '../EPeople/GetUserTreeViewList/',
        //contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (ChildrenList) {
            debugger
            $(".chartRemove").html("");
            if (ChildrenList != null) {
                debugger
                if (ChildrenList.length > 0) {
                    for (var i = 0; i < ChildrenList.length; i++) {
                        debugger
                        if (i == 0) {
                            //var bodyId = $('.chartlevel1').append('<div class="stiff-main-parent"><ul><li data-parent="a"> <div class="the-chart"> <img src="https://placeimg.com/100/100/animals" alt=""><p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li></ul></div>');
                            var bodyId = $('.chartlevel1').append('<div class="stiff-main-parent"><ul><li data-parent="a"><div class="the-chart"><img src="' + ChildrenList[i].ProfilePhoto + '" alt=""><p>Parent</p></div></li></ul></div>');
                            //var body = ' <div class="stiff-chart-level chartlevel2" data-level="02"><div class="stiff-child" data-child-from="a"><ul>'
                            var bodyInnerDiv = '<div class="stiff-chart-level chartRemove chartlevel2" data-level="02"><div class="stiff-child" data-child-from="a"><ul>';
                        }
                       // var bodyInnerDiv = '<div class="stiff-chart-level chartRemove chartlevel2" data-level="02"><div class="stiff-child" data-child-from="a"><ul><li data-parent="a01"><div class="the-chart"><img src="https://placeimg.com/100/100/animals" alt=""><p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li><li data-parent="a02"><div class="the-chart"><img src="https://placeimg.com/100/100/animals" alt=""><p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li><li data-parent="a03"><div class="the-chart"><img src="https://placeimg.com/100/100/animals" alt=""><p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li></ul> </div></div>';
                        else {
                            var plus = i + 1;
                            bodyInnerDiv = bodyInnerDiv + '<li data-parent="a0' + i + '" data-employeeId="' + ChildrenList[i].EmployeeId + '"><div class="the-chart"><img src="' + ChildrenList[i].ProfilePhoto + '" alt=""> <p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li>';
                        }
                    }
                    bodyInnerDiv = bodyInnerDiv + '</ul></div></div>';
                    //$(bodyInnerDiv).after($('.chartlevel1'));
                    $('.stiff-chart-inner').append(bodyInnerDiv);
                  // $('.chartlevel1').after(bodyInnerDiv);
                }
                else {
                    $(".stiff-chart-inner").html("<p>No user to display.</p>");
                }
                $("#myModalForViewUserChart").modal("show");
            }
        }
    });
}