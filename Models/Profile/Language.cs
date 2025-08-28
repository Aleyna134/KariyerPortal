using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KariyerPortal.Models.Identity; // AppUser

namespace KariyerPortal.Models.Profile
{
    public class Language
    {
        public int Id { get; set; }

        [Required]
        public string LanguageName { get; set; }

        [Required]
        public string ProficiencyLevel { get; set; } // Başlangıç, Orta, İleri

        // Foreign Key
        public Guid AppUserId { get; set; }

        // Navigation Property
        [ForeignKey(nameof(AppUserId))]
        public AppUser? AppUser { get; set; }
    }
}
