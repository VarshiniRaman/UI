using System.Web;
using System.Web.Mvc;

namespace HP.Robotics.OSSCommentsUploadUI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
