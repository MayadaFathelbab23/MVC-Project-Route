using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserManager<ApplicationUser> userManager , IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue = "")
		{
			var users = Enumerable.Empty<UserViewModel>();
			if (string.IsNullOrEmpty(SearchValue))
			{
				// manual mapping
				 users = await _userManager.Users.Select(
					U => new UserViewModel()
					{
						Id = U.Id,
						FName = U.FName,
						LName = U.LName,
						Email = U.Email,
						PhoneNumber = U.PhoneNumber,
						Roles =  _userManager.GetRolesAsync(U).Result
					}).ToListAsync();
				return View(users);
			}
			 var user = await _userManager.FindByEmailAsync(SearchValue);
			var mappedUser = new UserViewModel
            {
				Id = user.Id,
				FName = user.FName,
				LName = user.LName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Roles = _userManager.GetRolesAsync(user).Result
			};
			return View(new List<UserViewModel> { mappedUser});
        }


        public async Task<IActionResult> Details(string Id , string ViewName = "Details")
		{
			if (Id is null)
				return BadRequest();
			var user =  await _userManager.FindByIdAsync(Id);
			if(user is null)
				return NotFound();
			var mappedUser = _mapper.Map<ApplicationUser , UserViewModel>(user);
			return View( ViewName , mappedUser);
		}
		public async Task<IActionResult> Edit(string Id)
		{
			return await Details(Id , "Edit");
		}
		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel model , [FromRoute] string Id)
		{
			if(Id != model.Id)
				return BadRequest();
			if(ModelState.IsValid)
			{
				//var user = _mapper.Map<UserViewModel, ApplicationUser>(model);
				var user = await _userManager.FindByIdAsync (Id);
				user.FName = model.FName;
				user.LName = model.LName;
				user.PhoneNumber = model.PhoneNumber;
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
					return RedirectToAction("Index");
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);
			}
			return View(model);
		}
		public async Task<IActionResult> Delete(string Id)
		{
			return await Details(Id, "Delete");
		}
		[HttpPost]
		public async Task<IActionResult> Delete(UserViewModel model , [FromRoute] string Id)
		{
			if(Id != model.Id)
				return BadRequest();
			// get user by id from BD
			try
			{
                var user = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
			{
				ModelState.AddModelError(string.Empty , ex.Message);
				return RedirectToAction("Error", "Home");
			}
		}
	}
}
