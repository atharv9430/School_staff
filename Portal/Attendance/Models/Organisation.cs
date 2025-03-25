namespace Attendance.Models
{
    public class OrganisationTime
    {
        public int orgtimeid { get; set; }
        public string shiftName { get; set; }
        public TimeSpan InStartTime { get; set; }
        public TimeSpan InEndTime { get; set; }
        public TimeSpan OutStartTime { get; set; }
        public TimeSpan OutEndTime { get; set; }
        public int allowedRadius { get; set; }

    }

    public class AddOrganisation 
    {
        public int id { get; set; }
        public string organisationName { get; set; }
        public string organisationContactNumber { get; set; }
        public string organisationAddress { get; set; }
        public decimal organisationLatitude { get; set; }
        public decimal organisationLongitude { get; set; }

    }

    public class OrganisationList
    {
        public int id { get; set; }
        public string organisationName { get; set; }
        public string organisationContactNumber { get; set; }
        public string organisationAddress { get; set; }
        public double organisationLatitude { get; set; }
        public double organisationLongitude { get; set; }
        public string organisationPassword { get; set; }
    
    }

}
