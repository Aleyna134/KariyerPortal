
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KariyerPortal.Models
{

    public class UserDetail
    {
        public int Id { get; set; }

        public string? Cinsiyet { get; set; }
        public string? Vatandaslik { get; set; }
        public string? SurucuBelgesi { get; set; }
        public bool EmekliMi { get; set; }
        public bool AfettenEtkilendiMi { get; set; }

        public Guid AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
    }
}