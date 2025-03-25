using Attendance.Models;
using Attendance.Repository;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Attendance.Controllers
{
    [Authorize(Roles ="ORG")]
    public class DeviceVerificationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly DeviceRepository _repository;
        private readonly OrganisationRepository _OrganisationRepository;
        private readonly string _connectionString;
     

        public DeviceVerificationController(DeviceRepository repository, OrganisationRepository OrganisationRepository, IConfiguration configuration)
        {
            _repository = repository;
            _OrganisationRepository = OrganisationRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");


        }
      
        public IActionResult VerifyNewDevice()
        {
            List<StaffDevice> staffs = (List<StaffDevice>)_repository.GetStaffDeviceStatusList(Convert.ToInt32(User.Identity.Name));
            StaffDeviceListModel model = new StaffDeviceListModel
            {
                StaffDeviceList = staffs,
                //  CourseList=courses,


            };
            return View("~/Views/School/verfyDevice.cshtml", model);
        }
        [HttpPost]
        public IActionResult UpdateDeviceStatus(int id, int status)
        {
            if(id>0)
            {
              using(SqlConnection con=new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("upd_staffNewDevice", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@teacherId", System.Data.SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@deviceStatus", System.Data.SqlDbType.Int).Value = status;
                    cmd.ExecuteNonQuery();
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
            //var teacher = _context.Teachers.FirstOrDefault(t => t.TeacherId == id);
            //if (teacher != null)
            //{
            //    teacher.DeviceStatus = status;
            //    _context.SaveChanges();
            //    return Json(new { success = true });
            //}
            //return Json(new { success = false });
        }

    }
}
