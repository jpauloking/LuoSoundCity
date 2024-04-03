using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class ChartSongListViewModel
    {
        [Display(Name = "Chart")]
        public int ChartId { get; set; }
        [Display(Name = "Song")]
        public int SongId { get; set; }
        [Display(Name = "Date added")]
        [DataType(DataType.Date)]
        public DateTime DateAddedToChart { get; set; } = DateTime.Now;
        [Display(Name = "Weeks on chart")]
        [Range(0, UInt16.MaxValue)]
        public int NumberOfWeeksOnChart { get; set; }
        [Range(1, UInt16.MaxValue)]
        public int Position { get; set; }
        [Display(Name = "Weeks at position")]
        [Range(0, UInt16.MaxValue)]
        public int NumberOfWeeksAtPosition { get; set; }
        [Display(Name = "Previous position")]
        [Range(0, UInt16.MaxValue)]
        public int PreviousPosition { get; set; }
        [Display(Name = "Trending upwards")]
        public bool TrendingUpwards { get; set; } = false;
        [Display(Name = "Trending upwards by")]
        [Range(0, UInt16.MaxValue)]
        public int TrendingUpwardsBy { get; set; } = 0;
        [Display(Name = "Trending downwards")]
        public bool TrendingDownwards { get; set; } = false;
        [Display(Name = "Trending downwards by")]
        [Range(0, UInt16.MaxValue)]
        public int TrendingDownwardsBy { get; set; } = 0;
        public Song Song { get; set; } = default!;
        public Chart Chart { get; set; } = default!;
    }
}
