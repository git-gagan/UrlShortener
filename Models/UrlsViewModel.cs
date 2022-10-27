using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class UrlsViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string longUrl { get; set; }
        public string shortUrl { get; set; }
    }
}
