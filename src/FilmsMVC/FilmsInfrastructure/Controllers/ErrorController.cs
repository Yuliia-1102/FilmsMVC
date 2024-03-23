using Microsoft.AspNetCore.Mvc;

namespace FilmsInfrastructure.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
