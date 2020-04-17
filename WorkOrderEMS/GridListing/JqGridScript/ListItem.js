var UnApprovedCompanyurl = '/InvoiceManagement/GetAllItemList';
var AddCustomer = '/InvoiceManagement/CustomerManagementSetup/';
var veiwItemDetails = '/InvoiceManagement/ViewItem';

var LocationId; var ItemId;

$(function () {
    var act;
    $("#jsGrid-basic").jsGrid({
        height: "170%",
        width: "100%",
        filtering: false,
        editing: false,
        inserting: false,
        sorting: false,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: UnApprovedCompanyurl + '?flagApproved=N',
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "ItemCode", title: "Item Code", type: "text", width: 50 },
            { name: "ItemDescription", title: "Item Name", type: "text", width: 50 },
            { name: "ItemRate", title: "Item Rate", type: "text", width: 50 },
            { name: "Unit", title: "Item Unit", type: "text", width: 50 },
            { name: "SpecialNote", title: "Special Note", type: "text", width: 50 },
            { name: "TaxPercentage", title: "Tax Percentage", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "Edit" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            EditItem(item.Id)
                        }).append($iconPencilForEdit);

                    var $customButtonForView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            ViewDetails(item);
                        }).append($iconPencilForView);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customButtonForView);
                }
            }
        ]
    });

});

function ViewDetails(item) {

    ItemId = item.Id;

    $.ajax({
        type: "POST",
        url: veiwItemDetails + '?ItemId=' + ItemId,
        datatype: 'json',
        success: function (result) {
            console.log(result);
            $("#lblItemCode").html(result.ItemCode);
            $("#lblItemDescription").html(result.ItemDescription);
            $("#lblCategory").html(result.Category);
            $("#lblItemUnit").html(result.Unit);
            $("#lblItemRate").html(result.ItemRate);
            $("#lblTaxPercentage").html(result.TaxPercentage);
            $("#lblRevenueCode").html(result.Revenue);
            $("#lblExpenceType").html(result.Expense);
            $("#lblSpecialNote").html(result.SpecialNote);

            $('.modal-title').text("Item Detail");
            $("#myModalForGetItemDetails").modal('show');
            new fn_hideMaskloader();
        },
        error: function () {
            new fn_hideMaskloader();
        }
    });
}
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#jsGrid-basic").jsGrid({
        height: "170%",
        width: "100%",
        filtering: false,
        editing: false,
        inserting: false,
        sorting: false,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: UnApprovedCompanyurl + '?_search=' + $("#SearchText").val() + '&CustomerType=' + $("#CustomerType").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "ItemCode", title: "Item Code", type: "text", width: 50 },
            { name: "ItemDescription", title: "Item Name", type: "text", width: 50 },
            { name: "ItemRate", title: "Item Rate", type: "text", width: 50 },
            { name: "ItemUnit", title: "Item Unit", type: "text", width: 50 },
            { name: "SpecialNote", title: "Special Note", type: "text", width: 50 },
            { name: "TaxPercentage", title: "Tax Percentage", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "Edit" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            EditItem(item.Id)
                        }).append($iconPencilForEdit);

                    var $customButtonForView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            ViewDetails(item);
                        }).append($iconPencilForView);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customButtonForView);
                }
            }
        ]
    });
}
function EditItem(Id) {
    var addNewUrl = "/InvoiceManagement/Item?ItemId=" + Id;
    $('#RenderPageId').load(addNewUrl);
}
$("#AddItem").on("click", function (event) {
    event.preventDefault();
    var addNewUrl = "/InvoiceManagement/Item?ItemId=0";
    $('#RenderPageId').load(addNewUrl);
});