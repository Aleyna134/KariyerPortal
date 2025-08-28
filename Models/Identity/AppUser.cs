using KariyerPortal.Models.Identity;
using KariyerPortal.Models.Profile;
using Microsoft.AspNetCore.Identity;
using System;

namespace KariyerPortal.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string? AdSoyad { get; set; }
        public string? CVPath { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Phone { get; set; }
        public int? BirthYear { get; set; }
        public string? ProfileImagePath { get; set; }
        public virtual ICollection<UserJobExperience> UserJobExperiences { get; set; } = new List<UserJobExperience>();
        public virtual ICollection<Language> Languages { get; set; } = new List<Language>();
        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Project> Projects { get; set; }
        public ICollection<Education> Educations { get; set; }


        // Navigation property
        public UserDetail? UserDetail { get; set; }



    }
}
