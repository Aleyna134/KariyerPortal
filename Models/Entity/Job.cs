using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerPortal.Models.Entity
{
    public class Job
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public string? Title { get; set; }
        public string? CompanyName { get; set; }     // �irket ad�
        public string? Location { get; set; }        // �ehir / �l�e
        public string? JobType { get; set; }         // Tam zamanl� / Part-time
        public string? Description { get; set; }
        public string? CompanyLogoPath { get; set; } // �� ilan� listesi i�in
        public string? PhotoPath { get; set; }       // Job/Details sayfas� i�in


        public DateTime Created { get; set; }
        public int Status { get; set; }
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    }
}