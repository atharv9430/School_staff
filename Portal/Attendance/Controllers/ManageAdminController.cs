using Attendance.Models;
using Attendance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ManageAdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly OrganisationRepository _repository;


        public ManageAdminController(OrganisationRepository repository)
        {
            _repository = repository; 
        }
        public IActionResult Index()
        {
            return View();
        }
     
       
    }
}
