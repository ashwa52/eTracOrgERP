
var TERurl = '/TerminationDetaills/EmployeeTerminationList'
var base_url = window.location.origin;
$("#TerminationListID").jsGrid('loadData');
var FromDate, ToDate, EMPId;

(function ($) {
    debugger
    $("#FdTd").click(function () {
        FromDate = $(".d1").val();
        ToDate = $(".d2").val();
    });

    $("#ListT").click(function () {
        debugger
        $("#TerminationListID").jsGrid('loadData');
        $("#TerminationTeam").show();
    });
    Tstatus = "T";
    'use strict'
    //var data;

        $("#TerminationListID").jsGrid({
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
                        url: base_url + TERurl + "?ToDate=" + ToDate + "&FromDate=" + FromDate,
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
                        return $("<img>").attr("src", val).css({ height: 45, width: 45, "border-radius": "50px" }).on("click", function () {

                        });
                    }
                },
                { name: "name", title: "Employee Name", css: "text-center", width: 50, },
                { name: "seatTitle", title: "Title", css: "text-center", width: 53, },
                { name: "emp_id", title: "Employee Id", css: "text-center", width: 50, },






                //visible: true
                //{ name: 'UserType', width: 60, title: "User Type" },
                {
                    name: "Action", title: "Action", width: 65, itemTemplate: function (value, item) {
                        var $iconFolder = $("<span>").append('<i title=" Submit Corrective Action" class= "fa fa-file-text-o fa-lg" style="color:black;margin-left: 2px;margin-top: 6px;" ></i>');// $("<span>").attr({ class: "fa fa-folder-open fa-2x" }).attr({ style: "color:yellow;background-color:green;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                        var $iconUserView = $("<span>").append('<i title="Submit PIP" class= "fa fa-file-text-o fa-lg" style="color:black;margin-left: 2px;margin-top: 6px;" ></i>');//attr({ class: "fa fa-user fa-2x" }).attr({ style: "color:white;background-color:#36CA7E;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                        var $iconTransfer = $("<span>").append('<i title="Submit Resignation" class= "fa fa-file-text-o fa-lg" style="color:black;margin-left: 2px;margin-top: 6px;"></i>');//attr({ class: "fa fa-file fa-2x" }).attr({ style: "color:white;background-color:#D26C36;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                        var $iconText = $("<span>").append('<i title="Document an Incident" class= "fa fa-file-text-o fa-lg" style="color:black;margin-left: 2px;margin-top: 6px;" ></i>');//.attr({ class: "fa fa-file-text fa-2x" }).attr({ style: "color:white;background-color:#32ACDA;margin-left:20px;border-radius:35px;width:35px;height:35px" });
                        var $iconTermination = $("<span>").append('<i title="Submit Termination" class= "fa fa-times fa-lg" style="color:red;margin-left: 2px;margin-top: 6px;" ></i>');


                        var $customEditButton = $("<span style='margin-left:5%; width: 20px; height: 25px;border-radius: 20px;'>")
                            .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                            .attr({ id: "btn-Corrective-" + item.id }).click(function (e) {
                                debugger
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},

                                    url: base_url + '/Corrective/CorrectiveActionForm?EMPId=' + item.emp_id,

                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },

                                    success: function (result) {
                                        $('#RenderPageId').html(result);
                                    },
                                    error: function (xhr, status, error) {

                                    },

                                    complete: function () {
                                        fn_hideMaskloader();
                                    }

                                });
                                
                                e.stopPropagation();
                            }).append($iconFolder);


                        var $customUserViewButton = $("<span style=' width: 20px; height: 25px;border-radius: 20px;margin-left:14px;'>")
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


                        var $customTransferButton = $("<span style='width: 20px; height: 25px;border-radius: 20px;margin-left:14px;'>")
                            .attr({ title: jsGrid.fields.control.prototype.SignatureButtonTooltip })
                            .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                            }).append($iconTransfer);


                        var $customTextButton = $("<span style='width: 20px; height: 25px;border-radius: 20px;margin-left:14px;'>")
                            .attr({ title: jsGrid.fields.control.prototype.statusButtonTooltip })
                            .attr({ id: "btn-status-" + item.id }).click(function (e) {
                                GetEMPId = item.id;
                                $("#myModalForChangeStatusData").modal('show');

                            }).append($iconText);

                        var $customTerminationButton = $("<span style='width: 20px; height: 25px;border-radius: 20px;margin-left:14px;'>")
                            .attr({ title: jsGrid.fields.control.prototype.TerminationTooltip })
                            .attr({ id: "btn-Ter-" + item.emp_id }).click(function (e) {
                                debugger
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},

                                    url: base_url + '/TerminationDetaills/TerminationForm?EMPId=' + item.emp_id,

                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },

                                    error: function (xhr, status, error) {

                                    },
                                    success: function (result) {
                                        $('#RenderPageId').html(result);
                                    },
                                    complete: function () {
                                        fn_hideMaskloader();
                                    }

                                });

                            }).append($iconTermination);



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


})(jQuery);



