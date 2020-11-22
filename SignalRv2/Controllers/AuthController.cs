﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRv2.Controllers
{
    [Route("[action]")]
    public class AuthController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
           
            if (ModelState.IsValid)
            {         
                User user = new User { Email = model.Email, UserName = model.UserName, FirstName = model.FirstName, LastName = model.LastName };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>() {
                        new Claim(Constants.Identifier, user.Id),
                        new Claim("FullName", string.Format("{0} {1}", model.FirstName, model.LastName))                                
                };
               
                    await _userManager.AddClaimsAsync(user, claims);

                    await _signInManager.SignInAsync(user, true);
 
                    return RedirectToAction("Index", "Home");
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

    }
}
