using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Song
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
        public Artist Artist { get; set; } = default!;
        public List<Chart> Charts { get; set; } = new();
        public List<ChartSong> ChartSongs { get; set; } = new();

        // Todo - Add album
        // Todo - Add featured artists
    }
}
