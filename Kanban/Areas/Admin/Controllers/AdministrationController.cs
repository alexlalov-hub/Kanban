using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        //[HttpGet]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = "Admin"
        //    });

        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = "Developer"
        //    });

        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = "Client"
        //    });

        //    return Ok();
        //}


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
    }
}
