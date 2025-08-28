using System;

namespace KariyerPortal.Models
{
    public class UserDetail
    {
        public AppUser? AppUser { get; set; }

        public Guid AppUserId { get; set; }  // Foreign Key


        public string? Cinsiyet { get; set; }  // "Kadın" veya "Erkek"
        public string? Vatandaslik { get; set; } // Örn: "TC", "Diğer"
        public string? SurucuBelgesi { get; set; } // "Var" veya "Yok"

        public bool EmekliMi { get; set; }
        public bool EngelDurumu { get; set; }
        public bool AfettenEtkilendiMi { get; set; }
    }
}
