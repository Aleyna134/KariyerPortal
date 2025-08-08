public class CvViewModel
{
    public string AdSoyad { get; set; }
    public string Email { get; set; }
    public string Telefon { get; set; }
    public string Sehir { get; set; }
    public string Egitim { get; set; }
    public string Deneyim { get; set; }
    public string Yetenekler { get; set; }

    public IFormFile? YeniCV { get; set; }  // Dosya yükleme
    public string? EskiCVPath { get; set; }
}