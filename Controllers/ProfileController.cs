using KariyerPortal.Context;
using KariyerPortal.Models;
using KariyerPortal.Models.Identity;
using KariyerPortal.Models.Profile;
using KariyerPortal.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;


namespace KariyerPortal.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;


        public ProfileController(UserManager<AppUser> userManager, IWebHostEnvironment env, ApplicationDbContext context
            )
        {
            _userManager = userManager;
            _env = env;
            _context = context;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Languages) // <- ekle
                .Include(u => u.Skills) // Skills koleksiyonunu yükle
                .Include(u => u.Certificates)
                .Include(u => u.Projects)
                .Include(u => u.Educations)
                .Include(u => u.UserJobExperiences)


                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AppUser updatedUser, IFormFile? ProfileImage, IFormFile? CVFile)
        {
            var user = await _userManager.Users
                .Include(u => u.UserDetail)
                .Include(u => u.Languages)
                .Include(u => u.Skills)
                .Include(u => u.Certificates)
                .Include(u => u.Projects)
                .Include(u => u.Educations)
                .Include(u => u.UserJobExperiences)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null) return NotFound();

            // temel bilgiler
            user.Phone = updatedUser.Phone;
            user.BirthYear = updatedUser.BirthYear;
            user.City = updatedUser.City;
            user.District = updatedUser.District;

            // profil foto
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await ProfileImage.CopyToAsync(stream);

                // eski dosyayı sil (opsiyonel)
                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    var old = Path.Combine(_env.WebRootPath, "uploads", user.ProfileImagePath);
                    if (System.IO.File.Exists(old))
                        System.IO.File.Delete(old);
                }

                user.ProfileImagePath = fileName;
            }

            // cv dosyası
            if (CVFile != null && CVFile.Length > 0)
            {
                var ext = Path.GetExtension(CVFile.FileName).ToLowerInvariant();
                var allowed = new[] { ".pdf", ".doc", ".docx" };
                if (!allowed.Contains(ext))
                {
                    ModelState.AddModelError("", "CV sadece PDF/DOC/DOCX olabilir.");
                    return View(user); // dikkat: buraya uygun model döndür
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "cv");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await CVFile.CopyToAsync(stream);

                // eski cv sil
                if (!string.IsNullOrEmpty(user.CVPath))
                {
                    var old = Path.Combine(_env.WebRootPath, "uploads", user.CVPath);
                    if (System.IO.File.Exists(old))
                        System.IO.File.Delete(old);
                }

                user.CVPath = Path.Combine("cv", fileName).Replace("\\", "/");
            }

            // sadece tek sefer güncelle
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Message"] = "Bilgiler başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(user); // burada ViewModel dönüşüne dikkat et
        }




        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddOrUpdateJob(UserJobExperience job)
{
    try
    {
        var userIdString = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userIdString))
        {
            return Content("UserId boş.");
        }

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Content($"Geçersiz GUID: {userIdString}");
        }

        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            return Content($"Kullanıcı bulunamadı. UserId: {userIdString}");
        }

        if (!ModelState.IsValid)
        {
            Console.WriteLine("data:" + job.UserId);
            return Content("ModelState geçersiz.");
        }

        var user = await _context.Users
                                 .Include(u => u.UserJobExperiences)
                                 .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return Content("Kullanıcı nesnesi null.");
        }

        if (job.Id == 0)
        {
            job.UserId = userId;
            _context.UserJobExperiences.Add(job);
        }
        else
        {
            var existing = user.UserJobExperiences.FirstOrDefault(x => x.Id == job.Id);
            if (existing == null)
            {
                return Content("Güncellenecek iş bulunamadı.");
            }

            existing.Position = job.Position;
            existing.CompanyName = job.CompanyName;
            existing.StartDate = job.StartDate;
            existing.EndDate = job.EndDate;
        }

        var changes = await _context.SaveChangesAsync();

        return RedirectToAction("Profile");

    }
    catch (Exception ex)
    {
        return Content($"Hata: {ex.Message}");
    }
}
[HttpGet]
public async Task<IActionResult> Profile()
{
    var userId = _userManager.GetUserId(User);

    var user = await _context.Users
        .Include(u => u.UserJobExperiences)
        .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

    if (user == null)
        return NotFound();

    return View("Index", user);
}

