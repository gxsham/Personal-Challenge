using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PersonalChallenge.Data;
using PersonalChallenge.Models;
using PersonalChallenge.Models.ChallengeViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalChallenge.Controllers
{
	[Authorize]
	public class ChallengesController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IHostingEnvironment _environment;
		public ChallengesController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager
			,IHostingEnvironment environment)
		{
			_context = context;
			_userManager = userManager;
			_environment = environment;
		}
        // GET: Challenges
        public async Task<ActionResult> Index()
        {
			var user = await _userManager.FindByEmailAsync(User.Identity.Name);
			if(user.TeamId !=null)
			{
				var result = _context.Challenges.Where(x=>x.TeamId == user.TeamId).Select(x=> new ChallegeListViewModel
				{
					Id = x.Id,
					Description = x.Description,
					IsActive = x.IsActive,
					Name = x.Name,
					ChallengeTypeId = x.ChallengeTypeId
					
				}).ToList();
				var membersCount = _context.Users.Count(x => x.TeamId == user.TeamId);
				foreach (var item in result)
				{
					item.Proofs = _context.Proofs.Count(x=>x.ChallengeId == item.Id);
					item.Members = membersCount;
					item.ChallengeType = _context.ChallengeTypes.Find(item.ChallengeTypeId).Name;
				}
				return View(result);
			}
			else
				return View();
        }

        // GET: Challenges/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
			try
			{
				var challenge = _context.Challenges.Find(id);
				var result = new ChallengeDetailsViewModel
				{
					Id = challenge.Id,
					Description = challenge.Description,
					IsActive = challenge.IsActive,
					Name = challenge.Name,
					TeamId = challenge.TeamId
				};
				var user = await _userManager.FindByIdAsync(challenge.UserId);
				result.OwnerName = $"{user.Name} {user.Surname}";
				result.ChallengeType = _context.ChallengeTypes.Find(challenge.ChallengeTypeId).Name;
				result.Proofs = _context.Proofs.Where(x=>x.ChallengeId == id).Select(x=> new ProofViewModel
				{
					UserId = x.UserId,
					Comment = x.Comment,
					ContentPath = x.ContentPath,
					Quantity = x.Quantity
				}).ToList();
				if (_context.Proofs.Any(x => x.UserId == user.Id && x.ChallengeId == challenge.Id) || challenge.IsActive == false)
					result.AlreadyAdded = true;
				var measuredIn = _context.ChallengeTypes.Find(challenge.ChallengeTypeId).MeasuredIn;
				foreach (var item in result.Proofs)
				{
					user = await _userManager.FindByIdAsync(item.UserId);
					item.OwnerName = $"{user.Name} {user.Surname}";
					item.MesuredIn = measuredIn;
				}
				return View(result);
			}
			catch(Exception ex)
			{
				return RedirectToAction("Index");
			}
        }

        // GET: Challenges/Create
        public ActionResult Create()
        {
			ViewBag.ChallengeTypes = new SelectList(_context.ChallengeTypes.Where(x=>x.Approved), "Id", "Name");
			return View();
        }

        // POST: Challenges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateChallengeViewModel model)
        {
            try
            {
				// TODO: Add insert logic here
				var user = await _userManager.FindByEmailAsync(User.Identity.Name);
				var challenge = new Challenge
				{
					Id = Guid.NewGuid(),
					Description = model.Description,
					Name = model.Name,
					IsActive = true,
					UserId = user.Id,
					TeamId = (Guid)user.TeamId,
					ChallengeTypeId = model.ChallengeTypeId
				};
				await _context.Challenges.AddAsync(challenge);
				var proof = new Proof
				{
					Id = Guid.NewGuid(),
					Challenge = challenge,
					Comment = model.Comment,
					User = user,
					Quantity = model.Quantity
				};
				var path = Path.Combine(_environment.WebRootPath, "Challenges", challenge.Id.ToString());
				Directory.CreateDirectory(path);
				path = Path.Combine(path, proof.Id.ToString() + Path.GetExtension(model.ContentPath.FileName));
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await model.ContentPath.CopyToAsync(fileStream);
				}
				proof.ContentPath = proof.Id.ToString() + Path.GetExtension(model.ContentPath.FileName);
				await _context.Proofs.AddAsync(proof);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View();
            }
        }
		public async Task<ActionResult> AddProof(Guid id)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(User.Identity.Name);
				var proof = _context.Proofs.FirstOrDefault(x => x.UserId == user.Id && x.ChallengeId == id);
				if(proof !=null)
				{
					TempData["Errors"] = "You already added a proof";
					return RedirectToAction("Index");
				}
				else
				{
					return View();
				}
			}
			catch(Exception ex)
			{
				TempData["Errors"] = "Error occured";
				return View("Index");
			}
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async  Task<ActionResult> AddProof(Guid id, AddProofViewModel model)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(User.Identity.Name);
				var proof = new Proof
				{
					Id = Guid.NewGuid(),
					ChallengeId = id,
					Comment = model.Comment,
					User = user,
					Quantity = model.Quantity
				};
				var path = Path.Combine(_environment.WebRootPath, "Challenges", id.ToString(), $"{proof.Id}{Path.GetExtension(model.ContentPath.FileName)}");
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await model.ContentPath.CopyToAsync(fileStream);
				}
				proof.ContentPath = $"{proof.Id}{Path.GetExtension(model.ContentPath.FileName)}";
				await _context.Proofs.AddAsync(proof);
				await _context.SaveChangesAsync();
				var countMember = _context.Users.Count(x => x.TeamId == user.TeamId);
				var proofsCount = _context.Proofs.Count(x => x.ChallengeId == id);
				if(countMember == proofsCount)
				{
					var challenge = _context.Challenges.Find(id);
					challenge.IsActive = false;
					_context.Challenges.Update(challenge);
					await _context.SaveChangesAsync();
				}
				return RedirectToAction("Details", new { id = id });
			}
			catch (Exception)
			{
				return View();
			}
		}
        // GET: Challenges/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Challenges/Edit/5
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

        // GET: Challenges/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Challenges/Delete/5
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