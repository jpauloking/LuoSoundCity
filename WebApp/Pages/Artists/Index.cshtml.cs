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
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        public IndexModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }

        public IList<ArtistListViewModel> ArtistListViewModel { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Artist != null)
            {
                List<Artist> artists = await _context.Artist.ToListAsync();
                ArtistListViewModel = new List<ArtistListViewModel>();
                foreach (Artist artist in artists)
                {
                    ArtistListViewModel.Add(new()
                    {
                        Id = artist.Id,
                        Name = artist.Name,
                        Genre = artist.Genre,
                        Label = artist.Label,
                        DisplayImageUrl = artist.DisplayImageUrl,
                    });
                }
            }
        }
    }
}
