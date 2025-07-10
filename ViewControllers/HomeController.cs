using Microsoft.AspNetCore.Mvc;

namespace Web_Manage.ViewControllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
