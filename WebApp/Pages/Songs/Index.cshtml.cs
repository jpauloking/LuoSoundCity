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

namespace WebApp.Pages.Songs
{
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public IList<SongListViewModel> SongListViewModel { get;set; } = default!;

        public IndexModel(WebApp.Data.WebAppContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task OnGetAsync()
        {
            if (_context.Song != null)
            {
                List<Song> songs = await _context.Song
                    .Include(song => song.Artist)
                    .AsNoTracking()
                    .ToListAsync();
                SongListViewModel = new List<SongListViewModel>();
                string folderContainingAudioFiles = Path.Combine(webHostEnvironment.ContentRootPath, "audios");
                foreach (Song song in songs)
                {
                    SongListViewModel.Add(new()
                    {
                        Id = song.Id,
                        Title = song.Title,
                        Genre = song.Genre,
                        Artist = song.Artist,
                        ClipArtUrl = song.ClipArtUrl,
                        AudioFileUrl = Path.Combine(folderContainingAudioFiles, "demo.mp3")
                    });
                }
            }
        }
    }
}
