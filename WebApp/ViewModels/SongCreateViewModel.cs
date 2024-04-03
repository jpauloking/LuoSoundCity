using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class SongCreateViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = null!;
        [Display(Name = "Audio file")]
        public string? AudioFileUrl { get; set; }
        [Display(Name = "Clip art")]
        public string? ClipArtUrl { get; set; }
        [Display(Name = "Release date")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }
        [MaxLength(50)]
        public string? Genre { get; set; }
        public IFormFile? AudioFile { get; set; }
        public IFormFile? ClipArt { get; set; }
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }
    }
}
