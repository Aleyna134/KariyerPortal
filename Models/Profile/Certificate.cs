using System;

namespace KariyerPortal.Models.Profile
{
    public class Certificate
    {
        public int Id { get; set; }
        public Guid AppUserId { get; set; } // Kullanıcıya bağlamak için
        public string CertificateName { get; set; }
        public string Institution { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime? ExpirationDate { get; set; }
    }
}
