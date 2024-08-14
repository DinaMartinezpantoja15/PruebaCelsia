using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PruebaCelsia.Interfaces.Account;
using PruebaCelsia.ViewModels;

namespace PruebaCelsia.Controllers.Account
{
   // [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

          [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Error en los campos.";
                return View();
            }

            try
            {
                var user = await _accountService.Register(userVM);

                if (user.Id != 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                ViewData["Message"] = "No se pudo registrar el usuario, error fatal.";
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Error en los campos.";
                return View();
            }

            var user = await _accountService.Login(userVM);

            if (user == null)
            {
                ViewData["Message"] = "Correo o contraseña incorrectos.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var properties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();  // Limpia la sesión completamente
            return RedirectToAction("Login", "Account");
        }








        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}