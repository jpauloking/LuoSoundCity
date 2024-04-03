using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Chart
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = null!;
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [Range(1900,UInt16.MaxValue)]
        [Required]
        public int Year { get; set; }
        [Range(1, 12)]
        [Required]
        public int Month { get; set; }
        [Range(1, 53)]
        [Required]
        public int Week { get; set; }
        [Display(Name = "Created by")]
        [MaxLength(50)]
        [Required]
        public string CreatedBy { get; set; } = null!;
        [DataType(DataType.Date)]
        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [Display(Name = "Display image")]
        public string? DisplayImageUrl { get; set; }
        public List<Song> Songs { get; set; } = new();
        public List<ChartSong> ChartSongs { get; set; } = new();

        // Todo - Uncomment after thinking about how to maintain versions of a single chart :: public int Version { get; set; } // It is the same chart e.g. Weekly top 40 songs - but the versions change every week
    }
}
