using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            if (User?.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM input)
        {
            if (!ModelState.IsValid)
                return View(input);

            var user = _accountService.ValidateUser(input);

            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account is not allowed");
                return View(input);
            }

            var result = await _signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe, false);

            if (result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account is not allowed");

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
