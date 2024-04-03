using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ArtistCreateViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        [Display(Name = "Display image")]
        public string? DisplayImageUrl { get; set; }
        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }
        [Range(1960, UInt16.MaxValue)]
        [Display(Name = "Debut year")]
        public int? DebutYear { get; set; }
        [Range(18, UInt16.MaxValue)]
        public int Age { get; set; }
        [MaxLength(50)]
        public string? Genre { get; set; }
        [MaxLength(50)]
        public string? Label { get; set; }
        public IFormFile? DisplayImageFile { get; set; }
    }
}
