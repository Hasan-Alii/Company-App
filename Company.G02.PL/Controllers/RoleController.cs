using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        // Get, GetAll, Add, Update, Delete
        // Index, Create, Details, Edit, Delete

        public RoleController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Index of Roles
        public async Task<IActionResult> Index(string SearchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(U => U.Name
                                                .ToLower()
                                                .Contains(SearchInput.ToLower()))
                                                .Select(R => new RoleViewModel()
                                                {
                                                    Id = R.Id,
                                                    RoleName = R.Name
                                                }).ToListAsync();
            }
            return View(roles);
        }
        #endregion

        #region Create Role

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                //foreach(var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }
            return View();
        }

        #endregion

        #region Details of Role
        [HttpGet]
        public async Task<IActionResult> Details(string? Id, string ViewName = "Details")
        {
            try
            {
                if (Id is null) return BadRequest(); // 400 
                var roleFromDb = await _roleManager.FindByIdAsync(Id);
                
                if (roleFromDb is null) return NotFound(); // 404
                
                var role = new RoleViewModel()
                {
                    Id = roleFromDb.Id,
                    RoleName = roleFromDb.Name
                };
                return View(ViewName, role);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Update Role
        public async Task<IActionResult> Update(string? Id)
        {
            return await Details(Id, nameof(Update));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string? Id, RoleViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest(); // 400

                if (ModelState.IsValid) // Server Side Validation
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(Id);
                    if (roleFromDb is null) return NotFound(); // 404

                    roleFromDb.Id = model.Id;
                    roleFromDb.Name = model.RoleName;
                    
                    await _roleManager.UpdateAsync(roleFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(model);
        }
        #endregion

        #region Delete Role
        [HttpGet]
        public async Task<IActionResult> Delete(string? Id)
        {
            return await Details(Id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? Id, RoleViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest(); // 400

                if (ModelState.IsValid) // Server Side Validation
                {
                    var roleFromDb = await _roleManager.FindByIdAsync(Id);
                    if (roleFromDb is null) return NotFound(); // 404

                    await _roleManager.DeleteAsync(roleFromDb);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(model);
        }
        #endregion

        #region Add Or Remove User From Role
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            ViewData["RoleId"] = roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            if (users is null) return NotFound();

            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction(nameof(Update), new {Id = roleId});
            }

            return View(users);
        }
        #endregion
    }
}
