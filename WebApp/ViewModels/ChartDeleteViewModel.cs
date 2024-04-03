using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ChartDeleteViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = null!;
        [Range(1900, UInt16.MaxValue)]
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
    }
}
