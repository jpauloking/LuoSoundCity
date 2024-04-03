using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ArtistDeleteViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;
    }
}