[HttpPost]
public async Task<IActionResult> AddLanguage(Language model)
{
    if (ModelState.IsValid)
    {
        model.AppUserId = Guid.Parse(_userManager.GetUserId(User));
        _context.Languages.Add(model);
        await _context.SaveChangesAsync();

        var languages = _context.Languages
   .Where(x => x.AppUserId == model.AppUserId)
   .ToList();
        // Profile sayfasındaki listeyi güncelle
        return PartialView("_LanguageListPartial", languages);
    }

    return PartialView("_AddLanguagePartial", model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteLanguage(int id)
{
    // İlgili dili bul
    var lang = _context.Languages.Find(id);

    if (lang != null)
    {
        _context.Languages.Remove(lang);
        _context.SaveChanges();
    }

    // Kullanıcının kalan dillerini çek
    var userId = _userManager.GetUserId(User);
    var languages = _context.Languages
        .Where(x => x.AppUserId == Guid.Parse(userId))
        .ToList();

    // Güncel listeyi partial view ile dön
    return PartialView("_LanguageListPartial", languages);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddSkill(Skill model)
{
    if (ModelState.IsValid)
    {
        model.AppUserId = Guid.Parse(_userManager.GetUserId(User));
        _context.Skills.Add(model);
        await _context.SaveChangesAsync();

        var skills = _context.Skills
            .Where(x => x.AppUserId == model.AppUserId)
            .ToList();

        return PartialView("_SkillListPartial", skills);
    }

    return BadRequest(ModelState);
}

// POST: Profile/DeleteSkill
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteSkill(int id)
{
    var skill = _context.Skills.Find(id);
    if (skill != null)
    {
        _context.Skills.Remove(skill);
        _context.SaveChanges();
    }

    var userId = _userManager.GetUserId(User);
    var skills = _context.Skills
        .Where(x => x.AppUserId == Guid.Parse(userId))
        .ToList();

    return PartialView("_SkillListPartial", skills);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddCertificate(Certificate model)
{
    if (ModelState.IsValid)
    {
        model.AppUserId = Guid.Parse(_userManager.GetUserId(User));
        _context.Certificates.Add(model);
        await _context.SaveChangesAsync();

        var certificates = _context.Certificates
            .Where(x => x.AppUserId == model.AppUserId)
            .ToList();

        return PartialView("_CertificateListPartial", certificates);
    }

    return PartialView("_AddCertificatePartial", model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteCertificate(int id)
{
    var cert = _context.Certificates.Find(id);
    if (cert != null)
    {
        _context.Certificates.Remove(cert);
        _context.SaveChanges();
    }

    var userId = _userManager.GetUserId(User);
    var certificates = _context.Certificates
        .Where(x => x.AppUserId == Guid.Parse(userId))
        .ToList();

    return PartialView("_CertificateListPartial", certificates);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddProject(Project model)
{
    if (ModelState.IsValid)
    {
        model.AppUserId = Guid.Parse(_userManager.GetUserId(User));
        _context.Projects.Add(model);
        await _context.SaveChangesAsync();

        var projects = _context.Projects
            .Where(x => x.AppUserId == model.AppUserId)
            .ToList();

        return PartialView("_ProjectListPartial", projects);
    }

    return PartialView("_AddProjectPartial", model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteProject(int id)
{
    var project = _context.Projects.Find(id);
    if (project != null)
    {
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }

    var userId = _userManager.GetUserId(User);
    var projects = _context.Projects
        .Where(x => x.AppUserId == Guid.Parse(userId))
        .ToList();

    return PartialView("_ProjectListPartial", projects);
}
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddEducation(Education model)
{
    if (ModelState.IsValid)
    {
        // aktif kullanıcıyı Guid olarak alıyoruz
        model.AppUserId = Guid.Parse(_userManager.GetUserId(User));

        _context.Educations.Add(model);
        await _context.SaveChangesAsync();

        // aynı kullanıcıya ait tüm eğitim kayıtlarını çek
        var educations = _context.Educations
            .Where(x => x.AppUserId == model.AppUserId)
            .ToList();

        return PartialView("_EducationListPartial", educations);
    }

    return PartialView("_AddEducationPartial", model);
}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteEducation(int id)
{
    var education = _context.Educations.Find(id);
    if (education != null)
    {
        _context.Educations.Remove(education);
        _context.SaveChanges();
    }

    // aktif kullanıcıyı tekrar bul
    var userId = _userManager.GetUserId(User);
    var educations = _context.Educations
        .Where(x => x.AppUserId == Guid.Parse(userId))
        .ToList();

    return PartialView("_EducationListPartial", educations);
}


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> AddUserDetail(UserDetail model)
{
    if (ModelState.IsValid)
    {
        var userId = Guid.Parse(_userManager.GetUserId(User));
        model.AppUserId = userId;

        var existing = _context.UserDetails.FirstOrDefault(x => x.AppUserId == userId);
        if (existing != null)
        {
            // Güncelle
            existing.Cinsiyet = model.Cinsiyet;
            existing.Vatandaslik = model.Vatandaslik;
            existing.SurucuBelgesi = model.SurucuBelgesi;
            existing.EmekliMi = model.EmekliMi;
            existing.EngelDurumu = model.EngelDurumu;
            existing.AfettenEtkilendiMi = model.AfettenEtkilendiMi;

            _context.UserDetails.Update(existing);
        }
        else
        {
            // Yeni kayıt
            _context.UserDetails.Add(model);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Profile");

    }

    // ModelState hatalıysa formu tekrar göster
    return PartialView("_AddUserDetailPartial", model);
}

  



    }
}

    