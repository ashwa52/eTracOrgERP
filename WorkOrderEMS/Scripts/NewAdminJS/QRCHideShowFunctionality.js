$("#QRCType").change(function () {
    debugger
        var QRCVal = $("#QRCType :selected").val();
        if (QRCVal != null && QRCVal > 0 && QRCVal == 36) {
            $(".showMotorVehicleType").show();
        }
        else {
            $(".showMotorVehicleType").hide();
        }
});
function loadmotortype() {

}