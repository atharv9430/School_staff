using Attendance.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using iText.Kernel.Pdf;
//using iTextSharp.text;
//using iText.Layout.Element;
using System.Data.SqlClient;
using Attendance.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
//using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Rotativa.AspNetCore;



namespace Attendance.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ReportRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportController(ReportRepository repository, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
            
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AttendanceReport()
        {
            return View("~/Views/School/AttendanceReport.cshtml");
        }

        public IActionResult MonthlyReport(string monthyear)
        {
            ViewBag.MonthYear = monthyear;
            if (string.IsNullOrEmpty(monthyear) )
            {
                return BadRequest("Start date or end date is missing.");
            } 
            DateTime date = DateTime.ParseExact(monthyear, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);

            // Extract month and year
            int month = date.Month;
            int year = date.Year; 

            int daysInMonth = DateTime.DaysInMonth(year, month);

            ReportRepository report = new ReportRepository(_configuration);
            var response = report.GetMonthlyAttendance(month, year, Convert.ToInt32(User.Identity.Name));

            var model = new MonthlyReportViewModel
            {
                ReportData = response,
                DaysInMonth = daysInMonth,
                Month = new DateTime(year, month, 1).ToString("MMMM yyyy")
            };

            return View(model);
        }

        public IActionResult MonthlyAttendencePdfGenerate(string monthyear)
        {
            //ViewBag.MonthYear = TempData["monthyear"] as string;
            

            if (string.IsNullOrEmpty(monthyear))
            {
                return BadRequest("Start date or end date is missing.");
            } 
            DateTime date = DateTime.ParseExact(monthyear, "yyyy-MM", System.Globalization.CultureInfo.InvariantCulture);

            // Extract month and year
            int month = date.Month;
            int year = date.Year;
             
            int daysInMonth = DateTime.DaysInMonth(year, month);

            ReportRepository report = new ReportRepository(_configuration);
            var response = report.GetMonthlyAttendance(month, year, Convert.ToInt32(User.Identity.Name));

            ViewData["IsPdf"] = "true"; // Pass flag to view
            var model = new MonthlyReportViewModel
            {
                ReportData = response,
                DaysInMonth = daysInMonth,
                Month = new DateTime(year, month, 1).ToString("MMMM yyyy")
            };
            return new ViewAsPdf("MonthlyReport", model)
            {
                FileName = "Monthly_Attendance_Report.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape, // Set Landscape Mode
                PageSize = Rotativa.AspNetCore.Options.Size.A4, // Optional: Set page size
                CustomSwitches = "--disable-smart-shrinking --print-media-type --no-print-media-type" // Optional: Prevents layout distortion
            };
        }




   

    }
    // Model for Attendance Data
   
}
