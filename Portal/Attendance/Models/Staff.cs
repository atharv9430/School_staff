namespace Attendance.Models
{
    public class Staff
    {
        public int srno { get; set; } 
        public int teacherId { get; set; } 
        public int organisationId { get; set; }
        public string teacherCode { get; set; }
        public string teacherName { get; set; }
        public string teacherGender { get; set; }
        public string teacherType { get; set; }
        public string teacherDepartment { get; set; }
        public string teacherMobileNumber { get; set; }
        public string teacherDesignation { get; set; }
        public string teacherJoiningDate { get; set; }
        public string teacherEmailId { get; set; }
        public string teacherPassword { get; set; }
        public bool status { get; set; }

    }
    public class StaffAttendanceStatus
    {
        public string teacherName { get; set; }
         public string teacherCode { get; set; }

        public  string totaldays { get; set; }
        public  string daysPresent { get; set; }
        public  string daysAbsent { get; set; }

    }
    public class StaffCount
    {
        public int total { get; set; }
        public int present { get; set; }
        public int absent { get; set; }
    }
    public class staffdailyStatus
    {
        public int srno { get; set; }
        public string teacherName { get; set; }
        public string teacherCode { get; set; }
        public DateTime? PunchIn { get; set; }
        public DateTime? PunchOut { get; set; } 
        public string PunchInTime => PunchIn?.ToString("HH:mm:ss") ?? "N/A";
        public string PunchOutTime => PunchOut?.ToString("HH:mm:ss") ?? "N/A";
        public string AttendanceStatus { get; set; }
    }
}
