

$(document).ready(function () {
  
    $("#" + i.AssetName).click(function () {
        
        debugger
        $("." + i.AssetDetails).css("display", "inline");
        $("." + i.AssetDetails).datepicker();

    });

});