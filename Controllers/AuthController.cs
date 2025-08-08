using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using KariyerPortal.Models;

public class AuthController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // GET: Login formu
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Login işlemi
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Kullanıcı bulunamadı.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Admin"))
                return RedirectToAction("JobList", "Admin");

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError("", "Giriş başarısız.");
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Register(RegisterViewModel model)
{
    if (!ModelState.IsValid)
{
    // ModelState hatalarını görmek için breakpoint koy veya yazdır
    return View(model);
}
    if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdSoyad = model.AdSoyad
            };

            var result = await _userManager.CreateAsync(user, model.Password);

if (!result.Succeeded)
{
    foreach (var error in result.Errors)
    {
        // Ekrana bas, konsola yaz veya breakpoint koy
        Console.WriteLine(error.Description); // VSCode'da göremezsen ViewBag ile yazdır
        ModelState.AddModelError(string.Empty, error.Description);
    }
    return View(model); // Hataları ekrana göster
}
            if (result.Succeeded)
            {
                // Rol atama
                await _userManager.AddToRoleAsync(user, "User");

                // Otomatik login
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Login", "Auth");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

    return View(model);
}
    

}