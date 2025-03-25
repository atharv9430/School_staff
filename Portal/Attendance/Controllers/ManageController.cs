using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
