$(document).ready(function () {
    //******************** Password and Confirm Pwd Code **************************************//
    $('.pwd, .confpwd').on('keyup', function () {
        var spanclassname = (this).id;
        var modalname = $('#' + spanclassname).attr("data-modalname").slice(0, -1) + '_';
        if ($('#' + modalname + "ConfirmPassword").val() != null && $('#' + modalname + "ConfirmPassword").val() != '' && $('#' + modalname + "ConfirmPassword").val() != "") {
            if ($('#' + modalname + "Password").val() == $('#' + modalname + "ConfirmPassword").val()) {
                $('#' + modalname + "ConfirmPasswordSpan").hide();
            } else {
                $('#' + modalname + "ConfirmPasswordSpan").html("Password and Confirm Password must match.");
                $('#' + modalname + "ConfirmPasswordSpan").show();
            }
        }

    });

    $('#NewPassword, #ConfPassword').on('keyup', function () {
        
        //var spanclassname = (this).id;
        //var modalname = $('#' + spanclassname).attr("data-modalname").slice(0, -1) + '_';
        if ($("#ConfPassword").val() != null && $("#ConfPassword").val() != '' && $("#ConfPassword").val() != "") {
            if ($("#NewPassword").val() == $("#ConfPassword").val()) {
                $("#messageconfpwd").hide();
            } else {
                $('#errorMessage').html('');
                $("#messageconfpwd").html("Password and Confirm Password must match.");
                $("#messageconfpwd").show();
            }
        }

    });

    //minimum 8 characters
    var bad = /(?=.{8,}).*/;
    //Alpha Numeric plus minimum 8
    var good = /^(?=\S*?[a-z])(?=\S*?[0-9])\S{8,}$/;
    //Must contain at least one upper case letter, one lower case letter and (one number OR one special char).
    var better = /^(?=\S*?[A-Z])(?=\S*?[a-z])((?=\S*?[0-9])|(?=\S*?[^\w\*]))\S{8,}$/;
    //Must contain at least one upper case letter, one lower case letter and (one number AND one special char).
    var best = /^(?=\S*?[A-Z])(?=\S*?[a-z])(?=\S*?[0-9])(?=\S*?[^\w\*])\S{8,}$/;

    $('.pwd').on('keyup', function () {
        var password = $(this);
        var pass = password.val();
        var passLabel = $('[for="password"]');
        var stength = 'Weak';
        var pclass = 'danger';
        if (best.test(pass) == true) {
            stength = 'Very Strong';
            pclass = 'success';
        } else if (better.test(pass) == true) {
            stength = 'Strong';
            pclass = 'warning';
        } else if (good.test(pass) == true) {
            stength = 'Almost Strong';
            pclass = 'warning';
        } else if (bad.test(pass) == true) {
            stength = 'Weak';
        } else {
            stength = 'Very Weak';
        }

        var popover = password.attr('data-content', stength).data('bs.popover');
        popover.setContent();
        popover.$tip.addClass(popover.options.placement).removeClass('danger success info warning primary').addClass(pclass);

    });
    $('#NewPassword').on('keyup', function () {
        var password = $(this);
        var pass = password.val();
        var passLabel = $('[for="password"]');
        var stength = 'Weak';
        var pclass = 'danger';
        if (best.test(pass) == true) {
            stength = 'Very Strong';
            pclass = 'success';
        } else if (better.test(pass) == true) {
            stength = 'Strong';
            pclass = 'warning';
        } else if (good.test(pass) == true) {
            stength = 'Almost Strong';
            pclass = 'warning';
        } else if (bad.test(pass) == true) {
            stength = 'Weak';
        } else {
            stength = 'Very Weak';
        }

        var popover = password.attr('data-content', stength).data('bs.popover');
        popover.setContent();
        popover.$tip.addClass(popover.options.placement).removeClass('danger success info warning primary').addClass(pclass);

    });
    $('input[data-toggle="popover"]').popover({
        placement: 'top',
        trigger: 'focus'
    });
    $('#ChangePwdModal').on('hidden.bs.modal', function (e) {
        $(this)
          .find("input,textarea,select")
             .val('')
             .end()
          .find("input[type=checkbox], input[type=radio]")
             .prop("checked", "")
             .end();
        $('#errorMessage').html('');
    });
    $('#btnChange').click(function () {
        
        var objETracLoginModel;
        if ($('#NewPassword').val() == $('#ConfPassword').val()
            && $('#OldPassword').val() != null && $('#OldPassword').val() != ''
            && $('#NewPassword').val() != null && $('#NewPassword').val() != ''
            && $('#ConfPassword').val() != null && $('#ConfPassword').val() != '') {

            var oldpwd = $('#OldPassword').val();
            var newpwd = $('#NewPassword').val();
            objETracLoginModel = { "OldPassword": oldpwd, "NewPassword": newpwd };
            $.ajax({
                url: $_HostPrefix + '/Login/ChangePassword',
                data: JSON.stringify(objETracLoginModel),
                async: false,
                type: 'POST',
                //beforeSend: function () {
                //    new fn_showMaskloader('Changing...');
                //},
                contentType: "application/json",
                success: function (myresult) {
                    //toastr.success('Password changed successfully. Please login again.')                    
                    $('#errorMessage').html('');
                    if (myresult.Response == true) {
                        alert(myresult.Message);
                        toastr.success(myresult.Message);
                        window.location.href = $_HostPrefix;
                    }
                    else {
                        $('#ChangePwdModal').modal('toggle');
                        $('#errorMessage').html(myresult.Message)
                        toastr.error(myresult.Message);
                    }                    
                },
                error: function (er) {
                },
                //complete: function () {
                //    fn_hideMaskloader();
                //}

            });// ajax end block
            //$.ajax({
            //    url: '../Login/ChangePassword',
            //  //  url: '@Url.Action("ChangePassword", "Login")',
            //    data: { OldPassword: oldpwd, NewPassword: newpwd },
            //    type: 'POST',
            //    beforeSend: function () {
            //        new fn_showMaskloader('Changing...');
            //    },
            //    success: function (Data) {
            //        $('#errorMessage').html('')
            //        console.log(Data.Message);
            //        if (Data.Response == true) {
            //            toastr.success(data.Message)
            //        }
            //        else {
            //            $('#errorMessage').html(data.Message)
            //            toastr.error(data.Message)
            //        }
            //    },
            //    error: function (err) {
            //        console.log(err);
            //    },
            //    complete: function () {
            //        fn_hideMaskloader();
            //    }
            //});
        }
        else {
            $('#errorMessage').html('*Please fill mandatory fields.')
        }

    });
    //******************** Password and Confirm Pwd Code End here**************************************//

});


