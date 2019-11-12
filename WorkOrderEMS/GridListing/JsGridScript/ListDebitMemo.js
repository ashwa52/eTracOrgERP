var DebitMemourl = 'DebitMemo/GetDebitMemoListByLocationId';
function downloadFile() {
    var urlToSend = $("#UploadedDocumentName").val();

    var req = new XMLHttpRequest();
    req.open("GET", urlToSend, true);
    req.responseType = "blob";
    req.onload = function (event) {
        var blob = req.response;
        //var fileName = req.getResponseHeader("fileName") //if you have the fileName header available
        var link = document.createElement('a');
        link.href = window.URL.createObjectURL(blob);
        link.download = urlToSend;
        link.click();
    };
    req.send();
}

function getVendorList(IsRecurring) {
    var Location = $("#Location").val();
    $.ajax({
        url: $_HostPrefix + 'POTypeData/GetVendorList',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ Location: +Location, IsRecurring: IsRecurring }),
        success: function (result) {
            $("#Vendor").html("");
            $("#Vendor").append
                ($('<option></option>').val(null).html("---Select Vendor---"));
            for (var i = 0; i < result.length; i++) {
                $("#Vendor").append('<option value=' + result[i].CompanyId + '>' + result[i].CompanyNameLegal + '</option>');
            }
        },
        error: function () { alert(" Something went wrong..") },
    });
}

function getProductOrderList(data) {
    var POUrl = 'POTypeData/GetAllPOList';
    var Location = $("#Location").val();
    $.ajax({
        type: "GET",
        url: $_HostPrefix + POUrl + '?VendorId=' + data + '&Location=' + Location,
        dataType: "json",
        success: function (result) {
            $("#ProductOrderId").html("");
            $("#ProductOrderId").append
                ($('<option></option>').val(null).html("---Select Po List---"));
            for (var i = 0; i < result.length; i++) {
                $("#ProductOrderId").append('<option value=' + result[i].LogPOId + '>' + result[i].DisplayLogPOId + '</option>');
            }
        },
        error: function () { alert(" Something went wrong..") }
    });
}

function checkvalidation() {
    //Vendor
    var textStatus = $("#DebitMemoStatus option:selected").val();
    var textLocation = $("#Location option:selected").val();
    var textDebitAmount = $("#DebitAmount").val();
    var textVendor = $("#Vendor").val();
    if (textLocation == "") {
        alert("Please select the location");
        $("#Location").focus();
        return false;
    }
    if (textVendor == "") {
        alert("Please select the Vendor");
        $("#Vendor").focus();
        return false;
    }
    if (textDebitAmount == "") {
        alert("Please Enter debit amount");
        $("#DebitAmount").focus();
        return false;
    }
    if (!($.isNumeric(textDebitAmount))) {
        alert("Please Enter valid amount");
        $("#DebitAmount").val("");
        $("#DebitAmount").focus();
        return false;
    }
    if (textStatus == "0") {
        alert("Please select status");
        $("#DebitMemoStatus").focus();

        return false;
    }
    return true;
}

function checkvalidationForEdit() {
    var textStatus = $("#DebitMemoStatusEdit option:selected").val();
    if (textStatus == "0") {
        alert("Please select status");
        $("#DebitMemoStatusEdit").focus();
        return false;
    }
    return true;
}

