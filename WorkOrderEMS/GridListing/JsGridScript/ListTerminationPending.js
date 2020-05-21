
var TERPurl = '/TerminationDetaills/SetTerminationPendingList';
var base_url = window.location.origin;
//$("#ListTerminationPending").jsGrid('loadData');
var FromDate, ToDate;

(function ($) {

    $("#ListP").click(function () {
        //$("#TerminationListID").css('display', 'none');
        //$("#ListTerminationFinalize").css('display', 'none');
        var Tstatus = "P";
    debugger
    $("#FdTd").click(function () {
        FromDate = $(".d1").val();
        ToDate = $(".d2").val();
    });
    
        'use strict'
    $("#ListTerminationPending").jsGrid({
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
                    url: base_url + TERPurl + "?Todate=" + ToDate + "&FromDate=" + FromDate + "&tstatus=" + Tstatus,
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
            {
                name: "emp_photo", title: "Profile Image", width: 30,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" }).on("click", function () {

                    });
                }
            },
            { name: "name", title: "Employee Name", css: "text-center", width: 50, },
            { name: "seatTitle", title: "Title", css: "text-center", width: 50, },
            { name: "emp_id", title: "Employee Id", css: "text-center", width: 50, },
            { name: "Etype", title: "Type", css: "text-center", width: 50, },
            //{ name: "DateIssued", title: "Date Issued", css: "text-center", width: 50, },


            //visible: true
            //{ name: 'UserType', width: 60, title: "User Type" },
            {
                name: "Action", title: "Action", width: 60, itemTemplate: function (value, item) {
                    var $iconFolder = $("<span>").append('<i title="Review" class= "fa fa-file-text-o fa-2x" style="color:black;margin-left: 7px;margin-top: 6px;" ></i>');// $("<span>").attr({ class: "fa fa-folder-open fa-2x" }).attr({ style: "color:yellow;background-color:green;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconUserView = $("<span>").append('<i title="Edit" class= "fa fa-file-text-o fa-2x" style="color:black;margin-left: 7px;margin-top: 6px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                    var $iconTransfer = $("<span>").append('<i title="Submit" class= "fa fa-check fa-2x" style="color:green;margin-left: 7px;margin-top: 6px;"></i>');//attr({ class: "fa fa-file fa-2x" }).attr({ style: "color:white;background-color:#D26C36;margin-left:20px;border-radius:35px;width:35px;height:35px" });


                    var $customEditButton = $("<span style='margin-left:5%; width: 35px; height: 35px;border-radius: 35px;'>")
                        .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                        .attr({ id: "btn-file-" + item.id }).click(function (e) {
                            debugger
                            $.ajax({
                                type: "POST",
                                // data: { 'Id': item.id},
                                url: base_url + '/EPeople/GetFileViewTest?EMPId=' + item.id,
                                beforeSend: function () {
                                    new fn_showMaskloader('Please wait...');
                                },
                                contentType: "application/json; charset=utf-8",
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    debugger
                                    $("#ContaierAddFile").html(result);
                                    $("#myModalForAddFileData").modal('show');
                                },
                                complete: function () {
                                    fn_hideMaskloader();
                                }
                            });
                            //$("#myModalForAddFileData").modal("show");
                            //$("#EmployeeName").html(item.Name);
                            //$("#EmployeeDesignation").html(item.UserType);
                            //$("#EmployeeImage").attr("src", item.ProfileImage)
                            e.stopPropagation();
                        }).append($iconFolder);


                    var $customUserViewButton = $("<span style=' width: 35px; height: 35px;border-radius: 35px;margin-left:20px;'>")
                        .attr({ title: jsGrid.fields.control.prototype.profileButtonTooltip })
                        .attr({ id: "btn-profile-" + item.id }).click(function (e) {
                            $.ajax({
                                type: "POST",
                                // data: { 'Id': item.id},
                                url: base_url + '/EPeople/GetEmployeeDetailsForEdit?Id=' + item.id,
                                beforeSend: function () {
                                    new fn_showMaskloader('Please wait...');
                                },
                                contentType: "application/json; charset=utf-8",
                                error: function (xhr, status, error) {
                                },
                                success: function (result) {
                                    $("#ContaierEditUserInfo").html(result);
                                    $("#myModalForeditUserInfoData").modal('show');
                                },
                                complete: function () {
                                    fn_hideMaskloader();
                                }
                            });
                        }).append($iconUserView);


                    var $customTransferButton = $("<span style='width: 35px; height: 35px;border-radius: 35px;margin-left:20px;'>")
                        .attr({ title: jsGrid.fields.control.prototype.SignatureButtonTooltip })
                        .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                        }).append($iconTransfer);


                    //var $customTextButton = $("<span style='width: 35px; height: 35px;border-radius: 35px;margin-left:20px;'>")
                    //    .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                    //    .attr({ id: "btn-status-" + item.id }).click(function (e) {
                    //        GetEMPId = item.id;
                    //        $("#myModalForChangeStatusData").modal('show');

                    //    }).append($iconText);

                    //var $customTerminationButton = $("<span style='width: 35px; height: 35px;border-radius: 35px;margin-left:20px;'>")
                    //    .attr({ title: jsGrid.fields.control.prototype.TerminationTooltip })
                    //    .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                    //    }).append($iconTermination);



                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customUserViewButton).append($customTransferButton).append($customTextButton).append($customTerminationButton);
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

    });
})(jQuery);