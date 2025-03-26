using Attendance.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Attendance.Repository
{
    public class OrganisationRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public OrganisationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }
        public bool OrganisationRegistration(AddOrganisation obj)
        {
            try
            {

                if (!string.IsNullOrEmpty(obj.organisationName) && (!string.IsNullOrEmpty(obj.organisationContactNumber)) && (!string.IsNullOrEmpty(obj.organisationAddress)) && (obj.organisationLatitude != null) && (obj.organisationLongitude != null))
                {
                    string schoolCode = Guid.NewGuid().ToString().ToUpper();
                    using (SqlConnection con = new SqlConnection(_connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("ins_organisationRegistration", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@organisationCode", SqlDbType.NVarChar, 100).Value = schoolCode;
                        cmd.Parameters.Add("@organisationName", SqlDbType.NVarChar, 100).Value = obj.organisationName;
                       // cmd.Parameters.Add("@organisationLogo", SqlDbType.NVarChar, 100).Value = organisationLogo;
                        cmd.Parameters.Add("@organisationContactNumber", SqlDbType.NVarChar, 100).Value = obj.organisationContactNumber;
                        cmd.Parameters.Add("@organisationAddress", SqlDbType.NVarChar, 100).Value = obj.organisationAddress;
                        cmd.Parameters.Add("@organisationLatitude", SqlDbType.Decimal).Value = obj.organisationLatitude;
                        cmd.Parameters.Add("@organisationLongitude", SqlDbType.Decimal).Value = obj.organisationLongitude;
                        var userToken = GenerateJwtToken(obj.organisationName, obj.organisationContactNumber);
                        cmd.Parameters.Add("@organisationToken", SqlDbType.NVarChar, 500).Value = userToken;
                        int intResult = cmd.ExecuteNonQuery();
                        if (intResult == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    throw new Exception("Failed to save organsiation Detail.");
                }

            }
            catch (Exception ex)
            {

                throw new Exception("Failed to save organsiation Detail.", ex);
            }
        }


        private string GenerateJwtToken(string organisationName, string organisationContactNumber)
        {

            // from here
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Create claims
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, organisationName),
                new Claim(JwtRegisteredClaimNames.UniqueName, organisationContactNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            // Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddYears(20), // Token expiration time
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);  //
        }
        public IEnumerable<OrganisationTime> GetOrganisationTimes(int ORGID)
        {
            List<OrganisationTime> OrganisationTimeList = new List<OrganisationTime>();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("disp_organisationTime", con);
                    cmd.CommandType = CommandType.StoredProcedure; 
                    cmd.Parameters.Add("@organisationId", SqlDbType.Int).Value = ORGID;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        OrganisationTime obj_OrganisationTime = new OrganisationTime();
                        obj_OrganisationTime.orgtimeid = (int)dr["id"];

                        obj_OrganisationTime.InStartTime = dr["InStartTime"] != DBNull.Value ? (TimeSpan)dr["InStartTime"] : TimeSpan.Zero;
                        obj_OrganisationTime.InEndTime = dr["InEndTime"] != DBNull.Value ? (TimeSpan)dr["InEndTime"] : TimeSpan.Zero;
                        obj_OrganisationTime.OutStartTime = dr["OutStartTime"] != DBNull.Value ? (TimeSpan)dr["OutStartTime"] : TimeSpan.Zero;
                        obj_OrganisationTime.OutEndTime = dr["OutEndTime"] != DBNull.Value ? (TimeSpan)dr["OutEndTime"] : TimeSpan.Zero;

                        obj_OrganisationTime.allowedRadius = dr["allowedRadius"] != DBNull.Value ? Convert.ToInt32(dr["allowedRadius"]) : 0;

                        OrganisationTimeList.Add(obj_OrganisationTime);
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            return OrganisationTimeList;
        }

        public bool UpdateOrganisationTimes(OrganisationTime obj,int ORGID)
        {
            bool isUpdated = false;
             
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateOrganisationTimes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", obj.orgtimeid);
                    cmd.Parameters.AddWithValue("@organisationId", ORGID);
                    cmd.Parameters.AddWithValue("@InStartTime", obj.InStartTime);
                    cmd.Parameters.AddWithValue("@InEndTime", obj.InEndTime);
                    cmd.Parameters.AddWithValue("@OutStartTime", obj.OutStartTime);
                    cmd.Parameters.AddWithValue("@OutEndTime", obj.OutEndTime);
                    cmd.Parameters.AddWithValue("@AllowedRadius", obj.allowedRadius);   

                    SqlParameter resultParam = new SqlParameter("@IsUpdated", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    isUpdated = Convert.ToBoolean(resultParam.Value);
                }
            }
            return isUpdated;
        }


        public async Task<List<OrganisationList>> GetOrganisationsAsync()
        {
            var organisations = new List<OrganisationList>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetAllOrganisations", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                organisations.Add(new OrganisationList
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("organisationId")),
                                    organisationName = reader.GetString(reader.GetOrdinal("organisationName")),
                                    organisationContactNumber = reader.GetString(reader.GetOrdinal("organisationContactNumber")),
                                    organisationAddress = reader.GetString(reader.GetOrdinal("organisationAddress")),
                                    organisationLatitude = reader.GetDouble(reader.GetOrdinal("organisationLatitude")),
                                    organisationLongitude = reader.GetDouble(reader.GetOrdinal("organisationLongitude")),
                                    organisationPassword = reader.GetString(reader.GetOrdinal("organisationPassword"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return organisations;
        }
        public async Task<OrganisationList> GetOrganisationByIdAsync(int id)
        {
            OrganisationList organisation = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("GetOrganisationById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                organisation = new OrganisationList
                                {
                                    id = reader.GetInt32(reader.GetOrdinal("organisationId")),
                                    organisationName = reader.GetString(reader.GetOrdinal("organisationName")),
                                    organisationContactNumber = reader.GetString(reader.GetOrdinal("organisationContactNumber")),
                                    organisationAddress = reader.GetString(reader.GetOrdinal("organisationAddress")),
                                    organisationLatitude = Convert.ToDouble(reader["organisationLatitude"]),
                                    organisationLongitude = Convert.ToDouble(reader["organisationLongitude"]),
                                    organisationPassword = reader.GetString(reader.GetOrdinal("organisationPassword"))
                                };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return organisation;
        }
        public async Task<bool> UpdateOrganisationAsync(OrganisationList obj)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (var command = new SqlCommand("UpdateOrganisation", connection)) 
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@organisationId", obj.id);
                        command.Parameters.AddWithValue("@organisationName", obj.organisationName);
                        command.Parameters.AddWithValue("@organisationContactNumber", obj.organisationContactNumber);
                        command.Parameters.AddWithValue("@organisationAddress", obj.organisationAddress);
                        command.Parameters.AddWithValue("@organisationLatitude", obj.organisationLatitude);
                        command.Parameters.AddWithValue("@organisationLongitude", obj.organisationLongitude);
                        command.Parameters.AddWithValue("@organisationPassword", obj.organisationPassword);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
