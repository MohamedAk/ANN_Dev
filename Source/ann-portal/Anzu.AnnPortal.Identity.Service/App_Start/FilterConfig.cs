using System.Web;
using System.Web.Mvc;

namespace Anzu.AnnPortal.Identity.Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
