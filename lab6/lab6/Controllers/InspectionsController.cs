using Microsoft.AspNetCore.Mvc;

namespace lab6.Controllers
{
    public class InspectionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
