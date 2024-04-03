using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ChartSongCreateViewModel
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
        [Display(Name = "Date song reached position")]
        public DateTime DateSongReachedPosition { get; set; } = DateTime.Now;
        [Display(Name = "Weeks at position")]
        [Range(0, UInt16.MaxValue)]
        public int NumberOfWeeksAtPosition { get; set; }
        [Display(Name = "Previous position")]
        [Range(0, UInt16.MaxValue)]
        public int PreviousPosition { get; set; }
    }
}
