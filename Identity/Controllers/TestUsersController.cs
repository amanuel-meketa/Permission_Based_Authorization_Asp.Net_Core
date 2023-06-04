using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Controllers
{
    [Authorize]
    public class TestUsersController : Controller
    {
        private readonly IdentityContext _context;

        public TestUsersController(IdentityContext context)
        {
            _context = context;
        }

        [Authorize(Policy = Permissions.Permissions.TestUser.View)]
        public async Task<IActionResult> Index()
        {
              return View(await _context.testUsers.ToListAsync());
        }

        [Authorize(Policy = Permissions.Permissions.TestUser.Details)]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.testUsers == null)
            {
                return NotFound();
            }

            var testUser = await _context.testUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testUser == null)
            {
                return NotFound();
            }

            return View(testUser);
        }

        [Authorize(Policy = Permissions.Permissions.TestUser.Create)]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Permissions.TestUser.Create)]
        public async Task<IActionResult> Create([Bind("Id,FullName")] TestUser testUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testUser);
        }

        [Authorize(Policy = Permissions.Permissions.TestUser.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.testUsers == null)
            {
                return NotFound();
            }

            var testUser = await _context.testUsers.FindAsync(id);
            if (testUser == null)
            {
                return NotFound();
            }
            return View(testUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Permissions.TestUser.Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName")] TestUser testUser)
        {
            if (id != testUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestUserExists(testUser.Id))
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
            return View(testUser);
        }

        [Authorize(Policy = Permissions.Permissions.TestUser.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.testUsers == null)
            {
                return NotFound();
            }

            var testUser = await _context.testUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testUser == null)
            {
                return NotFound();
            }

            return View(testUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Permissions.TestUser.Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.testUsers == null)
            {
                return Problem("Entity set 'IdentityContext.testUsers'  is null.");
            }
            var testUser = await _context.testUsers.FindAsync(id);
            if (testUser != null)
            {
                _context.testUsers.Remove(testUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestUserExists(int id)
        {
          return _context.testUsers.Any(e => e.Id == id);
        }
    }
}
