using Attendance.Models;
using System.Data.SqlClient;
using System.Data;

namespace Attendance.Repository
{
    public class DeviceRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public DeviceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }
        public IEnumerable<StaffDevice> GetStaffDeviceStatusList(int ORGID)
        {
            List<StaffDevice> StaffDeviceListStatus = new List<StaffDevice>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_staffDeviceList", con);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = ORGID;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        StaffDevice obj_StaffDevice = new StaffDevice();
                        obj_StaffDevice.teacherName = dr["teacherName"].ToString();
                        obj_StaffDevice.teacherCode = dr["teacherCode"].ToString();
                        obj_StaffDevice.teacherMobileNumber = dr["teacherMobileNumber"].ToString();
                        obj_StaffDevice.deviceId = dr["deviceId"].ToString();
                        obj_StaffDevice.deviceStatus = Convert.ToInt32(dr["deviceStatus"]);
                        obj_StaffDevice.teacherId = Convert.ToInt32(dr["teacherId"]);


                        StaffDeviceListStatus.Add(obj_StaffDevice);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return StaffDeviceListStatus;
        }
    }
}
