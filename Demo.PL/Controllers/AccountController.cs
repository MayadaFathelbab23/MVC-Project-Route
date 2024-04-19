using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;

        public AccountController(UserManager<ApplicationUser> userManager , 
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSettings emailSettings)
        {
			_userManager = userManager;
			_signInManager = signInManager;
            _emailSettings = emailSettings;
        }
        #region Sign Up - Step 0 [Identification - Registeration]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid) // Server-side validation
            {
                // Username is unique
                var User = await _userManager.FindByNameAsync(model.Username);
                if(User == null)
                {
					// Mapp to IdentityUser
					User = new ApplicationUser()
					{
						// UserName = model.Email.Split('@')[0],
						UserName = model.Username,
						Email = model.Email,
						IsAgree = model.IsAgree,
						FName = model.FName,
						LName = model.LName,

					};
					// Create User
					var result = await _userManager.CreateAsync(User, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction("Login");
                    foreach(var error in  result.Errors)
                        ModelState.AddModelError(string.Empty , error.Description);
				}
                ModelState.AddModelError(string.Empty, "Username is already exists");
               
            }
            return View(model);
        }
		#endregion


		#region Login - Step 1 - Local login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                // 1. Validate User is exists by email
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    // 2. Validate password
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        // RememberMe => save token in cookie storage even after closing browser
                        if (result.Succeeded)
                            return RedirectToAction("Index" , "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login !!");

                
            }
            return View(model);
        }

        // Google Login
        public IActionResult GoogleLogin()
        {
            var AuthProp = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(AuthProp , GoogleDefaults.AuthenticationScheme);
        }
        // Google Response
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
                claim => new
                {
                    claim.Issuer ,
                    claim.OriginalIssuer ,
                    claim.Type ,
                    claim.Value
                });
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync(); // Delete Token 
            return RedirectToAction("Login");
        }
		#endregion

		#region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(ForgetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user); // valid one time for this user
                var resetPasswordLink = Url.Action("ResetPassword" ,"Account" , new {email  = model.Email , Token = token} , Request.Scheme);
                // https://localhost:44303/ResetPassword/Account?email=user@gmail.com&Token=fhhdhjdjjcnjd
                if(user is not null)
                {
                    // Send Email
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = resetPasswordLink
                    };
                    //EmailSettings.SendEmail(email);
                    _emailSettings.SenEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
                ModelState.AddModelError(string.Empty, "Email not Exists");
            }
            return View("ForgetPassword" , model);
        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
		#endregion


		#region Reset Password
        public IActionResult ResetPassword(string email , string Token)
        {
            // Tranfer data from this action to next action
            TempData["email"] = email;
            TempData["token"] = Token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Need User to reset its password
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Login));
                foreach(var error in result.Errors)
                    ModelState.AddModelError(string.Empty , error.Description);
            }
            return View(model);
        }
		#endregion

	}

}
