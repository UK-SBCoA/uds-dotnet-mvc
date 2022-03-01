using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Entities;

namespace UDS.Net.Web.Controllers
{
    public class ExaminerController : Controller
    {
        private readonly UdsContext _context;

        public ExaminerController(UdsContext context)
        {
            _context = context;
        }

        // GET: Examiner
        public async Task<IActionResult> Index()
        {
            return View(await _context.Examiners.ToListAsync());
        }

        // GET: Examiner/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners
                .FirstOrDefaultAsync(m => m.Initials == id);
            if (examiner == null)
            {
                return NotFound();
            }

            return View(examiner);
        }

        // GET: Examiner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Examiner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Initials,Name,Username")] Examiner examiner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examiner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(examiner);
        }

        // GET: Examiner/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners.FindAsync(id);
            if (examiner == null)
            {
                return NotFound();
            }
            return View(examiner);
        }

        // POST: Examiner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Initials,Name,Username")] Examiner examiner)
        {
            if (id != examiner.Initials)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examiner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExaminerExists(examiner.Initials))
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
            return View(examiner);
        }

        // GET: Examiner/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var examiner = await _context.Examiners
                .FirstOrDefaultAsync(m => m.Initials == id);
            if (examiner == null)
            {
                return NotFound();
            }

            return View(examiner);
        }

        // POST: Examiner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var examiner = await _context.Examiners.FindAsync(id);
            _context.Examiners.Remove(examiner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExaminerExists(string id)
        {
            return _context.Examiners.Any(e => e.Initials == id);
        }
    }
}
