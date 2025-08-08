using KariyerPortal.Context;
using KariyerPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KariyerPortal.Controllers
{
    [Authorize]
    public class UserDetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public UserDetailController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET & POST: /UserDetail/Hakkimda
        [HttpGet]
        public async Task<IActionResult> Hakkimda()
        {
            var user = await _userManager.GetUserAsync(User);
            var hakkimda = await _context.UserDetails.FirstOrDefaultAsync(ud => ud.AppUserId == user.Id);

            if (hakkimda == null)
            {
                hakkimda = new UserDetail { AppUserId = user.Id };
            }

            return View(hakkimda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hakkimda(UserDetail model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            var existing = await _context.UserDetails.FirstOrDefaultAsync(ud => ud.AppUserId == user.Id);

            if (existing == null)
            {
                model.AppUserId = user.Id;
                _context.UserDetails.Add(model);
            }
            else
            {
                // Güncelle
                existing.Cinsiyet = model.Cinsiyet;
                existing.Vatandaslik = model.Vatandaslik;
                existing.SurucuBelgesi = model.SurucuBelgesi;
                existing.EmekliMi = model.EmekliMi;
                existing.AfettenEtkilendiMi = model.AfettenEtkilendiMi;

                _context.UserDetails.Update(existing);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Hakkimda));
        }
    }
}