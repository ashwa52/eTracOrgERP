var RequisitonList = '../EPeople/RequisitionList';

(function ($) {
    'use strict'
    var data;
        $("#ListRquisitionData").jsGrid({
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
                        url: RequisitonList,
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
                { name: 'RequisitionType', width: 160, title: "Requisition Type", css: "text-center" },//visible: true
                { name: 'ActionStatus', width: 210, title: "Status" },
                        { name: "SeatingName", width: 150, title: "VSC Name" },
                         {
                             name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                                 var $iconPencil, $iconTrash;
                                 if(item.ActionStatus == 'Waiting') {
                                     $iconPencil = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                                     $iconTrash = $("<i>").attr({ class: "fa fa-times" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                                 } else {
                                     //$iconPencil = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" }).attr("disabled", "disabled");
                                     //$iconTrash = $("<i>").attr({ class: "fa fa-times" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" }).attr("disabled", "disabled");
                                 }
                                 var $customEditButton = $("<span>")
                                     .attr({ title: jsGrid.fields.control.prototype.approveButtonTooltip })
                                     .attr({ id: "btn-approve-" + item.RequisitionId }).click(function (e) {
                                         ApproveRejectStatus(item.RequisitionId,"A");
                                     }).append($iconPencil);
                                 var $customDeleteButton = $("<span>")
                                       .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                       .attr({ id: "btn-delete-" + item.RequisitionId }).click(function (e) {
                                           ApproveRejectStatus(item.RequisitionId, "X");
                                       }).append($iconTrash);

                                 return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton);
                             }
                         }
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

function ApproveRejectStatus(Id, Status)
{
    debugger
    $.ajax({
        type: "POST",
        // data: { 'Id': item.id},
        url: '../EPeople/ApproveRejectRequisition?Id=' + Id + "&Status=" + Status,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (message) {
            toastr.success(message.Message)
            $("#ListRquisitionData").jsGrid("loadData");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}