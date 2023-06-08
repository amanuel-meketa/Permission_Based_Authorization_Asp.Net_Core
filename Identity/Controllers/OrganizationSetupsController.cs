using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Identity.Helpers;

namespace Identity.Controllers
{
    public class OrganizationSetupsController : Controller
    {
        private readonly IdentityContext _context;

        public OrganizationSetupsController(IdentityContext context)
        {
            _context = context;
        }

        // GET: OrganizationSetups
        public async Task<IActionResult> Index()
        {
              return View(await _context.OrganizationSetups.ToListAsync());
        }

        // GET: OrganizationSetups/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.OrganizationSetups == null)
            {
                return NotFound();
            }

            var organizationSetup = await _context.OrganizationSetups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizationSetup == null)
            {
                return NotFound();
            }

            return View(organizationSetup);
        }

        // GET: OrganizationSetups/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationSetup organizationSetup, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                organizationSetup.Id = Guid.NewGuid();
                var logo = FileService.UploadedFile(file);
                organizationSetup.logo = logo;
                _context.Add(organizationSetup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizationSetup);
        }

        // GET: OrganizationSetups/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.OrganizationSetups == null)
            {
                return NotFound();
            }

            var organizationSetup = await _context.OrganizationSetups.FindAsync(id);
            if (organizationSetup == null)
            {
                return NotFound();
            }
            return View(organizationSetup);
        }

        // POST: OrganizationSetups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Motto,logo,Abbreviation,Phone,Email,Address,MapLink,CreatedDate")] OrganizationSetup organizationSetup)
        {
            if (id != organizationSetup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizationSetup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationSetupExists(organizationSetup.Id))
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
            return View(organizationSetup);
        }
       
        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Delete)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.OrganizationSetups == null)
            {
                return Problem("Entity set 'IdentityContext.OrganizationSetups'  is null.");
            }
            var organizationSetup = await _context.OrganizationSetups.FindAsync(id);
            if (organizationSetup != null)
            {
                _context.OrganizationSetups.Remove(organizationSetup);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationSetupExists(Guid id)
        {
          return _context.OrganizationSetups.Any(e => e.Id == id);
        }
    }
}
