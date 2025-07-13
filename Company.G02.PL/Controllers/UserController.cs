using AutoMapper;
using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.G02.PL.Controllers
{
    [Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        // Get, GetAll, Add, Update, Delete
        // Index, Details, Edit, Delete

        public UserController(
			UserManager<ApplicationUser> userManager,
			IMapper mapper
			)
        {
			_userManager = userManager;
            _mapper = mapper;
        }

		#region Index of Users
		public async Task<IActionResult> Index(string SearchInput)
		{
			var users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(SearchInput))
			{
				users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync();
			}
			else
			{
				users = await _userManager.Users.Where(U => U.Email
											    .ToLower()
											    .Contains(SearchInput.ToLower()))
											    .Select(U => new UserViewModel()
											    {
												    Id = U.Id,
												    FirstName = U.FirstName,
												    LastName = U.LastName,
												    Email = U.Email,
												    Roles = _userManager.GetRolesAsync(U).Result
											    }).ToListAsync();
			}
			return View(users);
		}
        #endregion

        #region Details of User
        [HttpGet]
        public async Task<IActionResult> Details(string? Id, string ViewName = "Details")
        {
            try
            {
                if (Id is null) return BadRequest(); // 400 
                var userFromDb = await _userManager.FindByIdAsync(Id);
                if (userFromDb is null) return NotFound(); // 404
                var result = _mapper.Map<UserViewModel>(userFromDb);
                return View(ViewName, result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Update User
        public async Task<IActionResult> Update(string? Id)
        {
            return await Details(Id, nameof(Update));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] string? Id, UserViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest(); // 400
                
                if (ModelState.IsValid) // Server Side Validation
                {
                    var userFromDb = await _userManager.FindByIdAsync(Id);
                    if (userFromDb is null) return NotFound(); // 404
                    
                    userFromDb.FirstName = model.FirstName;
                    userFromDb.LastName = model.LastName;
                    userFromDb.Email = model.Email;

                    await _userManager.UpdateAsync(userFromDb);
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

        #region Delete User
        [HttpGet]
        public async Task<IActionResult> Delete(string? Id)
        {
            return await Details(Id, nameof(Delete));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? Id, UserViewModel model)
        {
            try
            {
                if (Id != model.Id) return BadRequest(); // 400

                if (ModelState.IsValid) // Server Side Validation
                {
                    var userFromDb = await _userManager.FindByIdAsync(Id);
                    if (userFromDb is null) return NotFound(); // 404

                    await _userManager.DeleteAsync(userFromDb);
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
    }
}
