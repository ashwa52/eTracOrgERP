var base_url = window.location.origin;
var TransferUrl = '/GlobalAdmin/GetListCostCodeForTranferBudgetAsPerLocation';
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_GetLocation = $("#AddBudgetForLocation").val();
(function ($) {
    'use strict'
    $("#ListAllocatedBudget").jsGrid({
        width: "100%",
        height: "500px",
        inserting: true,
        filtering: true,
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
                    url: base_url + TransferUrl + "?Loc=" + $_GetLocation,
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
            { name: 'Description', width: 160, title: "Cost Code", css: "text-center" },//visible: true
            { name: 'AssignedPercent', type: "text", width: 70, title: "Assigned%"},
            { name: "AssignedAmount", width: 150, title: "Assigned Amount" },
            { name: "RemainingAmount", width: 150, title: "Remaining Amount" },
            { name: "Year", width: 150, title: "Year" },
            { name: "BudgetAmount", width: 150, title: "Budget Amount" },
            { name: "BudgetSource", width: 150, title: "Budget Source" },
            {
                name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencil, $iconTrash;
                    $iconPencil = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                    var $customEditButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.approveButtonTooltip })
                        .attr({ id: "btn-approve-" + item.id }).click(function (e) {
                        }).append($iconPencil);
                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
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