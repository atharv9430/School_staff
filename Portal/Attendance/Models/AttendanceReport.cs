namespace Attendance.Models
{
    public class AttendanceRecord
    {
        public int EmpCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Present { get; set; }
        public int HalfLeave { get; set; }
        public int WeeklyOff { get; set; }
        public int Absent { get; set; }
        public int Leave { get; set; }
        public int PaidDays { get; set; }
        public int LateHours { get; set; }
        public string WorkHours { get; set; }
        public string OverTime { get; set; }
        public List<DailyAttendance> DailyRecords { get; set; } = new List<DailyAttendance>();
    }

    public class DailyAttendance
    {
        public string Date { get; set; }
        public string ArrivedTime { get; set; }
        public string DepartureTime { get; set; }
        public string WorkingHours { get; set; }
        public string OverTime { get; set; }
        public string Status { get; set; } // P, A, WO, etc.
    }

}
