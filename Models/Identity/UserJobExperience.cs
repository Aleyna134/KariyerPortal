namespace KariyerPortal.Models.Identity
{
    public class UserJobExperience
    {
        
            public int Id { get; set; }
            public string? Position { get; set; }
            public string? CompanyName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }  // Bitiş tarihi boş olabilir
            public Guid UserId { get; set; }
            public AppUser? User { get; set; }

    }


}
