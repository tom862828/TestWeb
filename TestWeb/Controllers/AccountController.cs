using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TestWeb.Models;
using TestWeb.Repositories;

namespace TestWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }
        public IActionResult Signin()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(string username, string password)
        {
            using (DbConnection dc = new DbConnection())
            {
                string sql = @"select * from users where User_name = @User_name and User_password = @User_password";
                var user = (await dc.QueryAsync<Account>(sql, new { User_name = username.ToUpper(), User_password = password })).FirstOrDefault();

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.User_name)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Set session
                    HttpContext.Session.SetInt32("User_session", user.Key_no);
                    //return RedirectToAction("Index", "Home");
                    return Redirect("/Home");
                }
                else
                {
                    ViewBag.LoginFailedFlag = "Y";
                    return View();
                }
            }
        }
    }
}
