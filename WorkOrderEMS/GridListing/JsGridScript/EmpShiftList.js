var BillUrl = 'EPeople/GetListEmpWiseShift';
var id;
$(function () {

    $('#myModelRejectBill').on('hidden.bs.modal', function () {
        $("#backgroundDiv").css("display", "none");
    });

    var act;
    var _searchresult = $("#searchPreBilltext").val(); 
    $(function () {        
        
        $("#tbl_PreBillList").jsGrid({
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
                        url: $_HostPrefix + BillUrl + '?txtSearch=' + _searchresult,
                        data: filter,
                        dataType: "json"
                    });
                }
            },

            fields: [
                { name: "FirstName", title: "First Name", type: "text", width: 50 },
                { name: "LastName", title: "Last Name", type: "text", width: 50 },
                {
                    name: "UserId", title: "Action", width: 50, itemTemplate: function (value, item) {
                        var $iconPencilForDelete = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:green;font-size: 22px;" });
                        var $customDeleteButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Delete" })
                            .attr({ id: "btn-delete-" + item.UserId }).click(function (e) {
                                DeleteItem(item.UserId);
                            }
                            ).append($iconPencilForDelete);

                        return $("<div>").attr({ class: "btn-toolbar" }).append($customDeleteButton);
                    }
                },
            ]
        });
    });
   
     

    $("#searchPreBilltext").keyup(function () {
        doSearch();
    });
});

var timeoutHnd;
var flAuto = true;
function doSearch() {

    var act;
    var _searchresult = $("#searchPreBilltext").val();
    $("#tbl_PreBillList").jsGrid({
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
                    url: $_HostPrefix + BillUrl + '?txtSearch=' + _searchresult,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "FirstName", title: "First Name", type: "text", width: 50 },
            { name: "LastName", title: "Last Name", type: "text", width: 50 }
        ]
    });
}
function DeleteItem(UserId) {
    var addNewUrl = "/EPeople/EmpWiseShiftDelete?UserId=" + UserId;
    $('#RenderPageId').load(addNewUrl);
}

 
 