using AgroindustryManagementWeb.Services.Calculations;
using AgroindustryManagementWeb.Services.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // The URL of your IdentityServer project
                options.Authority = "https://localhost:5001";

                // These must match the Config.cs in IdentityServer
                options.ClientId = "mvc_client";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.GetClaimsFromUserInfoEndpoint = true;
                // Scopes you need
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                // Keep the token so we can read it later if needed
                options.SaveTokens = true;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    NameClaimType = "name", // Maps the 'name' claim to User.Identity.Name
                    RoleClaimType = "role"
                };
            });

builder.Services.AddDbContext<AGDatabaseContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IAGDatabaseService, AGDatabaseService>();
builder.Services.AddScoped<IAGCalculationService, AGCalculationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
