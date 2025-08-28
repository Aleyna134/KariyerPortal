using KariyerPortal.Context;       // DbContext için
using KariyerPortal.Models;        // Job, JobApplication ve AppUser için
using KariyerPortal.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApplicationDetail(Guid applicationId)
        {
            var application = await _context.JobApplications
                .Include(a => a.User)
                    .ThenInclude(u => u.UserJobExperiences)
                .Include(a => a.User)
                    .ThenInclude(u => u.Educations)
                .Include(a => a.User)
                    .ThenInclude(u => u.Languages)
                .Include(a => a.User)
                    .ThenInclude(u => u.Skills)
                .Include(a => a.User)
                    .ThenInclude(u => u.Certificates)
                .Include(a => a.User)
                    .ThenInclude(u => u.Projects)
                .Include(a => a.User)
                    .ThenInclude(u => u.UserDetail)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return NotFound();

            var viewModel = new ApplicationDetailViewModel
            {
                UserName = application.User.UserName,
                AdSoyad = application.User.AdSoyad,
                City = application.User.City,
                District = application.User.District,
                Phone = application.User.Phone,
                BirthYear = application.User.BirthYear,
                CVPath = application.CVPath,
                JobExperiences = application.User.UserJobExperiences,
                Educations = application.User.Educations,
                Languages = application.User.Languages,
                Skills = application.User.Skills,
                Certificates = application.User.Certificates,
                Projects = application.User.Projects,
                UserDetail = application.User.UserDetail
            };

            return View(viewModel);
        }


       


    }
}