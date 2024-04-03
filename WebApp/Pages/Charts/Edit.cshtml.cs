using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Charts
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ChartEditViewModel ChartEditViewModel { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public List<SelectListItem> SongsSelectList { get; set; } = default!;
        [BindProperty]
        public ChartSongCreateViewModel? ChartSongCreateViewModel { get; set; }
        [BindProperty]
        public int SelectedSongId { get; set; } = default!;

        public EditModel(WebApp.Data.WebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            await CreateSongSelectList();

            Chart chart = (await _context.Chart
                .Include(chart => chart.Songs)
                .FirstOrDefaultAsync(m => m.Id == id))!;
            if (chart == null)
            {
                return NotFound();
            }
            ChartEditViewModel = new()
            {
                Id = chart.Id,
                Title = chart.Title,
                Description = chart.Description,
                Year = chart.Year,
                Month = chart.Month,
                Week = chart.Week,
                DisplayImageUrl = chart.DisplayImageUrl,
                CreatedBy = chart.CreatedBy,
                Songs = chart.Songs,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CreateSongSelectList();
            if (!ModelState.IsValid || _context.Chart == null || ChartEditViewModel == null)
            {
                return Page();
            }
            if (ChartExists(ChartEditViewModel.Id))
            {
                if (NewImageFileExists())
                {
                    string displayImageFileName = CreatePhotoFileName();
                    RemovePhotoFileFromImagesFolder(ChartEditViewModel?.DisplayImageUrl!);
                    CopyPhotoFileToImagesFolder(displayImageFileName);
                    ChartEditViewModel!.DisplayImageUrl = displayImageFileName ?? "no_image.png";
                }
                Chart chart = (await _context.Chart
                    .FirstOrDefaultAsync(m => m.Id == ChartEditViewModel!.Id))!;
                chart.Id = ChartEditViewModel.Id;
                chart.Title = ChartEditViewModel.Title;
                chart.Description = ChartEditViewModel.Description;
                chart.Year = ChartEditViewModel.Year;
                chart.Month = ChartEditViewModel.Month;
                chart.Week = ChartEditViewModel.Week;
                chart.DisplayImageUrl = ChartEditViewModel.DisplayImageUrl;
                chart.Songs = ChartEditViewModel.Songs;

                _context.Attach(chart).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChartExists(ChartEditViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("./Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAddSongsToChartAsync()
        {
            await CreateSongSelectList();
            if (_context?.Chart is null)
            {
                return Page();
            }
            if (ChartExists(ChartEditViewModel.Id))
            {
                Chart chart = (await _context.Chart
                    .FirstOrDefaultAsync(m => m.Id == ChartEditViewModel!.Id))!;
                Song song = (await _context.Song.FirstOrDefaultAsync(song => song.Id == SelectedSongId))!;
                if (song is not null && chart is not null)
                {
                    ChartSong chartSongDetailsFromChart = chart.ChartSongs.FirstOrDefault(chartSong => chartSong.SongId == song.Id && chartSong.ChartId == chart.Id)!;
                    bool songIsAlreadyOnChart = chartSongDetailsFromChart is not null;
                    if (songIsAlreadyOnChart)
                    {
                        chart.ChartSongs.Remove(chartSongDetailsFromChart!);
                    }
                    if (ChartSongCreateViewModel is not null)
                    {
                        // Todo - Figure out how to make comparisons between the chartSongDetailsFromChart and user provided values.
                        chart.ChartSongs.Add(new()
                        {
                            Song = song,
                            Chart = chart,
                            ChartId = chart.Id,
                            SongId = song.Id,
                            Position = ChartSongCreateViewModel.Position,
                            PreviousPosition = ChartSongCreateViewModel.PreviousPosition,
                            NumberOfWeeksAtPosition = ChartSongCreateViewModel.NumberOfWeeksAtPosition,
                            NumberOfWeeksOnChart = ChartSongCreateViewModel.NumberOfWeeksOnChart,
                            DateAddedToChart = ChartSongCreateViewModel.DateAddedToChart,
                            DateSongReachedPosition = ChartSongCreateViewModel.DateSongReachedPosition,
                        });
                    }
                    _context.Attach(chart).State = EntityState.Modified;
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ChartExists(ChartEditViewModel.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToPage("Details", new { chart!.Id });
                }
            }
            return Page();
        }

        private async Task CreateSongSelectList()
        {
            List<Song> allSongs = await _context.Song
                            .AsNoTracking()
                            .ToListAsync();
            SongsSelectList = new();
            foreach (Song song in allSongs)
            {
                SongsSelectList.Add(new()
                {
                    Text = song.Title,
                    Value = song.Id.ToString()
                });
            }
        }

        private bool ChartExists(int id)
        {
            return (_context.Chart?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool NewImageFileExists()
        {
            return !string.IsNullOrEmpty(ChartEditViewModel?.DisplayImageFile?.FileName);
        }

        private void RemovePhotoFileFromImagesFolder(string photoPath)
        {
            if (!string.IsNullOrEmpty(photoPath))
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoPath);
                System.IO.File.Delete(absolutePhotoFileName);
            }
        }

        private string CreatePhotoFileName()
        {
            string photoFileName = $"{Guid.NewGuid()}_{ChartEditViewModel?.DisplayImageFile?.FileName}";
            return photoFileName;
        }

        private void CopyPhotoFileToImagesFolder(string photoFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoFileName);
            FileStream fileStream = new(absolutePhotoFileName, FileMode.Create);
            ChartEditViewModel?.DisplayImageFile?.CopyTo(fileStream);
            fileStream.Dispose();
        }
    }
}
