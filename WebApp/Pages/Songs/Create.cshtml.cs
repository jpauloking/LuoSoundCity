using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Songs
{
    public class CreateModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public SongCreateViewModel SongCreateViewModel { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int ArtistId { get; set; }
        public List<SelectListItem> Artists { get; set; } = new();

        public CreateModel(WebApp.Data.WebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGet()
        {
            await CreateArtistsSelectList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await CreateArtistsSelectList();
            if (!ModelState.IsValid || _context.Song == null || SongCreateViewModel == null)
            {
                return Page();
            }
            if (NewAudioFileExists())
            {
                string audioFileName = CreateAudioFileName();
                RemoveAudioFileFromAudiosFolder((SongCreateViewModel?.AudioFileUrl)!);
                CopyAudioFileToAudiosFolder(audioFileName);
                SongCreateViewModel!.AudioFileUrl = audioFileName;
            }
            if (NewImageFileExists())
            {
                string imageFile = CreatePhotoFileName();
                RemovePhotoFileFromImagesFolder((SongCreateViewModel?.ClipArtUrl)!);
                CopyPhotoFileToImagesFolder(imageFile);
                SongCreateViewModel!.ClipArtUrl = imageFile;
            }
            SongCreateViewModel.Artist = await _context.Artist
                .FirstOrDefaultAsync(artist => artist.Id == SongCreateViewModel.ArtistId);
            if (SongCreateViewModel.Artist is null)
            {
                ModelState.AddModelError(string.Empty, "Artist was not found in the database");
                return Page();
            }
            _context.Song.Add(new()
            {
                Id = SongCreateViewModel.Id,
                Title = SongCreateViewModel.Title,
                Genre = SongCreateViewModel.Genre,
                AudioFileUrl = SongCreateViewModel.AudioFileUrl,
                ClipArtUrl = SongCreateViewModel.ClipArtUrl,
                ReleaseDate = SongCreateViewModel.ReleaseDate,
                Artist = SongCreateViewModel.Artist
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private async Task CreateArtistsSelectList()
        {
            List<Artist> artists = await _context.Artist.ToListAsync();
            foreach (Artist artist in artists)
            {
                Artists.Add(new()
                {
                    Text = artist.Name,
                    Value = artist.Id.ToString(),
                    Selected = artist.Id == ArtistId
                });
            }
        }

        private bool NewImageFileExists()
        {
            return !string.IsNullOrEmpty(SongCreateViewModel?.ClipArt?.FileName);
        }

        private bool NewAudioFileExists()
        {
            return !string.IsNullOrEmpty(SongCreateViewModel?.AudioFile?.FileName);
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

        private void RemoveAudioFileFromAudiosFolder(string audioFileName)
        {
            if (!string.IsNullOrEmpty(audioFileName))
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string absolutePhotoFileName = Path.Combine(webRootPath, "audios", audioFileName);
                System.IO.File.Delete(absolutePhotoFileName);
            }
        }

        private string CreatePhotoFileName()
        {
            string photoFileName = $"{Guid.NewGuid()}_{SongCreateViewModel?.ClipArt?.FileName}";
            return photoFileName;
        }

        private string CreateAudioFileName()
        {
            string audioFileName = $"{Guid.NewGuid()}_{SongCreateViewModel?.AudioFile?.FileName}";
            return audioFileName;
        }

        private void CopyPhotoFileToImagesFolder(string photoFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoFileName);
            FileStream fileStream = new(absolutePhotoFileName, FileMode.Create);
            SongCreateViewModel?.ClipArt?.CopyTo(fileStream);
            fileStream.Dispose();
        }

        private void CopyAudioFileToAudiosFolder(string audioFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absoluteAudioFileName = Path.Combine(webRootPath, "audios", audioFileName);
            FileStream fileStream = new(absoluteAudioFileName, FileMode.Create);
            SongCreateViewModel?.AudioFile?.CopyTo(fileStream);
            fileStream.Dispose();
        }
    }
}
