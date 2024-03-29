﻿using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kanban.Areas.Admin.Controllers
{
    public class AdministrationController : BaseController
    {
        private readonly UserManager<User> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly ApplicationDbContext context;

        public AdministrationController(UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AllUsers(string searchedName)
        {
            IQueryable<User> users = from u in context.Users select u;

            if (!string.IsNullOrEmpty(searchedName))
            {
                users = users.Where(user =>
                   user.FirstName.Contains(searchedName)
                || user.LastName.Contains(searchedName)
                || user.Email.Contains(searchedName));
            }

            return View(users
                .Select(u => new UserListViewModel()
                {
                    Id = u.Id,
                    Name = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    Role = userManager.GetRolesAsync(u).Result
                })
                .ToList());
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || context.Users == null)
            {
                return NotFound();
            }

            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            };

            var userToEdit = await userManager.FindByIdAsync(id);

            if (userToEdit == null)
            {
                return NotFound();
            }

            userToEdit.FirstName = user.FirstName;
            userToEdit.LastName = user.LastName;
            userToEdit.Email = user.Email;

            if (ModelState.IsValid)
            {
                try
                {
                    await userManager.UpdateAsync(userToEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return RedirectToAction("NotFound", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllUsers));
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult StartAdmin(int? id)
        {
            if (id == null || context.Tasks == null)
            {
                return NotFound();
            }

            ViewBag.Users = new SelectList(userManager.GetUsersInRoleAsync("Developer").Result, "Id", "UserName");
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> StartAdmin(int? id, Models.Task task)
        {
            if (id == null || context.Tasks == null)
            {
                return NotFound();
            }

            var taskToStart = context.Tasks.Where(t => t.Id == id).Include(c => c.Creator).FirstOrDefault();

            if (taskToStart == null)
            {
                return NotFound();
            }

            if (taskToStart != null)
            {
                try
                {
                    taskToStart.ExecutorId = task.ExecutorId;
                    taskToStart.BeginDate = DateTime.Now;
                    taskToStart.Status = Models.Task.Statuses.InProgress;
                    context.Update(taskToStart);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Task", new { area = "" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }

            ViewBag.Users = new SelectList(userManager.GetUsersInRoleAsync("Developer").Result, "Id", "UserName");
            return View();
        }


        public async Task<IActionResult> MakeDeveloper(string id)
        {
            if (id == null || context.Users == null)
            {
                return NotFound();
            }

            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var currentRole = userManager.GetRolesAsync(user).Result.First();
                await userManager.RemoveFromRoleAsync(user, currentRole);
                await userManager.AddToRoleAsync(user, "Developer");
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("AllUsers");
        }

        public async Task<IActionResult> MakeClient(string id)
        {
            if (id == null || context.Users == null)
            {
                return NotFound();
            }

            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var currentRole = userManager.GetRolesAsync(user).Result.First();
                await userManager.RemoveFromRoleAsync(user, currentRole);
                await userManager.AddToRoleAsync(user, "Client");
            }
            catch (Exception ex)
            {
                throw;
            }

            return RedirectToAction("AllUsers");
        }

        private bool UserExists(string id)
        {
            return (context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
