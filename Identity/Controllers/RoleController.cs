using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Helpers;
using Identity.Models;
using Identity.Permissions;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Roles.View)]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.OrderBy(x => x.Name);
            var rolesViewModel = roles.Select(role => new RoleViewModel
            {
                Name = role.Name,
                Description = role.Description,
                Id = role.Id
            }).ToList();

            return View(rolesViewModel);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Roles.Create)]
        public IActionResult Add()
        {
            return View(new AddRoleViewModel());
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Roles.Create)]
        public async Task<IActionResult> Add(AddRoleViewModel addRoleViewModel)
        {
            if (!ModelState.IsValid)
                return View(addRoleViewModel);

            if (await _roleManager.FindByNameAsync(addRoleViewModel.Name) != null)
            {
                ModelState.AddModelError(string.Empty, "The role already exists. Please try a different one!");
                return View(addRoleViewModel);
            }

            var appRole = new AppRole(addRoleViewModel.Name) { Description = addRoleViewModel.Description };
            var rs = await _roleManager.CreateAsync(appRole);
            if (rs.Succeeded)
                return RedirectToAction("Index", "Role", new { id = appRole.Id, succeeded = rs.Succeeded, message = rs.ToString() });

            ModelState.AddModelError(string.Empty, rs.ToString());
            return View(addRoleViewModel);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Roles.Edit)]
        public async Task<IActionResult> Edit(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            var editRoleViewModel = new Role
            {
                RoleId = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(editRoleViewModel);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Roles.Edit)]
        public async Task<IActionResult> Edit(Role editRoleViewModel)
        {
            if (!ModelState.IsValid)
                return View(editRoleViewModel);

            var role = await _roleManager.FindByIdAsync(editRoleViewModel.RoleId);
            if (role == null)
                return NotFound();

            role.Name = editRoleViewModel.Name;
            role.Description = editRoleViewModel.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, result.ToString());
            return View(editRoleViewModel);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Permissions.Roles.ManageClaims)]
        public async Task<IActionResult> ManageRolePermissions(string roleId, string permissionValue, int? pageNumber, int? pageSize)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return RedirectToAction("Index");

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var allPermissions = PermissionHelper.GetAllPermissions();

            if (!string.IsNullOrWhiteSpace(permissionValue))
            {
                allPermissions = allPermissions.Where(x => x.Value.ToLower().Contains(permissionValue.Trim().ToLower())).ToList();
            }

            var managePermissionsClaim = new List<ManageClaimViewModel>();
            foreach (var permission in allPermissions)
            {
                var managePermissionClaim = new ManageClaimViewModel { Type = permission.Type, Value = permission.Value };
                if (roleClaims.Any(x => x.Value == permission.Value))
                {
                    managePermissionClaim.Checked = true;
                }
                managePermissionsClaim.Add(managePermissionClaim);
            }

            var paginatedList = PaginatedList<ManageClaimViewModel>.CreateFromLinqQueryable(
                managePermissionsClaim.AsQueryable(),
                pageNumber ?? 1,
                pageSize ?? 12);

            var manageRolePermissionsViewModel = new ManageRolePermissionsViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                PermissionValue = permissionValue,
                ManagePermissionsViewModel = paginatedList
            };

            if (allPermissions.Count > 0)
                return View(manageRolePermissionsViewModel);

            return RedirectToAction("Index", "Role", new { succeeded = false, message = "No Permissions exist" });
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Roles.ManageClaims)]
        public async Task<IActionResult> ManageRoleClaims(ManageRoleClaimViewModel manageRoleClaimViewModel)
        {
            if (!ModelState.IsValid)
                return Json(new { Succeeded = false, Message = "Failed" });

            var roleById = await _roleManager.FindByIdAsync(manageRoleClaimViewModel.RoleId);
            var roleByName = await _roleManager.FindByNameAsync(manageRoleClaimViewModel.RoleName);

            if (roleById != roleByName)
                return Json(new { Succeeded = false, Message = "Failed" });

            var allClaims = await _roleManager.GetClaimsAsync(roleById);
            var claimExists = allClaims.Where(x => x.Type == manageRoleClaimViewModel.Type && x.Value == manageRoleClaimViewModel.Value).ToList();

            switch (manageRoleClaimViewModel.Checked)
            {
                case true when claimExists.Count == 0:
                    await _roleManager.AddClaimAsync(roleById, new Claim(manageRoleClaimViewModel.Type, manageRoleClaimViewModel.Value));
                    break;
                case false when claimExists.Count > 0:
                    foreach (var claim in claimExists)
                    {
                        await _roleManager.RemoveClaimAsync(roleById, claim);
                    }
                    break;
            }

            return Json(new { Succeeded = true, Message = "Success" });
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Permissions.Roles.Delete)]
        public async Task<IActionResult> DeleteRoleConfirmed(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return RedirectToAction("Index");

            return RedirectToAction("Index", new { succeeded = false, message = result.ToString() });
        }
    }
}
