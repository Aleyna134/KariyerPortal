namespace KariyerPortal.Models;

public class JobListResponse
{

    public string? CreatedBy { get; set; }

    public string? Title { get; set; }
    public string? CompanyName { get; set; }      // Yeni eklendi
    public string? Location { get; set; }         // Yeni eklendi
    public string? JobType { get; set; }
    public string? Description { get; set; }

    public DateTime Created { get; set; }
    public int Status { get; set; }
    public Guid Id { get; set; }
    public string? CompanyLogoPath { get; set; } // Liste için logo
    public string? PhotoPath { get; set; }       // Detay sayfasý için büyük fotoðraf

}