using Attendance.Models;

namespace Attendance.Models
{
    public class HomeViewModel
    {
    }
    public class StaffListModel
    {
        public List<Staff> StaffList { get; set; } = new List<Staff>();
    }
    public class OrganisationTimeModel    
    {
        public List<OrganisationTime> OrganisationTimeList { get; set; } = new List<OrganisationTime>();
    }
    public class StaffAttendanceStatusModel       
    {
        public List<StaffAttendanceStatus> StaffAttendanceStatusList { get; set; } = new List<StaffAttendanceStatus>();
    }
    public class StaffCountModel
    {
        public List<StaffCount> StaffCountList { get; set; } = new List<StaffCount>();
        public List<staffdailyStatus> StaffStatusList { get; set; } = new List<staffdailyStatus>();
    }
    public class StaffDeviceListModel   
    {
        public List<StaffDevice> StaffDeviceList { get; set; } = new List<StaffDevice>();
}
}
