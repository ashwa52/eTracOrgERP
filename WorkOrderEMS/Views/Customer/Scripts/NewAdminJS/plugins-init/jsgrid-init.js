
var clients;
//Bind JSGrid o show Location list, But need to add controller method to fetch he data will implemement later
(function ($) {
    'use strict'
   
    //basic jsgrid table
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetListLocation',
        datatype: 'json',
        contentType: "application/json",
        success: function (data) {           
            //clients = data;
            //clients = data.rows;
            var act;
            $("#jsGrid-basic").jsGrid({
                width: "100%",
                height: "400px",
                inserting: true,
                editing: true,
                sorting: true,
                paging: true,
                rownum: 10,
                deleteConfirm: "Do you really want to delete client?",
                loadMessage: "Please, wait...",
                onDataLoading: function (args) {
                    debugger
                    var $_row = $("#grid").jsGrid("rowByItem", item);
                    // cancel loading data if 'name' is empty
                    if (args.filter.name === "") {
                        args.cancel = true;
                    }
                },
                data: data,
                //onRefreshingfunction(args) { 
                //},
                onRefreshed: function (args) {
                $(".jsgrid-insert-row").hide();
                    $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
                //    debugger
                //    var items = args.grid.option("data");
                //    var rows = args.grid.data;
                //    for (var row = 0; row < rows.length; row++) {
                //        debugger
                //        act = '<div><button>edit</button></div>';

                //        //$("#jsGrid-basic").jsGrid("insertItem", { Name: "John", Age: 25, Country: 2 }).done(function () {
                //        //    console.log("insertion completed");
                //        //});
                //        $('#jsGrid').jsGrid('fieldOption', 'act', 'visible', 'true');
                //    };
                },
                fields: [
                    //{ name: "Id", visible: false },
                    { name: "Address", css: "text-center",  validate: "required" },//visible: true
                    { name: "City",  css: "text-center" },
                    { name: "LocationName", title: "Location Name", css: "text-center" },
                    { name: "PhoneNo", title: "Phone No", css: "text-center" },
                    { name: "ZipCode", title: "Zip Code", css: "text-center" },
                    {
                        name: "act", items: act, title: "Action", css: "text-center", itemTemplate: function (value, item) {
                            //TO add icon edit and delete to perform update and delete operation
                            var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:yellow;font-size: 22px;" });
                            var $iconTrash = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:red;font-size: 22px;" });
                            var $iconChart = $("<i>").attr({ class: "fa fa-plus" }).attr({ style: "color:#149D48;font-size: 22px;" });
                            var $iconDollar = $("<i>").attr({ class: "fa fa-usd" }).attr({ style: "color:#166C88;font-size: 22px;" });

                            var $customEditButton = $("<span style='padding: 0 10px 0 0;'>")
                                .attr({title: jsGrid.fields.control.prototype.editButtonTooltip})
                                .attr({ id: "btn-edit-" + item.Id }).click(function (e) {

                                    var addNewUrl = "../GlobalAdmin/EditLocationSetup?loc=" + item.Id;
                                    $('#RenderPageId').load(addNewUrl);
                                    e.stopPropagation();
                                  }).append($iconPencil);
                            var $customDeleteButton = $("<span style='padding: 0 10px 0 0;'>")
                                 .attr({title: jsGrid.fields.control.prototype.deleteButtonTooltip})
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

                            var $customChartButton = $("<span style='padding: 0 10px 0 0;'>")
                               .attr({ title: jsGrid.fields.control.prototype.costCodeButtonTooltip })
                               .attr({ id: "btn-chart-" + item.Id }).click(function (e) {
                                   window.location.href = '../GlobalAdmin/TreeView/?loc=' + item.Id;
                                   e.stopPropagation();
                               }).append($iconChart);
                            var $customDollarButton = $("<span style='padding: 0 10px 0 0;'>")
                              .attr({ title: jsGrid.fields.control.prototype.budgetButtonTooltip })
                              .attr({ id: "btn-dollar-" + item.Id }).click(function (e) {
                                  window.location.href = '../GlobalAdmin/BudgetAllocation/?loc=' + item.Id;
                                  e.stopPropagation();
                              }).append($iconDollar);

                           return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton).append($customChartButton).append($customDollarButton);

                            //var ed = "<a href='javascript:void(0)' class='EditRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-pencil fa-1x' style='color:yellow;'></span></a>";
                            //var de = "<a href='javascript:void(0)' class='DeleteRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-delete fa-1x' style='color:black;'></span></a>";
                            //var vi = "<a href='javascript:void(0)' class='ViewRecord' Id='" + item.Id + "' style='margin-right: 10px;cursor:pointer;'><span class='mdi mdi-eye fa-1x' style='color:black'></span></a>";
                            ////return $("<span>").attr("class", ed);
                            //var alldiv = "<span>" + ed + "</span>" + "<span>" + de + "</span>" + "<span>" + vi + "</span>";
                            //return $("<div>").html(alldiv);
                    }},
                    //{ type: "control" }
                ],
                
                //On row click show location details
                rowClick: function (args) {
                    
                    console.log(args)
                    var getData = args.item;
                    var keys = Object.keys(getData);
                    var text = [];
                    var url ="../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
                    $('#RenderPageId').load(url);
                    //window.location.href = "../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
                    //$.each(keys, function (idx, value) {
                    //    text.push(value + " : " + getData[value])
                    //});

                    //$("#label").text(text.join(", "))
                }
            });
        },
        error: function (err, e, er) {
        }

    });
   
})(jQuery);
$(document).ready(function () {
    
    $(".EditRecord").click(function (event) {
        debugger
        this
        event.preventDefault();
        var addNewUrl = "../GlobalAdmin/EditLocationSetup";
        $('#RenderPageId').load(addNewUrl);
    });
    $(".jsgrid-edit-button").click(function (event) {
        debugger
        this;
        $(".jsgrid-insert-row").hide();
        event.preventDefault();
        var addNewUrl = "../NewAdmin/AddNewLocation";
        $('#RenderPageId').load(addNewUrl);
    });
})