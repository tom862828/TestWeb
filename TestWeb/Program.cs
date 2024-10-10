using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using MySqlConnector.Logging;
using TestWeb.Filters;

var builder = WebApplication.CreateBuilder(args);

// Setup log4net cofig
var logRepository = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
var log = LogManager.GetLogger(typeof(Program));
MySqlConnectorLogManager.Provider = new Log4netLoggerProvider();

// Output the default logging to log4net
builder.Logging.ClearProviders().AddConsole().AddLog4Net();

// Add services to encrypt and decrypt data (e.g: private key to encrypt and decrypt antiforgery token)
builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"D:\keys\"))
                .SetApplicationName("testWeb");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services for CSRF attack
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "testWeb.AntiforgeryCookie";
    options.FormFieldName = "AntiforgeryFieldname";
});

builder.Services.AddDistributedMemoryCache();

// Session timeout is 10 minutes
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // If user is not log in, user will be redirected to signin page
        options.LoginPath = "/Account/Signin";
        // Setup SlidingExpiration
        options.SlidingExpiration = true;
        // Setup cookie timeout
        options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    });

// Register global filters
builder.Services.AddMvc(options =>
{
    options.Filters.Add<SessionTimeoutAttribute>();
});

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
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "account",
    pattern: "{action}",
    defaults: new { controller = "Account", action = "Signin" });

app.Run();
