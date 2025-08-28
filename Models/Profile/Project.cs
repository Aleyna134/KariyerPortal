using System;
using System.ComponentModel.DataAnnotations;

namespace KariyerPortal.Models.Profile
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public DateTime ProjectDate { get; set; } = DateTime.Now;

        public string Description { get; set; }

        public Guid AppUserId { get; set; }
    }
}
