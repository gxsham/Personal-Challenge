using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PersonalChallenge.Models;
using PersonalChallenge.Data;
using PersonalChallenge.Models.UserViewModels;

namespace PersonalChallenge.Controllers
{
	[Authorize]
    public class HomeController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;
		public HomeController(UserManager<ApplicationUser> userManager,
			ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		[AllowAnonymous]
		public IActionResult Index()
        {
			var count = _context.Teams.Sum(x=>x.ChallengesDone);
            return View(new CountViewModel { Count = count});
        }

		public async Task<IActionResult> MyProfile()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			var result = new ProfileViewModel
			{
				Name = user.Name,
				Surname = user.Surname,
				Birthday = user.Birthday,
				ChallengesDone = user.ChallengesDone,
				Email = user.Email,
				TeamId = user.TeamId,
				ProfilePic = user.ProfilePic,
				
			};
			if (user.TeamId != null)
				result.TeamName = _context.Teams.First(x => x.Id == user.TeamId).Name;
			return View(result);
		}
		[AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
