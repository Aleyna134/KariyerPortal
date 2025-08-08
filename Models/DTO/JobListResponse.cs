namespace KariyerPortal.Models;

public class JobListResponse
{

    public string? CreatedBy { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime Created { get; set; }
    public int Status { get; set; }
    public Guid Id { get; set; } 

    }