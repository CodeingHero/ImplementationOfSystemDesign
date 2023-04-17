using SystemDesign.EF.Models;

namespace SystemDesign.Responses {
  public class ShortenUrlResponse {
    public string UserID { get; set; }
    public string UrlKey { get; init; }
    public string? OriginalUrl { get; set; }
    public string? ShortenedUrl { get; set; }
    public DateTime? CreationTime { get; set; }
    public DateTime? ExpirationTime { get; set; }
  }
}
