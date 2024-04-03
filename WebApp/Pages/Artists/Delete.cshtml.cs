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
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        [BindProperty]
        public ArtistDeleteViewModel ArtistDeleteViewModel { get; set; } = default!;

        public DeleteModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
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
            else 
            {
                ArtistDeleteViewModel = new()
                {
                    Id = artist.Id,
                    Name = artist.Name
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Artist == null)
            {
                return NotFound();
            }
            var artist = await _context.Artist.FindAsync(id);

            if (artist != null)
            {
                _context.Artist.Remove(artist);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
