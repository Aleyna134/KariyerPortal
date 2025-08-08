using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KariyerPortal.Context;       // DbContext için
using KariyerPortal.Models;        // Job, JobApplication ve AppUser için
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KariyerPortal.Controllers
{
    [Authorize(Roles = "Admin")]  // Sadece adminlere açık
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin anasayfa - ilanların listesi ve başvuru sayıları
        public async Task<IActionResult> JobList()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Applications)
                    .ThenInclude(a => a.User)
                .ToListAsync();

            return View(jobs);
        }

        // Belirli ilana ait başvuranların listesi
        public async Task<IActionResult> JobApplications(Guid jobId)
        {
            var applications = await _context.JobApplications
                .Where(a => a.JobId == jobId)
                .Include(a => a.User)
                .ToListAsync();

            var job = await _context.Jobs.FindAsync(jobId);
            ViewBag.JobTitle = job?.Title ?? "İlan";

            return View(applications);
        }
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteJob(Guid id)
{
    var job = await _context.Jobs
        .Include(j => j.Applications)
        .FirstOrDefaultAsync(j => j.Id == id);

    if (job == null)
        return NotFound();

    // Varsa başvuruları da sil
    if (job.Applications != null && job.Applications.Any())
    {
        _context.JobApplications.RemoveRange(job.Applications);
    }

    _context.Jobs.Remove(job);
    await _context.SaveChangesAsync();

    TempData["Message"] = "İlan başarıyla silindi.";
    return RedirectToAction(nameof(JobList));
}
    }
}