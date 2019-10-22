$("#QRCType").change(function () {
    debugger
    var QRCVal = $("#QRCType :selected").val();
    if (QRCVal != null && QRCVal > 0 && QRCVal == 36) {
        $(".showMotorVehicleType").show();        
    }
    else {
        $(".showMotorVehicleType").hide();
        $('#MotorType').val(-1);
        $('#imgvehicletype').prop('src', "");
    }
});

function showvehicletypeimage(me) {
    debugger
    if ($(me).val() != undefined && $(me).val() != '' && parseInt($(me).val(), 10) > 0) {
        var vehicletypeimgpath = $('#hddnimgaeserverpath').val();
        vehicletypeimgpath = vehicletypeimgpath + $('#' + me.id + ' option:selected').text() + ".png";
        $('#imgvehicletype').prop('src', vehicletypeimgpath);
    } else { $('#imgvehicletype').prop('src', ""); $("#PermitTypePrice").val("") }
}
function loadmotortype() {

}

function PrintDivIndexForLicence(DivId) {

    _isPrintDone = false;
    if (!_isPrintDone) {
        //var divToPrint = document.getElementById('DivQRCIndex');
        var vehicletype = '';
        var motortype = '';
        var specialnotes = '';
        var make = '';
        var model = '';
        var phone = '';
        var divToQRC = document.getElementById("container2");
        var popupWin = window.open('', '_blank', 'width=800,height=500');
        popupWin.document.open();
        if ($("#lblVehicleType").html() != null && $("#lblVehicleType").html() != "" && $("#txtQrcType").html() == 'Vehicle') {
            vehicletype = "<p></p><strong style='font-size: 16px;'>Vehicle Type </strong> <br/>"
                      + $("#lblVehicleType").html();
        }
        if ($("#lblMotorType").html() != null && $("#lblMotorType").html() != "" && $("#txtQrcType").html() == 'Vehicle') {
            motortype = "<p></p><strong style='font-size: 16px;'>Motor Type </strong> <br/>"
                      + $("#lblMotorType").html();
        }
        if ($("#txtSpecialNotes").html() != null && $("#txtSpecialNotes").html() != "") {
            specialnotes = "<p></p><strong style='font-size: 16px;'>Special Notes </strong> <br/>"
                      + $("#txtSpecialNotes").html();
        }
        if ($("#divMake").html() != null && $("#divMake").html() != "") {
            make = "<p></p><strong style='font-size: 16px;'>Make</strong> <br/>"
                      + $("#divMake").html();
        }
        if ($("#txtModel").html() != null && $("#txtModel").html() != "") {
            model = "<p></p><strong style='font-size: 16px;'>Model </strong><br/>"
                      + $("#txtModel").html();
        }
        if ($("#TelephoneNo").html() != null && $("#TelephoneNo").html() != "") {
            phone = "<p></p><strong style='font-size: 16px;'>Phone </strong><br/>"
                      + $("#TelephoneNo").html();
        }
        //popupWin.document.write("<html><body onload='window.print(); window.close();'><div style='width:800px;height:300px;'>" + divToPrint.innerHTML + "</div></body></html>");
        popupWin.document.write("<html><body onload='window.print();'><div style='margin-left: 96px; margin-right: 100px; width: 520px;' class='row '><table id='tblToPrint' style='width: 470px; border: 1px solid #0aa0cd; padding: 10px;'><tr><td valign='top' style='width: 210px;'><p></p><strong style='font-size: 16px;'> QRC ID</strong> <br/>"
            + $("#lblQRCId").html() + "<p></p><strong style='font-size: 16px;'>QRC Name </strong> <br/>"
            + $("#qrcNameTxt").html() + phone + vehicletype + motortype + specialnotes
            + "</td><td td valign='top';>"
            + "<p></p><strong style='font-size: 16px;'>Location Name </strong><br/>"
            + $("#divLocation").html() + make + model
            + "<p><strong style='font-size: 16px;'>QRC Code</strong></p><p>" + divToQRC.innerHTML + "</p></td></tr></tbody></table></td></tr></table></div></body></html>");

        if (popupWin.closed == false) {
            popupWin.document.close();
        }
        _isPrintDone = true;
    }
    //$('.noprint').show();
}
//function tooglevehicletype() {
//    debugger
//    var divdaata1 = $('.VehicleType1');
//    if (parseInt($('#QRCType option:selected').val()) == 36) {
//        $("#VehicleType").val(53);
//        $("#VehicleType").prop("disabled", true);
//        divdaata1.show();
//        var golfcartimgpath = $('#hddnimgaeserverpath').val();
//        if ($_hddnMotorTypeVehicle == undefined || $_hddnMotorTypeVehicle == '') {
//            $_hddnMotorTypeVehicle = 53;//Motor type Vehicle;
//        }

//        if ($('#VehicleType').val() != undefined && $('#VehicleType').val() != '' && parseInt($('#VehicleType').val(), 10) > 0 && parseInt($('#VehicleType').val(), 10) == parseInt($_hddnMotorTypeVehicle, 10)) {
//            $('#MotorType').removeAttr('disabled');

//            $('#imgvehicletype').prop('src', "");
//        }
//        else {
//            $('#MotorType').prop('disabled', true); $('#MotorType').val(-1);
//            $('#imgvehicletype').prop('src', "");

//            if ($('#VehicleType').val() != undefined && $('#VehicleType').val() != '' && parseInt($('#VehicleType').val(), 10) > 0 && parseInt($('#VehicleType').val(), 10) != parseInt($_hddnMotorTypeVehicle, 10)) {
//                golfcartimgpath = golfcartimgpath + $('#VehicleType option:selected').text() + ".png";
//                $('#imgvehicletype').prop('src', golfcartimgpath);
//            }
//        }
//        $(".testing").hide();
//    }
//    else {
//        divdaata1.hide();
//    }
//}