var CreditInvoiceListUrl = '/InvoiceManagement/GetAllCreditInvoiceList';
var CreateCreditInvoice = '/InvoiceManagement/CreateCreditInvoice/';
var EditCreateCreditInvoice = '/InvoiceManagement/EditCreateCreditInvoice/';
var ViewInvoiceDetails = '/InvoiceManagement/GetCreditInvoiceDataToView/';
var ViewPDF = '/InvoiceManagement/PrintInvoicePDF/';

var GETPDF = '/InvoiceManagement/ExportResultPDF/';
var DownloadPDF = '/InvoiceManagement/DownloadExportFile/'

var LocationId; var InvoiceId; var ClientLocationCode;

$(function () {
    var act;
    $("#jsGrid-basic").jsGrid({
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
        pageIndex: 1,
        noDataContent: "Data Not Available",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",  
                    url: CreditInvoiceListUrl + '?flagApproved=N' + "&LocationId=" + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },
        fields: [
            { name: "CreditMemoNumber", title: "Credit Memo Number", type: "text", width: 50 },
            { name: "InvoiceNumber", title: "Invoice Number", type: "text", width: 50 },
            { name: "CreditIssuedDateDisplay", title: "Credit Issue Date", type: "text", width: 50 },
            { name: "CreditMemoTypeDesc", title: "Credit Type", type: "text", width: 50 },
            { name: "GrandTotal", title: "Invoice Total", type: "text", width: 50, align: "right" },
            { name: "TotalCreditAmount", title: "Credit Total", type: "text", width: 50, align: "right" },
            { name: "CreditMemoStatusDesc", title: "Credit Status", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconPdfForView = $("<i>").attr({ class: "fa fa-file-pdf-o" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconForApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconClose = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    
                    var $customEditButton = "";
                    if (item.CreditMemoStatus == "1" || item.CreditMemoStatus == "0") {
                        $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Edit" })
                            .attr({ id: "btn-edit-" + item.CreditMemoId }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},
                                    url: EditCreateCreditInvoice + item.CreditMemoId + "?IsDraft=" + item.IsDraft,
                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    error: function (xhr, status, error) {
                                    },
                                    success: function (result) {
                                        $("#divInvoiceData").html('');
                                        $("#divInvoiceData").html(result);
                                        $("#myModalForGetInvoiceDateForCreditMemo").modal("show");
                                    },
                                    complete: function () {
                                        fn_hideMaskloader();
                                    }
                                });
                            }).append($iconPencilForEdit);
                    } else {
                        $customEditButton = "";
                        //$customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Edit." })
                        //    .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        //    }).append($iconPencilForEdit);
                    }
                    var $customButtonForView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View" })
                        .attr({ id: "btn-edit-" + item.CreditMemoId }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.CreditMemoStatus == "0") {
                                ViewCreditInvoiceDetails(item, true);
                            }
                            else {
                                ViewCreditInvoiceDetails(item, false);
                            }
                        }).append($iconPencilForView);

                    var $customButtonForPdfView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View PDF" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.CreditMemoStatus == "0") {
                                ViewCreditMemo_PDF(item, true);
                            }
                            else {
                                ViewCreditMemo_PDF(item, false);
                            }
                        }).append($iconPdfForView);

                    var $customButtonForApprove = "";
                    if (item.CreditMemoStatus == "1") {
                        $customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Approve" })
                            .attr({ id: "btn-Approve-" + item.CreditMemoId }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                ApproveDetails(item);
                            }).append($iconForApprove);

                    } else {
                        $customButtonForApprove = "";
                        //$customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Approve." })
                        //    .attr({ id: "btn-Approve-" + item.Id }).click(function (e) {
                        //    }).append($iconForApprove);
                    }

                    var $customButtonForCancel = "";
                    if (item.CreditMemoStatus == "1") {
                        $customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Cancel Credit Invoice" })
                            .attr({ id: "CancelBill" + item.CreditMemoId }).click(function (e) {
                                ShowCancelInvoiceModal(item);
                            }).append($iconClose);
                    } else {
                        $customButtonForCancel = "";
                        //$customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Cancel." })
                        //    .attr({ id: "btn-cancel-" + item.Id }).click(function (e) {
                        //    }).append($iconClose);
                    }

                    return $("<div>").attr({ class: "btn-toolbar" })
                        .append($customEditButton)
                        .append($customButtonForView)
                        .append($customButtonForPdfView)
                        .append($customButtonForApprove)
                        //.append($customButtonForCancel);
                }
            }
        ],
        onRefreshed: function (args) {
            var items = args.grid.option("data");
            var total = { Name: "GrandTotal", "GrandTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                total.GrandTotal += item.GrandTotal;
            });

            var totalPayback = { Name: "TotalCreditAmount", "TotalCreditAmount": 0, IsTotal: false };
            items.forEach(function (item) {
                totalPayback.TotalCreditAmount += item.TotalCreditAmount;
            });

            var $totalRow = $("<tr>");
            args.grid._renderCells($totalRow, total);
            args.grid._renderCells($totalRow, totalPayback);
            args.grid._content.append("<tr class=\"GridTotal\">"
                + "<td></td>" + "<td></td>" + "<td></td>"
                + "<td class=\"GridTotal\"><b>Total : </b></td>" + "<td class=\"GridTotal\"><b>" + total.GrandTotal.toFixed(2) + "</b><td><b>" + totalPayback.TotalCreditAmount.toFixed(2) + "</b></td>" + "<td></td></tr>");
        },
    });

});

