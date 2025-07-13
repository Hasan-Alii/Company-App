using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    //[Authorize]
    // default => [AllowAnonymous]
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
            _signInManager = signInManager;
        }


        #region Sign Up
        // SignUp Button
        [HttpGet] // /Account/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // SignUp Page
        [HttpPost] // /Account/SignUp
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user is null)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        if (user is null)
                        {
                            user = new ApplicationUser()
                            {
                                UserName = model.UserName,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Email = model.Email,
                                IsAgree = model.IsAgree,
                            };
                            var result = await _userManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                return RedirectToAction(nameof(SignIn));
                            }
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Email Already Exists!");
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "User Name Already Exists!");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View(model);
        }
        #endregion

        #region Sign In
        // SignIn Button
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // SignIn Page
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user is not null)
                    {
                        var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                        if (flag)
                        {
                            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemeberMe, false);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Invalid SignIn!");
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "This Email Doesn't Have An Account!");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                }
            }
            return View();
        }
        #endregion

        #region Sign Out
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
		#endregion

		#region Forget Password
		[HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Create Token

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);



                    // Create Reset Password Url
                    // Url redirect to reset password page
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

                    var email = new Email
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    // Send Email

                    EmailSettings.SendEmail(email);

                    return RedirectToAction(nameof(CheckYourInbox));
                }
                ModelState.AddModelError(string.Empty, "Invalid Operation Please Try Again!!");
            }
            return View(nameof(ForgetPassword), model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        } 
        #endregion

        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                var user = await _userManager.FindByEmailAsync(email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid Operation, Please try again!");

            return View(model);
        } 
        #endregion

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
