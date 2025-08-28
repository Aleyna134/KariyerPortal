using KariyerPortal.Context;
using KariyerPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using KariyerPortal.Models.ViewModel;
using KariyerPortal.Models.Entity;

namespace KariyerPortal.Controllers
{
    public class JobController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public JobController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Job/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateRequest job)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

                string? logoPath = null;
                string? photoPath = null;

                // wwwroot/uploads klasörünü al
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                // Şirket Logosu kaydet
                if (job.CompanyLogo != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(job.CompanyLogo.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await job.CompanyLogo.CopyToAsync(stream);
                    }
                    logoPath = "/uploads/" + fileName;
                }

                // Fotoğraf kaydet
                if (job.Photo != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(job.Photo.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await job.Photo.CopyToAsync(stream);
                    }
                    photoPath = "/uploads/" + fileName;
                }

                _context.Jobs.Add(new Job
                {
                    Id = Guid.NewGuid(),
                    Title = job.Title,
                    CompanyName = job.CompanyName,
                    Location = job.Location,
                    JobType = job.JobType,
                    Description = job.Description,
                    Created = DateTime.Now,
                    CreatedBy = currentUser!.Id,
                    Status = 1,
                    CompanyLogoPath = logoPath,
                    PhotoPath = photoPath
                });

                await _context.SaveChangesAsync();
                return RedirectToAction("JobList","Admin");
            }

            return View(job);
        }


        // GET: Job/List
        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var jobsRaw = await _context.Jobs.ToListAsync();
            var jobs = new List<JobListResponse>();

            foreach (var job in jobsRaw)
            {
                var user = await _userManager.FindByIdAsync(job.CreatedBy.ToString());

                jobs.Add(new JobListResponse
                {
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Created = job.Created,
                    Status = job.Status,
                    CreatedBy = user?.UserName ?? "Bilinmiyor",
                    CompanyLogoPath = job.CompanyLogoPath,  // ⭐ burada ekledik
                    CompanyName = job.CompanyName,   // ekledik
                    Location = job.Location,         // ekledik
                    JobType = job.JobType,            // ekledik

                });
            }

            return View(jobs);
        }

        // GET: Job/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id, string? returnUrl = null)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            var user = await _userManager.FindByIdAsync(job.CreatedBy.ToString());

            var viewModel = new JobListResponse
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Created = job.Created,
                CreatedBy = user?.UserName ?? "Bilinmiyor",
                CompanyLogoPath = job.CompanyLogoPath,
                PhotoPath = job.PhotoPath,
                CompanyName = job.CompanyName,   // ekledik
                Location = job.Location,         // ekledik
                JobType = job.JobType            // ekledik
            };
            ViewBag.ReturnUrl = returnUrl; // Geri dön linki için

            // ViewBag.Message = TempData["Message"]?.ToString();
            return View(viewModel);
        }

        // GET: Job/Apply/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Apply(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == id && a.UserId == user.Id);

            if (alreadyApplied)
            {
                TempData["Message"] = "Bu ilana zaten başvurdunuz.";
                return RedirectToAction("Details", new { id });
            }

            var viewModel = new JobApplicationViewModel
            {
                JobId = id,
                JobTitle = job.Title ?? "",
                CompanyName = job.CompanyName,
                Location = job.Location,
                JobType = job.JobType
            };

            return View(viewModel);
        }

        // POST: Job/Apply
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            var alreadyApplied = await _context.JobApplications
                .AnyAsync(a => a.JobId == model.JobId && a.UserId == user.Id);

            if (alreadyApplied)
            {
                TempData["Message"] = "Bu ilana zaten başvurdunuz.";
                return RedirectToAction("Details", new { id = model.JobId });
            }

            if (model.CV == null || model.CV.Length == 0)
            {
                ModelState.AddModelError("CV", "CV dosyası yüklenmelidir.");
                return View(model);
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{model.CV.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.CV.CopyToAsync(stream);
            }

            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                JobId = model.JobId,
                UserId = user.Id,
                CVPath = uniqueFileName,
                AppliedAt = DateTime.Now
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Başvurunuz başarıyla alındı.";
            return RedirectToAction("Details", new { id = model.JobId });
        }

        // ✅ EKLENDİ: Kullanıcının kendi başvurularını listelemesi
        [Authorize]
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);
            var applications = await _context.JobApplications
                .Include(a => a.Job)
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            return View(applications);
        }
        public async Task<IActionResult> Index()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Applications)
                    .ThenInclude(a => a.User)  // ⭐ kullanıcıyı da getir
                .ToListAsync();

            return View(jobs);
        }

    }
}