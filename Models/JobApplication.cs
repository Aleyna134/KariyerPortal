using System;

namespace KariyerPortal.Models
{
    public class JobApplication
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public string CVPath { get; set; } = string.Empty;


        public DateTime AppliedAt { get; set; }
        public string? CoverLetter { get; set; } // Kullanıcı açıklama yazmak isterse
             public  Job Job { get; set; }

    }
}