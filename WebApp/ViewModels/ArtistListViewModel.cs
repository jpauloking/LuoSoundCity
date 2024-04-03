using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ArtistListViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        [Display(Name = "Display image")]
        public string? DisplayImageUrl { get; set; }
        [MaxLength(50)]
        public string? Genre { get; set; }
        [MaxLength(50)]
        public string? Label { get; set; }
    }
}
