using System;
using Microsoft.AspNetCore.Identity;

namespace KariyerPortal.Models;

public class AppUser : IdentityUser<Guid>
{
    public string? AdSoyad { get; set; }
    public string? CVPath { get; set; }

public string? City { get; set; }
public string? District { get; set; }
public string? Phone { get; set; }
public int? BirthYear { get; set; }
public string? ProfileImagePath { get; set; }
public virtual UserDetail UserDetail { get; set; }

}