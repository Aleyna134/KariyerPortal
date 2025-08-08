using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KariyerPortal.Models;
using Microsoft.AspNetCore.Authorization;
using KariyerPortal.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KariyerPortal.Controllers;

namespace KariyerPortal.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    
   

    public async Task<IActionResult> Index()
    {
        var jobsRaw = await _context.Jobs.ToListAsync();
        var jobList = new List<JobListResponse>();

        foreach (var job in jobsRaw)
        {
            var user = await _userManager.FindByIdAsync(job.CreatedBy.ToString());

            jobList.Add(new JobListResponse
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                Created = job.Created,
                Status = job.Status,
                CreatedBy = user?.UserName ?? "Bilinmiyor"
            });
        }

        return View(jobList); // View'e job listesi gönder
    }
        
    

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  

}
