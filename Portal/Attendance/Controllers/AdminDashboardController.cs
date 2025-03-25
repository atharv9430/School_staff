using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult dashBoardPage()
        {
            return View("~/Views/Admin/Dashboard.cshtml");
        }
    }
}
