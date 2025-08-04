using Microsoft.AspNetCore.Identity;

namespace KariyerPortal.Models;

public class AppUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }

}