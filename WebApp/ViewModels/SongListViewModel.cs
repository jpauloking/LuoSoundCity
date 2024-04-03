using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class SongListViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = default!;
        [MaxLength(50)]
        public string? Genre { get; set; }
        public Artist Artist { get; set; } = default!;
        [Display(Name = "Clip art")]
        public string? ClipArtUrl { get; set; }
        [Display(Name = "Audio file")]
        public string? AudioFileUrl { get; set; }
    }
}
