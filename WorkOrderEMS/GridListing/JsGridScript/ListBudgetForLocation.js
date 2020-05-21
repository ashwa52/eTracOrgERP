var base_url = window.location.origin;
var BLUrl = '/GlobalAdmin/GetBudgetListByLocationId';
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var loc = $("#AddBudgetForLocation").val();
var $_GetLocation = $("#AddBudgetForLocation").val();

//to calculate budget
var rowId;
var assignedPercent;
var sumofAssignedPercent = 0;
var sumofAssignedAmt = 0;
var sumofRemainingAmount = 0
var Year;
var BudgetAmount;
var add = 0;
var IsGridSaved = false;
//end calculate

(function ($) {
    'use strict'
    $("#ListBudgetAllocation").jsGrid({
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
                    url: base_url + BLUrl + "?Loc=" + $_GetLocation,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onItemUpdated: function (item) {
            // Força  o refrsh
            var $totalRow = $("<tr>").addClass("total-row");
            $("#ListBudgetAllocation").jsGrid("refresh");
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
            //var data = [];
            var items = args.grid.option("data");
            var total = { Name: "AssignedPercent", "AssignedPercent": 0, IsTotal: true };
            var total = { Name: "AssignedAmount", "AssignedAmount": 0, IsTotal: true };
            var total = { Name: "RemainingAmount", "RemainingAmount": 0, IsTotal: true };

            total.AssignedPercent = 0;
            total.AssignedAmount = 0;
            total.RemainingAmount = 0;
            items.forEach(function (item) {
                if (item.AssignedPercent != null) {
                    total.AssignedPercent += parseFloat((item.AssignedPercent));

                }
                if (item.AssignedAmount != null) {
                    total.AssignedAmount += parseFloat((item.AssignedAmount));
                }
                if (item.AssignedAmount != null) {
                    total.RemainingAmount += parseFloat((item.RemainingAmount));
                }
                $("#BudgetAmount").html(item.BudgetAmount);
            });
            add = total.AssignedPercent;
            if (add > 100) {
                bootbox.dialog({
                    message: "Total of Assigned % not be Greater than 100.",
                    danger: {
                        label: "cancel",
                        classname: "btn btn-primary",
                        callback: function () {
                        }
                    }
                });
            }
            var $totalRow = $("<tr>").addClass("total-row");
             args.grid._renderCells($totalRow, total);
             args.grid._content.append($totalRow);
             $(".total-row input").attr("readonly", true).attr("border","aliceblue");

        },
        fields: [
            { name: 'CostCode', width: 160, title: "Cost Code", css: "text-center" },//visible: true
            {
                name: 'AssignedPercent', type: "text", width: 70, title: "Assigned%", itemTemplate: function (value, item) {
                    return $("<input>").attr("value", value).attr("class", "form-control input-rounded").on("keyup", function (value, item) {
                        var $_this = this;
                        if ($_this.value != "" && $_this.value != undefined) {
                            var ListModel = [];
                            var budget = parseInt($("#BudgetAmount").html());
                            var assignedAmount = parseInt($_this.parentElement.parentElement.children[2].innerText);
                            var renainingAmount = parseInt($_this.parentElement.parentElement.children[3].innerText);
                            var costCode = parseInt($_this.parentElement.parentElement.children[0].innerText);
                            var calculate = budget * (parseInt($_this.value) / 100);
                            ////from here will code tomorrow
                            var itemData;
                            var data = $("#ListBudgetAllocation").jsGrid("option", "data");
                            for (var i = 0 ; i < data.length; i++) {
                                var getData = data[i];
                                if (data[i].CostCode == costCode) {
                                    IsGridSaved = true;
                                    getData.AssignedPercent = $_this.value;
                                    getData.AssignedAmount = calculate;
                                    itemData = getData;
                                }
                                ListModel.push(getData);
                            }
                            $("#ListBudgetAllocation").jsGrid("option", "data", ListModel)
                            $_this.parentElement.parentElement.children[2].innerText = calculate;
                            var items = $("#ListBudgetAllocation").jsGrid("option", "data");
                            var totalAssignedPercent = 0;
                            var totalAssignedAmount = 0;
                            var totalRemainingAmount = 0;
                            items.forEach(function (item) {
                                if (item.AssignedPercent != null) {
                                    totalAssignedPercent += parseFloat((item.AssignedPercent));
                                }
                                if (item.AssignedAmount != null) {
                                    totalAssignedAmount += parseFloat((item.AssignedAmount));
                                }
                                if (item.AssignedAmount != null) {
                                    totalRemainingAmount += parseFloat((item.RemainingAmount));
                                }
                                $("#BudgetAmount").html(item.BudgetAmount);
                            });
                            $(".total-row input").attr("value", totalAssignedPercent);
                            $(".total-row").children()[2].innerHTML = totalAssignedAmount;
                            $(".total-row").children()[3].innerHTML = totalRemainingAmount;
                        }
                    });
                }
            },
                    { name: "AssignedAmount", width: 150, title: "Assigned Amount" },                    
                    { name: "RemainingAmount", width: 150, title: "Remaining Amount" },                    
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

$("#AddBudget").click(function (event) {
    $("#myModalForBudgetAmount").modal('show');
});
$("#btnAddCostCode").click(function (event) {
    if (IsGridSaved == true) {
        bootbox.dialog({
            message: "Please Save Budget, Otherwise Your data will lost..",
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                    IsGridSaved = false;
                }
            }
        });
    }
    else {      
        $.ajax({
            type: "GET",
            url: base_url + "/GlobalAdmin/TreeView/?loc=" + loc,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                $('#RenderPageId').html(Data);
            },
            error: function (err) {
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    }
});
$("#btnSaveForCostCode").click(function (event) {
    var txtValue = 0; var txtPreviousBudgetAmt = 0;
    txtValue = parseInt($("#NewBudgetAmount").val());
    txtPreviousBudgetAmt = parseInt($("#BudgetAmount").html());//.html(txtValue);
    var addValue = txtValue + txtPreviousBudgetAmt;
    $("#BudgetAmount").html(addValue);
    var selectedYear = $("#YearSelected option:selected").val();
    $("#myModalForBudgetAmount").modal('hide');
    $.ajax({
        type: 'POST',
        url: base_url + '/GlobalAdmin/SaveBudgetAmountForLocation/',
        data: { BudgetAmount: txtValue, locationId: $_GetLocation, BudgetYear: selectedYear },
        datatype: 'json',
        success: function (result) {
            $("#ListBudgetAllocation").jsGrid("loadData");
            IsGridSaved = false;
        },
        error: function (result) {
            alert('Fail ');
        }
    });
});

$("#btnSaveAllBudget").click(function (event) {
    if (add > 100) {
        bootbox.dialog({
            message: "Total of Assigned % not be Greater than 100.",
            danger: {
                label: "cancel",
                classname: "btn btn-primary",
                callback: function () {
                }
            }
        });
    }
    else {
        var GridData = $("#ListBudgetAllocation").jsGrid("option", "data");
        
        var selectedYearForGrid = $("#YearSelected option:selected").val();
        $.ajax({
            type: 'POST',
            url: base_url+'/GlobalAdmin/SaveAllBudgetGridData/',
            data: { obj: GridData, LocationId: loc, Year: selectedYearForGrid },
            //contentType: 'application/json; charset=utf-8',
            datatype: 'json',
            success: function (result) {
                $("#ListBudgetAllocation").jsGrid("loadData");
                toastr.success(result);
                IsGridSaved = true;
            },
            error: function (result) {
                alert('Fail ');
            }
        });
    }
});