function OpenTerminationFinalize() {
    
    debugger
    $("#TerminationTeam").hide();
    $("#TerminationPending").hide();
    $("#TerminationFinalize").show();
    $("#ListTerminationFinalize").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();
                debugger
                $.ajax({
                    url: base_url + '/TerminationDetaills/EmployeeTerminationFinalizeList',
                    dataType: 'json',
                    success: function (data) {
                        deferred.resolve(data);
                    }
                });

                return deferred.promise();
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
        },


        fields: [
            {
                title: "Image", name: "emp_photo", width: 30,
                itemTemplate: function (value) {
                    debugger
                    return $("<img>").attr("src", value).css({ height: 45, width: 45, "border-radius": "50px" });
                }
            },

            {
                title: "Applicant Name", name: "name", width: 50
           
            },

            { title: "seat Title", width: 50, name: "seatTitle" },
            { title: "Etype", width: 50, name: "Etype" },
            { title: "Date Issued", width: 50, name: "dateIssued" },
        ]
    });

}


function OpenTerminationPending() {

    debugger
    $("#TerminationTeam").hide();
    $("#TerminationPending").show();
    $("#TerminationFinalize").hide();
    $("#ListTerminationPending").jsGrid({
        width: "100%",
        height: "400px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();
                debugger
                $.ajax({
                    url: base_url + '/TerminationDetaills/EmployeeTerminationPendingList',
                    dataType: 'json',
                    success: function (data) {
                        deferred.resolve(data);
                    }
                });

                return deferred.promise();
            }
        },

        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
        },

        fields: [
            {
                title: "Image", name: "emp_photo", width: 30,
                itemTemplate: function (value) {
                    debugger
                    return $("<img>").attr("src", value).css({ height: 45, width: 45, "border-radius": "50px" });
                }
            },
            {
                title: "Applicant Name", name: "name", width: 30
                
            },

            { title: "seat Title", width: 30, name: "seatTitle" },
            { title: "Etype", width: 30, name: "Etype" },
            { title: "status", width: 30, name: "status" },

            {
                name: "Action", title: "Action", width: 55, itemTemplate: function (value, item) {

                    var ActionType = item.Etype;
                    switch (ActionType) {

                        //Actions In case of Termination
                        case 'Termination':
                            var $iconView = $("<span>").append('<i title="View" class= "fa fa-eye fa-lg" style=" color:black; margin-left:-5px; margin-top: 6px;" ></i>');
                            var $AssetIcon = $("<span>").append('<i title="Asset Status" class= "fa fa-file-text-o fa-lg" style=" color:black; margin-left:1px; margin-top: 6px;" ></i>');
                            var $ReviewIcon = $("<span>").append('<i title="Review" class= "fa fa-file-text-o fa-lg" style=" color:black; margin-left:1px; margin-top: 6px;" ></i>');
                            var $ApproveButtonName = $("<span>").append('<p title="Approve" style=" color:black; margin-top: 1px; font-size: 11px; font-family: sans-serif;" >Approve</p>');
                            var $DenyButtonName = $("<span>").append('<p title="Deny" style=" color:black; margin-top: 1px; font-size: 11px; font-family:sans-serif;" >Deny</p>');
                            var $ApprovedIcon = $("<span>").append('<i title="Approved" class= "fa fa-check fa-lg" style="color:green; margin-left: 17px;margin-top: 6px;" ></i>');
                            var $InitiateTerminationButtonName = $("<span>").append('<p title="Initiate" style=" color:black; margin-top: 2px; font-size: 11px; font-family:sans-serif;" >Initiate Termination</p>');
                            var $FinalizeTerminationButtonName = $("<span>").append('<p title="Finalize" style=" color:black; margin-top: 2px; font-size: 11px; font-family:sans-serif;" >Finalize</p>');
                            
                            var $customEditB = $("<span style='margin-left:3%; width: 15px; height: 20px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-file-" + item.emp_id }).click(function (e) {
                                    debugger
                                    $.ajax({
                                        type: "POST",
                                     
                                        url: base_url + '/TerminationDetaills/TerminationFormAllDetails?EMPId=' + item.emp_id,

                                        beforeSend: function () {
                                            new fn_showMaskloader('Please wait...');
                                        },

                                        error: function (xhr, status, error) {

                                        },
                                        success: function (ResultData) {
                                            debugger
                                            $("#TEmpName").text(ResultData.Emp_Name);
                                            $("#TEmpId").text(ResultData.EmpId);
                                            $("#TMangagername").text(ResultData.Manager_Name);
                                            $("#Tlastdayworked").text(ResultData.Last_Day_Worked);
                                            $("#TDate").text(ResultData.Termination_Date);
                                            $("#TReasonforleaving").text(ResultData.Reason_For_Leaving);
                                            $("#TExplanation").text(ResultData.detailed_Expalnation);
                                            $("#TMReason").text(ResultData.Final_Incident_Termination);
                                            if (ResultData.Re_Hire == 'Y') {
                                                $("#TRehire").text("Yes");
                                            }
                                            else {
                                                $("#TRehire").text("No");
                                            }
                                            
                                            $("#myModalForEmployeeTermination").modal('show');
                                        },
                                        complete: function () {
                                            fn_hideMaskloader();
                                        }

                                    });


                                    e.stopPropagation();
                                }).append($iconView);

                            var $AssetListB = $("<span style='margin-left:2%; width: 17px; height: 25px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Asset-" + item.emp_id }).click(function (e) {

                                    $.ajax({
                                        type: "POST",
                                        // data: { 'Id': item.id},

                                        url: base_url + '/TerminationDetaills/EmployeeAssetList?EMPId=' + item.emp_id,

                                        beforeSend: function () {
                                            new fn_showMaskloader('Please wait...');
                                        },

                                        error: function (xhr, status, error) {

                                        },
                                        success: function (result) {
                                            $('#RenderPageId').html(result);
                                        },
                                        complete: function () {
                                            fn_hideMaskloader();
                                        }

                                    });


                                    e.stopPropagation();
                                }).append($AssetIcon);


                            var $ApprovedDiv = $("<span style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#0bec46a1;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "Icon-Approved-" + item.emp_id })
                                .css("display", "none").append($ApprovedIcon);


                            var $ApproveButton = $("<button style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#3897f840;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Approve-" + item.emp_id }).click(function (e) {
                                    var GetType = item.Etype;
                                    debugger
                                    switch (GetType) {
                                        case 'Termination':
                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/HrTerminationApprove?EmpId=" + item.emp_id,
                                                success: function (ResultData) {

                                                }
                                            });
                                            $("#" + "btn-Approve-" + item.emp_id).hide();
                                            $("#" + "Icon-Approved-" + item.emp_id).css("display", "inline");
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            $("#" + "btn-Initiate-" + item.emp_id).css("display", "inline");
                                            break;
                                        case 'CorrectiveAction':
                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/Corrective/SetHrCorrectiveActionApprove?EmpId=" + item.emp_id,
                                                success: function (ResultData) {

                                                }
                                            });
                                            $("#" + "btn-Approve-" + item.emp_id).hide();
                                            $("#" + "Icon-Approved-" + item.emp_id).css("display", "inline");
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            //$("#" + "btn-Initiate-" + item.emp_id).css("display", "inline");
                                            break;
                                    }
                                   
                                    e.stopPropagation();
                                }).append($ApproveButtonName);

                            var $DenyButton = $("<button style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#ff000073;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Deny-" + item.emp_id }).click(function (e) {
                                    debugger
                                    var GetType = item.Etype;
                                    switch (GetType) {
                                        case 'Termination':
                                            $("#" + "btn-Approve-" + item.emp_id).hide();

                                            $("#myModalForDenialReason").modal();

                                            $("#SaveDenialReason").click(function () {
                                                $("#" + "btn-Deny-" + item.emp_id).hide();
                                                debugger
                                                var HRDenialReason = $("#TerminationHrDenial").serialize();
                                                if (HRDenialReason != null) {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: base_url + '/TerminationDetaills/HrDenialReason?EmpId=' + item.emp_id,
                                                        data: HRDenialReason,
                                                        success: function (ResultData) {

                                                        }


                                                    });
                                                }

                                            });
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            break;

                                        case 'CorrectiveAction':
                                            $("#" + "btn-Approve-" + item.emp_id).hide();

                                            $("#myModalForCADenialReason").modal();

                                            $("#SaveCADenialReason").click(function () {
                                                $("#" + "btn-Deny-" + item.emp_id).hide();
                                                debugger
                                                var HRDenialReason = $("#CorrectiveActionHrDenial").serialize();
                                                if (HRDenialReason != null) {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: base_url + '/Corrective/EmployeeCorrectiveActionHrDenial?EmpId=' + item.emp_id,
                                                        data: HRDenialReason,
                                                        success: function (ResultData) {

                                                        }


                                                    });
                                                }

                                            });
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            break;

                                    }



                                    e.stopPropagation();
                                }).append($DenyButtonName);

                            var $ReviewButton = $("<span style='margin-left:2%; width: 17px; height: 25px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Review-" + item.emp_id }).click(function (e) {
                                    debugger
                                    var GetType = item.Etype;
                                    switch (GetType) {
                                        case 'Termination':

                                            debugger

                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/GetTerminationDetailsForReview?EmpId=" + item.emp_id,
                                                success: function (ResultData) {
                                                    if (ResultData != null) {
                                                        $("#ReviewEmpId").text(ResultData.EmpId);
                                                        $("#ReviewEmpName").text(ResultData.Emp_Name);
                                                        //$("#ReviewTerminationStatus").text(ResultData.HR_Decision);
                                                        $("#ReviewReason").text(ResultData.HrDenyReason);
                                                        $("#ReviewComment").text(ResultData.HrDenyComment);
                                                        if (ResultData.HR_Decision == 'N') {
                                                            $("#ReviewTerminationStatus").text("Denied by HR");
                                                        }
                                                        else {
                                                            $("#ReviewTerminationStatus").text("Waiting");
                                                        }

                                                    }

                                                }


                                            });
                                            $("#myModalForManagerReview").modal();

                                            $("#ClickAccept").click(function () {
                                                debugger
                                                $.ajax({
                                                    type: "POST",
                                                    url: window.location.origin + '/TerminationDetaills/TerminationDenyAcknowledge?EmpId=' + item.emp_id,
                                                    success: function (result) {
                                                        $('#RenderPageId').html(result);
                                                    },

                                                });

                                            });

                                            break;

                                        case 'CorrectiveAction':

                                            debugger

                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/Corrective/CorrectiveActiondetailsForReview?EmpId=" + item.emp_id,
                                                success: function (ResultData) {
                                                    if (ResultData != null) {
                                                        $("#ReviewEmpId").text(ResultData.EmpId);
                                                        $("#ReviewEmpName").text(ResultData.Emp_Name);
                                                        //$("#ReviewTerminationStatus").text(ResultData.HR_Decision);
                                                        $("#ReviewReason").text(ResultData.HrDenyReason);
                                                        $("#ReviewComment").text(ResultData.HrDenyComment);
                                                        if (ResultData.HR_Decision == 'N') {
                                                            $("#ReviewTerminationStatus").text("Denied by HR");
                                                        }
                                                        else {
                                                            $("#ReviewTerminationStatus").text("Waiting");
                                                        }

                                                    }

                                                }


                                            });
                                            $("#myModalForManagerReview").modal();

                                            $("#ClickAccept").click(function () {
                                                debugger
                                                $.ajax({
                                                    type: "POST",
                                                    url: window.location.origin + '/Corrective/CorrectiveActionDenyAcknowledge?EmpId=' + item.emp_id,
                                                    success: function (result) {
                                                        $('#RenderPageId').html(result);
                                                    },

                                                });

                                            });

                                            break;

                                    }

                                    e.stopPropagation();
                                }).append($ReviewIcon);



                            var $InitiateTermination = $("<button style='margin-left:2%; width: 102px; height: 26px;border-radius: 35px; background-color:#c91ecc85;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Initiate-" + item.emp_id }).click(function (e) {
                                    debugger
                                    $("#myModalForWitnessPresent").modal();

                                    $("#SaveEliteWitnessDetails").click(function () {
                                        debugger
                                        $("#" + "btn-Initiate-" + item.emp_id).css("display", "none");
                                        $("#" + "btn-Finalize-" + item.emp_id).css("display", "inline");

                                        var EliteWitnessData = $("#EliteWitnessForm").serialize();

                                        if (EliteWitnessData != null) {
                                            $.ajax({
                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/TerminationEmployeeWitnessDetails?EmpId=" + item.emp_id,
                                                data: EliteWitnessData,
                                                success: function (ResultData) {

                                                }


                                            });
                                        }

                                    });


                                    $("#SaveOtherCompanyWitnessDetails").click(function () {

                                        $("#" + "btn-Initiate-" + item.emp_id).css("display", "none");
                                        $("#" + "btn-Finalize-" + item.emp_id).css("display", "inline");

                                        var OtherCompanyWitnessData = $("#OtherCompanyWitnessForm").serialize();
                                        if (OtherCompanyWitnessData != null) {
                                            $.ajax({
                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/TerminationEmployeeWitnessDetails?EmpId=" + item.emp_id,
                                                data: OtherCompanyWitnessData,
                                                success: function (ResultData) {

                                                }


                                            });
                                        }

                                    });




                                    e.stopPropagation();
                                }).append($InitiateTerminationButtonName);

                            var $FinalizeTermination = $("<button style='margin-left:2%; width: 102px; height: 26px;border-radius: 35px; background-color:#ffeb00bd;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Finalize-" + item.emp_id })
                                .css("display", "none").click(function (e) {
                                    debugger
                                    $("#myModalForSure").modal();
                                    $("#SaveSure").click(function () {

                                        $.ajax({
                                            type: "POST",
                                            url: "/TerminationDetaills/EmployeeAssetAllocation?EmpId=" + item.emp_id,
                                            success: function (ResultData) {
                                                $('#RenderPageId').html(ResultData);
                                            }


                                        });
                                    });
                                    e.stopPropagation();
                                }).append($FinalizeTerminationButtonName);

                            debugger
                            switch ($_userTypeId) {

                                case "493":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($ApproveButton).append($ApprovedDiv).append($DenyButton);
                                    break;
                                case "1":
                                    if (item.status == "HR Approved") {

                                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ReviewButton).append($InitiateTermination).append($FinalizeTermination);
                                    }
                                    else {

                                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ReviewButton);
                                    }
                                    
                                    break;
                                case "2":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ReviewButton).append($InitiateTermination).append($FinalizeTermination);
                                    break;
                                case "5":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ReviewButton).append($InitiateTermination).append($FinalizeTermination);

                                default:
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ReviewButton).append($InitiateTermination).append($FinalizeTermination);
                            }
                            //return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ApproveButton).append($ApprovedDiv).append($DenyButton).append($InitiateTermination).append($FinalizeTermination);
                            break;



                        ////Actions In case of Corrective Action
                        case 'CorrectiveAction':
                            var $iconView = $("<span>").append('<i title="View" class= "fa fa-eye fa-lg" style=" color:black; margin-left:-5px; margin-top: 6px;" ></i>');
                            var $MeetingIcon = $("<span>").append('<i title="Meeting" class= "fa fa-clock-o fa-lg" style=" color:black; margin-left:1px; margin-top: 6px;" ></i>');
                            var $ReviewIcon = $("<span>").append('<i title="Review" class= "fa fa-file-text-o fa-lg" style=" color:black; margin-left:1px; margin-top: 6px;" ></i>');
                            var $ApproveButtonName = $("<span>").append('<p title="Approve" style=" color:black; margin-top: 1px; font-size: 11px; font-family: sans-serif;" >Approve</p>');
                            var $DenyButtonName = $("<span>").append('<p title="Deny" style=" color:black; margin-top: 1px; font-size: 11px; font-family:sans-serif;" >Deny</p>');
                            var $ApprovedIcon = $("<span>").append('<i title="Approved" class= "fa fa-check fa-lg" style="color:green; margin-left: 17px;margin-top: 6px;" ></i>');
                            var $StartMeetingButton = $("<span>").append('<p title="Start Meeting" style=" color:black; margin-top: 2px; font-size: 11px; font-family:sans-serif;" >Start Meeting</p>');

                            var $customEditB = $("<span style='margin-left:3%; width: 15px; height: 20px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-file-" + item.emp_id }).click(function (e) {
                                    debugger
                                    //$.ajax({
                                    //    type: "POST",
                                    //    // data: { 'Id': item.id},

                                    //    url: base_url + '/Corrective/GetTerminationDetails?EMPId=' + item.emp_id,

                                    //    beforeSend: function () {
                                    //        new fn_showMaskloader('Please wait...');
                                    //    },

                                    //    error: function (xhr, status, error) {

                                    //    },
                                    //    success: function (result) { 
                                    //        $("#myModalForEmployeeCorrectiveAction").modal('show');
                                    //    },
                                    //    complete: function () {
                                    //        fn_hideMaskloader();
                                    //    }

                                    //});


                                    e.stopPropagation();
                                }).append($iconView);

                            var $MeetingButton = $("<span style='margin-left:2%; width: 17px; height: 25px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Meeting-" + item.emp_id }).click(function (e) {
                                    debugger
                                    $.ajax({
                                        type: "POST",
                                        url: base_url + "/Corrective/EmployeeIsExempt?EmpId=" + item.emp_id,
                                        success: function (ResultData) {
                                            if (ResultData.IsExempt == 'Y') {
                                                debugger
                                                $("#myModalForSchdeludingMeeting").modal();
                                                var MeetingDetails = $("#SetMeeting").serializeArray();

                                                $("#submitMeetingOne").click(function () {
                                                    debugger
                                                    var Mdateone = $("#MeetingDateOne").val();
                                                    var Mtimeone = $("#MeetingTimeOne").val();
                                                    MeetingDetails.push({ name: "MeetingDateOne", value: Mdateone });
                                                    MeetingDetails.push({ name: "MeetingTimeOne", value: Mtimeone });
                                                    $("#myModalForsecondmeetingtime").modal();
                                                });
                                                $("#SetMeetingSecond").click(function () {
                                                    $("#submitMeetingOne").hide();
                                                    $("#DateRowTwo").css("display", "flex");
                                                });
                                                $("#submitTwo").click(function () {
                                                    var Mdatetwo = $("#MeetingDateTwo").val();
                                                    var Mtimetwo = $("#MeetingTimeTwo").val();
                                                    MeetingDetails.push({ name: "MeetingDateTwo", value: Mdatetwo });
                                                    MeetingDetails.push({ name: "MeetingTimeTwo", value: Mtimetwo });
                                                    $("#myModalForthirdmeetingtime").modal();
                                                });
                                                $("#SetMeetingThird").click(function () {
                                                    $("#submitTwo").hide();
                                                    $("#DateRowThree").css("display", "flex");
                                                });
                                                $("#SetMeetingIsExemptY").click(function () {
                                                    var Mdatethree = $("#MeetingDateThree").val();
                                                    var Mtimethree = $("#MeetingTimeThree").val();
                                                    MeetingDetails.push({ name: "MeetingDateThree", value: Mdatethree });
                                                    MeetingDetails.push({ name: "MeetingTimeThree", value: Mtimethree });
                                                    $.ajax({

                                                             type: "POST",
                                                             url: window.location.origin + "/Corrective/MeetingWithEmolyee?EMPId=" + item.emp_id,
                                                             data: MeetingDetails,
                                                             beforeSend: function () {
                                                                    new fn_showMaskloader('Please wait...');
                                                             },

                                                             error: function (xhr, status, error) {

                                                             },
                                                             success: function (ResultData) {

                                                             },
                                                             complete: function () {
                                                                    fn_hideMaskloader();
                                                             }


                                                           });

                                                });
                        
                                            }
                                            else {
                                                $("#myModalForSchdeludingMeetingN").modal();
                                                var MeetingDetails = $("#SetMeeting").serializeArray();
                                                $("#SetMeetingIsExemptN").click(function () {
                                                    var MdateoneN = $("#MeetingDateNonExempt").val();
                                                    var MtimeoneN = $("#MeetingTimeNonExempt").val();
                                                    MeetingDetails.push({ name: "MeetingDateNonExempt", value: MdateoneN });
                                                    MeetingDetails.push({ name: "MeetingTimeNonExempt", value: MtimeoneN });
                                                    $.ajax({

                                                        type: "POST",
                                                        url: window.location.origin + "/Corrective/MeetingWithEmolyeeNonExempt?EMPId=" + item.emp_id,
                                                        data: MeetingDetails,
                                                        beforeSend: function () {
                                                            new fn_showMaskloader('Please wait...');
                                                        },

                                                        error: function (xhr, status, error) {

                                                        },
                                                        success: function (ResultData) {

                                                        },
                                                        complete: function () {
                                                            fn_hideMaskloader();
                                                        }


                                                    });


                                                });
                                            }
                                        }
                                    });

                                    //$("#myModalForSchdeludingMeeting").modal();
                                 
                                    e.stopPropagation();
                                }).append($MeetingIcon);


                            var $ApprovedDiv = $("<span style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#0bec46a1;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "Icon-Approved-" + item.emp_id })
                                .css("display", "none").append($ApprovedIcon);


                            var $ApproveButton = $("<button style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#3897f840;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Approve-" + item.emp_id }).click(function (e) {
                                    var GetType = item.Etype;
                                    debugger
                                    switch (GetType) {
                                        case 'Termination':
                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/HrTerminationApprove?EmpId=" + item.emp_id,
                                                success: function (ResultData) {

                                                }
                                            });
                                            $("#" + "btn-Approve-" + item.emp_id).hide();
                                            $("#" + "Icon-Approved-" + item.emp_id).css("display", "inline");
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            $("#" + "btn-Initiate-" + item.emp_id).css("display", "inline");
                                            break;
                                        case 'CorrectiveAction':
                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/Corrective/EmployeeCorrectiveActionHRApproval?EmpId=" + item.emp_id,
                                                success: function (ResultData) {

                                                }
                                            });
                                            $("#" + "btn-Approve-" + item.emp_id).hide();
                                            $("#" + "Icon-Approved-" + item.emp_id).css("display", "inline");
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            //$("#" + "btn-Initiate-" + item.emp_id).css("display", "inline");
                                            break;
                                    }
                                    e.stopPropagation();
                                }).append($ApproveButtonName);

                            var $DenyButton = $("<button style='margin-left:3%; width: 55px; height: 26px;border-radius: 35px; background-color:#ff000073;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Deny-" + item.emp_id }).click(function (e) {
                                    var GetType = item.Etype;
                                    switch (GetType) {
                                        case 'Termination':
                                            $("#" + "btn-Approve-" + item.emp_id).hide();

                                            $("#myModalForDenialReason").modal();

                                            $("#SaveDenialReason").click(function () {
                                                $("#" + "btn-Deny-" + item.emp_id).hide();
                                                debugger
                                                var HRDenialReason = $("#TerminationHrDenial").serialize();
                                                if (HRDenialReason != null) {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: base_url + '/TerminationDetaills/HrDenialReason?EmpId=' + item.emp_id,
                                                        data: HRDenialReason,
                                                        success: function (ResultData) {

                                                        }


                                                    });
                                                }

                                            });
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            break;

                                        case 'CorrectiveAction':
                                            $("#" + "btn-Approve-" + item.emp_id).hide();

                                            $("#myModalForCADenialReason").modal();

                                            $("#SaveCADenialReason").click(function () {
                                                $("#" + "btn-Deny-" + item.emp_id).hide();
                                                debugger
                                                var HRDenialReason = $("#CorrectiveActionHrDenial").serialize();
                                                if (HRDenialReason != null) {
                                                    $.ajax({
                                                        type: "POST",
                                                        url: base_url + '/Corrective/EmployeeCorrectiveActionHrDenial?EmpId=' + item.emp_id,
                                                        data: HRDenialReason,
                                                        success: function (ResultData) {

                                                        }


                                                    });
                                                }

                                            });
                                            $("#" + "btn-Deny-" + item.emp_id).hide();
                                            break;

                                    }



                                    e.stopPropagation();
                                }).append($DenyButtonName);

                            var $ReviewButton = $("<span style='margin-left:2%; width: 17px; height: 25px;border-radius: 20px;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Review-" + item.emp_id }).click(function (e) {
                                    debugger
                                    var GetType = item.Etype;
                                    switch (GetType) {
                                        case 'Termination':

                                            debugger

                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/TerminationDetaills/GetTerminationDetailsForReview?EmpId=" + item.emp_id,
                                                success: function (ResultData) {
                                                    if (ResultData != null) {
                                                        $("#ReviewEmpId").text(ResultData.EmpId);
                                                        $("#ReviewEmpName").text(ResultData.Emp_Name);
                                                        //$("#ReviewTerminationStatus").text(ResultData.HR_Decision);
                                                        $("#ReviewReason").text(ResultData.HrDenyReason);
                                                        $("#ReviewComment").text(ResultData.HrDenyComment);
                                                        if (ResultData.HR_Decision == 'N') {
                                                            $("#ReviewTerminationStatus").text("Denied by HR");
                                                        }
                                                        else {
                                                            $("#ReviewTerminationStatus").text("Waiting");
                                                        }

                                                    }

                                                }


                                            });
                                            $("#myModalForManagerReview").modal();

                                            $("#ClickAccept").click(function () {
                                                debugger
                                                $.ajax({
                                                    type: "POST",
                                                    url: window.location.origin + '/TerminationDetaills/TerminationDenyAcknowledge?EmpId=' + item.emp_id,
                                                    success: function (result) {
                                                        $('#RenderPageId').html(result);
                                                    },

                                                });

                                            });


                                            break;
                                        case 'CorrectiveAction':

                                            debugger

                                            $.ajax({

                                                type: "POST",
                                                url: base_url + "/Corrective/CorrectiveActiondetailsForReview?EmpId=" + item.emp_id,
                                                success: function (ResultData) {
                                                    if (ResultData != null) {
                                                        $("#ReviewEmpId").text(ResultData.EmpId);
                                                        $("#ReviewEmpName").text(ResultData.Emp_Name);
                                                        //$("#ReviewTerminationStatus").text(ResultData.HR_Decision);
                                                        $("#ReviewReason").text(ResultData.HrDenyReason);
                                                        $("#ReviewComment").text(ResultData.HrDenyComment);
                                                        if (ResultData.HR_Decision == 'N') {
                                                            $("#ReviewTerminationStatus").text("Denied by HR");
                                                        }
                                                        else {
                                                            $("#ReviewTerminationStatus").text("Waiting");
                                                        }

                                                    }

                                                }


                                            });
                                            $("#myModalForManagerReview").modal();

                                            $("#ClickAccept").click(function () {
                                                debugger
                                                $.ajax({
                                                    type: "POST",
                                                    url: window.location.origin + '/Corrective/CorrectiveActionDenyAcknowledge?EmpId=' + item.emp_id,
                                                    success: function (result) {
                                                        $('#RenderPageId').html(result);
                                                    },

                                                });

                                            });


                                            break;

                                    }

                                    e.stopPropagation();
                                }).append($ReviewIcon);



                            var $InitiateMeeting = $("<button style='margin-left:2%; width: 102px; height: 26px;border-radius: 35px; background-color:#1e64cca6;'>")
                                .attr({ title: jsGrid.fields.control.prototype.filesButtonTooltip })
                                .attr({ id: "btn-Meeting-" + item.emp_id }).click(function (e) {
                                    debugger
                                    $.ajax({
                                        type: "POST",
                                        // data: { 'Id': item.id},

                                        url: base_url + '/Corrective/CorrectiveActionFormReview?EMPId=' + item.emp_id,

                                        beforeSend: function () {
                                            new fn_showMaskloader('Please wait...');
                                        },

                                        success: function (result) {
                                            $('#RenderPageId').html(result);
                                        },
                                        error: function (xhr, status, error) {

                                        },

                                        complete: function () {
                                            fn_hideMaskloader();
                                        }

                                    });




                                    e.stopPropagation();
                                }).append($StartMeetingButton);

                            debugger
                            switch ($_userTypeId) {

                                case "493":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($ApproveButton).append($ApprovedDiv).append($DenyButton);
                                    break;
                                case "1":
                                    if (item.status == "HR Approved") {
                                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($MeetingButton).append($ReviewButton).append($InitiateMeeting);
                                    }
                                    else {
                                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($MeetingButton).append($ReviewButton);
                                    }
                                    
                                    break;
                                case "2":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($MeetingButton).append($ReviewButton).append($InitiateMeeting);
                                    break;
                                case "5":
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($MeetingButton).append($ReviewButton).append($InitiateMeeting);
                                    break;
                                default:
                                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($MeetingButton).append($ReviewButton).append($InitiateMeeting);
                            }
                            //return $("<div>").attr({ class: "btn-toolbar" }).append($customEditB).append($AssetListB).append($ApproveButton).append($ApprovedDiv).append($DenyButton).append($InitiateTermination).append($FinalizeTermination);
                            break;
                            break;
                    }
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
   
}

