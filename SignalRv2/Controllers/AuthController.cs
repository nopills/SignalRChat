using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Models.ViewModels;
using SignalRv2.Services.EmailSender;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRv2.Controllers
{
 
    public class AuthController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }
        public IChatRepo _chatRepo { get; }
        public IEmailSender _emailSender { get; }

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IChatRepo chatRepo, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _chatRepo = chatRepo;
            _emailSender = emailSender;
        }

    
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                //if(user != null && !(await _userManager.IsEmailConfirmedAsync(user)))
                //{                    
                //        ModelState.AddModelError(string.Empty, "Please confirm your email adress");
                //        return View(model);                   
                //}

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Password is not correct");
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                      new Claim(Constants.Identifier, user.Id),
                      new Claim("FullName", string.Format("{0} {1}", model.FirstName, model.LastName))
                    };

                    await _userManager.AddClaimsAsync(user, claims);

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callback = Url.Action("ConfirmEmail", "Auth", new { token, email = model.Email }, Request.Scheme);
                    var message = new EmailMessage(model.Email, "Confirmation email", callback);
                    await _emailSender.SendEmailAsync(message);

                    // await _signInManager.SignInAsync(user, true);

                    return RedirectToAction("EmailConfirmation", "Auth");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null ||   !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action("ResetPassword", "Auth", new { token, email = model.Email }, Request.Scheme);
                var message = new EmailMessage(model.Email, "Reset password", callback);
                await _emailSender.SendEmailAsync(message);
                return RedirectToAction("ForgotPasswordConfirmation");

            }
            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return RedirectToAction("ResetPasswordConfirmation");

                var resetPass = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                if(resetPass.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                else
                {
                    foreach(var err in resetPass.Errors)
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if(token == null || email == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return View("Error");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                return View();
            }
            return View("Error");
        }

        public IActionResult EmailConfirmation()
        {
            return View();
        }
    }
}
