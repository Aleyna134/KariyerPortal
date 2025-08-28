namespace KariyerPortal.Models;

public class JobCreateRequest
    {
        public Guid Id { get; set; }

        public Guid CreatedBy { get; set; } 

        public string? Title { get; set; }
        public string? CompanyName { get; set; }     // Þirket adý
        public string? Location { get; set; }        // Þehir / Ýlçe
        public string? JobType { get; set; }         // Tam zamanlý / Part-time
        public string? Description { get; set; }

        public DateTime Created { get; set; }
        public int Status { get; set; }
        public IFormFile? CompanyLogo { get; set; }
        public IFormFile? Photo { get; set; }
      

}