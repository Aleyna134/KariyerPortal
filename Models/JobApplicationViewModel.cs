using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace KariyerPortal.Models
{
    public class JobApplicationViewModel
    {
        public Guid JobId { get; set; }
        public string JobTitle { get; set; } = "";
        [Required(ErrorMessage = "CV dosyası zorunludur.")]
        public IFormFile CV { get; set; } = null!;
    }
}