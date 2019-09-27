var facilityList = 'VendorManagement/ListCompanyFacilityByVendorId/'

$(function () {
    $("#tbl_AllFacilityDetailsList").jsGrid({
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

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: $_HostPrefix + facilityList + "?VendorId=" + $_VendorID + "&LocationId=" + $_locationId,
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
           // { name: "VendorId", title: "Vendor Id", type: "text", width: 50 ,hidden:true },
            
            { name: "ProductServiceType", title: "Type", type: "text", width: 50 },
            { name: "ProductServiceName", title: "Product/Service Name", type: "text", width: 50 },
            { name: "UnitCost", title: "Unit Cost", type: "text", width: 50 },
            { name: "Tax", title: "Tax", type: "text", width: 50 },
            { name: "Costcode", title: "Costcode", type: "text", width: 50 },
            { name: "ProductImage", title: "ProductImage", type: "text", width: 50 },
             
             
        ]
    });




    //$("#tbl_AllFacilityDetailsList").jqGrid({
    //    url: $_HostPrefix+ facilityList + "?VendorId=" + $_VendorID + "&LocationId=" + $_locationId,
    //    datatype: 'json',
    //    height: 'auto',
    //    width: 700,
    //    autowidth: true,
    //    colNames: ['VendorId', 'Type', 'Product/Service Name', 'Unit Cost', 'Tax', 'Costcode', 'ProductImage', ],
    //    colModel: [{ name: 'VendorId', width: 100, sortable: true,hidden:true },
    //    { name: 'ProductServiceType', width: 100, sortable: true },
    //    { name: 'ProductServiceName', width: 350, sortable: false },
    //    { name: 'UnitCost', width: 200, sortable: false },
    //    { name: 'Tax', width: 150, sortable: false },
    //    { name: 'CostCodeDisplay', width: 250, sortable: false },
    //    { name: 'ProductImage', width: 170, sortable: false, formatter: imageFormat }],
    //    //{ name: 'act', index: 'act', width: 200, sortable: false }],
    //    rownum: 10,
    //    rowList: [10, 20, 30],
    //    scrollOffset: 0,
    //    pager: '#divAllFacilityDetailsListPager',
    //    //sortname: 'DriverLicenseNo',
    //    viewrecords: true,
    //    gridview: true,
    //    loadonce: false,
    //    //multiSort: true,
    //    rownumbers: true,
    //    emptyrecords: "No records to display",
    //    shrinkToFit: false,
    //    sortorder: 'asc',
    //    caption: "List of Facility",
    //    gridComplete: function () {

    //        var ids = jQuery("#tbl_AllFacilityDetailsList").jqGrid('getDataIDs');
    //        for (var i = 0; i < ids.length; i++) {
    //            var cl = ids[i];
    //            //be = '<div><a href="javascript:void(0)" class="EditRecord" Id="' + cl + '" title="edit" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-pencil fa-2x texthover-greenlight"></span><span class="tooltips">Edit</span></a>'
    //            //de = '<a href="javascript:void(0)" class="deleteRecord" title="delete" cid="' + cl + '" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="icon-trash fa-2x texthover-bluelight"></span><span class="tooltips">Delete</span></a>';
    //            //vi = '<a href="javascript:void(0)" class="viewRecord" title="view" vid="' + cl + '" id="DriverView" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="ui-icon ui-icon-disk"></span><span class="tooltips">Detail</span></a>';
    //            //qrc = '<a href="javascript:void(0)" class="qrc" title="Detail" data-value="' + cl + '" id="QRCGenerate" style=" float: left;margin-right: 10px;cursor:pointer;"><span class="glyphicon glyphicon-list-alt texthover-bluelight"></span><span class="tooltips">Detail</span></a></div>';
    //            //jQuery("#tbl_DriverList").jqGrid('setRowData', ids[i], { act: be + de + qrc }); ///+ qrc 
    //        }
    //        if ($("#tbl_AllFacilityDetailsList").getGridParam("records") <= 20) {
    //            $("#divAllFacilityDetailsListPager").hide();
    //        }
    //        else {
    //            $("#divAllFacilityDetailsListPager").show();
    //        }
    //        if ($('#tbl_AllFacilityDetailsList').getGridParam('records') === 0) {
    //            $('#tbl_AllFacilityDetailsList tbody').append("<div style='padding: 6px; font-size: 12px;'>No records found.</div>");
    //        }
    //    },
    //});
    //if ($("#tbl_AllFacilityDetailsList").getGridParam("records") > 20) {
    //    jQuery("#tbl_AllFacilityDetailsList").jqGrid('navGrid', '#divAllFacilityDetailsListPager', { edit: false, add: false, del: false, search: false, edittext: "Edit" });
    //}
});

function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue == "")
    { return ""; }
    else {
        return '<img src="' + cellvalue + '" class="gridimage" id="productImage" onclick="EnlargeImageView(this);"/>';
    }
}