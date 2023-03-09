using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Kanban.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;
        public TaskController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            return _context.Tasks != null ? View(await _context.Tasks.Include(c => c.Creator).Include(e => e.Executor).ToListAsync()) : Problem("Entity set 'ApplicationDbContext.Tasks'  is null.");
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.Include(c => c.Creator).Include(e => e.Executor).Include(c => c.Comments).FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreatorId,ExecutorId,CreatedDate,BeginDate,ExpectedEndDate,Description")] Models.Task task)
        {
            if (task.ExpectedEndDate <= DateTime.Now)
            {
                ModelState.AddModelError("ExpectedEndDate", "End date must not be before today");
            }

            if (ModelState.IsValid)
            {
                task.CreatorId = _userManager.GetUserId(User);
                task.Status = Models.Task.Statuses.Open;
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        //// GET: Task/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Tasks == null)
        //    {
        //        return NotFound();
        //    }

        //    var task = await _context.Tasks.FindAsync(id);
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(task);
        //}

        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Start(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = _context.Tasks.Where(t => t.Id == id).Include(c => c.Creator).FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                task.CreatorId = task.Creator.Id;
                task.ExecutorId = _userManager.GetUserId(User);
                task.BeginDate= DateTime.Now;
                task.Status = Models.Task.Statuses.InProgress;
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finish(int id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = _context.Tasks.Where(t => t.Id == id).Include(c => c.Creator).Include(e => e.Executor).FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                task.CreatorId = task.Creator.Id;
                task.Status = Models.Task.Statuses.Finished;
                task.ActualEndDate= DateTime.Now;
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = _context.Tasks.Where(t => t.Id == id).Include(c => c.Creator).Include(e => e.Executor).FirstOrDefault();

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                task.CreatorId = task.Creator.Id;
                task.Status = Models.Task.Statuses.InProgress;
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);

            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
