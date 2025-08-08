namespace KariyerPortal.Models;

public class JobCreateRequest
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; } 

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime Created { get; set; }
        public int Status { get; set; }

    }