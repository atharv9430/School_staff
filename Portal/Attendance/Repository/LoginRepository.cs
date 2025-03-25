using System.Data.SqlClient;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Attendance.Repository
{
    public class LoginRepository
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _configuration = configuration;
        }
        public int IsValidOrganisationLogin(string username, string password)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string userToken = GenerateJwtToken(username, password);
                SqlCommand cmd = new SqlCommand("OrganisationLoginCheck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@organisationContactNumber", SqlDbType.NVarChar, 100).Value = username;
                cmd.Parameters.Add("@organisationPassword", SqlDbType.NVarChar, 100).Value = password; 
                cmd.Parameters.Add("@token", SqlDbType.NVarChar, 1000).Value = userToken;
                try
                {
                    con.Open();
                    var userCount = cmd.ExecuteScalar();
                    // return userCount > 0;
                    if (userCount != null && int.TryParse(userCount.ToString(), out int id))
                    {
                        return id;
                    }
                    return 0;
                }
                catch (Exception ex)
                { 
                    throw new Exception("An error occurred while fetching students from the database.", ex);
                }
            }
        }
        private string GenerateJwtToken(string username, string password)
        { 
            // from here
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // Create claims
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.UniqueName, password),
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
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
