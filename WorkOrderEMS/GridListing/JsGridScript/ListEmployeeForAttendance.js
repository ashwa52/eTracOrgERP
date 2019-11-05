var HOBurl = '../eTime/GetAttendanceDataUserwise';
var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
var $_MonthVal = parseInt(new Date().getMonth() + 1);
$("#MonthList").val($_MonthVal);
$("#MonthList").change(function () {
    $_MonthVal = $(this).val();
    $("#ListEmployeeManagement").jsGrid("loadData");
});
$("#PunchTime").click(function () {
    $.ajax({
        type: "GET",
        data: { 'Id': item.id, 'LocationId': $("#drp_MasterLocation1 option:selected").val() },
        url: '../EPeople/GetUserListByUserId/',
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            if (result !== null) {
                $("#ViewUserListDetails").html(result);
                $("#myModalForUserList").modal("show");
                //var bodyId = $('#viewProfileData').append('<div class=div_' + result + '><img src='+result.ProfileImage+'/></div><br>');
                //if(result.ChildrenList.legth > 0)
                //{
                //    for(var i=0;i<result.ChildrenList.legth;i++){
                //        bodyId.append('<span><img src='+result.ChildrenList[i].ProfileImage+'</span>')
                //    }                                                     
                //}
                //$("#ModalProfilePreview").modal('show');
            }
        }
    });
});
(function ($) {
    'use strict'
    var data;
    $("#ListEmployeeManagement").jsGrid({
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
                    url: HOBurl + '?LocationId=' + $_LocationId + '&Month=' + $_MonthVal,
                    datatype: 'json',
                    contentType: "application/json"
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            {
                name: "DatumString", title: "Day", width: 30, //formatter: 'date', formatoptions: { newformat: 'd/m/Y' },
                itemTemplate: function (val, item) {
                    return $("<label>" + val + "<br/>" + item.dyName +  "</label>");
                }
            },
            { name: 'InTimeString', width: 80, title: "In Time" },//visible: true
            { name: 'OutTimeString', width: 60, title: "Out Time" },
            { name: 'TotalHoursString', width: 60, title: "Total Time" },
            {
                name: "AttendanceView", title: "Status", width: 30, css: "text-center", itemTemplate: function (value, item) {
                    console.log(item);
                    var $DiagramView = $("<span>");
                    //if (item.OutTime !== null || item.OutTime !== undefined) {
                    //    $DiagramView = $("<span>").attr({ class: "fa fa-clock-o fa-2x" }).attr({ style: "color:green;" });
                    //}

                    if (item.Present === "A") {
                        $DiagramView = $("<span>").attr({ class: "fa fa-clock-o fa-2x" }).attr({ style: "color:red;" }).attr({ title: "Absent" });
                    } else if (item.Present === "L") {
                        $DiagramView = $("<span>").attr({ class: "fa fa-clock-o fa-2x" }).attr({ style: "color:yellow;" }).attr({ title: "On Leave" });
                    } else if (item.Present === "P") {
                        $DiagramView = $("<span>").attr({ class: "fa fa-clock-o fa-2x" }).attr({ style: "color:green;" }).attr({ title: "Present" });
                    }
                               
                    return $("<div>").attr({ class: "btn-toolbar" }).append($DiagramView);
                           }
                       }
        ]
    });
})(jQuery);
