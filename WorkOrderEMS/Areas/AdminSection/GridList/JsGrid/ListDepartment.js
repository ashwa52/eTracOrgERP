
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
                    return $.ajax({
                        type: "GET",
                        url: '../AdminSection/Department/GetListDepartment',
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
                    { name: "DepartmentName", title: "Department Name", css: "text-center" },//visible: true  
                    { name: "IsActive_Grid", type: "checkbox", title: "Is Active", sorting: false }, {
                        name: "act",  title: "Action", css: "text-center", itemTemplate: function (value, item) {
                            var $iconPencil = $("<i>").attr({ class: "mdi mdi-pencil " }).attr({ style: "color:#E3BD0F " });
                            var $iconTrash = $("<i>").attr({ class: "mdi mdi-delete" }).attr({ style: "color:#EB470E" });;
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.DeptId }).click(function (e) {
                                    var addNewUrl = "../AdminSection/Department/EditDepartment?Id=" + item.DeptId;
                                    $.ajax({
                                        type: "POST",
                                        url: addNewUrl,
                                        beforeSend: function() {
                                            $("#loading_image").show();
                                        },
                                        success: function (editData) {
                                            //$("#jsGrid-basic").jsGrid("loadData");
                                            if (editData != null) {
                                                $("#DepartmentId").val(editData.DeptId);
                                                $("#DepartmentName").val(editData.DepartmentName);
                                                $("#SaveDeptData").text("Update")
                                                $("#myModalForDepartment").modal("show");
                                            }
                                            
                                        },
                                        error: function (err) {
                                        },
                                        complete: function (data) {
                                            $("#loading_image").hide();
                                        }

                                    });

                                    
                                    $('#ViewModalEdit').load(addNewUrl);
                                    e.stopPropagation();
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                  .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                  .attr({ id: "btn-delete-" + item.DeptId }).click(function (e) {
                                      $.ajax({
                                          type: "POST",
                                          url: "../Department/DeleteDepartment?id=" + item.DeptId,
                                          success: function (Data) {
                                              $("#JsListDepartment").jsGrid("loadData");
                                              toastr.success("Department Deleted successfully.");
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
                    var url = "../NewAdmin/DisplayLocationData/?LocationId=" + getData.LocationId;
                    $('#RenderPageId').load(url);
                }
            });
    //    }
    //})
    //basic jsgrid table
    

})(jQuery);
$(document).ready(function () {
    $("#AddDepartment").click(function () {

        $("#DepartmentId").val("");
        $("#DepartmentName").val("");
        $("#SaveDeptData").text("Save")
        $("#myModalForDepartment").modal("show");
    })
    $("#BackToAdmin").click(function(){
        
        var url = "../AdminSection/AdminDashboard/Index";// @Url.Action("Index", "AdminDashboard", new { area = "AdminSection" });
        window.location.href = url;
    });
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
});