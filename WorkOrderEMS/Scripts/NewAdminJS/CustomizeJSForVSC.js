

$(document).ready(function () {
    //Collaps menu and show only icons ..
    //$('#main-wrapper').toggleClass("menu-toggle");
    GetDropdownForChar();
    //$(".hamburger").hide();
    //$(".nav-user img").css({ "width": "30px", "height": "30px" });
    function GetDropdownForChar() {
        $.ajax({
            type: "GET",
            url: "../EPeople/GetListRequisition",
            success: function (data) {
                if (data != null) {
                    debugger

                    $('#DepartmentRequisition').html('');
                    $('#SuperiorRequisition').html('');
                    var optionsSuperior = '';
                    var optionsDepartment = '';
                    optionsSuperior += '<option value="0">-Select Superior--</option>';
                    optionsDepartment += '<option value="0">-Select Department--</option>';
                    for (var i = 0; i < data.listSuperiour.length; i++) {
                        optionsSuperior += '<option value="' + data.listSuperiour[i].parentId + '">' + data.listSuperiour[i].SeatingName + '</option>';
                    }
                    for (var i = 0; i < data.listDepartment.length; i++) {
                        optionsDepartment += '<option value="' + data.listDepartment[i].DeptId + '">' + data.listDepartment[i].DepartmentName + '</option>';
                    }
                    $('#SuperiorRequisition').append(optionsSuperior);
                    $('#DepartmentRequisition').append(optionsDepartment);
                }
            }
        })
    }
    //Add and remove text box of JD for VSC
    $('.addrows').click(function (e) {
        var divID = $('#routeDivRequisition div.dymanicAdd').length;
        $('#routeDivRequisition').append('<div class="form-group row dymanicAdd d' + divID + '"><div class="col-sm-11 getJobDesc"><input type="text" class="form-control input-rounded required" placeholder="Job Description" value="" /></div><div class="col-sm-1"><a class="addrows minusSign" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;" aria-hidden="true"></i></a></div></div>');
        $('#routeDivRequisition').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });
    //$("#AddChart").click(function () {
    //    $("#routeDiv").html("");
    //    $("#SeatingName").val("");
    //    $("#JobDescription").val("");
    //    $("#VCSId").val("");
    //    tinymce.activeEditor.setContent("");
    //    tinymce.editors.RolesAndResponsibility.setContent("");
    //    GetDropdownForChar();
    //    $("#myModalForChart").modal("show");
    //});
    ///To save Form Data 
    $('#SendVSCForApproval').click(function (e) {
        debugger
        // 
        var content = $("#tinymce").innerHTML;
        var content = window.parent.tinymce.get('RolesAndResponsibilityRequisition').getContent();
        $('#RolesAndResponsibilityRequisition').val(content);
        createAddJobDescArray();
        var dataObject = $("#SaveVCSChartFormForRequisition").serialize();

        $.ajax({
            type: "POST",
            url: '../EPeople/SendVCSForApproval', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
            data: dataObject,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                debugger
                GetDropdownForChar();
                $("#DepartmentRequisition").val("");
                $("#SeatingNameRequisition").val("");
                $("#JobDescriptionData").val("");
                $("#VCSIdRequisition").val("");
                $("#SuperiorRequisition").val("");
                $("#RateOfPay").val("");
                $("#JobDescriptionRequistion").val("");
                $("#routeDivRequisition").html("");
                //var addNewUrl = "@Url.Action("ePeopleDashboard", "NewAdmin")";
                $('#RenderPageId').load('../NewAdmin/ePeopleDashboard');
                tinymce.activeEditor.setContent("");
                $("#myModalForRequisitionVSCChart").modal("hide");
                $("#ListRquisitionData").jsGrid("loadData");
                //var addNewUrl = '../AdminSection/OrgChart/Index';// "@Url.Action("Index", "OrgChart", new { area = "AdminSection" })";
                //$('#RenderPageId').load(addNewUrl);
                toastr.success(Data.Message);
            },
            error: function (err) {
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    })
    //$('#SendVSCForApproval').click(function (e) {
    //    debugger
    //    createAddJobDescArray();
    //    var content = $("#tinymce").innerHTML;
    //    var content = window.parent.tinymce.get('RolesAndResponsibilityRequisition').getContent();
    //    $('#RolesAndResponsibilityRequisition').val(content);
    //    var dataObject = $("#SaveVCSChartFormForRequisition").serialize();

    //    $.ajax({
    //        type: "POST",
    //        url: '../EPeople/SendVCSForApproval', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
    //        data: dataObject,
    //        beforeSend: function () {
    //            new fn_showMaskloader('Please wait...');
    //        },
    //        success: function (Data) {
    //            debugger
    //            GetDropdownForChar();
    //            $("#DepartmentRequisition").val("");
    //            $("#SeatingNameRequisition").val("");
    //            $("#JobDescriptionData").val("");
    //            $("#VCSIdRequisition").val("");               
    //            $("#SuperiorRequisition").val("");
    //            $("#RateOfPay").val("");
    //            $("#JobDescriptionRequistion").val("");
    //            $("#routeDivRequisition").html("");
    //            //var addNewUrl = "@Url.Action("ePeopleDashboard", "NewAdmin")";
    //            $('#RenderPageId').load('../NewAdmin/ePeopleDashboard');
    //            tinymce.activeEditor.setContent("");
    //            $("#myModalForRequisitionVSCChart").modal("hide");
    //            //var addNewUrl = '../AdminSection/OrgChart/Index';// "@Url.Action("Index", "OrgChart", new { area = "AdminSection" })";
    //            //$('#RenderPageId').load(addNewUrl);
    //            toastr.success(Data.Message);
    //        },
    //        error: function (err) {
    //        },
    //        complete: function () {
    //            fn_hideMaskloader();
    //        }
    //    });
    //})

    ///This is use to add job description seperated by line
    function createAddJobDescArray() {
        JobDescFormat = $('#JobDescriptionRequistion').val() + '|';
        $("#routeDivRequisition div.dymanicAdd .getJobDesc").each(function () {
            var myObjJson = {};
            $this = $(this)
            var job = $this.find("input").val();
            JobDescFormat = JobDescFormat + job + '| ';
        });
        $('#JobDescriptionData').val(JobDescFormat);
    }


})