function ViewCreditInvoiceDetails(item,IsDraft) {
    $.ajax({
        type: "POST",
        url: ViewInvoiceDetails + '/' + item.CreditMemoId + "?IsDraft=" + IsDraft,
        datatype: 'json',
        success: function (result) {
            if (result.CreditMemoStatus == "0") {
                $("#LblCreditMemoNumberHeader,#lblCreditMemoNumber").hide();
                $("#LblDraftCreditMemoNumberHeader,#lblDraftCreditMemoNumber").show();
            }
            else {
                $("#LblCreditMemoNumberHeader,#lblCreditMemoNumber").show();
                $("#LblDraftCreditMemoNumberHeader,#lblDraftCreditMemoNumber").hide();
            }
            $("#lblCreditMemoNumber").html(result.CreditMemoNumber);
            $("#lblDraftCreditMemoNumber").html(result.DraftCreditMemoNumber);
            $("#lblclientcompanyName").html(result.ClientCompanyName);
            $("#lblclientlocationName").html(result.ClientLocationName);
            $("#lblCustomerType").html(result.ContractTypeDesc);
            $("#lblEmployeeIssuingCredit").html(result.EntryByDisplay);
            $("#lblClientPointofContactName").html(result.ClientPointOfContactName);
            $("#lblPositionTitle").html(result.PositionTitle);
            $("#lblInvoiceNumber").html(result.InvoiceNumber);
            $("#lblInvoiceDate").html(result.InvoiceDateDisplay);
            $("#lblCreditIssuedDate").html(result.CreditIssuedDateDisplay);
            $("#lblLocationCode").html(result.LocationCode);
            $("#lblLocationAddress").html(result.LocationAddress);
            $("#lblInvoiceCreateDate").html(result.EntryOnDisplay);
            $("#lblInvoiceCreateBy").html(result.EntryByDisplay);
            $("#lblInvoiceModifiedDate").html(result.ModifiedOnDisplay);
            $("#lblInvoiceModifiedBy").html(result.ModifiedByDisplay);
            $("#lblCreditMemoTypeDesc").html(result.CreditMemoTypeDesc);
            //$("#lblInvoiceTotal").html(result.GrandTotal);
            //$("#lblInvoiceDueDate").html(result.InvoiceDueDateDisplay);
            //$("#lblInvoiceBalance").html(result.PendingAmount);
            $("#lblDateLastSentToclient").html(result.InvoiceLastSenttoclientDate);
            $("#lblApproveRejectBy").html(result.ApprovedByDisplay);
            $("#lblReceivedDate").html(result.ApprovedOnDisplay);
            $("#lblStatus").html(result.CreditMemoStatusDesc);
            $("#lblComment").html(result.Comment);
            $('#tblInvoiceDocuments tbody').empty();
            $('#tblInvoiceDocuments').html("");

            if (result.ListInvoiceItemDetails != null) {
                $('#records_table').html('');
                $('#tblInvoiceDetails').html("");
                $('#tblInvoiceDocuments tbody').empty();
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;">'
                    + '<th>Item</th>'
                    + '<th> Item Type</th>'
                    + '<th> Revenue A/C</th>'
                    + '<th>Qty</th> '
                    + '<th> Unit Cost</th>'
                    //+ '<th> Tax(%)</th>'
                    //+ '<th>Tax Amount</th>'
                    + '<th>Total Cost</th>'
                    + '<th>Credit Amount</th>'
                    + ' <th>Credit Reason</th></tr > ';
                $('#tblInvoiceDetails').append(thHTML);
                if (result.ListInvoiceItemDetails.length > 0) {
                    for (i = 0; i < result.ListInvoiceItemDetails.length; i++) {
                        var trHTML = '';
                        trHTML +=
                            '<tr><td>' + result.ListInvoiceItemDetails[i].ItemDescription +
                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemTypeDesc +
                            '</td><td>' + result.ListInvoiceItemDetails[i].RevenueAccountDesc +
                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemQty +
                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemUnitCost +
                            //'</td><td>' + result.ListInvoiceItemDetails[i].TaxPercentage +
                            //'</td><td>' + result.ListInvoiceItemDetails[i].TaxAmount +
                            '</td><td>' + result.ListInvoiceItemDetails[i].TotalCost +
                            '</td><td>' + result.ListInvoiceItemDetails[i].CreditAmt +
                            '</td><td>' + result.ListInvoiceItemDetails[i].CreditReasonDesc +
                            '</td></tr>';

                        $('#tblInvoiceDetails').append(trHTML);
                    }
                    var footer = "<tr><td></td><td></td><td></td><td></td><td><b>Total:</b></td><td><b>" + /*result.TaxAmount*/ result.GrandTotal + "</b></td><td><b>" + result.TotalCreditAmount + "</b></td><td></td></tr>";
                    $('#tblInvoiceDetails').append(footer);
                }
                var thHTML1 = '';
                thHTML1 += '<tr style="background-color:#0792bc;">'
                    + '<th></th>'
                    + '<th>Document Name</th>'
                    + '<th>Modified Date</th>'
                    + '<th>Uploaded By</th></tr>'
                    + '<tr><td><a class="btn btn-secondary" style="border-radius:25px;width:90px;" href="' + $_hostingPrefix + 'Content\\eCountingDocs\\InvoiceDocs\\' + result.InvoiceDocument + '" download>Download</a></td><td>' + result.InvoiceDocument + '</td><td></td><td></td></tr>'
                $('#tblInvoiceDocuments').append(thHTML1);
            }
            $('.modal-title').text("Credit Invoice Details");
            $("#myModalViewCreditInvoiceDetails").modal('show');
            new fn_hideMaskloader();
        },
        error: function () {
            new fn_hideMaskloader();
        }
    });
}
function ViewCreditMemo_PDF(item, IsDraft) {
    $.ajax({
        type: "POST",
        url: GETPDF + item.CreditMemoId + "?IsDraft=" + IsDraft,
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        new fn_hideMaskloader();
        window.location.href = DownloadPDF + "?stFile=" + data.aaData;
    })
}
//function ViewCreditMemo_PDF(item, IsDraft) {
//    $.ajax({
//        type: "POST",
//        url: ViewInvoiceDetails + '/' + item.CreditMemoId + "?IsDraft=" + IsDraft,
//        datatype: 'json',
//        async:false,
//        success: function (result) {
//            if (result.CreditMemoStatus == "0") {
//                $("#LblCreditMemoNumberHeader,#lblCreditMemoNumber").hide();
//                $("#LblDraftCreditMemoNumberHeader,#lblDraftCreditMemoNumber").show();
//            }
//            else {
//                $("#LblCreditMemoNumberHeader,#lblCreditMemoNumber").show();
//                $("#LblDraftCreditMemoNumberHeader,#lblDraftCreditMemoNumber").hide();
//            }
//            $("#lblCreditMemoNumber").html(result.CreditMemoNumber);
//            $("#lblDraftCreditMemoNumber").html(result.DraftCreditMemoNumber);
//            $("#lblclientcompanyName").html(result.ClientCompanyName);
//            $("#lblCustomerType").html(result.ContractTypeDesc);
//            $("#lblEmployeeIssuingCredit").html(result.EntryByDisplay);
//            $("#lblClientPointofContactName").html(result.ClientPointOfContactName);
//            $("#lblPositionTitle").html(result.PositionTitle);
//            $("#lblInvoiceNumber").html(result.InvoiceNumber);
//            $("#lblInvoiceDate").html(result.InvoiceDateDisplay);
//            $("#lblCreditIssuedDate").html(result.CreditIssuedDateDisplay);
//            $("#lblLocationCode").html(result.LocationCode);
//            $("#lblLocationAddress").html(result.LocationAddress);
//            $("#lblInvoiceCreateDate").html(result.EntryOnDisplay);
//            $("#lblInvoiceCreateBy").html(result.EntryByDisplay);
//            $("#lblInvoiceModifiedDate").html(result.ModifiedOnDisplay);
//            $("#lblInvoiceModifiedBy").html(result.ModifiedByDisplay);
//            $("#lblCreditMemoTypeDesc").html(result.CreditMemoTypeDesc);
//            //$("#lblInvoiceTotal").html(result.GrandTotal);
//            //$("#lblInvoiceDueDate").html(result.InvoiceDueDateDisplay);
//            //$("#lblInvoiceBalance").html(result.PendingAmount);
//            $("#lblDateLastSentToclient").html(result.InvoiceLastSenttoclientDate);


