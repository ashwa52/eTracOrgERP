//var HOBurl = '../GlobalAdmin/GetListITAdministratorForJSGrid';
var HOBurl = '../EPeople/EmployeeManagementList';
var clients;
var GetEMPId;
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
            {
                name: 'Name', width: 80, title: "Employee Name", itemTemplate: function (value, item) {
                    return $("<div>").append("" + item.Name + "<br><span style='font-size:10px;color:black;font-style:italic;'>" + item.UserType + "</span></div>");
                }
            },//visible: true
            //{ name: 'UserType', width: 60, title: "User Type" },
                    {
                        name: "UserTask", title: "User Task", width: 60, itemTemplate: function (value, item) {
                            var $iconFolder = $("<span>").append('<i class= "fa fa-folder-open" style="color:yellow;margin-left: 11px;margin-top: 12px;" ></i>');// $("<span>").attr({ class: "fa fa-folder-open fa-2x" }).attr({ style: "color:yellow;background-color:green;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconUserView = $("<span>").append('<i class= "fa fa-user" style="color:black;margin-left: 11px;margin-top: 12px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconTransfer = $("<span>").append('<i class= "fa fa-file" style="color:white;margin-left: 11px;margin-top: 12px;"></i>');//attr({ class: "fa fa-file fa-2x" }).attr({ style: "color:white;background-color:#D26C36;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $iconText = $("<span>").append('<i class= "fa fa-file-text" style="color:white;margin-left: 11px;margin-top: 12px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                            var $customEditButton = $("<span style='background: green; width: 35px; height: 35px;border-radius: 35px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-file-" + item.id }).click(function (e) {
                                    $("#myModalForAddFileData").modal("show");
                                    $("#EmployeeName").html(item.Name);
                                    $("#EmployeeDesignation").html(item.UserType);
                                    $("#EmployeeImage").attr("src", item.ProfileImage)
                                    e.stopPropagation();
                                }).append($iconFolder);
                            var $customUserViewButton = $("<span style='background: #36CA7E; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                  .attr({ title: jsGrid.fields.control.prototype.profileButtonTooltip })
                                  .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                                      $.ajax({
                                          type: "POST",
                                         // data: { 'Id': item.id},
                                          url: '../EPeople/GetEmployeeDetailsForEdit?Id=' + item.id,
                                          beforeSend: function () {
                                              new fn_showMaskloader('Please wait...');
                                          },
                                          contentType: "application/json; charset=utf-8",
                                          error: function (xhr, status, error) {
                                          },
                                          success: function (result) {
                                              $("#ContaierEditUserInfo").html(result);
                                              $("#myModalForeditUserInfoData").modal('show');
                                          },
                                          complete: function () {
                                              fn_hideMaskloader();
                                          }
                                      });
                                  }).append($iconUserView);
                            var $customTransferButton = $("<span style='background: #D26C36; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.SignatureButtonTooltip })
                                 .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                                 }).append($iconTransfer);
                            var $customTextButton = $("<span style='background: #32ACDA; width: 35px; height: 35px;border-radius: 35px;margin-left:15px;'>")
                                 .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                                 .attr({ id: "btn-status-" + item.id }).click(function (e) {
                                     GetEMPId = item.id;
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
                                       $.ajax({
                                           type: "GET",
                                           data: { 'Id': item.id, 'LocationId': $("#drp_MasterLocation1 option:selected").val() },
                                           url: '../EPeople/GetUserListByUserId/',
                                           contentType: "application/json; charset=utf-8",
                                           beforeSend: function () {
                                               new fn_showMaskloader('Please wait...');
                                           },
                                           error: function (xhr, status, error) {
                                           },
                                           success: function (result) {
                                               if (result != null || result > 0) {
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
                                               else {
                                                   $("#ViewUserListDetails").html("No Record found")
                                               }
                                           },
                                           complete: function () {
                                               fn_hideMaskloader();
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
                                   .attr({ title: jsGrid.fields.control.prototype.diagramEmployeeTooltip })
                                   .attr({ id: "btn-diagram-" + item.id }).click(function (e) {
                                       $('#RenderPageId').load('../EPeople/ChartDetailsView?Id=' + item.id);
                                       //$.ajax({
                                       //    type: "POST",
                                       //    // data: { 'Id': item.id},
                                       //    url: '../EPeople/ChartDetailsView',
                                       //    beforeSend: function () {
                                       //        new fn_showMaskloader('Please wait...');
                                       //    },
                                       //    contentType: "application/json; charset=utf-8",
                                       //    error: function (xhr, status, error) {
                                       //    },
                                       //    success: function (result) {
                                       //        debugger
                                       //        $("#containerDivForViewChart").html(result);
                                       //    },
                                       //    complete: function () {
                                       //        fn_hideMaskloader();
                                       //    }
                                       //});                                 
                                   }).append($DiagramView);
                               return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                           }
                       },
                        {
                            name: "UserView", title: "User View", width: 30, css: "text-center", itemTemplate: function (value, item) {
                                var $UserView = $("<span>").attr({ class: "fa fa-user-circle fa-2x" }).attr({ style: "color:black;" });
                                var $customEditButton = $("<span>")
                                    .attr({ title: jsGrid.fields.control.prototype.userEmployeeTooltip })
                                    .attr({ id: "btn-user-view" + item.id }).click(function (e) {
                                        debugger
                                        $("#myModalForViewUserChart").modal("show");
                                        ViewTreeUserData(item.id);

                                      
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


$(".callNextEmployeeData").click(function () {
    var getId = $(this).attr("data-employeeId");
    $.ajax({
        type: "POST",
        data: { 'Id': id, 'model': '@Model' },
        url: '../EPeople/GetUserTreeViewListTesting/',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        //contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (ChildrenList) {
            $(".chartRemove").html("");
            var bodyInnerDiv1;
            if (ChildrenList != null) {

                if (ChildrenList.length > 0) {
                    for (var i = 0; i < ChildrenList.length; i++) {
                        if (i == 0) {
                            bodyInnerDiv1 = '<div class="stiff-chart-level chartRemove chartlevel2" data-level="02"><div class="stiff-child" data-child-from="a"><ul>';
                        }
                        else {
                            var plus = i + 1;
                            bodyInnerDiv1 = bodyInnerDiv1 + '<li class="callNextEmployeeData" data-parent="a0' + i + '" data-employeeId="' + ChildrenList[i].EmployeeId + '"><div class="the-chart"><img src="' + ChildrenList[i].ProfilePhoto + '" alt="" style="border-radius:100px;width:82px;"> <p>Lorem ipsum dolor sit amet, consectetur adipisicing.</p></div></li>';
                        }
                    }
                    var secondDiv1 = bodyInnerDiv1 + '</ul></div></div>';
                    $('.stiff-chart-inner').append(secondDiv1);
                }
                else {
                    $(".stiff-chart-inner").html("<p>No user to display.</p>");
                }
                $("#myModalForViewUserChart").modal("show");
            }
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
})
$(document).ready(function () {
    $("#AddRequisition").click(function () {
        $("#myModalForRequisitionAction").modal("show");
    })

    $(".ActionRequisition").change(function () {
       var value =  $(this).attr("value");
       if (value == 1) {
           $.ajax({
               type: "POST",
               // data: { 'Id': item.id},
               url: '../EPeople/OpenVSCFormForRequistion',
               beforeSend: function () {
                   new fn_showMaskloader('Please wait...');
               },
               contentType: "application/json; charset=utf-8",
               error: function (xhr, status, error) {
               },
               success: function (result) {
                   $("#divOpenRquisitionForm").html(result);
                   $("#myModalForRequisitionVSCChart").modal("show");
                   $("#myModalForRequisitionAction").modal("hide");
               },
               complete: function () {
                   fn_hideMaskloader();
               }
           });
       }
       else if (value == 2) {
           $.ajax({
               url: '../EPeople/GetListOfVSCChart',
               type: 'POST',
               contentType: "application/json",
               success: function (result) {

                   var VSCDDL = '<select id="GetDetailsRequisition" class="form-control input-rounded"><option value="0">-Select Requisition for delete-</option>'
                   for (var i = 1; i < result.length;i++) {
                       VSCDDL = VSCDDL + '<option value="' + result[i].Id + '">' + result[i].SeatingName + '</option>';
                   }
                   VSCDDL = VSCDDL + '</select>';
                   $("#divOpenRquisitionActionDelete").html(VSCDDL);
                   $("#myModalForRequisitionAction").modal("hide");
                   $("#myModalForGetDetailsToDeleteRequisition").modal('show');
               },
               error: function (er) {
               }
           });
       }
       else {

       }
    })
    $("#GetDetailsRequisition").change(function () {
        var value = $("#GetDetailsRequisition option:selected").val();
        $.ajax({
            url: '../EPeople/GetVCSDetailsById?VSCId=' + value,
            type: 'POST',
            contentType: "application/json",
            success: function (result) {
                $("#divOpenRquisitionActionDelete").html(result);
                $("#myModalForRequisitionAction").modal("hide");
                $("#txtSeatingName").html(result.SeatingName);
                $("#txtJobDescription").html(result.JobDescription);
                $("#txtDepartmentName").html(result.DepartmentName);
                $("#txtRateOfPay").html(result.RateOfPay);
                $("#txtEmploymentClassification").html(result.EmploymentClassification);
                $("#txtEmploymentStatus").html(result.EmploymentStatus);
                $("#txtRolesAndResponsibility").html(result.RolesAndResponsibility);
                $("#myModalForGetDetailsToDeleteRequisition").modal('show');
            },
            error: function (er) {
            }
        });

    })

    $("#AddDemotion").click(function () {
        $.ajax({
            type: "GET",
            // data: { 'Id': item.id},
            url: '../EPeople/OpenDemotionForm?Id=' + GetEMPId,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (FormView) {
                $("#divOpenDemotionForm").html(FormView);
                $("#myModalForDemotionEmployee").modal("show");
                $("#myModalForChangeStatusData").modal("hide");
                $("#ChangeTitle").html("Demotion");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    })

    $("#EmploymentStatusChange").click(function () {
        $.ajax({
            type: "GET",
            // data: { 'Id': item.id},
            url: '../EPeople/OpenEmploymentStatusChange?Id=' + GetEMPId,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (FormView) {
                $("#divOpenDemotionForm").html(FormView);
                $("#myModalForDemotionEmployee").modal("show");
                $("#myModalForChangeStatusData").modal("hide");
                $("#ChangeTitle").html("Employment Status Change");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    });

    
})