$(function () {

    $("#fileuploadCompany").change(function () {
        $("#dvCompanyPreview").html("");
        var allowedExtensions = /(\.pdf|\.doc|\.docx|\.txt)$/i;
        var size = this.files[0].size;
        if (!allowedExtensions.exec($(this).val().toLowerCase())) {
            alert("Invalid file type");
            $(this).val("");
            return false;
        }
        else if (size >= 4194304) {
            alert("Max file size 4 mb.");
            $(this).val("");
            return false;
        }
    })
    // this will be called on submit
    $("#debitMemoForm").submit(function (event) {


        if (checkvalidation() == false) {
        } else {
            var dataString;
            event.preventDefault();

            if ($("#debitMemoForm").attr("enctype") == "multipart/form-data") {
                //this only works in some browsers.
                //purpose? to submit files over ajax. because screw iframes.
                //also, we need to call .get(0) on the jQuery element to turn it into a regular DOM element so that FormData can use it.
                dataString = new FormData($("#debitMemoForm").get(0));
                contentType = false;
                processData = false;
            } else {
                // regular form, do your own thing if you need it
            }
            $.ajax({
                type: "POST",
                url: 'DebitMemo/SaveDebitMemoByVendorId',
                data: dataString,
                dataType: "json", //change to your own,
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    debugger;
                    if (data == true) {
                        DoDeibitSearch();
                        $("#myModalForNewDebitMemo").modal('hide');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //do your own thing
                    alert("fail");
                }
            });
        }

    }); //end .submit()

    $("#debitMemoEditForm").submit(function (event) {

        if (checkvalidationForEdit()) {
            var dataString;
            event.preventDefault();

            if ($("#debitMemoEditForm").attr("enctype") == "multipart/form-data") {
                //this only works in some browsers.
                //purpose? to submit files over ajax. because screw iframes.
                //also, we need to call .get(0) on the jQuery element to turn it into a regular DOM element so that FormData can use it.
                dataString = new FormData($("#debitMemoEditForm").get(0));
                contentType = false;
                processData = false;
            } else {
                // regular form, do your own thing if you need it
            }
            $.ajax({
                type: "POST",
                url: 'DebitMemo/SaveDebitMemoByVendorId',
                data: dataString,
                dataType: "json", //change to your own,
                contentType: contentType,
                processData: processData,
                success: function (data) {
                    debugger;
                    if (data == true) {
                        DoDeibitSearch();
                        $("#myModalForEditDebitMemo").modal('hide');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    //do your own thing
                    alert("fail");
                }
            });
        }
    }); //end .submit()

    $("#searchDebitMemotext").keyup(function () {
        DoDeibitSearch()
    });

    $("#AddDebitMemo").on("click", function (event) {

        $("#ActionModeU").val("");
        $("#ActionModeI").val("I");

        $("#Location").val($("#Location option:first").val());//selecting top index
        $("#Vendor").val($("#Vendor option:first").val());    //selecting top index
        $("#ProductOrderId").val($("#ProductOrderId option:first").val());//selecting top index

        $("#DebitAmount").val("");
        $("#DebitAmount").attr("placeholder", "Please enter debit amount").val("").focus().blur();//for changing placeholder

        $("#Note").val("");//UploadedDocumentName
        $("#Note").attr("placeholder", "Enter note regarding debit memo").val("").focus().blur();//for changing placeholder

        $("#DebitMemoStatus").val($("#DebitMemoStatus option:first").val());//selecting top index

        $("#UploadedDocumentName").val("");

        $("#ActionModeI").val("I");

        $("#myModalForNewDebitMemo").modal('show');

        //window.location.href = $_HostPrefix + AddCompany;
    });

    var act;
    $("#jsGrid_DebitMemoList").jsGrid({
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
                    url: DebitMemourl,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "DebitAmount", title: "Debit Amount", type: "text", width: 50 },
            { name: "DisplayDate", title: "Date", type: "text", width: 50 },
            { name: "Note", title: "Note", type: "text", width: 50 },
            { name: "DisplayDebitMemoStatus", title: "Status", type: "text", width: 50 },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconList = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "Edit Debit Memo Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);
                        }).append($iconPencil);

                    var $customCheck = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Debit Memo" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetailsOnly(item);
                        }).append($iconList);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customCheck);
                }
            }
        ]
    });
});

function DoDeibitSearch() {
    var act;
    var _searchresult = $("#searchDebitMemotext").val();

    var act;
    $("#jsGrid_DebitMemoList").jsGrid({
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
                    url: DebitMemourl + '?txtSearch=' + _searchresult,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "VendorName", title: "Vendor Name", type: "text", width: 50 },
            { name: "DebitAmount", title: "Debit Amount", type: "text", width: 50 },
            { name: "DisplayDate", title: "Date", type: "text", width: 50 },
            { name: "Note", title: "Note", type: "text", width: 50 },
            { name: "DisplayDebitMemoStatus", title: "Status", type: "text", width: 50 },
            {
                name: "act", items: act, title: "View Details", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconList = $("<i>").attr({ class: "fa fa-list" }).attr({ style: "color:black;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "Edit Debit Memo Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);
                        }).append($iconPencil);

                    var $customCheck = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Debit Memo" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetailsOnly(item);
                        }).append($iconList);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customCheck);
                }
            }
        ]
    });
}

//veiw details for edit
function ViewDetails(item) {
    $("#ActionModeU").val("U");
    $("#ActionModeI").val("");
    //debugger;//myAddModalLabel
    //$("#Location").val(item.Location);
    //$("#Vendor").val(item.VendorId);//ProductOrderId ProductOrderId
    //$("#ProductOrderId").val(item.PurchaseOrderId);
    //$("#lblDebitAmount").val(item.DebitAmount);
    //$("#DebitMemoStatus").val(item.Status);//DebitMemoStatus
    //$("#Note").val(item.Note);//UploadedDocumentName

    $("#DebitId").val(item.DebitId);
    $("#lblEditLocationName").html(item.LocationName);
    $("#lblEditVendor").html(item.VendorName);//ProductOrderId ProductOrderId
    $("#lblEditPoOrderId").html(item.ProductOrderName);
    $("#lblEditDebitAmount").html(item.DebitAmount);
    $("#lblEditNote").html(item.Note);//UploadedDocumentName
    $("#DebitMemoStatusEdit").val(item.Status);//DebitMemoStatus //DebitMemoStatusEdit
    
    $("#editNewDocument").val("");//to set edit

    $("#UploadedDocumentName").val(item.UploadedDocumentName);
    $("#UploadedEditDocumentName").val(item.UploadedEditDocumentName);
    
    $("#myModalForEditDebitMemo").modal('show');
}

function ViewDetailsOnly(item) {
    
    $("#lblDMId").html(item.DebitId);
    $("#lblDMLocation").val(item.LocationName);
    $("#lblVendorName").html(item.VendorName);
    $("#lblPOName").html(item.ProductOrderName);
    $("#lblDebitAmount").html(item.DebitAmount);
    $("#lblDmStatus").html(item.DisplayDebitMemoStatus);
    $("#lblDebitNotes").html(item.Note);
    $("#lblDMDate").html(item.DisplayDate);
    
    $("#UploadedDocumentName").val(item.UploadedDocumentName);
    if (item.UploadedDocumentName == "") {
        $("#fileForDownload").hide();
        $("#nofilemsg").html("No file found");
    } else {
        $("#fileForDownload").show();
        $("#nofilemsg").html("");
    }
    
    $("#myModalForDMDetails").modal('show');

}