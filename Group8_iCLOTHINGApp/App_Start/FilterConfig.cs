using System.Web;
using System.Web.Mvc;

namespace Group8_iCLOTHINGApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
