﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class ChartCreateViewModel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        [Required]
        public string Title { get; set; } = null!;
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        [Range(1900, UInt16.MaxValue)]
        [Required]
        public int Year { get; set; } = DateTime.Now.Year;
        [Range(1, 12)]
        [Required]
        public int Month { get; set; } = DateTime.Now.Month;
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
        [Display(Name = "Display image")]
        public IFormFile? DisplayImageFile { get; set; }
    }
}
