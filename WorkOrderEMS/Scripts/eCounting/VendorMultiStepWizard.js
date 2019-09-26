$(document).ready(function () {
    //jQuery time
    var current_fs, next_fs, previous_fs; //fieldsets
    var left, opacity, scale; //fieldset properties which we will animate
    var animating; //flag to prevent quick multi-click glitches
    $(document).on('click', '.field-wrap label', function () {
        $(this).next('input[type="text"]').focus();
    })
    $(".next").click(function () {
        var VendorTypeValue = $('#VendorType').val();
        var vendorName = $('#CompanyName').val(); $('#SecondaryCompany').val(vendorName);
        var PaymentMode = $('#PrimaryPaymentMode').val();
        var PaymentModeText = $('#PrimaryPaymentMode :selected').text();
        $('#VendorTypeContract').val(VendorTypeValue).trigger('change');
        $('#PaymentMode').val(PaymentMode).trigger('change');
        $('#SecondaryCompany,#VendorTypeContract,#VendorFacilityName,#PaymentMode').css("background-color", "#EBEBE4");
        $('#lblVendorType,#lblSecondaryCompany,#lblVendorFacilityName,#lblAccountPaymentMode').addClass('active highlight');
        $('#SecondaryCompany,#VendorFacilityName').attr('readonly', true);
        $('#VendorTypeContract,#PaymentMode').attr('disabled', 'disabled');
        $('#VendorFacilityName').val(vendorName);
        if (VendorTypeValue == 1 || VendorTypeValue == 2 || VendorTypeValue == 3) {
            $('.ShowHideIfVendor123').show();
            $('.hidelatefine').show();
        }
        else {
            $('.ShowHideIfVendor123').hide();
            $('.hidelatefine').hide();
        }

        if (PaymentMode == 1 || PaymentModeText == "Card") {
            $('.CardSelectHideShow').show()
            $('.wiredSelectHideShow').hide();
        }
        else if (PaymentMode == 2 || PaymentModeText == "Wired") {
            $('.CardSelectHideShow').hide()
            $('.wiredSelectHideShow').show();
        }
        else {
            $('.CardSelectHideShow').hide()
            $('.wiredSelectHideShow').hide();
        }
        
        if ($("#msform").valid()) 
        {
         
            if ($('#VendorFacilityInformation').is(":visible") && $('#ProductList').val() === '') {
                alert('Add atleast one product/service');
                return false;
            }
            if (animating) return false;
            animating = true;

            current_fs = $(this).parent();
            next_fs = $(this).parent().next();
             
            //activate next step on progressbar using the index of next_fs
            $("#progressbar div a").eq($("fieldset").index(current_fs)).removeClass("active btn-success");
            $("#progressbar div a").eq($("fieldset").index(next_fs)).addClass("active btn-success");
            //CopyLocationDetails();
            //show the next fieldset
            next_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now, mx) {
                    //as the opacity of current_fs reduces to 0 - stored in "now"
                    //1. scale current_fs down to 80%
                    scale = 1 - (1 - now) * 0.2;
                    //2. bring next_fs from the right(50%)
                    left = (now * 50) + "%";
                    //3. increase opacity of next_fs to 1 as it moves in
                    opacity = 1 - now;
                    current_fs.css({ 'transform': 'scale(' + scale + ')' });
                    next_fs.css({ 'left': left, 'opacity': opacity });
                },
                duration: 800,
                complete: function () {
                    current_fs.hide();
                    animating = false;

                },
                //this comes from the custom easing plugin
                easing: 'easeInOutBack'
            });
        }
    });



    $(".previous").click(function () {
        if (animating) return false;
        animating = true;

        current_fs = $(this).parent();
        previous_fs = $(this).parent().prev();

        //de-activate current step on progressbar

        $("#progressbar div a").eq($("fieldset").index(current_fs)).removeClass("active btn-success"); 
        $("#progressbar div a").eq($("fieldset").index(previous_fs)).addClass("active btn-success");

        //show the previous fieldset
        previous_fs.show();
        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now, mx) {
                //as the opacity of current_fs reduces to 0 - stored in "now"
                //1. scale previous_fs from 80% to 100%
                scale = 0.8 + (1 - now) * 0.2;
                //2. take current_fs to the right(50%) - from 0%
                left = ((1 - now) * 50) + "%";
                //3. increase opacity of previous_fs to 1 as it moves in
                opacity = 1 - now;
                current_fs.css({ 'left': left });
                previous_fs.css({ 'transform': 'scale(' + scale + ')', 'opacity': opacity });
            },
            duration: 800,
            complete: function () {
                current_fs.hide();
                animating = false;
            },
            //this comes from the custom easing plugin
            easing: 'easeInOutBack'
        });
    });

    $(".submit").click(function () {
        return false;
    });


    $('#msform').find('input, textarea, select').on('keyup blur focus', function (e) {
        var $this = $(this),
            label = $this.prev('label');
        if (e.type === 'keyup') {
            if ($this.val() === '') {
                label.removeClass('active highlight');
                $("#addr2Country").show();

            } else {
                $("#addr2Country").show();
                label.addClass('active highlight');
            }
        }
        else if (e.type === 'blur') {
            if ($this.val() === '') {
                label.removeClass('active highlight');
            } else {
                label.addClass(' active highlight');
            }
        } else if (e.type === 'focus') {

            if ($this.val() === '') {
                label.removeClass('highlight');
            }
            else if ($this.val() !== "") {
                label.addClass('active highlight');
            }
        }
    });

    $('.tab a').on('click', function (e) {
         
        e.preventDefault();
        $(this).parent().addClass('active');
        $(this).parent().siblings().removeClass('active');
        target = $(this).attr('href');
        $('.tab-content > div').not(target).hide();
        $(target).fadeIn(600);
    });
});