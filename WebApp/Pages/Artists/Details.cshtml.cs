using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Pages.Artists
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        public DetailsModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }

        public ArtistDetailsViewModel ArtistDetailsViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }

            Artist artist = (await _context.Artist
                .Include(artist => artist.Songs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id))!;
            if (artist == null)
            {
                return NotFound();
            }
            else
            {
                ArtistDetailsViewModel = new()
                {
                    Id = artist.Id,
                    Name = artist.Name,
                    DateOfBirth = artist.DateOfBirth,
                    Age = artist.Age,
                    DebutYear = artist.DebutYear ?? 0,
                    Genre = artist.Genre,
                    Label = artist.Label,
                    DisplayImageUrl = artist.DisplayImageUrl,
                    Songs = artist.Songs
                };
            }
            return Page();
        }
    }
}
