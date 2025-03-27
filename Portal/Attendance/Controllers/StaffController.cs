using Attendance.Models;
using Attendance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize(Roles ="ORG")]
    public class StaffController : Controller
    {
        private readonly IConfiguration _configuration; 
        private readonly StaffRepository _repository;
        private readonly OrganisationRepository _OrganisationRepository; 

        public StaffController(StaffRepository repository, OrganisationRepository OrganisationRepository)
        {
            _repository = repository;
            _OrganisationRepository = OrganisationRepository;
        }
        public IActionResult Index()
        { 
            return View();
        }
        public IActionResult AddStaff()
        { 
            return View("~/Views/School/AddStaff.cshtml");  
        } 
        public IActionResult manageTime()
        {

            return View("~/Views/School/AttendanceTime.cshtml");   
        }
        public IActionResult manualAttendance()
        {
            List<manualAttendance> manualAttendances = (List<manualAttendance>)_repository.getmanualAttendanceStaffList(Convert.ToInt32(User.Identity.Name));
            manualAttendanceModel model = new manualAttendanceModel
            {
                manualAttendanceList = manualAttendances
            };

            return View("~/Views/School/ManualAttendance.cshtml",model);
        }

        public IActionResult StaffList()
        {
            List<Staff> staffs = (List<Staff>)_repository.GetStaffs(Convert.ToInt32(User.Identity.Name));
            StaffListModel model = new StaffListModel
            {
                StaffList = staffs,
                //  CourseList=courses,


            };
            return View("~/Views/School/StaffList.cshtml",model);  
        }
        public IActionResult organisationTime()
        {
            List<OrganisationTime> OrganisationTimes = (List<OrganisationTime>)_OrganisationRepository.GetOrganisationTimes(Convert.ToInt32(User.Identity.Name));
            OrganisationTimeModel model = new OrganisationTimeModel
            {
                OrganisationTimeList = OrganisationTimes,
            };
            return View("~/Views/School/TimeList.cshtml", model);   
        }
        [HttpPost]
        public IActionResult UpdateOrganisationTime(OrganisationTime obj)
        {
            var isUpdated = _OrganisationRepository.UpdateOrganisationTimes(obj, Convert.ToInt32(User.Identity.Name));
            if (isUpdated)
                return Json(new { success = true });
            else
                return Json(new { success = false });
        }


        public IActionResult StaffAttendance()
        {
            List<StaffAttendanceStatus> StaffAttendanceStatuss = (List<StaffAttendanceStatus>)_repository.GetStaffAttendance(Convert.ToInt32(User.Identity.Name));
            StaffAttendanceStatusModel model = new StaffAttendanceStatusModel
            {
                StaffAttendanceStatusList = StaffAttendanceStatuss,
            };
            return View("~/Views/School/StaffCount.cshtml", model);
        }
        [HttpPost]
        public IActionResult StaffDelete(int id)
        { 
                bool isDeleted = false;
                string message = "";

                try
                {
                    // Call repository to delete the record
                    isDeleted = _repository.DeleteStaff(id,Convert.ToInt32(User.Identity.Name)); // Replace with your actual delete function

                    if (isDeleted)
                    {
                        message = "Staff record deleted successfully.";
                    }
                    else
                    {
                        message = "Failed to delete staff record.";
                    }
                }
                catch (Exception ex)
                {
                    message = "Error occurred while deleting: " + ex.Message;
                }

                // Return JSON response to the frontend
                return Json(new { success = isDeleted, responseText = message });
            }
        public IActionResult EditStaffDetail(int id)
        {
            var staff = _repository.GetStaffById(id,Convert.ToInt32(User.Identity.Name)); // Fetch staff details from DB

            if (staff == null)
            {
                return NotFound(); // Handle invalid staff ID
            } 
            return View(staff);
        }
        [HttpPost]
        public IActionResult UpdateStaffDetail(Staff obj)
        {
            var staff = _repository.UpdateStaff(obj, Convert.ToInt32(User.Identity.Name)); // Fetch staff details from DB

            TempData["isUpdated"] = staff.isUpdated;
            TempData["responseMsg"] = staff.responseMsg;
            return RedirectToAction("EditStaffDetail", new {id=obj.teacherId});
        }
        public IActionResult AddStaffDetail(string staffCode, string stfName, string stfType, string stfDepartment, string stfmobNumber, string stfjoiningDate, string stfEmail, string stfPasssword, string stfDesignation)
        {
            int result = _repository.addStaff(staffCode, stfName, stfType, stfDepartment, stfmobNumber, stfjoiningDate, stfEmail, stfPasssword, stfDesignation, Convert.ToInt32(User.Identity.Name));
            if (result > 0)
            {
                return View("~/Views/School/AddStaff.cshtml");

            }
            else
            {
                return View("~/Views/School/AddStaff.cshtml");
            }
        }
     
        public IActionResult AddStaffTiming(int shiftId, TimeOnly InStartTime, TimeOnly InEndTime, TimeOnly OutStartTime, TimeOnly OutEndTime, string allowedRadius)
        {
            int result = _repository.addStaffTiming(shiftId, InStartTime, InEndTime, OutStartTime, OutEndTime, allowedRadius, Convert.ToInt32(User.Identity.Name));
            if (result > 0)
            {
                return View("~/Views/School/AttendanceTime.cshtml");

            }
            else
            {
                return View("~/Views/School/AttendanceTime.cshtml");
            }
        }
        public IActionResult AddManualAttendance(int teacherId,string attendanceStatus,TimeOnly attendanceTime)
        {
            int result = _repository.addManualAttendance(teacherId, attendanceStatus, attendanceTime);
            if (result > 0)
            {
                return View("~/Views/School/ManualAttendance.cshtml");

            }
            else
            {
                return View("~/Views/School/ManualAttendance.cshtml");
            }
        }
    }
}