//            $('#tblInvoiceDocuments tbody').empty();
//            $('#tblInvoiceDocuments').html("");

//            if (result.ListInvoiceItemDetails != null) {
//                $('#records_table').html('');
//                $('#tblInvoiceDetails').html("");
//                $('#tblInvoiceDocuments tbody').empty();
//                var thHTML = '';
//                thHTML += '<tr style="background-color:#0792bc;">'
//                    + '<th>Item</th>'
//                    + '<th> Item Type</th>'
//                    + '<th> Revenue A/C</th>'
//                    + '<th>Qty</th> '
//                    + '<th> Unit Cost</th>'
//                    + '<th> Tax(%)</th>'
//                    + ' <th>Tax Amount</th>'
//                    + '<th>Total Cost</th></tr > ';
//                $('#tblInvoiceDetails').append(thHTML);
//                if (result.ListInvoiceItemDetails.length > 0) {
//                    for (i = 0; i < result.ListInvoiceItemDetails.length; i++) {
//                        var trHTML = '';
//                        trHTML +=
//                            '<tr><td>' + result.ListInvoiceItemDetails[i].ItemDescription +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemTypeDesc +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].RevenueAccountDesc +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemQty +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].ItemUnitCost +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].TaxPercentage +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].TaxAmount +
//                            '</td><td>' + result.ListInvoiceItemDetails[i].TotalCost +
//                            '</td></tr>';

