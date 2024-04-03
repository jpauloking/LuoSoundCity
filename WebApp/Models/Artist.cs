using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; } = null!;
        public string? DisplayImageUrl { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Range(18, UInt16.MaxValue)]
        public int Age { get; set; }
        [Range(1960, UInt16.MaxValue)]
        public int? DebutYear { get; set; }
        [MaxLength(50)]
        public string? Genre { get; set; }
        [MaxLength(50)]
        public string? Label { get; set; }
        public List<Song> Songs { get; set; } = new();

        //Todo - Add social medias
        //Todo - Add albums
    }
}
