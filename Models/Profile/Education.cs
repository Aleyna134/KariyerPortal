using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KariyerPortal.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }

        public Guid AppUserId { get; set; }   // Guid ile ilişki

        [Required]
        public string EducationType { get; set; } // Lisans, Ön Lisans, Yüksek Lisans, Doktora, Lise

        // Ortak alanlar
        public string SchoolName { get; set; }
        public string Department { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}
