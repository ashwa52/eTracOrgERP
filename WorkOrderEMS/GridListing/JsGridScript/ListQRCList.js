var QRCurl = '../QRCSetup/GetQRCListForJsGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

(function ($) {
    'use strict'
    var data;
    $("#ListQRC").jsGrid({
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
                debugger
                return $.ajax({
                    type: "GET",
                    url: QRCurl + '?locationId=' + $_LocationId + '&SearchQRCType='+ 0,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onDataLoading: function (args) {
            return $.ajax({
                type: "GET",
                url: QRCurl + '?locationId=' + $_LocationId + '&SearchQRCType=' + 0,
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
            { name: 'QRCodeID', width: 160, title: "QRCode ID", css: "text-center" },//visible: true
            { name: 'QRCName', width: 150, title: "QRC Item Name" },
                    { name: "QRCTYPE", width: 150, title: "QRC Type" },
                    { name: "SpecialNotes", width: 150, title: "Special Notes" },                                      
                    {
                        name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                            debugger
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