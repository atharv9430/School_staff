using System.Data.SqlClient;
using System.Data;
using Attendance.Models;
using Microsoft.IdentityModel.Tokens;

namespace Attendance.Repository
{
    public class StaffRepository
    {
        private readonly string _connectionString;
        public StaffRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public int addStaff(string staffCode, string stfName, string stfType, string stfDepartment, string stfmobNumber, string stfjoiningDate, string stfEmail, string stfPasssword, string stfDesignation, int loginuserid)
        {
            try
            {
                if (!string.IsNullOrEmpty(staffCode) &&(!string.IsNullOrEmpty(staffCode)))
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("ins_staffDetail", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        var userId = loginuserid;
                        cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = userId;// staff.teacherCode;
                        cmd.Parameters.Add("@teacherCode", SqlDbType.NVarChar, 100).Value = staffCode;
                        cmd.Parameters.Add("@teacherName", SqlDbType.NVarChar, 100).Value = stfName;
                        cmd.Parameters.Add("@teacherGender", SqlDbType.NVarChar, 100).Value ="Gender";
                        cmd.Parameters.Add("@teacherType", SqlDbType.NVarChar, 100).Value = stfType;
                        cmd.Parameters.Add("@teacherDepartment", SqlDbType.NVarChar, 100).Value = stfDepartment;
                        cmd.Parameters.Add("@teacherMobileNumber", SqlDbType.NVarChar, 100).Value = stfmobNumber;
                        cmd.Parameters.Add("@teacherDesignation", SqlDbType.NVarChar, 100).Value = stfDesignation;
                        cmd.Parameters.Add("@teacherJoiningDate", SqlDbType.NVarChar, 100).Value = stfjoiningDate;
                        cmd.Parameters.Add("@teacherEmailId", SqlDbType.NVarChar, 100).Value = stfEmail;
                        cmd.Parameters.Add("@teacherPassword", SqlDbType.NVarChar, 100).Value = stfPasssword;
                        int intResult = cmd.ExecuteNonQuery();
                        if (intResult > 0)
                        {
                            return intResult;
                        }
                    }

                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int addStaffTiming(int shiftId, TimeOnly InStartTime, TimeOnly InEndTime, TimeOnly OutStartTime, TimeOnly OutEndTime, string allowedRadius, int loginuserid)
        {
            try
            {
                if (shiftId!=null && (InStartTime!=null))
                {
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("ins_staffTiming", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        var organisationId = loginuserid;
                        cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                        cmd.Parameters.Add("@shiftId", SqlDbType.Int).Value = shiftId;
                        cmd.Parameters.Add("@InStartTime", SqlDbType.Time).Value = InStartTime.ToTimeSpan(); ;
                        cmd.Parameters.Add("@InEndTime", SqlDbType.Time).Value = InEndTime.ToTimeSpan(); ;
                        cmd.Parameters.Add("@OutStartTime", SqlDbType.Time).Value = OutStartTime.ToTimeSpan(); ;
                        cmd.Parameters.Add("@OutEndTime", SqlDbType.Time).Value = OutEndTime.ToTimeSpan(); ;
                        cmd.Parameters.Add("@allowedRadius", SqlDbType.Int).Value = allowedRadius;
                        int intResult = cmd.ExecuteNonQuery();
                        if (intResult > 0)
                        {
                            return intResult;
                        }
                    }

                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        } 
        public int addManualAttendance(int teacherId, string attendanceStatus, string attendanceTime)
        {
            try
            {
                if(teacherId!=null &&(attendanceStatus!=null)&&(attendanceTime!=null))
                {
                    using(SqlConnection con=new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("ins_ManualAttendance", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@teacherId", SqlDbType.Int).Value = teacherId;
                     
                        cmd.Parameters.Add("@attendanceStatus", SqlDbType.NVarChar, 100).Value = attendanceStatus;
                        cmd.Parameters.Add("@staffAttendanceTime", SqlDbType.NVarChar,100).Value = attendanceTime;
                        int intResult = cmd.ExecuteNonQuery();
                        if (intResult > 0)
                        {
                            return intResult;
                        }
                    }
                }
                return 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<Staff> GetStaffs(int loginuserid)
        {
            List<Staff> StaffList = new List<Staff>();
            int srno = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_StaffByOrganisation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var organisationId = loginuserid;
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        srno++;
                        Staff obj_staff = new Staff();
                        obj_staff.srno = srno;
                        obj_staff.organisationId = Convert.ToInt32(dr["organisationId"]);
                        obj_staff.teacherId = Convert.ToInt32(dr["teacherId"]);
                        obj_staff.teacherCode = dr["teacherCode"].ToString();
                        obj_staff.teacherName = dr["teacherName"].ToString();
                        obj_staff.teacherGender = dr["teacherGender"].ToString();
                        obj_staff.teacherType = dr["teacherType"].ToString();
                        obj_staff.teacherDepartment = dr["teacherDepartment"].ToString();
                        obj_staff.teacherMobileNumber = dr["teacherMobileNumber"].ToString();
                        obj_staff.teacherDesignation = dr["teacherDesignation"].ToString();
                        obj_staff.teacherJoiningDate = dr["teacherJoiningDate"].ToString();
                        obj_staff.teacherEmailId = dr["teacherEmailId"].ToString();
                        obj_staff.teacherPassword = dr["teacherPassword"].ToString();
                        obj_staff.status = Convert.ToBoolean(dr["status"]);
                        StaffList.Add(obj_staff);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return StaffList;   //
        }
        public IEnumerable<manualAttendance> getmanualAttendanceStaffList(int loginuserid)
        {
            List<manualAttendance> manualAttendanceList = new List<manualAttendance>();
            int srno = 0;
            try
            {
                using(SqlConnection con=new SqlConnection(_connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("disp_manualAttendanceStaffList", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var organisationId = loginuserid;
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while(dr.Read())
                    {
                        srno++;
                        manualAttendance obj_manualAttendance = new manualAttendance();
                        obj_manualAttendance.srno = srno;
                        
                        obj_manualAttendance.teacherId = Convert.ToInt32(dr["teacherId"]);
                        obj_manualAttendance.teacherName = dr["teacherName"].ToString();
                        obj_manualAttendance.teacherMobileNumber = dr["teacherMobileNumber"].ToString();
                        manualAttendanceList.Add(obj_manualAttendance);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return manualAttendanceList;
        }
        public IEnumerable<staffdailyStatus> GetStaffStatusList(int loginuserid)
        {
            List<staffdailyStatus> StaffListStatus = new List<staffdailyStatus>();
            try
            {
                int srno = 0;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_TeacherAttendanceStatusForToday", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var organisationId = loginuserid;
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        srno++;
                        staffdailyStatus obj_StaffCount = new staffdailyStatus(); 
                        obj_StaffCount.srno = srno;
                        obj_StaffCount.teacherName =dr["teacherName"].ToString();
                        obj_StaffCount.teacherCode = dr["teacherCode"].ToString();
                        obj_StaffCount.AttendanceStatus = dr["AttendanceStatus"].ToString();
                        obj_StaffCount.AttendanceStatus = dr["AttendanceStatus"].ToString();
                        obj_StaffCount.PunchIn = dr["PunchIn"] != DBNull.Value ? Convert.ToDateTime(dr["PunchIn"]) : (DateTime?)null;
                        obj_StaffCount.PunchOut = dr["PunchOut"] != DBNull.Value ? Convert.ToDateTime(dr["PunchOut"]) : (DateTime?)null;



                        StaffListStatus.Add(obj_StaffCount);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return StaffListStatus;
        }
        public IEnumerable<StaffCount> GetStaffCount(int loginuserid)
        {
            List<StaffCount> StaffCountList = new List<StaffCount>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_StaffCount", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //var organisationId = loginuserid;
                    var organisationId = loginuserid;
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        StaffCount obj_StaffCount = new StaffCount();
                        obj_StaffCount.present = Convert.ToInt32(dr["present"]);
                        obj_StaffCount.absent = Convert.ToInt32(dr["absent"]);
                        obj_StaffCount.total = Convert.ToInt32(dr["total"]);

                        StaffCountList.Add(obj_StaffCount);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return StaffCountList;
        }

        public IEnumerable<StaffAttendanceStatus> GetStaffAttendance(int loginuserid)
        {
            List<StaffAttendanceStatus> StaffAttendanceStatusList = new List<StaffAttendanceStatus>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_staffStatusCountMonthly", con);    // Staff Current Status Absent or present with name
                    cmd.CommandType = CommandType.StoredProcedure;
                    var organisationId = loginuserid;
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = organisationId;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        StaffAttendanceStatus obj_staffAttendanceStatus = new StaffAttendanceStatus();
                        obj_staffAttendanceStatus.teacherName = dr["teacherName"].ToString();
                        obj_staffAttendanceStatus.teacherCode = dr["teacherCode"].ToString();
                        obj_staffAttendanceStatus.daysPresent = dr["present"].ToString();
                        obj_staffAttendanceStatus.daysAbsent = dr["absent"].ToString(); 
                     

                        StaffAttendanceStatusList.Add(obj_staffAttendanceStatus);

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            return StaffAttendanceStatusList;
        }

        public Staff GetStaffById(int id, int ORGID)
        {
            Staff staffDetails = null;

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_GetStaffByID", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@organisationId", ORGID);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        staffDetails = new Staff
                        {
                            teacherId = Convert.ToInt32(dr["teacherId"]), 
                            teacherCode = dr["teacherCode"].ToString(),
                            teacherName = dr["teacherName"].ToString(),
                            teacherGender = dr["teacherGender"].ToString(),
                            teacherType = dr["teacherType"].ToString(),
                            teacherDepartment = dr["teacherDepartment"].ToString(),
                            teacherMobileNumber = dr["teacherMobileNumber"].ToString(),
                            teacherDesignation = dr["teacherDesignation"].ToString(),
                            teacherJoiningDate = dr["teacherJoiningDate"].ToString(),
                            teacherEmailId = dr["teacherEmailId"].ToString(),
                            teacherPassword = dr["teacherPassword"].ToString(),
                            status = Convert.ToBoolean(dr["status"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return staffDetails;
        }

        public (bool isUpdated,string responseMsg) UpdateStaff(Staff staff,int ORGID)
        {
            bool isUpdated = false;
            string responseMessage = "";
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateStaff", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@teacherId", staff.teacherId);
                    cmd.Parameters.AddWithValue("@organisationId", ORGID);
                    cmd.Parameters.AddWithValue("@teacherCode", staff.teacherCode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherName", staff.teacherName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherGender", staff.teacherGender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherType", staff.teacherType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherDepartment", staff.teacherDepartment ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherMobileNumber", staff.teacherMobileNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherDesignation", staff.teacherDesignation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherJoiningDate", staff.teacherJoiningDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherEmailId", staff.teacherEmailId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@teacherPassword", staff.teacherPassword ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@status", staff.status);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        isUpdated = Convert.ToBoolean(dr["result"]);
                        responseMessage = dr["responseMsg"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                responseMessage = "Error: " + ex.Message;

            }

            return (isUpdated, responseMessage);
        }
        public bool DeleteStaff(int id,int ORGID)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteStaff", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@teacherId", id);
                cmd.Parameters.AddWithValue("@organisationId", ORGID);

                con.Open();
                var result = cmd.ExecuteScalar();
                return Convert.ToBoolean(result);
            }
        }



    }
}
