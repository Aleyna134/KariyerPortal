using System;
using System.ComponentModel.DataAnnotations.Schema;
using KariyerPortal.Models; // AppUser burada tanımlı olmalı

namespace KariyerPortal.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }
        public int Status { get; set; }
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    }
}