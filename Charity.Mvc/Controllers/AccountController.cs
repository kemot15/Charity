using System.Collections.Immutable;
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
                    
                    await SendEmailTokenConfirmationAsync(user);
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

        public async Task SendEmailTokenConfirmationAsync(User user)
        {
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(VerifyEmail), "Account", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());
            var email = new EmailViewModel
            {
                To = user.Email,
                Subject = "Wiadomość weryfikująca email",
                IsHtml = true,
                Body = $"Kliknij link, żeby potwierdzić adres e-mail <a href=\"{link}\">Link</a> </br> Jeżeli link nie działa skopiuj go do przegladarki {link}"
            };
            await EmailService.SendEmailAsync(email);
        }

        

        public async Task<IActionResult> VerifyEmail (string userId, string token)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null) return RedirectToAction("Registration", "Account");
            var result = await UserManager.ConfirmEmailAsync(user, token);
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

            var user = id == 0 ? await UserManager.GetUserAsync(User) : await _adminService.GetUserAsync(id);
            if (user == null)
                return RedirectToAction("Login", "Account");//redirect to Login?


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
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.GetUserAsync(User);
            if (user == null) return View("Login", "Account");
                        
            user.Name = model.Name;
            user.LastName = model.LastName;

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

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {         
          
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Nie ma takiego użytkownika" });
                if (!await UserManager.IsEmailConfirmedAsync(user)){
                    ModelState.AddModelError(string.Empty, "Użytkownik nie ma zweryfikowanego adresu e-mail");
                    return View(model);
                }
                if (await UserManager.IsLockedOutAsync(user))
                {
                    ModelState.AddModelError(string.Empty, "Użytkownik jest zablokowany");
                    return View(model);
                }
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                await SendEmailTokenPasswordResetAsync(user);

                return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Wiadomość z linkiem do zresetowania hasła została wysłana na podany adres e-mail" });
            }
            return View(model);                     
            
        }

        public async Task SendEmailTokenPasswordResetAsync(User user)
        {
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var link = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());
            var email = new EmailViewModel
            {
                To = user.Email,
                Subject = "Wiadomość weryfikująca email",
                IsHtml = true,
                Body = $"Kliknij link, żeby zresetować hasło <a href=\"{link}\">Link</a> </br> Jeżeli link nie działa skopiuj go do przegladarki {link}"
            };
            await EmailService.SendEmailAsync(email);
        }

        public IActionResult ForgotPasswordConfirmation(string confirmationText)
        {
            ViewBag.ConfirmationText = confirmationText;
            return View();
        }

        
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            //var user = await UserManager.FindByEmailAsync(email);
            //if (user == null)
            //    return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Nie odnaleziono użytkownika" });
            //var correctToken = await UserManager.GeneratePasswordResetTokenAsync(user);
            //correctToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(correctToken));

            //if (correctToken != token)
            //    return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Token jest niepoprawny" });
            var model = new NewPasswordViewModel
            {
                Email = email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(NewPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Nie odnaleziono użytkownika" });
            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction("ForgotPasswordConfirmation", "Account", new { confirmationText = "Hasło zostało pomyślnie zmienione" });
        }
    }
}
