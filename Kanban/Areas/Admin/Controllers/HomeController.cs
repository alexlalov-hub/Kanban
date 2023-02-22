using Microsoft.AspNetCore.Mvc;

namespace Kanban.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
