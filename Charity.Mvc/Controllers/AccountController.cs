using System.Text;
using System.Threading.Tasks;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services.Email;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Charity.Mvc.Controllers
{
    public class AccountController : Controller
    {
        protected UserManager<User> UserManager { get; }
        protected SignInManager<User> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }
        private readonly IAdminService _adminService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager, IAdminService adminService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm]RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user, "User");

                    //wyslanie email z potwierdzeniem logowania
                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, code}, Request.Scheme, Request.Host.ToString());
                    var email = new EmailViewModel
                    {
                        Subject = "Wiadomość weryfikująca email",
                        IsHtml = true,
                        Body = $"Kliknij link do weryfikacji <a href=\"{link}\">Weryfikacja email</a> </br> Jeżeli link nie działa skopiuj go do przegladarki {link}"
                    };
                    await EmailService.SendEmailAsync(email);
                    return RedirectToAction("Login", "Account");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            ViewBag.Error = true;

            return View(model);
        }

        public async Task<IActionResult> VerifyEmail (string userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("Registration", "Account");
            var result = await UserManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return RedirectToAction("Registration", "Account");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Error = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //ViewData["Title"] = Tabs.Other;
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Account");
                }

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Użytkownik nie ma zweryfikowanego adresu e-mail");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Uzytkownik jest zablokowany");
                }
                else
                {
                    ModelState.AddModelError("", "Błąd logowania");
                }
            }
            ViewBag.Error = true;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id = 0)
        {

            var user = id == 0 ? await UserManager.GetUserAsync(User) : await _adminService.GetUser(id);
            if (user == null)
                return RedirectToAction("Login", "Account");//redirect to Login?
            //var address = await _adminService.GetUserAddress(user.Id);
            //if (address != null) user.Address = address;
            //else
            //    user.Address = new Address();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            //ViewBag.Active = "Account";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.GetUserAsync(User);
            if (user == null) return View("Login", "Account");

            //user.Address = await _accountService.GetUserAddress(user.Id);
            //if (user.Address == null)
            //    user.Address = new Address();
            user.Name = model.Name;
            user.LastName = model.LastName;
            //user.PhoneNumber = model.PhoneNumber;
            //user.Address.Street = model.Street;
            //user.Address.ZipCode = model.ZipCode;
            //user.Address.City = model.City;
            //user.Address.Country = model.Country;

            var result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                ViewBag.Status = "Dane zostały pomyślnie zmienione";
                return View(model);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            //return RedirectToAction("Index", "Dashboard");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PasswordEdit()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PasswordEdit(EditPassViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            //var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var result = await UserManager.ChangePasswordAsync(user, model.OldPass, model.Password);  //ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                ViewBag.Status = "Hasło zostało pomyślnie zmienione";
                return View();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        //public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        //{
        //    if (email == null)
        //    {
        //        return RedirectToPage("/Index");
        //    }

        //    var user = await UserManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with email '{email}'.");
        //    }

        //    Email = email;
        //    // Once you add a real email sender, you should remove this code that lets you confirm the account
        //    DisplayConfirmAccountLink = true;
        //    if (DisplayConfirmAccountLink)
        //    {
        //        var userId = await UserManager.GetUserIdAsync(user);
        //        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        EmailConfirmationUrl = Url.Page(
        //            "/Account/ConfirmEmail",
        //            pageHandler: null,
        //            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //            protocol: Request.Scheme);
        //    }

        //    return View();
        //}
    }
}
