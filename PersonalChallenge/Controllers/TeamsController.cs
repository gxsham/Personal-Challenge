using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalChallenge.Models.TeamViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using PersonalChallenge.Data;
using PersonalChallenge.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace PersonalChallenge.Controllers
{
	[Authorize]
    public class TeamsController : Controller
    {
		private readonly IHostingEnvironment _evironment;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		public TeamsController(IHostingEnvironment environment,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			_evironment = environment;
			_context = context;
			_userManager = userManager;
		}
        // GET: Teams
        public ActionResult Index()
        {
			var list = _context.Teams.Select(x => new TeamListViewModel { Id = x.Id,
				Name = x.Name,
				TeamPic = x.TeamPic,
				ChallengesDone = x.ChallengesDone,
				ActiveChallenges = x.Challenges.Count(y => y.IsActive),
				Members = x.Members.Count()
			});
            return View(list);
        }

		public async  Task<ActionResult> MyTeam()
		{
			var user = await _userManager.FindByEmailAsync(User.Identity.Name);
			if (user.TeamId == null)
				return RedirectToAction("Index");
			else
				return RedirectToAction("Details", new {id = user.TeamId });
		}
        // GET: Teams/Details/5
        public ActionResult Details(Guid id)
        {
			var team = _context.Teams.Find(id);
			var result = new TeamDetailsViewModel
			{
				Name = team.Name,
				Description = team.Description,
				ChallengesDone = team.ChallengesDone,
				TeamPic = team.TeamPic,
			};
			result.Challenges = _context.Challenges.Select(x => new ChallengeDetailsViewModel
			{
				Id = x.Id,
				Description = x.Description,
				IsActive = x.IsActive,
				Name = x.Name
			});
			result.Members = _context.Users.Where(x => x.TeamId == team.Id).Select(x => new MemberDetailsViewModel
			{
				Id = x.Id,
				Name = x.Name,
				Surname = x.Surname,
				ChallengesDone = x.ChallengesDone
			});
            return View(result);
        }

        // GET: Teams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateTeamViewModel model)
        {
            try
            {
				var team = new Team {Id = Guid.NewGuid(), Name = model.Name, Description = model.Description };
				var path = Path.Combine(_evironment.WebRootPath, "TeamPics", team.Id + Path.GetExtension(model.TeamPic.FileName));
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await model.TeamPic.CopyToAsync(fileStream);
				}
				team.TeamPic = team.Id + Path.GetExtension(model.TeamPic.FileName);

				_context.Teams.Add(team);
				await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Enroll(Guid id)
		{
			var user = await _userManager.FindByEmailAsync(User.Identity.Name);
			if(user.TeamId == null)
			{
				user.TeamId = id;
				await _userManager.UpdateAsync(user);
				return RedirectToAction("Details","Teams", new { id = id });
			}
			else
			{
				TempData["Error"] = "Please unroll first";
				return RedirectToAction("Index");
			}
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<ActionResult> Unroll()
		{
			var user = await _userManager.FindByEmailAsync(User.Identity.Name);
			user.TeamId = null;
			await _userManager.UpdateAsync(user);
			return RedirectToAction("Index");
		}
        // GET: Teams/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Teams/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Teams/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}