//                        $('#tblInvoiceDetails').append(trHTML);
//                    }
//                    var footer = "<tr><td></td><td></td><td></td><td></td><td></td><td><b>Total:</b></td><td><b>" + result.TaxAmount + "</b></td><td><b>" + result.GrandTotal + "</b></td></tr>";
//                    $('#tblInvoiceDetails').append(footer);
//                }
//                var thHTML1 = '';
//                thHTML1 += '<tr style="background-color:#0792bc;">'
//                    + '<th></th>'
//                    + '<th>Document Name</th>'
//                    + '<th>Modified Date</th>'
//                    + '<th>Uploaded By</th></tr>'
//                    + '<tr><td><a class="btn btn-secondary" style="border-radius:25px;width:90px;" href="' + $_hostingPrefix + 'Content\\eCountingDocs\\InvoiceDocs\\' + result.InvoiceDocument + '" download>Download</a></td><td>' + result.InvoiceDocument + '</td><td></td><td></td></tr>'
//                $('#tblInvoiceDocuments').append(thHTML1);
//            }
//            $('.modal-title').text("Invoice Details");
//            $("#myModalViewCreditInvoiceDetails").modal('show');
//            new fn_hideMaskloader();
//        },
//        error: function () {
//            new fn_hideMaskloader();
//        }
//    });

