using Attendance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Attendance.Repository
{
    public class ReportRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public ReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }
        //public DataTable GetMonthlyAttendance(int month, int year, HttpContext httpContext)
        public DataTable GetMonthlyAttendance(int Month,int Year,int SelectedstaffType,int ORGID)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMonthlyAttendanceReport2", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; 
                    
                    cmd.Parameters.AddWithValue("@Month", Month);
                    cmd.Parameters.AddWithValue("@Year", Year);
                    cmd.Parameters.AddWithValue("@staffTypeId", SelectedstaffType);
                    cmd.Parameters.AddWithValue("@organisationId", ORGID);

                    conn.Open();
                    
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
    }
}
