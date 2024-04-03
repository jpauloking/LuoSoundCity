using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Pages.Artists
{
    public class CreateModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ArtistCreateViewModel CreateArtistViewmodel { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? ReturnURL { get; set; }

        public CreateModel(WebApp.Data.WebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Artist is null || CreateArtistViewmodel is null)
            {
                return Page();
            }
            if (NewImageFileExists())
            {
                string imageFile = CreatePhotoFileName();
                RemovePhotoFileFromImagesFolder((CreateArtistViewmodel?.DisplayImageUrl)!);
                CopyPhotoFileToImagesFolder(imageFile);
                CreateArtistViewmodel!.DisplayImageUrl = imageFile;
            }
            _context.Artist.Add(new()
            {
                Name = CreateArtistViewmodel.Name,
                DateOfBirth = CreateArtistViewmodel.DateOfBirth,
                Age = CreateArtistViewmodel.Age,
                DebutYear = CreateArtistViewmodel.DebutYear,
                Genre = CreateArtistViewmodel.Genre,
                Label = CreateArtistViewmodel.Label,
                DisplayImageUrl = CreateArtistViewmodel.DisplayImageUrl
            });
            await _context.SaveChangesAsync();
            // Todo - Get the artist id for the artist that was just created and send it as route param or query called artistId.
            int artistId = 1;
            if (!string.IsNullOrEmpty(ReturnURL) && Url.IsLocalUrl(ReturnURL))
            {
                return LocalRedirect($"{ReturnURL}/?ArtistId={artistId}");
            }
            return RedirectToPage("./Index");
        }

        private bool NewImageFileExists()
        {
            return !string.IsNullOrEmpty(CreateArtistViewmodel?.DisplayImageFile?.FileName);
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
            string photoFileName = $"{Guid.NewGuid()}_{CreateArtistViewmodel?.DisplayImageFile?.FileName}";
            return photoFileName;
        }

        private void CopyPhotoFileToImagesFolder(string photoFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoFileName);
            FileStream fileStream = new(absolutePhotoFileName, FileMode.Create);
            CreateArtistViewmodel?.DisplayImageFile?.CopyTo(fileStream);
            fileStream.Dispose();
        }
    }
}
