﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Charity.Mvc.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        protected UserManager<User> UserManager { get; }
        protected SignInManager<User> SignInManager { get; }
        protected RoleManager<IdentityRole<int>> RoleManager { get; }
        private readonly IAdminService _adminService;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager, IAdminService adminService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList(string selectedRole = "0")
        {
            var roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem { Text = "Wszystkie role", Value = "0" });
            foreach (var role in RoleManager.Roles)
            {
                roleList.Add(new SelectListItem { Text = role.Name, Value = role.Id.ToString() });
            }
            ViewBag.RoleList = roleList;
            var roleName = await RoleManager.FindByIdAsync(selectedRole);
            var users = selectedRole == "0" ? await UserManager.Users.ToListAsync() : await UserManager.GetUsersInRoleAsync(roleName.Name);
            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Lock(int id)
        {
            await _adminService.UserLockAsync(id);
            return RedirectToAction("UserList");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchRole(string roleName, int id)
        {
            var user = await _adminService.GetUserAsync(id);
            if (user == null)
                return RedirectToAction("List");
            if (await UserManager.IsInRoleAsync(user, roleName))
            {
                await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            else
            {
                await UserManager.AddToRoleAsync(user, roleName);

            }
            return RedirectToAction("Edit", "Account", new { id });
        }
    }
}