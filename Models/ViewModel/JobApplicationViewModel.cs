using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KariyerPortal.Models.ViewModel
{
    public class JobApplicationViewModel
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = "";
        [Required(ErrorMessage = "CV dosyası zorunludur.")]
        public IFormFile CV { get; set; } = null!;
        public string CompanyName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;

    }
}