using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SignalRv2.Hubs;
using SignalRv2.Models;
using SignalRv2.Models.ViewModels;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Controllers
{
    [Authorize]
    [Route("[action]")]
    public class HomeController : Controller
    {
        public UserManager<User> _userManager { get; }
        public SignInManager<User> _signInManager { get; }
        public IChatService _chatService;
        public IChatRepo _chatRepo;
        public IHubContext<ChatHub> _hubContext;
     
        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, IChatService chatService, IChatRepo chatRepo, IHubContext<ChatHub> hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _chatService = chatService;
            _chatRepo = chatRepo;
            _hubContext = hubContext;
        }

      

        public IActionResult im()
        {
            return View();
        }

        public IActionResult ChangeUserInfo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserInfo(ChangeUserInfoViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = _chatRepo.GetUserByName(User.Identity.Name);
                if(user != null)
                {
                    if (!_chatService.IsValidUserInfo(model.FirstName, model.LastName))
                    {
                        ModelState.AddModelError(string.Empty, $"Invalid FirstName or LastName");
                    }
                    await _chatRepo.ChangeUserInfo(user, model.FirstName, model.LastName, model.AvatarUrl);
                    return RedirectToAction("IM");
                } 
                else
                {
                    ModelState.AddModelError(string.Empty, $"Cannot find user with username {User.Identity.Name}");
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        return View("ChangePasswordConfirmation");
                    }
                    else
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, err.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Cannot find user with username {User.Identity.Name}");
                }
            }
            return View(model);
        }

        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
