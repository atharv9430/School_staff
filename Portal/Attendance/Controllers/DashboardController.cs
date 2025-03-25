using Attendance.Models;
using Attendance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize(Roles = "ORG")]
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly StaffRepository _repository;
        private readonly OrganisationRepository _OrganisationRepository;
        public DashboardController(StaffRepository repository, OrganisationRepository OrganisationRepository)
        {
            _repository = repository;
            _OrganisationRepository = OrganisationRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SchoolDashBoardpage(int userId)
        {
          
            return View("~/Views/School/Dashboard.cshtml",userId);
        }
        public IActionResult StaffCount()    
        {
            List<StaffCount> staffsCount = (List<StaffCount>)_repository.GetStaffCount(Convert.ToInt32(User.Identity.Name));
            List<staffdailyStatus> staffStatusDaily = (List<staffdailyStatus>)_repository.GetStaffStatusList(Convert.ToInt32(User.Identity.Name));
            StaffCountModel model = new StaffCountModel
            {
                StaffCountList = staffsCount,
                StaffStatusList= staffStatusDaily
            };
            return View("~/Views/School/Dashboard.cshtml", model);
        }
    }
}
