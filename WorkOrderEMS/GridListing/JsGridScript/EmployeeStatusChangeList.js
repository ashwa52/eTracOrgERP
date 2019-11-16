var statusList = '/EPeople/EmployeestatusList';
var base_url = window.location.origin;
var empStatusId = 0;
(function ($) {
    'use strict'
    var data;
    $("#EmployeeStatusChangeGrid").jsGrid({
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
                    url: base_url + statusList,
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
            { name: 'ESC_ChangeType', width: 160, title: "Type", css: "text-center" },//visible: true
            { name: 'ESC_Date', width: 210, title: "Date" },
                    { name: "ESC_EMP_EmployeeId", width: 150, title: "Employee" },
                    { name: "ESC_ApprovedBy", width: 150, title: "Approved By" },
                    { name: "ESC_ApprovalStatus", width: 150, title: "Status" },
                     {
                         name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                             var $iconPencil, $iconTrash;
                             if (item.ESC_ApprovalStatus == 'Not Approved') {
                                 $iconPencil = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                                 $iconTrash = $("<i>").attr({ class: "fa fa-times" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                             } else {                                
                             }
                             var $customEditButton = $("<span>")
                                 .attr({ title: jsGrid.fields.control.prototype.approveButtonTooltip })
                                 .attr({ id: "btn-approve-" + item.RequisitionId }).click(function (e) {
                                     ApproveRejectEmployeeStatus(item.ESC_Id, "A", null);
                                 }).append($iconPencil);
                             var $customDeleteButton = $("<span>")
                                   .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                   .attr({ id: "btn-delete-" + item.RequisitionId }).click(function (e) {
                                       $("#myModelForComment").modal('show');
                                       empStatusId = item.ESC_Id;
                                       
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

$(document).ready(function () {
    $("#AddComment").click(function () {
        var comment = $("#rejectComment").val();
        ApproveRejectEmployeeStatus(empStatusId, "R", comment);
    })
})

function ApproveRejectEmployeeStatus(Id, Status, comment) {
    $.ajax({
        type: "POST",
        // data: { 'Id': item.id},
        url: base_url + '/EPeople/ApproveRejectEmployeeStatus?Id=' + Id + "&Status=" + Status + "&Comment=" + comment,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (message) {
            $("#myModelForComment").modal('hide');
            $("#rejectComment").val("");
            toastr.success(message)
            $("#EmployeeStatusChangeGrid").jsGrid("loadData");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}