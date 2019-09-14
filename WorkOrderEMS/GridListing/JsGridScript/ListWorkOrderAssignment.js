
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
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
                    url: '../NewAdmin/GetWorkOrderList?LocationId=' + $_LocationId + '&workRequestProjectId=' + $_workRequestAssignmentId,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onDataLoading: function (args) {
            return $.ajax({
                type: "GET",
                url: '../NewAdmin/GetWorkOrderList?LocationId=' + $_LocationId + '&workRequestProjectId=' + $_workRequestAssignmentId,
                datatype: 'json',
                contentType: "application/json",
            });
        },
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
                    { name: "WorkRequestTypeName", width: 150, title: "Work Request Type", css: "text-center" },
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
                        name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                            var $iconPencil = $("<i>").attr({ class: "mdi mdi-pencil " }).attr({ style: "color:yellow" });
                            var $iconTrash = $("<i>").attr({ class: "mdi mdi-delete" }).attr({ style: "color:red" });;
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                    var addNewUrl = "../GlobalAdmin/EditLocationSetup?loc=" + item.Id;
                                    $('#RenderPageId').load(addNewUrl);
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
            //{ type: "control" }
        ],
        rowClick: function (args) {
            this
            console.log(args)
            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
            //var url = "../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
            //$('#RenderPageId').load(url);
        }
    });
    //    }
    //})
    //basic jsgrid table


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