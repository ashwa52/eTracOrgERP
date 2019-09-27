var QRCurl = '../QRCSetup/GetQRCListForJsGrid';
var clients;
var $_LocationId = $("#drp_MasterLocation option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();

(function ($) {
    'use strict'
    var data;
    $("#ListQRC").jsGrid({
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
                    url: QRCurl + '?locationId=' + $("#drp_MasterLocation1 option:selected").val() + '&SearchQRCType=' + 0,
                    datatype: 'json',
                    contentType: "application/json",
                });
            }
        },
        //onDataLoading: function (args) {
        //    return $.ajax({
        //        type: "GET",
        //        url: QRCurl + '?locationId=' + $_LocationId + '&SearchQRCType=' + 0,
        //        datatype: 'json',
        //        contentType: "application/json",
        //    });
        //},
        //data: response,
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");

        },
        fields: [
            { name: 'QRCodeID', width: 160, title: "QRCode ID", css: "text-center" },//visible: true
            { name: 'QRCName', width: 150, title: "QRC Item Name" },
                    { name: "QRCTYPE", width: 150, title: "QRC Type" },
                    { name: "SpecialNotes", width: 150, title: "Special Notes" },                                      
                    {
                        name: "act", title: "Action", width: 100, css: "text-center", itemTemplate: function (value, item) {
                            var $iconCheckIn = null;
                            var $iconDamage = null;
                            var $iconPencil = $("<i>").attr({ class: "fa fa-pencil" }).attr({ style: "color:#ddc300;font-size:22px;margin-left:8px;" });
                            var $iconTrash = $("<i>").attr({ class: "fa fa-trash" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                            var $iconQRC = $("<i>").attr({ class: "fa fa-qrcode" }).attr({ id: "QRCGenerate" }).attr({ style: "font-size:22px;margin-left:8px;" });
                            if ((item.CheckOutStatus == 'False' || item.CheckOutStatus == '0') && (item.QRCTYPEId == "36" || item.QRCTYPEId == 36)) {
                                $iconCheckIn = $("<i>").attr({ class: "fa fa-check-circle-o" }).attr({ style: "font-size:22px;margin-left:8px;" });
                            }
                            if ((item.CheckOutStatus == 'False' || item.CheckOutStatus == '0') && ((item.IsDamage == 'True' || item.IsDamage == 1) && (item.IsDamageVerified == null || item.IsDamageVerified == 'YesNull' || item.IsDamageVerified == undefined)) && (item.QRCTYPEId == "36" || item.QRCTYPEId == 36))
                            {
                                $iconDamage = $("<i>").attr({ class: "fa fa-check-circle-o" }).attr({ style: "font-size:22px;margin-left:8px;" });
                                $iconCheckIn = $("<i><img>").attr({ src: "../Content/Images/car_damage.png" });
                            }
                            var $customEditButton = $("<span>")
                                .attr({ title: jsGrid.fields.control.prototype.editButtonTooltip })
                                .attr({ id: "btn-edit-" + item.id }).click(function (e) {
                                    var addNewUrl = "../QRCSetup/Edit?qr=" + item.id;
                                    $('#QRCForm').load(addNewUrl);
                                    $("#OperationDARListDiv, .dispayListQRCName, #OperationDARListDiv, .dispayListQRCName").hide();
                                    $("#OperationCreateQRCListDiv, .dispayCreateQRCName , .createQRCForm").show();
                                
                                    e.stopPropagation();
                                }).append($iconPencil);
                            var $customDeleteButton = $("<span>")
                                  .attr({ title: jsGrid.fields.control.prototype.deleteButtonTooltip })
                                  .attr({ id: "btn-delete-" + item.id }).click(function (e) {
                                      $.ajax({
                                          type: "POST",
                                          url: "../QRCSetup/Delete?qr=" + item.id,
                                          success: function (Data) {
                                              $("#ListQRC").jsGrid("loadData");
                                              
                                          },
                                          error: function (err) {
                                          }

                                      });

                                      e.stopPropagation();
                                  }).append($iconTrash);

                            var $customQRCButton = $("<span>")
                              .attr({ title: jsGrid.fields.control.prototype.qrcButtonTooltip })
                              .attr({ id: "btn-qrc-" + item.id }).click(function (e) {
                                QRCGenerate(item.id);
                                  e.stopPropagation();
                              }).append($iconQRC);

                            //var $customQRCButton = $("<span>")
                            //  .attr({ title: jsGrid.fields.control.prototype.qrcButtonTooltip })
                            //  .attr({ id: "btn-qrc-" + item.id }).click(function (e) {
                            //      debugger
                            //      e.stopPropagation();
                            //  }).append($iconQRC);
                            return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton).append($customDeleteButton).append($customQRCButton);
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
$(document).ready(function () {
    $("#drp_MasterLocation").change(function () {
        $_LocationId = $("#drp_MasterLocation option:selected").val();
        $("#ListQRC").jsGrid("loadData");
    })
});

//View QRC Code
function QRCGenerate(id) {
    //$("#QRCGenerate").click(function (event) {
    //var id = $(this).attr("data-value");

    //showPopupRelativeMessage("Are you sure want to Generate QRC?", $(this), "QRC Generate For Vehicle!", function () {

    $.ajax({
        type: "GET",
        data: { qr: id },
        url: "../QRCSetup/QRCGenerate",
        dataType: "json",
        async: false,
        contentType: "application/json; charset=utf-8",
        error: function (xhr, status, error) {

            closeAjaxProgress();
        },
        success: function (result) {
            $("#qrcNameTxt").html(result.data.QRCName);
            $("#txtSerialNo").html(result.data.SerialNo);
            $("#txtMake").html(result.data.Make);
            // $("#txtSpecialNotes").html(result.data.SpecialNotes);
            if (result.data.Model == '' || result.data.Model == null) {
                $("#Model").hide();
                $("#txtModel").hide();
            }
            else {
                $("#txtModel").html(result.data.Model);
            }
            $("#divLocation").html(result.data.LocationName)

            for (var i = 0; i < result.data.QRCTypeList.length ; i++) {
                if (result.data.QRCTYPE == result.data.QRCTypeList[i]["GlobalCodeId"]) {
                    $("#txtQrcType").html(result.data.QRCTypeList[i]["CodeName"]);
                }
            }
            if (result.data.VehicleType == '' || result.data.VehicleType == null) {
                $("#VehicleType").hide(); $("#lblVehicleType").hide();
            }
            else {
                for (var i = 0; i < result.data.QRCTypeList.length ; i++) {
                    if (result.data.VehicleType == result.data.QRCTypeList[i]["GlobalCodeId"]) {
                        $("#qrcNameTxt").html(result.data.VehicleTypeList[i]["CodeName"]);
                    }

                }

                for (var i = 0; i < result.data.VehicleTypeList.length ; i++) {
                    if (result.data.VehicleType == result.data.VehicleTypeList[i]["GlobalCodeId"]) {
                        $("#lblVehicleType").html(result.data.VehicleTypeList[i]["CodeName"]);
                    }
                }
                $("#VehicleType").show(); $("#lblVehicleType").show();
            }

            if (result.data.MotorType == '' || result.data.MotorType == null) {
                $("#MotorType").hide(); $("#lblMotorType").hide();
            } else {
                for (var i = 0; i < result.data.MotorTypeList.length ; i++) {
                    if (result.data.MotorType == result.data.MotorTypeList[i]["GlobalCodeId"]) {
                        $("#lblMotorType").html(result.data.MotorTypeList[i]["CodeName"]);
                    }
                }
                $("#MotorType").show(); $("#lblMotorType").show();
            }
            $("#lblLocationName").html(result.data.Location);
            $("#lblQRCId").html(result.data.QRCodeID);
            $("#lblDriverName").val(result.data.DriverName);
            $("#lblLicenseNo").html(result.data.LicenseNo);
            $("#lblVehicleImage").html(result.data.VehicleImage);
            $("#lblQRC").html(result.data.QRCName);
            if (result.data.SpecialNotes == '' || result.data.SpecialNotes == null) {
                $("#labelSpecialNotes").hide(); $("#txtSpecialNotes").hide();
            }
            else {
                $("#txtSpecialNotes").html(result.data.SpecialNotes);
                $("#labelSpecialNotes").show(); $("#txtSpecialNotes").show();
            }

            if (result.data.Make == '' || result.data.Make == null) {
                $("#lblMake").hide(); $("#divMake").hide();
            }
            else {
                $("#divMake").html(result.data.Make);
                $("#lblMake").show(); $("#divMake").show();
            }
            $("#lblSpecialNotes").html(result.data.SpecialNotes);
            //$("#lblQRCTYPE").html(result.data.QRCTYPE);
            //$("#").html(result.data.VendorID);
            $("#lblCompanyName").html(result.data.CompanyName);
           // $("#").html(result.data.ContactName);
            $("#lblBusinessNo").html(result.data.BusinessNo);
            $("#pDriverName").html("<b>Company Name:- </b>" + result.data.CompanyName);
            $("#pCompanyName").html("<b>Driver Name:- </b>" + result.data.DriverName);

            if (result.data.VendorName == '' || result.data.VendorName == null) {
                $("#VendorName").hide(); $("#lblVendorName").hide();
            }
            else {
                $("#VendorName").html(result.data.VendorName);
                $("#VendorName").show(); $("#lblVendorName").show();
            }

            if (result.data.PointOfContact == '' || result.data.PointOfContact == null) {
                $("#PointOfContact").hide(); $("#lblPointOfContact").hide();
            }
            else {
                $("#PointOfContact").html(result.data.PointOfContact);
                $("#PointOfContact").show(); $("#lblPointOfContact").show();
            }
            if (result.data.TelephoneNo == '' || result.data.TelephoneNo == null) {
                $("#TelephoneNo").hide(); $("#lblTelephoneNo").hide();
            }
            else {
                $("#TelephoneNo").html(result.data.TelephoneNo);
                $("#TelephoneNo").show(); $("#lblTelephoneNo").show();
            }
            if (result.data.EmialAdd == '' || result.data.EmialAdd == null) {
                $("#EmialAdd").hide(); $("#lblEmialAdd").hide();
            }
            else {
                $("#EmialAdd").html(result.data.EmialAdd);
                $("#EmialAdd").show(); $("#lblEmialAdd").show();
            }

            if (result.data.IExpDate == '01-01-01' || result.data.IExpDate == null || result.data.IExpDate == '01/01/01') {
                $("#InsuranceExpDate").hide(); $("#lblInsuranceExpDate").hide();
            }
            else {
                $("#InsuranceExpDate").html(result.data.IExpDate);
                $("#InsuranceExpDate").show(); $("#lblInsuranceExpDate").show();
            }
            if (result.data.WExpDate == '01-01-01' || result.data.WExpDate == null || result.data.WExpDate == '01/01/01') {
                $("#WarrantyEndDate").hide(); $("#lblWarrantyEndDate").hide();
            }
            else {
                $("#WarrantyEndDate").html(result.data.WExpDate);
                $("#WarrantyEndDate").show(); $("#lblWarrantyEndDate").show();
            }
            if (result.data.Website == '' || result.data.Website == null) {
                $("#Website").hide(); $("#lblWebsite").hide();
            }
            else {
                $("#Website").html(result.data.Website);
                $("#Website").show(); $("#lblWebsite").show();
            }
            if (result.data.PurchaseType == '' || result.data.PurchaseType == null) {
                $("#ddlPurchaseType").hide(); $("#PurchaseType").hide();
            }
            else {
                for (var i = 0; i < result.data.PurchaseTypeList.length ; i++) {
                    if (result.data.PurchaseType == result.data.PurchaseTypeList[i]["GlobalCodeId"]) {
                        $("#ddlPurchaseType").html(result.data.PurchaseTypeList[i]["CodeName"]);
                    }
                }
                $("#ddlPurchaseType").show(); $("#PurchaseType").show();
            }


            if (result.data.PurchaseTypeRemark == '' || result.data.PurchaseTypeRemark == null) {
                $("#PurchaseTypeRemark").hide(); $("#lblPurchaseTypeRemark").hide();
            }
            else {
                $("#PurchaseTypeRemark").html(result.data.PurchaseTypeRemark);
                $("#PurchaseTypeRemark").show(); $("#lblPurchaseTypeRemark").show();
            }

            $("#divCreatedBy").html(result.data.UserModel.FirstName + ' ' + result.data.UserModel.LastName);

            $("#divCreatedOn").html(result.data.CreatedOn);

            $('#myModalFORQR').modal('show');

            generateqrcodeByVJ(result.data.QRCodeID, result.data.QRCSizeGenerate);

            $("#myModalFORQR :text").attr("readOnly", "true");
        },
        Complete: function (result) {
            closeAjaxProgress();
            console.log('Ajax Div');
            $("#ajaxProgress").css("display", "none");
            event.stopPropagation();
        }
    }); // ajax call end
    //});// pop up alert show


}

function generateqrcodeByVJ(id, sizeGenerate) {
    var $_QRCIDNumber = id;
    var size = (sizeGenerate != undefined && sizeGenerate != '' && $.trim(sizeGenerate) != '') ? $.trim(sizeGenerate) : '155';
    size = (size != undefined && size != '' && $.trim(size) != '') ? $.trim(size) : '155';
    var qrcsize = size.toLowerCase().split('x');

    size = qrcsize[0];
    size = size.trim();
    'use strict';

    var isOpera = Object.prototype.toString.call(window.opera) === '[object Opera]',

        guiValuePairs = [
            ["size", "px"],
            ["minversion", ""],
            ["quiet", " modules"],
            ["radius", "%"],
            ["msize", "%"],
            ["mposx", "%"],
            ["mposy", "%"]
        ],

        updateGui = function () {

            $.each(guiValuePairs, function (idx, pair) {

                var $label = $('label[for="' + pair[0] + '"]');

                $label.text($label.text().replace(/:.*/, ': ' + $('#' + pair[0]).val() + pair[1]));
            });
        },

        updateQrCode = function (mykey, mycontainer) {
            //var EncryptQRC = $('#EncryptQRC').val();
            var EncryptQRC = mykey;

            // ;
            //alert('test 2');
            //lblQRCId

            EncryptQRC = id.toString();//$_QRCIDNumber;


            var options = {
                //render: $("#render").val(),
                render: "image",//render: "image",

                //ecLevel: $("#eclevel").val(),
                ecLevel: "Q",// L=Low, M=Medium,

                //minVersion: parseInt($("#minversion").val(), 10),
                minVersion: parseInt(5, 10),

                fill: '#333333',
                //fill: $("#fill").val(),

                //background: $("#background").val(),
                background: '#ffffff',
                // fill: $("#img-buffer")[0],

                //text: $("#text").val(),
                //text: 'my name is Developer, i am a SSE having around 5 years of experience' + new Date() + '',

                text: EncryptQRC,


                //size: parseInt($("#size").val(), 10),



                size: parseInt(size, 10),

                //radius: parseInt($("#radius").val(), 10) * 0.01,
                radius: parseInt(50, 10) * 0.01,

                //quiet: parseInt($("#quiet").val(), 10),
                quiet: parseInt(1, 10),

                //mode: parseInt($("#mode").val(), 10),
                mode: parseInt(0, 10),

                //mSize: parseInt($("#msize").val(), 10) * 0.01,
                mSize: parseInt(11, 10) * 0.01,
                //mPosX: parseInt($("#mposx").val(), 10) * 0.01,
                mPosX: parseInt(50, 10) * 0.01,
                //mPosY: parseInt($("#mposy").val(), 10) * 0.01,
                mPosY: parseInt(50, 10) * 0.01,

                //label: $("#label").val(),
                label: 'Smartian says',
                //fontname: $("#font").val(),
                fontname: 'Ubuntu',
                //fontcolor: $("#fontcolor").val(),
                fontcolor: '#ff9818',

                //image: $("#img-buffer")[0]
                image: 'http://localhost:57572/Images/upload.jpg'
            };
            //$('"#'+mycontainer+'"').empty().qrcode(options);

            $('#container2').empty().qrcode(options);
            //$("#container").attr('class', 'show');


        },

        update = function () {

            updateGui();
            //updateQrCode();
            updateQrCode('saadad', 'container2');
        },

        onImageInput = function () {

            var input = $("#image")[0];

            if (input.files && input.files[0]) {

                var reader = new FileReader();

                reader.onload = function (event) {
                    $("#img-buffer").attr("src", event.target.result);
                    $("#mode").val("4");
                    setTimeout(update, 250);
                };
                reader.readAsDataURL(input.files[0]);
            }
        },

        download = function (event) {

            var data = $("#container2 canvas")[0].toDataURL('image/png');
            $("#download").attr("href", data);
        };


    $(function () {

        //if (isOpera) {
        //    $('html').addClass('opera');
        //    $('#radius').prop('disabled', true);
        //}

        //$("#download").on("click", download);
        //$("#image").on('change', onImageInput);
        //$("input, textarea, select").on("input change", update);        
        var EncryptQRC = id;
        var EncryptLastQRC = id;


        //if (_hddnUpdateMode != 'True' && EncryptLastQRC != undefined && EncryptLastQRC != '') {
        if (EncryptLastQRC != undefined && EncryptLastQRC != '') {
            //alert('EncryptLastQRC');
            // ;
            updateGui();
            updateQrCode(EncryptLastQRC, 'container2');
        }

        if (EncryptQRC != undefined && EncryptQRC != '') {
            //alert('new EncryptQRC');
            updateGui();
            updateQrCode();
        }

    });
}