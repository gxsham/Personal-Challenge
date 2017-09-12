using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalChallenge.Data;
using PersonalChallenge.Models;
using Microsoft.AspNetCore.Authorization;

namespace PersonalChallenge.Controllers
{
	[Authorize]
    public class ChallengeTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChallengeTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChallengeTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChallengeTypes.ToListAsync());
        }

        // GET: ChallengeTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeType = await _context.ChallengeTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeType == null)
            {
                return NotFound();
            }

            return View(challengeType);
        }

        // GET: ChallengeTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChallengeTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,MeasuredIn")] ChallengeType challengeType)
        {
            if (ModelState.IsValid)
            {
                challengeType.Id = Guid.NewGuid();
				challengeType.Approved = false;
                _context.Add(challengeType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(challengeType);
        }

        // GET: ChallengeTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeType = await _context.ChallengeTypes.SingleOrDefaultAsync(m => m.Id == id);
            if (challengeType == null)
            {
                return NotFound();
            }
            return View(challengeType);
        }

        // POST: ChallengeTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,MeasuredIn,Approved")] ChallengeType challengeType)
        {
            if (id != challengeType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(challengeType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChallengeTypeExists(challengeType.Id))
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
            return View(challengeType);
        }

        // GET: ChallengeTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var challengeType = await _context.ChallengeTypes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (challengeType == null)
            {
                return NotFound();
            }

            return View(challengeType);
        }

        // POST: ChallengeTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var challengeType = await _context.ChallengeTypes.SingleOrDefaultAsync(m => m.Id == id);
            _context.ChallengeTypes.Remove(challengeType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChallengeTypeExists(Guid id)
        {
            return _context.ChallengeTypes.Any(e => e.Id == id);
        }
    }
}
