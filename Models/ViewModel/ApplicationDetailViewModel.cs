using KariyerPortal.Models.Identity;
using KariyerPortal.Models.Profile;
using System.Collections.Generic;

namespace KariyerPortal.Models.ViewModel
{
    public class ApplicationDetailViewModel
    {
        public string UserName { get; set; }
        public string AdSoyad { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Phone { get; set; }
        public int? BirthYear { get; set; }
        public string CVPath { get; set; }

        public ICollection<UserJobExperience> JobExperiences { get; set; }
        public ICollection<Education> Educations { get; set; }
        public ICollection<Language> Languages { get; set; }
        public ICollection<Skill> Skills { get; set; }
        public ICollection<Certificate> Certificates { get; set; }
        public ICollection<Project> Projects { get; set; }
        public UserDetail UserDetail { get; set; }

    }
}
