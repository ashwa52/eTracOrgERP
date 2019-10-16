//var DebitMemourl = 'DebitMemo/GetDebitMemoListByLocationId';
//$(function () {

//    $("#searchDebitMemotext").keyup(function () {
//        DoDeibitSearch()
//    });

//    var act;
//    $("#jsGrid_DebitMemoList").jsGrid({
//        height: "170%",
//        width: "100%",
//        filtering: false,
//        editing: false,
//        inserting: false,
//        sorting: false,
//        paging: true,
//        autoload: true,
//        pageSize: 10,
//        pageButtonCount: 5,

//        controller: {
//            loadData: function (filter) {
//                return $.ajax({
//                    type: "GET",
//                    url: DebitMemourl,
//                    data: filter,
//                    dataType: "json"
//                });
//            }
//        },

//        fields: [            
//            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
//            { name: "DebitAmount", title: "Debit Amount", type: "text", width: 50 },
//            { name: "DisplayDate", title: "Date", type: "text", width: 50 },
//            { name: "Note", title: "Note", type: "text", width: 50 },
//            { name: "DisplayDebitMemoStatus", title: "Status", type: "text", width: 50 },            
//            {
//                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
//                    //TO add icon edit and delete to perform update and delete operation
//                    var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:green;font-size: 22px;" });
//                    var $iconList = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });

//                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
//                        .attr({ title: "Edit Debit Memo Details" })
//                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
//                            ViewDetails(item);
//                        }).append($iconPencil);

//                    var $customCheck = $("<span style='padding: 0 5px 0 0;'>")
//                        .attr({ title: "View Debit Memo" })
//                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
//                            ViewDetailsOnly(item);
//                        }).append($iconList);

//                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customCheck);                  
//                }
//            }
//        ]
//    });


//    function DoDeibitSearch()
//    {
//        var act;
//        var _searchresult = $("#searchDebitMemotext").val();

//        var act;
//        $("#jsGrid_DebitMemoList").jsGrid({
//            height: "170%",
//            width: "100%",
//            filtering: false,
//            editing: false,
//            inserting: false,
//            sorting: false,
//            paging: true,
//            autoload: true,
//            pageSize: 10,
//            pageButtonCount: 5,

//            controller: {
//                loadData: function (filter) {
//                    return $.ajax({
//                        type: "GET",
//                        url: DebitMemourl + '?txtSearch=' + _searchresult,
//                        data: filter,
//                        dataType: "json"
//                    });
//                }
//            },

//            fields: [
//                { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
//                { name: "DebitAmount", title: "Debit Amount", type: "text", width: 50 },
//                { name: "DisplayDate", title: "Date", type: "text", width: 50 },
//                { name: "Note", title: "Note", type: "text", width: 50 },
//                { name: "DisplayDebitMemoStatus", title: "Status", type: "text", width: 50 },
//                {
//                    name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
//                        //TO add icon edit and delete to perform update and delete operation
//                        var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:green;font-size: 22px;" });
//                        var $iconList = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });

//                        var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
//                            .attr({ title: "Edit Debit Memo Details" })
//                            .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
//                                ViewDetails(item);
//                            }).append($iconPencil);

//                        var $customCheck = $("<span style='padding: 0 5px 0 0;'>")
//                            .attr({ title: "View Debit Memo" })
//                            .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
//                                ViewDetailsOnly(item);
//                            }).append($iconList);                       

//                        return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customCheck);
//                    }
//                }
//            ]
//        });
//    }

//    function ViewDetails(item)
//    {
//        $("#ActionMode").val("U");
//        debugger;//myAddModalLabel
//        //$("#Location").val(item.Location);
//        //$("#Vendor").val(item.VendorId);//ProductOrderId ProductOrderId
//        //$("#ProductOrderId").val(item.PurchaseOrderId);
//        //$("#lblDebitAmount").val(item.DebitAmount);
//        //$("#DebitMemoStatus").val(item.Status);//DebitMemoStatus
//        //$("#Note").val(item.Note);//UploadedDocumentName

//        $("#lblEditLocationName").html(item.LocationName);
//        $("#lblEditVendor").html(item.VendorName);//ProductOrderId ProductOrderId
//        $("#lblEditPoOrderId").html(item.ProductOrderName);
//        $("#lblEditDebitAmount").html(item.DebitAmount);
//        $("#lblEditNote").html(item.Note);//UploadedDocumentName
//        $("#DebitMemoStatusEdit").val(item.Status);//DebitMemoStatus //DebitMemoStatusEdit
        


//        $("#UploadedDocumentName").val(item.UploadedDocumentName);
//        $("#UploadedEditDocumentName").val(item.UploadedEditDocumentName);
        

//        //$("#myAddModalLabel").hide();
//        //$("#myEditModalLabel").show();
//        //$("#myModalForNewDebitMemo").modal('show');myModalForEditDebitMemo
//        $("#myModalForEditDebitMemo").modal('show');
//    }

//    function ViewDetailsOnly(item) {

//        debugger;
//        $("#lblDMId").html(item.DebitId);
//        $("#lblDMLocation").val(item.LocationName);        
//        $("#lblVendorName").html(item.VendorName);
//        $("#lblPOName").html(item.ProductOrderName);
//        $("#lblDebitAmount").html(item.DebitAmount);
//        $("#lblDmStatus").html(item.DisplayDebitMemoStatus);
//        $("#lblDebitNotes").html(item.Note);
//        $("#lblDMDate").html(item.DisplayDate);
        
//        $("#UploadedDocumentName").val(item.UploadedDocumentName);//editNewDocument
//        //$("#editNewDocument").val(item.editNewDocument);
//        $("#myModalForDMDetails").modal('show');
        
//    }    

//    $("#AddDebitMemo").on("click", function (event) {

//        //$("#myAddModalLabel").show();
//        //$("#myEditModalLabel").hide();

//        $("#Location").val($("#Location option:first").val());//selecting top index
//        $("#Vendor").val($("#Vendor option:first").val());    //selecting top index    
//        $("#ProductOrderId").val($("#ProductOrderId option:first").val());//selecting top index

//        $("#DebitAmount").val("");
//        $("#DebitAmount").attr("placeholder", "Please enter debit amount").val("").focus().blur();//for changing placeholder
        
//        $("#Note").val("");//UploadedDocumentName
//        $("#Note").attr("placeholder", "Enter note regarding debit memo").val("").focus().blur();//for changing placeholder   
        
//        $("#DebitMemoStatus").val($("#DebitMemoStatus option:first").val());//selecting top index

//        $("#UploadedDocumentName").val("");

//        $("#ActionMode").val("I");        

//        $("#myModalForNewDebitMemo").modal('show');
        
//        //window.location.href = $_HostPrefix + AddCompany;
//    });
    
//});