//    var objBody = new Object();
//    objBody.Body = $('#tblInvoiceDetails').html();
//    debugger;
//    $.ajax({
//        type: "POST",
//        url: ViewPDF,
//        datatype: 'application/json',
//        contentType: 'application/json',
//        data: JSON.stringify({ Body: objBody.Body }),
//        success: function (result) {
//            debugger;
//            console.log(result);
//        },
//        error: function () {
//            new fn_hideMaskloader();
//        }
//    });
//}
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#jsGrid-basic").jsGrid({
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
        pageIndex: 1,
        noDataContent: "Data Not Available",
        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: CreditInvoiceListUrl + '?_search=' + $("#SearchText").val() + "&InvoiceStatus="+"&InvoiceType=" + $("#InvoiceType").val() + "&ClientLocationCode=" + $("#ClientLocationCodeSearchParams").val() + "&StartDate=" + $("#StartDate").val() + "&EndDate=" + $("#EndDate").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "CreditMemoNumber", title: "Credit Memo Number", type: "text", width: 50 },
            { name: "InvoiceNumber", title: "Invoice Number", type: "text", width: 50 },
            { name: "CreditIssuedDateDisplay", title: "Credit Issue Date", type: "text", width: 50 },
            { name: "CreditMemoTypeDesc", title: "Credit Type", type: "text", width: 50 },
            { name: "GrandTotal", title: "Invoice Total", type: "text", width: 50, align: "right" },
            { name: "TotalCreditAmount", title: "Credit Total", type: "text", width: 50, align: "right" },
            { name: "CreditMemoStatusDesc", title: "Credit Status", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconPdfForView = $("<i>").attr({ class: "fa fa-file-pdf-o" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconForApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconClose = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    
                    var $customEditButton = "";
                    if (item.CreditMemoStatus == "1" || item.CreditMemoStatus == "0") {
                        $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Edit" })
                            .attr({ id: "btn-edit-" + item.CreditMemoId }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},
                                    url: EditCreateCreditInvoice + item.CreditMemoId + "?IsDraft=" + item.IsDraft,
                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    success: function (result) {
                                        $("#divInvoiceData").html('');
                                        $("#divInvoiceData").html(result);
                                        $("#myModalForGetInvoiceDateForCreditMemo").modal("show");
                                    },
                                    error: function (xhr, status, error) {
                                        fn_hideMaskloader();
                                    },
                                    complete: function () {
                                        fn_hideMaskloader();
                                    }
                                });
                            }).append($iconPencilForEdit);
                    } else {
                        $customEditButton = "";
                        //$customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Edit." })
                        //    .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                        //    }).append($iconPencilForEdit);
                    }

                    var $customButtonForView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View" })
                        .attr({ id: "btn-edit-" + item.CreditMemoId }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.CreditMemoStatus == "0") {
                                ViewCreditInvoiceDetails(item, true);
                            }
                            else {
                                ViewCreditInvoiceDetails(item, false);
                            }
                        }).append($iconPencilForView);
                    var $customButtonForPdfView = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View PDF" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.CreditMemoStatus == "0") {
                                ViewCreditMemo_PDF(item, true);
                            }
                            else {
                                ViewCreditMemo_PDF(item, false);
                            }
                        }).append($iconPdfForView);
                    var $customButtonForApprove = "";
                    if (item.CreditMemoStatus == "1") {
                        $customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Approve" })
                            .attr({ id: "btn-Approve-" + item.CreditMemoId }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                ApproveDetails(item);
                            }).append($iconForApprove);

                    } else {
                        $customButtonForApprove = "";
                        //$customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Approve." })
                        //    .attr({ id: "btn-Approve-" + item.Id }).click(function (e) {
                        //    }).append($iconForApprove);
                    }

                    var $customButtonForCancel = "";
                    if (item.CreditMemoStatus == "1") {
                        $customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Cancel Credit Invoice" })
                            .attr({ id: "CancelBill" + item.CreditMemoId }).click(function (e) {
                                ShowCancelInvoiceModal(item);
                            }).append($iconClose);
                    } else {
                        $customButtonForCancel = "";
                        //$customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Cancel." })
                        //    .attr({ id: "btn-cancel-" + item.Id }).click(function (e) {
                        //    }).append($iconClose);
                    }

                    return $("<div>").attr({ class: "btn-toolbar" })
                        .append($customEditButton)
                        .append($customButtonForView)
                        .append($customButtonForPdfView)
                        .append($customButtonForApprove);
                        //.append($customButtonForCancel);
                }
            }
        ],
        onRefreshed: function (args) {
            var items = args.grid.option("data");
            var total = { Name: "GrandTotal", "GrandTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                total.GrandTotal += item.GrandTotal;
            });

            var totalPayback = { Name: "TotalCreditAmount", "TotalCreditAmount": 0, IsTotal: false };
            items.forEach(function (item) {
                totalPayback.TotalCreditAmount += item.TotalCreditAmount;
            });

            var $totalRow = $("<tr>");
            args.grid._renderCells($totalRow, total);
            args.grid._renderCells($totalRow, totalPayback);
            args.grid._content.append("<tr class=\"GridTotal\">"
                + "<td></td>" + "<td></td>"
                + "<td class=\"GridTotal\"><b>Total : </b></td>" + "<td class=\"GridTotal\"><b>" + total.GrandTotal.toFixed(2) + "</b><td><b>" + totalPayback.TotalCreditAmount.toFixed(2) + "</b></td>" + "<td></td></tr>");

        },
    });
}
function filter(args) {
}

