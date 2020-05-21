var HOBurl = '../EPeople/GetListOfHolidayForJSGrid';


var clients;
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
(function ($) {
    'use strict'
    var data;
    $("#ListHolidayManagement").jsGrid({
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
                    url: HOBurl + '?locationId=' + $_LocationId +'&Typ=0',
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            
            { name: 'Id', width: 20 , title: "Id" },
            { name: 'HolidayName', width: 100, title: "Holiday Name" },
            { name: 'HolidayDates', width: 60, title: "Holiday Date" },
            { name: 'Description', width: 100, title: "Description" },
            { name: 'HolidayType', width: 60, title: "Holiday Type" }
        ],
        rowClick: function (args) {

            var getData = args.item;
            var keys = Object.keys(getData);
            var text = [];
        }
    });

    

})(jQuery);


