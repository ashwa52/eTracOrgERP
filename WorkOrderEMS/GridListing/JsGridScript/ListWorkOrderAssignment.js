
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy=0;//= $("#drp_MasterLocation option:selected").val();

(function ($) {
    'use strict'
    var data;
    $("#ListWorkOrderAsssignment").jsGrid({
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
                    url: '../NewAdmin/GetWorkOrderList?LocationId=' + $("#drp_MasterLocation1 option:selected").val() + '&workRequestProjectId=' + $_workRequestAssignmentId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        //onDataLoading: function (args) {
        //    return $.ajax({
        //        type: "GET",
        //        url: '../NewAdmin/GetWorkOrderList?LocationId=' + $_LocationId + '&workRequestProjectId=' + $_workRequestAssignmentId,
        //        datatype: 'json',
        //        contentType: "application/json",
        //    });
        //},
        //data: response,
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
           
        },
        fields: [
            //{ name: "Id", visible: false },
            { name: "CodeID", width: 160, title: "Work Order Id", css: "text-center" },//visible: true
            { name: "PriorityLevelName", width: 150, title: "Priority Level", css: "text-center" },
                    //{ name: "WorkRequestTypeName", width: 150, title: "Work Request Type", css: "text-center" },
                    { name: "WorkRequestStatusName", width: 150, title: "Status", css: "text-center" },
                    { name: "QRCType", width: 180, title: "QRC Type", css: "text-center" },
                    { name: "AssignToUserName", width: 150, title: "Assign To User", css: "text-center" },
                    { name: "WorkRequestProjectTypeName", width: 150, title: "ProjectType", css: "text-center" },
                    { name: "FacilityRequestType", width: 150, title: "Facility Request Type", css: "text-center" },
                    {name: "ProfileImage",title: "Profile Image",
                        itemTemplate: function(val, item) {
                            return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {
                               
                            });
                        }
                    },
                    {
                        name: "AssignedWorkOrderImage", Img: "AssignedWorkOrderImage", width: 100, title: "WorkOrder Image", itemTemplate: function (val, item) {
                            return $("<img>").attr("src", val).css({ height: 50, width: 50,"border-radius": "50px" }).on("click", function () {

                            });
                        }
                    },
                    { name: "CreatedByUserName", width: 190, title: "Submitted By", css: "text-center" },
                    {
                        name: "act", title: "Action", width: 150, css: "text-center", itemTemplate: function (value, item) {
                             var $iconAssign = "";
                             var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:yellow;font-size:22px;" });
                            var $iconTrash = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:red;font-size:26px;margin-left:8px;" });
                            //var $iconList = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:#26B8C7;font-size:26px;margin-left:8px;" });
                            if (item.PriorityLevel != 11 && item.WorkRequestStatusName == "Pending")
                            {
                                 $iconAssign = $("<i>").attr({ class: "fa fa-file-text" }).attr({ style: "color:green;font-size:26px;margin-left:8px;" });
                            }
                            else {

                            }
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                                    var addNewUrl = "../GlobalAdmin/Edit?Id=" + item.id;
                                    $('#WOFormDetails').load(addNewUrl);
                                    e.stopPropagation();
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                  .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                  .attr({ id: "btn-delete-" + item.id }).click(function (e) {
                                      $("#myModalForDeleteWO").modal("show");
                                      $("#DeleteWO").click(function (e) {
                                          $.ajax({
                                              type: "POST",
                                              url: "../GlobalAdmin/DeleteWorkRequest?id=" + item.id,
                                              success: function (Data) {
                                                  $("#myModalForDeleteWO").modal("hide");
                                                  $("#ListWorkOrderAsssignment").jsGrid("loadData");
                                                  toastr.success(Data.Message);
                                              },
                                              error: function (err) {
                                              }

                                          });
                                      });

                                      e.stopPropagation();
                                  }).append($iconTrash);
                            //var $customListButton = $("<span>")
                            //   .attr({ title: jsGrid.fields.control.prototype.listButtonTooltip })
                            //   .attr({ id: "btn-list-" + item.id }).click(function (e) {
                            //       var addNewUrl = "../GlobalAdmin/EditLocationSetup?loc=" + item.id;
                            //       $('#RenderPageId').load(addNewUrl);
                            //       e.stopPropagation();
                            //   }).append($iconList);
                            var $customAssignButton = $("<span>")
                              .attr({ title: jsGrid.fields.control.prototype.assignButtonTooltip })
                              .attr({ id: "btn-assign-" + item.id }).click(function (e) {
                                  $.ajax({
                                      type: "GET",
                                      data: { 'id': item.id, 'ProblemDesc': item.ProblemDesc, 'PriorityLevel': item.PriorityLevel, 'ProjectDesc': item.ProjectDesc, 'WorkRequestType': item.WorkRequestType, 'locationId': item.LocationID, 'AssignedTime': item.AssignedTime, 'AssignToUserId': item.AssignToUserId },
                                      url: '../GlobalAdmin/_AssignWorkAssignmentRequest/',
                                      contentType: "application/json; charset=utf-8",
                                      error: function (xhr, status, error) {
                                      },
                                      success: function (result) {
                                          //$("#AssignEmployeeDiv").html(result);
                                          $("#divDetailPreviewAssignData").html(result);
                                          $("#myModalForAssignEmployeeData").modal('show');
                                      }
                                  });
                                  e.stopPropagation();
                              }).append($iconAssign);
                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton).append($customAssignButton);
                        }
            },
            //{ type: "control" }
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            ViewWODetails(getData);
            
            var text = [];
            //var url = "../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
            //$('#RenderPageId').load(url);
        }
    });
    //    }
    //})
    //basic jsgrid table

   


    $.ajax({
        type: "GET",
        url: '../GlobalAdmin/GetUnseenNotifications',
        datatype: 'json',
        contentType: "application/json",
        success: function (response) {
            $("#UrgentWorkOrdersList").html(response);
        },
        error: function (data) {
            debugger;
        }
    });

})(jQuery);
$(document).ready(function () {

    $(".EditRecord").click(function (event) {
        this
        event.preventDefault();
        var addNewUrl = "../GlobalAdmin/EditLocationSetup";
        $('#RenderPageId').load(addNewUrl);
    });
    $(".jsgrid-edit-button").click(function (event) {
        this;
        $(".jsgrid-insert-row").hide();
        event.preventDefault();
        var addNewUrl = "../NewAdmin/AddNewLocation";
        $('#RenderPageId').load(addNewUrl);
    });
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();
        $("#ListWorkOrderAsssignment").jsGrid("loadData");
    })
});
function ViewWODetails(detials) {
    $("#lblCodeNo").html(detials.CodeID);
    $("#lblProjectType").html(detials.WorkRequestProjectTypeName);
    if (detials.WorkRequestType == null || detials.WorkRequestType == "") {
        $("#labelWorkRequestType").hide();
        $("#lblWorkRequestType").hide();
    }
    else {
        $("#lblWorkRequestType").html(detials.WorkRequestTypeName);
        $("#labelWorkRequestType").show();
        $("#lblWorkRequestType").show();
    }

    $("#lblPriorityLevel").html(detials.PriorityLevelName);
    if (detials.WorkRequestProjectType == 128 && detials.QRCType != 'N/A' && detials.QRCType != '') {
        $("#lblQRCType").html(detials.QRCType);
        $("#labelQRCType").show();
        $("#lblQRCType").show();
    } else {
        $("#labelQRCType").hide();
        $("#lblQRCType").hide();
    }

    if (detials.WorkRequestProjectType != 279) {
        $("#lblWorkRequestStatus").html(detials.WorkRequestStatusName);
        $("#labellWorkRequestStatus").show();
        $("#lblWorkRequestStatus").show();
    }
    else {
        $("#labellWorkRequestStatus").hide();
        $("#lblWorkRequestStatus").hide();
    }
    $("#lblSubmittedOn").html(detials.CreatedDate);
    if (detials.ProblemDesc == null || detials.ProblemDesc == "") {
        $("#labellProblemDescription").hide();
        $("#lblProblemDescription").hide();
    }
    else {
        $("#lblProblemDescription").html(detials.ProblemDesc);
        $("#labellProblemDescription").show();
        $("#lblProblemDescription").show();
    }
    if (detials.ProjectDesc == null || detials.ProjectDesc == "") {
        $("#labellProjectDescription").hide();
        $("#lblProjectDescription").hide();
    }
    else {
        $("#lblProjectDescription").html(detials.ProjectDesc);
        $("#labellProjectDescription").show();
        $("#lblProjectDescription").show();
    }
    if (detials.AssignedTime != null && detials.AssignedTime != "") {
        $("#lblTimeAssigned").html(detials.AssignedTime);
        $("#labellTimeAssigned").show();
        $("#lblTimeAssigned").show();
    }
    else {
        $("#labellTimeAssigned").hide();
        $("#lblTimeAssigned").hide();
    }
    if (detials.WorkRequestProjectType == 279) {
        $("#lblStartDate").html(detials.StartDate);
        $("#lblEndDate").html(detials.EndDate);
        $("#lblWeekdays").html(detials.WeekDays);
        $("#lblStartTime").html(detials.ConStartTime);
        $("#labellStartDate").show();
        $("#lblStartDate").show();
        $("#labellEndDate").show();
        $("#lblEndDate").show();
        $("#labellWeekdays").show();
        $("#lblWeekdays").show();
        $("#labellStartTime").show();
        $("#lblStartTime").show();
    }
    else {
        $("#labellStartDate").hide();
        $("#lblStartDate").hide();
        $("#labellEndDate").hide();
        $("#lblEndDate").hide();
        $("#labellWeekdays").hide();
        $("#lblWeekdays").hide();
        $("#labellStartTime").hide();
        $("#lblStartTime").hide();
    }

    if (detials.WorkRequestProjectType == 256) {
        $("#lblCustomerName").html(detials.CustomerName);
        $("#lblVehicleMake").html(detials.VehicleMake);
        $("#lblCustomerContact").html(detials.CustomerContact);
        $("#lblVehicleYear").html(detials.VehicleYear);
        $("#lblVehicleModel").html(detials.VehicleModel);
        $("#lblVehicleColor").html(detials.VehicleColor);
        $("#lblDriverLicenseNo").html(detials.DriverLicenseNo);
        $("#labellCustomerName").show();
        $("#lblCustomerName").show();
        $("#labellVehicleMake").show();
        $("#lblVehicleMake").show();
        $("#labellCustomerContact").show();
        $("#lblCustomerContact").show();
        $("#labellVehicleYear").show();
        $("#lblVehicleYear").show();
        $("#labellVehicleColor").show();
        $("#lblVehicleColor").show();
        $("#labellDriverLicenseNo").show();
        $("#lblDriverLicenseNo").show();
        $("#lblVehicleModel").show();
        $("#labellVehicleModel").show();
        $("#labellAssignedWorkImage").hide();
        $("#lblAssignedWorkImage").hide();

    }
    else {
        $("#labellCustomerName").hide();
        $("#lblCustomerName").hide();
        $("#labellVehicleMake").hide();
        $("#lblVehicleMake").hide();
        $("#labellCustomerContact").hide();
        $("#lblCustomerContact").hide();
        $("#labellVehicleYear").hide();
        $("#lblVehicleYear").hide();
        $("#labellVehicleColor").hide();
        $("#lblVehicleColor").hide();
        $("#labellDriverLicenseNo").hide();
        $("#lblDriverLicenseNo").hide();
        $("#lblVehicleModel").hide();
        $("#labellVehicleModel").hide();
        $("#labellAssignedWorkImage").show();
        $("#lblAssignedWorkImage").html(detials.AssignedWorkImage);
        $("#lblAssignedWorkImage").show();
        $("#divWoimg").show();
    }

    if (detials.AssignedWorkImage == '' || detials.AssignedWorkImage == null || detials.AssignedWorkImage == "") {
        $("#lblAssignedWorkImage").hide();
        $("#labellAssignedWorkImage").hide();
    }
    else {
        var img = $('#lblAssignedWorkImage img').attr('src');
        if (img != null && img != undefined && img != '') {
            if (img.split("/").pop() == 'no-asset-pic.png') {
                $("#divWoimg").hide();
            }
        }

    }

    if (detials.AssignToUserId != null && detials.AssignToUserId != "" && detials.AssignToUserId != '') {
        $('#divEmpAssigned').show();
        $("#lblProfile img").attr('src',detials.ProfileImage);//.html(detials.ProfileImage);
        //$("#lblProfile").html(detials.ProfileImage);
        $("#lblAssignToUser").html(detials.AssignToUserName);
    }
    else {
        $('#divEmpAssigned').hide();
    }
    if (detials.WorkRequestProjectType != 279 && detials.TotalTime != null && detials.TotalTime != "") {
        $("#lblTotalTimeTaken").html(detials.TotalTime);
        $("#labelTotalTimeTaken").show();
        $("#lblTotalTimeTaken").show();
    }
    else {
        $("#labelTotalTimeTaken").hide();
        $("#lblTotalTimeTaken").hide();
    }

    $("#lblLocationName").html(detials.LocationName);
    $("#ModalDetailWOPreview").modal("show");

};

