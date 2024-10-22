using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Identity.Data;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Identity.Helpers;
using Microsoft.AspNetCore.Hosting;

namespace Identity.Controllers
{
    public class OrganizationSetupsController : Controller
    {
        private readonly IdentityContext _context;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizationSetupsController(IdentityContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.View)]
        public async Task<IActionResult> Index()
        {
              return View(await _context.OrganizationSetups.ToListAsync());
        }

        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Details)]
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

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Create)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Create)]
        public async Task<IActionResult> Create(OrganizationSetup organizationSetup, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                organizationSetup.Id = Guid.NewGuid();
                var logo = FileService.UploadedFile(file, _webHostEnvironment);
                organizationSetup.logo = logo;
                _context.Add(organizationSetup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizationSetup);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Edit)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.OrganizationSetups == null)
                return NotFound();

            var organizationSetup = await _context.OrganizationSetups.FindAsync(id);
            if (organizationSetup == null)
                return NotFound();

            return View(organizationSetup);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Edit)]
        public async Task<IActionResult> Edit(OrganizationSetup organizationSetup, IFormFile logo)
        {
            var org = await _context.OrganizationSetups.FindAsync(organizationSetup.Id);
            if (org == null)
                return NotFound();

            if (logo != null && org.logo != logo.FileName)
            {
                var newLogo = FileService.UploadedFile(logo, _webHostEnvironment);
                FileService.DeleteFile(org.logo, _webHostEnvironment);
                org.logo = newLogo;
            }

            org.Name = organizationSetup.Name;
            org.Motto = organizationSetup.Motto;
            org.Abbreviation = organizationSetup.Abbreviation;
            org.Phone = organizationSetup.Phone;
            org.Email = organizationSetup.Email;
            org.Address = organizationSetup.Address;
            org.MapLink = organizationSetup.MapLink;

            try
            {
                _context.Update(org);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception();
            }
        }

        [Authorize(Policy = Permissions.Permissions.OrganizationSetup.Delete)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var organizationSetup = await _context.OrganizationSetups.FindAsync(id);
            if (organizationSetup == null)
                return NotFound("Entity set OrganizationSetups is null.");

            _context.OrganizationSetups.Remove(organizationSetup);
            await _context.SaveChangesAsync();
            FileService.DeleteFile(organizationSetup.logo, _webHostEnvironment);

            return RedirectToAction(nameof(Index));
        }
    }
}
