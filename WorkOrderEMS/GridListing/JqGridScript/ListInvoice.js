var InvoiceListUrl = '/InvoiceManagement/GetAllInvoiceList';
var CreditMemoListUrl = '/InvoiceManagement/GetAllInvoiceList';
var CreateInvoice = '/InvoiceManagement/CreateClientInvoice/';
var CreateCreditInvoice = '/InvoiceManagement/CreateCreditInvoice/';
var ViewInvoiceDetails = '/InvoiceManagement/GetInvoiceDataToView/';

var LocationId; var InvoiceId; var ClientLocationCode;
var base_url = window.location.origin;
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
                    url: InvoiceListUrl + '?flagApproved=N' + "&LocationId=" + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "InvoiceNumber", title: "Invoice Number", type: "text", width: 50 },
            { name: "InvoiceDateDisplay", title: "Issue Date", type: "text", width: 50 },
            { name: "InvoiceTypeDesc", title: "Invoice Type", type: "text", width: 50 },
            { name: "GrandTotal", title: "Invoice Total", type: "text", width: 50, align: "right" },
            { name: "PaymentTotal", title: "Payment Total", type: "text", width: 50, align: "right" },
            { name: "InvoiceStatusDesc", title: "Invoice Status", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconForReceivePayment = $("<i>").attr({ class: "fa fa-money" }).attr({ style: "color:#852b99;font-size: 22px;" });
                    var $iconForSubmission = $("<i>").attr({ class: "fa fa-arrow-up" }).attr({ style: "color:#166C88;font-size: 22px;" });
                    var $iconForApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconClose = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconForLate = $("<i>").attr({ class: "fa fa-bell" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconForCreditInvoice = $("<i>").attr({ class: "fa fa-database" }).attr({ style: "color:#852b99;font-size: 22px;" });

                    var $customEditButton = "";
                    if (item.InvoiceStatus == "1" || item.InvoiceStatus == "0") {
                        $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Edit" })
                            .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},
                                    url: CreateInvoice + item.Id + "?IsDraft=" + item.IsDraft,
                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    error: function (xhr, status, error) {
                                    },
                                    success: function (result) {
                                        $("#divCreateInvoice").html('');
                                        $("#divCreateInvoice").html(result);
                                        $("#divOpenPaymentReceiveForm").html('');
                                        $("#myModalForPaymentReceiveAction").modal("hide");
                                        $("#myModalForAddEditCreateInvoice").modal("show");
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
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.InvoiceStatus == "0") {
                                ViewDetails(item, true);
                            }
                            else {
                                ViewDetails(item, false);
                            }
                        }).append($iconPencilForView);

                    var $customButtonForApprove = "";
                    if (item.InvoiceStatus == "1") {
                        $customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Approve" })
                            .attr({ id: "btn-Approve-" + item.Id }).click(function (e) {
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

                    var $customButtonForSubmission = "";
                    if (item.InvoiceStatus == "2") {
                        $customButtonForSubmission = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Submission" })
                            .attr({ id: "btn-Submission-" + item.Id }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                SubmissionDetails(item);
                            }).append($iconForSubmission);
                    } else {
                        $customButtonForSubmission = "";
                        //$customButtonForSubmission = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Submission." })
                        //    .attr({ id: "btn-Submission-" + item.Id }).click(function (e) {
                        //    }).append($iconForSubmission);
                    }
                    var $customButtonForReceivePayment = "";
                    if (item.InvoiceStatus == "3") {
                        $customButtonForReceivePayment = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Receive Payment" })
                            .attr({ id: "btn-payment-" + item.Id }).click(function (e) {
                                ReceivedPayment(item.Id);
                                //new fn_showMaskloader('Please wait...');
                                //var addNewUrl = "/InvoiceManagement/ReceivePaymentClientInvoice?id=" + item.Id;
                                
                                //$('#RenderPageId').load(addNewUrl);
                                //e.stopPropagation();
                                //new fn_hideMaskloader();
                            }).append($iconForReceivePayment);
                    } else {
                        $customButtonForReceivePayment = "";
                        //$customButtonForReceivePayment = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Receive Payment." })
                        //    .attr({ id: "btn-payment-" + item.Id }).click(function (e) {
                        //    }).append($iconForReceivePayment);
                    }

                    var $customButtonForCancel = "";
                    if (item.InvoiceStatus == "1") {
                        $customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Cancel Invoice" })
                            .attr({ id: "CancelBill" + item.Id }).click(function (e) {
                                ShowCancelInvoiceModal(item);
                            }).append($iconClose);
                    } else {
                        $customButtonForCancel = "";
                        //$customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Cancel." })
                        //    .attr({ id: "btn-cancel-" + item.Id }).click(function (e) {
                        //    }).append($iconClose);
                    }

                    var $customButtonForLate = "";
                    if (item.PaymentReminder == true) {
                        $customButtonForLate = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Payment Reminder" })
                            .attr({ id: "btn-paymentreminder-" + item.Id }).click(function (e) {
                                ShowPaymentReminderModal(item);
                            }).append($iconForLate);
                    }

                    var $customButtonForCreditInvoice = "";
                    if (item.PaymentTotal > 0) {
                        if (item.IsCreditIssued == false) {
                            $customButtonForCreditInvoice = $("<span style='padding: 0 5px 0 0;'>")
                                .attr({ title: "Issue Credit" })
                                .attr({ id: "btn-issuecredit-" + item.Id }).click(function (e) {
                                    new fn_showMaskloader('Please wait...');
                                    $.ajax({
                                        type: "POST",
                                        // data: { 'Id': item.id},
                                        url: CreateCreditInvoice + item.Id + "?IsDraft=" + item.IsDraft,
                                        beforeSend: function () {
                                            new fn_showMaskloader('Please wait...');
                                        },
                                        contentType: "application/json; charset=utf-8",
                                        error: function (xhr, status, error) {
                                        },
                                        success: function (result) {
                                            console.log(result);
                                            $("#divInvoiceData").html('');
                                            $("#divInvoiceData").html(result);
                                            $("#myModalForGetInvoiceDateForCreditMemo").modal("show");
                                        },
                                        complete: function () {
                                            fn_hideMaskloader();
                                        }
                                    });
                                }).append($iconForCreditInvoice);
                        }
                        else {
                            $customButtonForCreditInvoice = $("<span style='padding: 0 5px 0 0;'>")
                                .attr({ title: "Credit invoice already issued." })
                                .attr({ id: "btn-issuecredit-" + item.Id }).click(function (e) {
                                }).append($iconForCreditInvoice);
                        }
                    }
                    

                    return $("<div>").attr({ class: "btn-toolbar" })
                        .append($customEditButton)
                        .append($customButtonForView)
                        .append($customButtonForApprove)
                        .append($customButtonForSubmission)
                        .append($customButtonForReceivePayment)
                        .append($customButtonForCancel)
                        .append($customButtonForLate)
                        .append($customButtonForCreditInvoice);
                }
            }
        ],
        onRefreshed: function (args) {
            var items = args.grid.option("data");
            var total = { Name: "GrandTotal", "GrandTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                total.GrandTotal += item.GrandTotal;
            });

            var totalPayback = { Name: "PaymentTotal", "PaymentTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                totalPayback.PaymentTotal += item.PaymentTotal;
            });

            var $totalRow = $("<tr>");
            args.grid._renderCells($totalRow, total);
            args.grid._renderCells($totalRow, totalPayback);
            args.grid._content.append("<tr class=\"GridTotal\">"
                + "<td></td>" + "<td></td>"
                + "<td class=\"GridTotal\"><b>Total : </b></td>" + "<td class=\"GridTotal\"><b>" + total.GrandTotal.toFixed(2) + "</b><td><b>" + totalPayback.PaymentTotal.toFixed(2) + "</b></td>" + "<td></td></tr>");

        },
    });
});
function ReceivedPayment(Id) {
    var addNewUrl = "/InvoiceManagement/ReceivePaymentClientInvoice?id=" + Id;
    $.ajax({
        type: "GET",
        url: base_url + addNewUrl,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {
        },
        success: function (result) {
            console.log(result);
            $("#divOpenPaymentReceiveForm").html('');
            $("#divOpenPaymentReceiveForm").html(result);
            $("#myModalForPaymentReceiveAction").modal("show");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function ViewDetails(item,IsDraft) {
    $.ajax({
        type: "POST",
        url: ViewInvoiceDetails + '/' + item.Id + "?IsDraft=" + IsDraft,
        datatype: 'json',
        success: function (result) {
            if (result.InvoiceStatus == "0") {
                $("#LblInvoiceNumberHeader,#lblInvoiceNumber").hide();
                $("#LblDraftNumberHeader,#lblDraftNumber").show();
            }
            else {
                $("#LblDraftNumberHeader,#lblDraftNumber").hide();
                $("#LblInvoiceNumberHeader,#lblInvoiceNumber").show();
            }
            $("#lblclientlocationName").html(result.ClientLocationName);
            $("#lblclientcompanyName").html(result.ClientCompanyName);
            $("#lblCustomerType").html(result.ContractTypeDesc);
            $("#lblInvoicePaymentTerms").html(result.InvoicePaymentTermsDesc);
            $("#lblClientPointofContactName").html(result.ClientPointOfContactName);
            $("#lblPositionTitle").html(result.PositionTitle);
            $("#lblInvoiceNumber").html(result.InvoiceNumber);
            $("#lblInvoiceDate").html(result.InvoiceDateDisplay);
            $("#lblInvoicePaymentDueDate").html(result.InvoiceDueDateDisplay);
            $("#lblLocationCode").html(result.LocationCode);
            $("#lblLocationAddress").html(result.LocationAddress);
            $("#lblReasonforOffSchedule").html(result.ReasonForOffScheduleInvoiceDesc);
            $("#lblInvoiceCreateDate").html(result.EntryOnDisplay);
            $("#lblInvoiceCreateBy").html(result.EntryByDisplay);
            $("#lblInvoiceModifiedDate").html(result.ModifiedOnDisplay);
            $("#lblInvoiceModifiedBy").html(result.ModifiedByDisplay);
            $("#lblInvoiceTypeProcess").html(result.InvoiceTypeDesc);
            $("#lblInvoiceTotal").html(result.GrandTotal);
            $("#lblInvoiceDueDate").html(result.InvoiceDueDateDisplay);
            $("#lblInvoiceBalance").html(result.PendingAmount);
            $("#lblDateLastSentToclient").html(result.InvoiceLastSenttoclientDate);
            $("#lblDraftNumber").html(result.DraftNumber);
            $("#lblTotalPaidAmount").html(result.PaymentTotal);
            $("#lblPaymentDate").html(result.PaymentReceiveDateDisplay);
            $("#lblInvoiceComment").html(result.Comment);

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
                    + '<th> Tax(%)</th>'
                    + ' <th>Tax Amount</th>'
                    + '<th>Total Cost</th></tr > ';
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
                            '</td><td>' + result.ListInvoiceItemDetails[i].TaxPercentage +
                            '</td><td>' + result.ListInvoiceItemDetails[i].TaxAmount +
                            '</td><td>' + result.ListInvoiceItemDetails[i].TotalCost +
                            '</td></tr>';

                        $('#tblInvoiceDetails').append(trHTML);
                    }
                    var footer = "<tr><td></td><td></td><td></td><td></td><td></td><td><b>Total:</b></td><td><b>" + result.TaxAmount + "</b></td><td><b>" + result.GrandTotal + "</b></td></tr>";
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
            $('.modal-title').text("Invoice Details");
            $("#myModalForGetUnApprovedCustomerDetails").modal('show');
            new fn_hideMaskloader();
        },
        error: function () {
            new fn_hideMaskloader();
        }
    });
}
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
                    url: InvoiceListUrl + '?_search=' + $("#SearchText").val() + "&InvoiceStatus=" + $("#InvoiceStatusForSearch").val() + "&InvoiceType=" + $("#InvoiceType").val() + "&ClientLocationCode=" + $("#ClientLocationCodeSearchParams").val() + "&StartDate=" + $("#StartDate").val() + "&EndDate=" + $("#EndDate").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "InvoiceNumber", title: "Invoice Number", type: "text", width: 50 },
            { name: "InvoiceDateDisplay", title: "Issue Date", type: "text", width: 50 },
            { name: "InvoiceTypeDesc", title: "Invoice Type", type: "text", width: 50 },
            { name: "GrandTotal", title: "Invoice Total", type: "text", width: 50, align: "right" },
            { name: "PaymentTotal", title: "Payment Total", type: "text", width: 50, align: "right" },
            { name: "InvoiceStatusDesc", title: "Invoice Status", type: "text", width: 50 },
            {
                name: "act", type: "control", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    var $iconPencilForEdit = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:black;font-size: 22px;" });
                    var $iconPencilForView = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconForReceivePayment = $("<i>").attr({ class: "fa fa-money" }).attr({ style: "color:#852b99;font-size: 22px;" });
                    var $iconForSubmission = $("<i>").attr({ class: "fa fa-arrow-up" }).attr({ style: "color:#166C88;font-size: 22px;" });
                    var $iconForApprove = $("<i>").attr({ class: "fa fa-check" }).attr({ style: "color:green;font-size: 22px;" });
                    var $iconClose = $("<i>").attr({ class: "fa fa-close" }).attr({ style: "color:red;font-size: 22px;" });
                    var $iconForLate = $("<i>").attr({ class: "fa fa-bell" }).attr({ style: "color:red;font-size: 22px;" });
					var $iconForCreditInvoice = $("<i>").attr({ class: "fa fa-database" }).attr({ style: "color:#852b99;font-size: 22px;" });

                    var $customEditButton = "";
                    if (item.InvoiceStatus == "1" || item.InvoiceStatus == "0") {
                        $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Edit" })
                            .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                $.ajax({
                                    type: "POST",
                                    // data: { 'Id': item.id},
                                    url: CreateInvoice + item.Id + "?IsDraft=" + item.IsDraft,
                                    beforeSend: function () {
                                        new fn_showMaskloader('Please wait...');
                                    },
                                    contentType: "application/json; charset=utf-8",
                                    error: function (xhr, status, error) {
                                    },
                                    success: function (result) {
                                        $("#divCreateInvoice").html('');
                                        $("#divCreateInvoice").html(result);
                                        $("#myModalForAddEditCreateInvoice").modal("show");
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
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            if (item.InvoiceStatus == "0") {
                                ViewDetails(item, true);
                            }
                            else {
                                ViewDetails(item, false);
                            }
                        }).append($iconPencilForView);

                    var $customButtonForApprove = "";
                    if (item.InvoiceStatus == "1") {
                        $customButtonForApprove = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Approve" })
                            .attr({ id: "btn-Approve-" + item.Id }).click(function (e) {
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

                    var $customButtonForSubmission = "";
                    if (item.InvoiceStatus == "2") {
                        $customButtonForSubmission = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Submission" })
                            .attr({ id: "btn-Submission-" + item.Id }).click(function (e) {
                                new fn_showMaskloader('Please wait...');
                                SubmissionDetails(item);
                            }).append($iconForSubmission);
                    } else {
                        $customButtonForSubmission = "";
                        //$customButtonForSubmission = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Submission." })
                        //    .attr({ id: "btn-Submission-" + item.Id }).click(function (e) {
                        //    }).append($iconForSubmission);
                    }

                    var $customButtonForReceivePayment = "";
                    if (item.InvoiceStatus == "3") {
                        $customButtonForReceivePayment = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Receive Payment" })
                            .attr({ id: "btn-payment-" + item.Id }).click(function (e) {
                                ReceivedPayment(item.Id);
                                //new fn_showMaskloader('Please wait...');
                                //var addNewUrl = "/InvoiceManagement/ReceivePaymentClientInvoice?id=" + item.Id;
                                //$('#RenderPageId').load(addNewUrl);
                                //e.stopPropagation();
                                //new fn_hideMaskloader();
                            }).append($iconForReceivePayment);
                    } else {
                        $customButtonForReceivePayment = "";
                        //$customButtonForReceivePayment = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Receive Payment." })
                        //    .attr({ id: "btn-payment-" + item.Id }).click(function (e) {
                        //    }).append($iconForReceivePayment);
                    }

                    var $customButtonForCancel = "";
                    if (item.InvoiceStatus == "1") {
                        $customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Cancel Invoice" })
                            .attr({ id: "CancelBill" + item.Id }).click(function (e) {
                                ShowCancelInvoiceModal(item);
                            }).append($iconClose);
                    } else {
                        $customButtonForCancel = "";
                        //$customButtonForCancel = $("<span style='padding: 0 5px 0 0;'>")
                        //    .attr({ title: "This invoice is not available for Cancel." })
                        //    .attr({ id: "btn-cancel-" + item.Id }).click(function (e) {
                        //    }).append($iconClose);
                    }

                    var $customButtonForLate = "";
                    if (item.PaymentReminder == true) {
                        $customButtonForLate = $("<span style='padding: 0 5px 0 0;'>")
                            .attr({ title: "Payment Reminder" })
                            .attr({ id: "btn-paymentreminder-" + item.Id }).click(function (e) {
                                ShowPaymentReminderModal(item);
                            }).append($iconForLate);
                    }

                    var $customButtonForCreditInvoice = "";
                    if (item.PaymentTotal > 0) {
                        if (item.IsCreditIssued == false) {
                            $customButtonForCreditInvoice = $("<span style='padding: 0 5px 0 0;'>")
                                .attr({ title: "Issue Credit" })
                                .attr({ id: "btn-issuecredit-" + item.Id }).click(function (e) {
                                    new fn_showMaskloader('Please wait...');
                                    $.ajax({
                                        type: "POST",
                                        // data: { 'Id': item.id},
                                        url: CreateCreditInvoice + item.Id + "?IsDraft=" + item.IsDraft,
                                        beforeSend: function () {
                                            new fn_showMaskloader('Please wait...');
                                        },
                                        contentType: "application/json; charset=utf-8",
                                        error: function (xhr, status, error) {
                                        },
                                        success: function (result) {
                                            console.log(result);
                                            $("#divInvoiceData").html('');
                                            $("#divInvoiceData").html(result);
                                            $("#myModalForGetInvoiceDateForCreditMemo").modal("show");
                                        },
                                        complete: function () {
                                            fn_hideMaskloader();
                                        }
                                    });
                                }).append($iconForCreditInvoice);
                        }
                        else {
                            $customButtonForCreditInvoice = $("<span style='padding: 0 5px 0 0;'>")
                                .attr({ title: "Credit invoice already issued." })
                                .attr({ id: "btn-issuecredit-" + item.Id }).click(function (e) {
                                }).append($iconForCreditInvoice);
                        }
                    }

                    return $("<div>").attr({ class: "btn-toolbar" })
                        .append($customEditButton)
                        .append($customButtonForView)
                        .append($customButtonForApprove)
                        .append($customButtonForSubmission)
                        .append($customButtonForReceivePayment)
                        .append($customButtonForCancel)
                        .append($customButtonForLate)
                        .append($customButtonForCreditInvoice);
                }
            }
        ],
        onRefreshed: function (args) {
            var items = args.grid.option("data");
            var total = { Name: "GrandTotal", "GrandTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                total.GrandTotal += item.GrandTotal;
            });

            var totalPayback = { Name: "PaymentTotal", "PaymentTotal": 0, IsTotal: false };
            items.forEach(function (item) {
                totalPayback.PaymentTotal += item.PaymentTotal;
            });

            var $totalRow = $("<tr>");
            args.grid._renderCells($totalRow, total);
            args.grid._renderCells($totalRow, totalPayback);
            args.grid._content.append("<tr class=\"GridTotal\">"
                + "<td></td>" + "<td></td>"
                + "<td class=\"GridTotal\"><b>Total : </b></td>" + "<td class=\"GridTotal\"><b>" + total.GrandTotal.toFixed(2) + "</b><td><b>" + totalPayback.PaymentTotal.toFixed(2) + "</b></td>" + "<td></td></tr>");

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
    //$("#myModelApproveRejectCustomer").modal('show');
    bootbox.confirm("Are you sure, You want to Deny Approval ?", function (result) {
        if (result) {
            var addNewUrl = "/InvoiceManagement/ClientInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        }
    });
}
function AppproveInvoice() {
   // $("#CommentCustomer").val("");
    $("#btnApproveInvoice").addClass("disabled");
    callApproveDenyInvoice("A");
}
function RejectCustomerAfterComment() {

    if ($("#Comment").val() != "") {

        callApproveDenyInvoice("R");
    }
    else {
        return false;
    }
}

function callApproveDenyInvoice(Action) {
    var objData = new Object();
    objData.InvoiceId = InvoiceId;
    objData.LocationId = $_locationId;
    objData.Comment = $("#CommentCustomer").val();
    objData.Action = Action;
    $.ajax({
        url: '../InvoiceManagement/ApproveInvoice',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objADCIM: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForApproveDeny").modal('hide');
            $("#btnApproveInvoice").removeClass("disabled");
            $("#jsGrid-basic").jsGrid("loadData");
            var addNewUrl = "/InvoiceManagement/ClientInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function ApproveDetails(item) {

    InvoiceId = item.Id;
    if (item.Status == "Rejected") {
        $("#btnApproveInvoice").hide();
        $("#btnRejectInvoice").hide();
    }
    else {
        $("#btnApproveInvoice").show();
        $("#btnRejectInvoice").show();
    }
    $('.modal-title').text("Client Invoice");
    $("#myModalForApproveDeny").modal('show');
    new fn_hideMaskloader();
}

function SubmissionDetails(item) {
    InvoiceId = item.Id;
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
            var addNewUrl = "/InvoiceManagement/ClientInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function ShowPaymentReminderModal(item) {
    InvoiceId = item.Id;
    ClientLocationCode = item.ClientLocationCode;
    $("#ModalForPaymentReminder").modal('show');
    new fn_hideMaskloader();
}
function SendPaymentReminder() {
    var objData = new Object();
    objData.InvoiceId = InvoiceId;
    objData.LocationId = $_locationId;
    objData.ClientLocationCode = ClientLocationCode;
    $.ajax({
        url: '../InvoiceManagement/PaymentRemainder',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objADCIM: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#ModalForPaymentReminder").modal('hide');
            $("#jsGrid-basic").jsGrid("loadData");
            var addNewUrl = "/InvoiceManagement/ClientInvoiceList";
            $('#RenderPageId').load(addNewUrl);
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function ShowCancelInvoiceModal(item) {
    InvoiceId = item.Id;
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
