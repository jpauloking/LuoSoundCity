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

namespace WebApp.Pages.Artists
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ArtistEditViewModel ArtistEditViewModel { get; set; } = default!;

        public EditModel(WebApp.Data.WebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            Artist artist = (await _context.Artist.FirstOrDefaultAsync(m => m.Id == id))!;
            if (artist == null)
            {
                return NotFound();
            }
            ArtistEditViewModel = new()
            {
                Id = artist.Id,
                Name = artist.Name,
                DateOfBirth = artist.DateOfBirth,
                Age = artist.Age,
                DebutYear = artist.DebutYear ?? 0,
                Genre = artist.Genre,
                Label = artist.Label,
                DisplayImageUrl = artist.DisplayImageUrl
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (ArtistExists(ArtistEditViewModel.Id))
            {
                if (NewImageFileExists())
                {
                    string displayImageFileName = CreatePhotoFileName();
                    RemovePhotoFileFromImagesFolder(ArtistEditViewModel?.DisplayImageUrl!);
                    CopyPhotoFileToImagesFolder(displayImageFileName);
                    ArtistEditViewModel!.DisplayImageUrl = displayImageFileName ?? "no_image.png";
                }
                Artist artist = (await _context.Artist.FirstOrDefaultAsync(m => m.Id == ArtistEditViewModel!.Id))!;
                artist.Name = ArtistEditViewModel!.Name;
                artist.DateOfBirth = ArtistEditViewModel.DateOfBirth;
                artist.Age = ArtistEditViewModel.Age;
                artist.DebutYear = ArtistEditViewModel.DebutYear;
                artist.Genre = ArtistEditViewModel.Genre;
                artist.Label = ArtistEditViewModel.Label;
                artist.DisplayImageUrl = ArtistEditViewModel.DisplayImageUrl;
                _context.Attach(artist).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(ArtistEditViewModel.Id))
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

        private bool ArtistExists(int id)
        {
            return (_context.Artist?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private bool NewImageFileExists()
        {
            return !string.IsNullOrEmpty(ArtistEditViewModel?.DisplayImageFile?.FileName);
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
            string photoFileName = $"{Guid.NewGuid()}_{ArtistEditViewModel?.DisplayImageFile?.FileName}";
            return photoFileName;
        }

        private void CopyPhotoFileToImagesFolder(string photoFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoFileName);
            FileStream fileStream = new(absolutePhotoFileName, FileMode.Create);
            ArtistEditViewModel?.DisplayImageFile?.CopyTo(fileStream);
            fileStream.Dispose();
        }
    }
}
