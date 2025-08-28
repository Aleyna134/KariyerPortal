using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerPortal.Models.Profile
{
    public class Skill
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yetenek adı gerekli")]
        public string SkillName { get; set; } = string.Empty;


        public Guid AppUserId { get; set; }

        [Required(ErrorMessage = "Seviye gerekli")]
        public string Level { get; set; } = string.Empty;

    }
}