//$("#CreateInvoice").on("click", function (event) {
//    //window.location.href = CreateInvoice;
//    $('#RenderPageId').load(CreateInvoice);
//});

$(document).ready(function () {

    //$(".ActionRequisition").change(function () {
    $("#CreateInvoice").click(function () {

        $.ajax({
            type: "POST",
            // data: { 'Id': item.id},
            url: base_url + '/InvoiceManagement/CreateClientInvoice',
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            contentType: "application/json; charset=utf-8",
            error: function (xhr, status, error) {
            },
            success: function (result) {
                $("#divCreateInvoice").html(result);
                $("#myModalForAddEditCreateInvoice").modal("show");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    });


})


function DenyInvoice() {
    bootbox.confirm("Are you sure, You want to Deny Approval ?", function (result) {
        if (result) {
            var addNewUrl = "/InvoiceManagement/ClientCreditInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        }
    });
}
function AppproveCreditInvoice() {
    //$("#CommentCustomer").val("");
    $("#btnApproveCreditInvoice").addClass("disabled");
    callApproveDenyCreditInvoice("A");
    var addNewUrl = "/InvoiceManagement/ClientCreditInvoiceList";
    $('#RenderPageId').load(addNewUrl);
}
function RejectCustomerAfterComment() {

    if ($("#Comment").val() != "") {

        callApproveDenyCreditInvoice("R");
    }
    else {
        return false;
    }
}

function callApproveDenyCreditInvoice(Action) {
    var objData = new Object();
    objData.InvoiceId = InvoiceId;
    objData.LocationId = $_locationId;
    objData.Comment = $("#CommentCustomer").val();
    objData.Action = Action;
    $.ajax({
        url: '../InvoiceManagement/ApproveRejectCreditMemo',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objCreditMemo: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForApproveDenyCreditInvoice").modal('hide');
            $("#btnApproveCreditInvoice").removeClass("disabled");
            $("#jsGrid-basic").jsGrid("loadData");
            var addNewUrl = "/InvoiceManagement/ClientCreditInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function ApproveDetails(item) {
    InvoiceId = item.CreditMemoId;
    if (item.CreditMemoStatus == "Rejected") {
        $("#btnApproveCreditInvoice").hide();
        $("#btnRejectInvoice").hide();
    }
    else {
        $("#btnApproveCreditInvoice").show();
        $("#btnRejectInvoice").show();
    }
    $('.modal-title').text("Client Invoice");
    $("#myModalForApproveDenyCreditInvoice").modal('show');
    new fn_hideMaskloader();
}

function SubmissionDetails(item) {
    InvoiceId = item.CreditMemoId;
    ClientLocationCode = item.ClientLocationCode;
    $("#btnSubmission").show();
    $("#btnSubmissionDeny").show();
    $('.modal-title').text("Client Invoice");
    $("#myModalForSubmission").modal('show');
    new fn_hideMaskloader();
}

function SubmissionInvoice() {
    $("#btnSubmission").addClass("disabled");

    var objData = new Object();
    objData.InvoiceId = InvoiceId;
    objData.LocationId = $_locationId;
    objData.ClientLocationCode = ClientLocationCode;
    $.ajax({
        url: '../InvoiceManagement/SubmissionInvoice',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objADCIM: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForSubmission").modal('hide');
            $("#btnSubmission").removeClass("disabled");
            $("#jsGrid-basic").jsGrid("loadData");
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function ShowCancelInvoiceModal(item) {
    InvoiceId = item.CreditMemoId;
    ClientLocationCode = item.ClientLocationCode;
    $("#btnCancleInvoice").show();
    $('.modal-title').text("Client Invoice");
    $("#myModalForCancel").modal('show');
    new fn_hideMaskloader();
}

function CancelInvoice() {
    $("#btnSubmission").addClass("disabled");

    var objData = new Object();
    objData.InvoiceId = InvoiceId;
    objData.LocationId = $_locationId;
    objData.ClientLocationCode = ClientLocationCode;
    objData.Comment = $("#CancelComment").val();
    $.ajax({
        url: '../InvoiceManagement/CancelInvoice',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objADCIM: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForCancel").modal('hide');
            $("#jsGrid-basic").jsGrid("loadData");
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function PageRedirect(Method, Controller) {
    var addNewUrl = "/" + Controller + "/" + Method;
    $('#RenderPageId').load(addNewUrl);
}