using System.Web.Mvc;

namespace WorkOrderEMS.Areas.AdminSection
{
    public class AdminSectionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminSection";
            }
        }

        //public override void RegisterArea(AreaRegistrationContext context) 
        //{
        //    context.MapRoute(
        //        "AdminSection_default",
        //        "AdminSection/{controller}/{action}/{id}",
        //        new { action = "Index", id = UrlParameter.Optional },
        //        namespaces: new[] { "WorkOrderEMS.Areas.AdminSection.Controllers" }
        //    );
        //}
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdminSection_default",
                //"{controller}/{action}/{id}",
                "AdminSection/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WorkOrderEMS.Areas.AdminSection.Controllers" }
            );
        }
    }
}