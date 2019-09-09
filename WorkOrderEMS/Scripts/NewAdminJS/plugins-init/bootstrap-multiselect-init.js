(function ($) {


    "use strict"

    
    //$('.basic-multiselect').multiselect();
    $('#ddlServices').multiselect({
        maxHeight: '300',
        onChange: function (element, checked) {
            debugger
            var Services = $('#ddlServices option:selected');
            var selected = [];
            $(Services).each(function (index, Services) {
                //if (index > 0) {
                var ss = $(Services).val();
                if (ss != undefined && parseInt(ss) > 0) {
                    //alert(parseInt(ss));// found
                    selected.push([$(this).val()]);
                }
                //}
            });
            $("#ServicesID").val(selected);
        }
    });

    
    $('.basic-multiselect-optgroup').multiselect({
        enableClickableOptGroups: true
    });

    $('.basic-multiselect-selectall').multiselect({
        enableClickableOptGroups: true, 
        includeSelectAllOption: true
    });


})(jQuery);