//TerminationForm js to check severence
function IsSeverance(IsSeverancePaid, TerminationData) {

    function addDays(dateObj, numDays) {
        dateObj.setDate(dateObj.getDate() + numDays);
        return dateObj;
    }
    
    if (IsSeverancePaid == 'Y') {

        $("#myModalForSureTocheckIsSeverance").modal('show');

        $("#LengthOfSeverence").change(function () {
            debugger
            var Lengthofseverance = $("#LengthOfSeverence").val();
            var currentdate = new Date();
            var commencedate = addDays(currentdate, parseInt(Lengthofseverance));
            $("#SeveranceCommencementDate").text(commencedate);
            $("#SaveSeveranceInfo").click(function () {
                debugger
                TerminationData.push({ name: "IsSeverence", value: "Y" });
                TerminationData.push({ name: "LengthOfSeverence", value: Lengthofseverance });
                $.ajax({
                    type: "POST",
                    url: window.location.origin + "/TerminationDetaills/SaveTerminationForm",
                    data: TerminationData,
                    beforeSend: function () {
                        new fn_showMaskloader('Please wait...');
                    },

                    error: function (xhr, status, error) {

                    },
                    success: function (ResultData) {

                    },
                    complete: function () {
                        fn_hideMaskloader();
                    }



                });
            });
        });

    }
    else {
        TerminationData.push({ name: "IsSeverence", value: "N" });
        if (TerminationData != null) {
            $.ajax({
                type: "POST",
                url: window.location.origin + "/TerminationDetaills/SaveTerminationForm",
                data: TerminationData,
                success: function (resultdata) {

                }



            });

        }


    }

}



//$("#ManagerReview").click(function () {
//    debugger

//    $.ajax({

//        type: "POST",
//        url: base_url + "/TerminationDetaills/GetTerminationDetailsForReview",
//        success: function (ResultData) {
//            if (ResultData != null) {
//                $("#ReviewEmpId").text(ResultData.EmpId);
//                $("#ReviewEmpName").text(ResultData.Emp_Name);
//                //$("#ReviewTerminationStatus").text(ResultData.HR_Decision);
//                $("#ReviewReason").text(ResultData.HrDenyReason);
//                $("#ReviewComment").text(ResultData.HrDenyComment);
//                if (ResultData.HR_Decision == 'N') {
//                    $("#ReviewTerminationStatus").text("Denied by HR");
//                }
//                else {
//                    $("#ReviewTerminationStatus").text("Waiting");
//                }

//            }
            
//        }


//    });
//    $("#myModalForManagerReview").modal();

//    $("#ClickAccept").click(function () {
//        debugger
//        $.ajax({
//            type: "POST",
//            url: window.location.origin + '/TerminationDetaills/TerminationDenyAcknowledge',
//            success: function (result) {
//                $('#RenderPageId').html(result);
//            },

//        });

//    });

//});


