

$(document).ready(function () {
    //Collaps menu and show only icons ..
    $('#main-wrapper').toggleClass("menu-toggle");
    GetDropdownForChar();
    $(".hamburger").hide();
    $(".nav-user img").css({ "width": "30px", "height": "30px" });
    function GetDropdownForChar() {
        $.ajax({
            type: "GET",
            url: "../AdminSection/AdminDashboard/GetList",
            success: function (data) {
                debugger
                if (data != null) {

                    $('#Department').html('');
                    $('#Superior').html('');
                    var optionsSuperior = '';
                    var optionsDepartment = '';
                    optionsSuperior += '<option value="0">-Select Superior--</option>';
                    optionsDepartment += '<option value="0">-Select Department--</option>';
                    for (var i = 0; i < data.listSuperiour.length; i++) {
                        debugger
                        optionsSuperior += '<option value="' + data.listSuperiour[i].parentId + '">' + data.listSuperiour[i].SeatingName + '</option>';
                    }
                    for (var i = 0; i < data.listDepartment.length; i++) {
                        debugger
                        optionsDepartment += '<option value="' + data.listDepartment[i].DeptId + '">' + data.listDepartment[i].DepartmentName + '</option>';
                    }
                    $('#Superior').append(optionsSuperior);
                    $('#Department').append(optionsDepartment);
                }
            }
        })
    }
    //Add and remove text box of JD for VSC
    $('.addrows').click(function (e) {
        var divID = $('#routeDiv div.dymanicAdd').length;
        $('#routeDiv').append('<div class="form-group row dymanicAdd d' + divID + '"><label class="col-sm-2 col-form-label">Job Description</label><div class="col-sm-9 getJobDesc"><input type="text" class="form-control input-rounded required" placeholder="Job Description" value="" /></div><div class="col-sm-1"><a class="addrows minusSign" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;" aria-hidden="true"></i></a></div></div>');
        $('#routeDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });
    $('.addjobtitlerows').click(function (e) {
        debugger
        var divID = $('#jobTitleDiv div.dymanicAdd').length;
        $('#jobTitleDiv').append('<div class="form-group row dymanicAdd d' + divID + '"><div class="col-sm-10 getJobTitleDesc"><input type="text" class="form-control input-rounded required" placeholder="Job Title" value="" style="width: 600px;" /></div><div class="col-sm-1"><a class="addjobtitlerows minusSign" id=d' + divID + '><i class="fa fa-minus-circle addColorPlusMinus fa-2x" style="cursor:pointer;margin-left: 30px;" aria-hidden="true"></i></a></div></div>');
        $('#jobTitleDiv').append('<script>jQuery("a.minusSign#d' + divID + '").click(function (){$("div.d' + divID + '").remove();  });</script>');
    });
   
    ///To save Form Data 
    $('#SaveVSC').click(function (e) {
        debugger
        createAddJobDescArray();
        var content = tinymce.activeEditor.getContent({ format: 'raw' });
        $('#RolesAndResponsibility').val(content);
        var dataObject = $("#SaveVCSChartForm").serialize();
        
        $.ajax({
            type: "POST",
            url:'../AdminSection/AdminDashboard/SaveVCS', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
            data: dataObject,

            success: function (Data) {
                debugger
                GetDropdownForChar();
                $("#routeDiv").html("");
                $("#SeatingName").val("");
                $("#JobDescription").val("");
                $("#myModalForChart").modal("hide");
                tinymce.activeEditor.setContent("");
            },
            error: function (err) {
            }
        });
    })

    ///This is use to add job description seperated by line
    function createAddJobDescArray() {
        debugger
        JobDescFormat = $('#JobDescription').val() + '|';
        $("#routeDiv div.dymanicAdd .getJobDesc").each(function () {
            debugger
            var myObjJson = {};
            $this = $(this)
            var job = $this.find("input").val();
            JobDescFormat = JobDescFormat + job + '| ';
        });
        $('#JobDesc').val(JobDescFormat);
    }

    //This is to save Job Title for Chart
    $('#SaveJobTitle').click(function (e) {
        debugger
        createAddJobTitleArray();
        var getId = $("#parentIdForJobTitle").val();
        var dataJobTitleObject = $("#SaveJobTitleForm").serialize();
        $.ajax({
            type: "POST",
            url: '../AdminSection/AdminDashboard/SaveJobTitle', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
            data: dataJobTitleObject,

            success: function (Data) {
                debugger
                $("#jobTitleDiv").html("");
                $("#parentIdForJobTitle").val("");
                $("#JobTitle").val("");
                $("#myModalForAddingJobTitle").modal("hide");
            },
            error: function (err) {
            }
        });
    })
    function createAddJobTitleArray() {
        JobTitleFormat = $('#JobTitle').val() + '|';
        $("#jobTitleDiv div.dymanicAdd .getJobTitleDesc").each(function () {
            var myObjJson = {};
            $this = $(this)
            var jobTitle = $this.find("input").val();
            JobTitleFormat = JobTitleFormat + jobTitle + '| ';
        });
        $('#JobTitleDesc').val(JobTitleFormat);
    }
  
})