function PrintDivForWorkDetails(DivId) {

    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var workrequesttype = '';
        var divToAssignedWorkImg = document.getElementById("lblAssignedWorkImage");
        var divToProfile = document.getElementById("lblProfile");
        var popupWin = window.open('', '_blank', 'width=800,height=600');
        popupWin.document.open();
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        if ($("#lblWorkRequestType").html() != null && $("#lblWorkRequestType").html() != "") {
            workrequesttype = "<p></p><strong style='font-size: 16px;'>Work Request Type </strong> <br/>"
                      + $("#lblWorkRequestType").html();
        }

        popupWin.document.write("<style>img {height: 110px;width: 115px;}</style><html><body onload='window.print();'><div style='margin-left: 96px; margin-right: 100px; width: 420px;' class='row '><table id='DivWorkDetailsIndex' style='width: 400px; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><p></p><strong style='font-size: 16px;'> Code No</strong> <br/>"
            + $("#lblCodeNo").html() + "<p></p><strong style='font-size: 16px;'>Project Type </strong> <br/>"
            + $("#lblProjectType").html() + workrequesttype + "<p></p><strong style='font-size: 16px;'>Priority Level </strong> <br/>"
            + $("#lblPriorityLevel").html() + "<p></p><strong style='font-size: 16px;'>Work Request Status </strong> <br/>"
            + $("#lblWorkRequestStatus").html() + "<p></p><strong style='font-size: 16px;'>Submitted On </strong> <br/>"
            + $("#lblSubmittedOn").html()
            + "</td><td td valign='top';>"
            + "<p></p><strong style='font-size: 16px;'>Location Name </strong><br/>"
            + $("#lblLocationName").html()
            + "<p></p><strong style='font-size: 16px;'>Employee Assigned</strong><br/> " + divToProfile.innerHTML + "<p></p><strong style='font-size: 16px;'>Work Order Image</strong><br/>" + divToAssignedWorkImg.innerHTML + "</td></tr></tbody></table></td></tr></table></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}