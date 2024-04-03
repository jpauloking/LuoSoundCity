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

namespace WebApp.Pages.Charts
{
    public class CreateModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public ChartCreateViewModel ChartCreateViewModel { get; set; } = default!;

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
            if (!ModelState.IsValid || _context.Chart == null || ChartCreateViewModel == null)
            {
                return Page();
            }
            if (NewImageFileExists())
            {
                string imageFile = CreatePhotoFileName();
                RemovePhotoFileFromImagesFolder((ChartCreateViewModel?.DisplayImageUrl)!);
                CopyPhotoFileToImagesFolder(imageFile);
                ChartCreateViewModel!.DisplayImageUrl = imageFile;
            }
            _context.Chart.Add(new()
            {
                Id = ChartCreateViewModel.Id,
                Title = ChartCreateViewModel.Title,
                Description = ChartCreateViewModel.Description,
                Year = ChartCreateViewModel.Year,
                Month = ChartCreateViewModel.Month,
                Week = ChartCreateViewModel.Week,
                CreatedBy = ChartCreateViewModel.CreatedBy,
                DisplayImageUrl = ChartCreateViewModel.DisplayImageUrl
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool NewImageFileExists()
        {
            return !string.IsNullOrEmpty(ChartCreateViewModel?.DisplayImageFile?.FileName);
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
            string photoFileName = $"{Guid.NewGuid()}_{ChartCreateViewModel?.DisplayImageFile?.FileName}";
            return photoFileName;
        }

        private void CopyPhotoFileToImagesFolder(string photoFileName)
        {
            string webRootPath = webHostEnvironment.WebRootPath;
            string absolutePhotoFileName = Path.Combine(webRootPath, "images", photoFileName);
            FileStream fileStream = new(absolutePhotoFileName, FileMode.Create);
            ChartCreateViewModel?.DisplayImageFile?.CopyTo(fileStream);
            fileStream.Dispose();
        }

    }
}
