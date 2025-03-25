using Attendance.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Attendance.Controllers
{
    [AllowAnonymous]
    public class loginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly LoginRepository _repository;

        public loginController(LoginRepository repository)
        {
            _repository = repository;

        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("dashBoardPage", "AdminDashboard");
                }
                else
                {
                return RedirectToAction("StaffCount", "Dashboard");
                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> schoolLogin(string username, string password)
        {

            if (username == "admin" && password == "admin")
            {
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, 0.ToString()),
                            new Claim(ClaimTypes.Role, "Admin")  // Add role claim
                        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(claimsIdentity),
                                              authProperties);
                //return View("~/Views/A'dmin/Dashboard.cshtml");
                return RedirectToAction("dashBoardPage", "AdminDashboard");
            }
            else
            {
                try
                {
                    var userId = _repository.IsValidOrganisationLogin(username, password);

                    if (userId > 0)
                    { 
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, userId.ToString()),
                            new Claim(ClaimTypes.Role, "ORG")  // Add role claim
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties { IsPersistent = true };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                      new ClaimsPrincipal(claimsIdentity),
                                                      authProperties);

                        //HttpContext.Session.SetInt32("userId", userId);
                        //return View("~/Views/School/Dashboard.cshtml");
                        return RedirectToAction("StaffCount", "Dashboard");
                    }
                    else
                    {
                        TempData["success"] = false;
                        return RedirectToAction("Index"); 
                    }

                }
                catch (Exception)
                {
                    TempData["success"] = false;
                    return RedirectToAction("Index");
                }
            }

            //if (username == "school" && password == "school") // Example check
            //{
            //   // return RedirectToAction("School/Dashboard.cshtml"); // Redirect to the Dashboard action
            //    return View("~/Views/School/Dashboard.cshtml");
            //}
            //else if(username== "admin" && password == "admin")
            //{
            //    return View("~/Views/Admin/Dashboard.cshtml");
            //}

            // ViewBag.ErrorMessage = "Invalid username or password.";
            //return View(); // Reload the login page with an error message
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "login");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
