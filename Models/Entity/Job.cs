using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerPortal.Models.Entity
{
    public class Job
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; }

        public string? Title { get; set; }
        public string? CompanyName { get; set; }     // Þirket adý
        public string? Location { get; set; }        // Þehir / Ýlçe
        public string? JobType { get; set; }         // Tam zamanlý / Part-time
        public string? Description { get; set; }
        public string? CompanyLogoPath { get; set; } // Ýþ ilaný listesi için
        public string? PhotoPath { get; set; }       // Job/Details sayfasý için


        public DateTime Created { get; set; }
        public int Status { get; set; }
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    }
}