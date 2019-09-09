
(function ($) {
    'use strict'
    var data;
    //$.ajax({
    //    type: "GET",
    //    url: '../AdminSection/Department/GetListDepartment',
    //    //data: filter,
    //    datatype: 'json',
    //    contentType: "application/json",
    //    success: function (response) {
    //        debugger
    //        data = response;
    //        var act;
            $("#JsListDepartment").jsGrid({
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
                    loadData: function(filter) {
                        return $.ajax({
                            type: "GET",
                            url: '../AdminSection/Department/GetListDepartment',
                            datatype: 'json',
                            contentType: "application/json",
                        });
                    }},
                onDataLoading: function (args) {
                    debugger
                    return $.ajax({
                        type: "GET",
                        url: '../AdminSection/Department/GetListDepartment',
                        datatype: 'json',
                        contentType: "application/json",
                    });
                },
                //data: response,
                onRefreshed: function (args) {
                    debugger
                    $(".jsgrid-insert-row").hide();
                    $(".jsgrid-filter-row").hide()
                    $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
                    return $.ajax({
                        type: "GET",
                        url: '../AdminSection/Department/GetListDepartment',
                        datatype: 'json',
                        contentType: "application/json",
                    });
                },
                fields: [
                    //{ name: "Id", visible: false },
                    { name: "DepartmentName", title: "Department Name", css: "text-center" },//visible: true  
                    { name: "IsActive_Grid", type: "checkbox", title: "Is Active", sorting: false }, {
                        name: "act",  title: "Action", css: "text-center", itemTemplate: function (value, item) {
                            var $iconPencil = $("<i>").attr({ class: "mdi mdi-pencil " }).attr({ style: "color:#E3BD0F " });
                            var $iconTrash = $("<i>").attr({ class: "mdi mdi-delete" }).attr({ style: "color:#EB470E" });;
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.DeptId }).click(function (e) {
                                    debugger
                                    var addNewUrl = "../AdminSection/Department/EditDepartment?Id=" + item.DeptId;
                                    $.ajax({
                                        type: "POST",
                                        url: addNewUrl,
                                        success: function (editData) {
                                            debugger
                                            //$("#jsGrid-basic").jsGrid("loadData");
                                            if (editData != null) {
                                                $("#DepartmentId").val(editData.DeptId);
                                                $("#DepartmentName").val(editData.DepartmentName);
                                                $("#SaveDeptData").text("Update")
                                                $("#myModalForDepartment").modal("show");
                                            }
                                            
                                        },
                                        error: function (err) {
                                        }

                                    });

                                    
                                    $('#ViewModalEdit').load(addNewUrl);
                                    e.stopPropagation();
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                  .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                  .attr({ id: "btn-delete-" + item.DeptId }).click(function (e) {
                                      debugger
                                      $.ajax({
                                          type: "POST",
                                          url: "../GlobalAdmin/DeleteLocation?id=" + item.Id,
                                          success: function (Data) {
                                              debugger
                                              //$("#jsGrid-basic").jsGrid("loadData");
                                              var addNewUrl = "../GlobalAdmin/ListLocation";
                                              $('#ViewModalEdit').load(addNewUrl);
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
                    debugger
                    this
                    console.log(args)
                    var getData = args.item;
                    var keys = Object.keys(getData);
                    var text = [];
                    var url = "../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
                    $('#RenderPageId').load(url);
                }
            });
    //    }
    //})
    //basic jsgrid table
    

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
});