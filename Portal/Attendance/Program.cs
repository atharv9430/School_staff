using Attendance;
using Attendance.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ReportRepository>(); 
builder.Services.AddScoped<StaffRepository>();
builder.Services.AddScoped<LoginRepository>();
builder.Services.AddScoped<OrganisationRepository>();
builder.Services.AddScoped<DeviceRepository>();

// Configure Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login/Index";  // Redirect if not authenticated
        options.AccessDeniedPath = "/AccessDenied";  // Redirect if unauthorized
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Session timeout
    });
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));  // 🔹 This enforces global authentication
});

var app = builder.Build();


//Environment.SetEnvironmentVariable("ITEXT_BOUNCY_CASTLE_FACTORY_NAME", "bouncy-castle");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Attendance API");
//        c.RoutePrefix = string.Empty; // Serve Swagger UI at the root (e.g., '/')
//    });
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 

// Ensure authentication runs before authorization
app.UseAuthentication();  // Add this
app.UseAuthorization();


app.UseRotativa();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=login}/{action=Index}/{id?}");

app.Run();
