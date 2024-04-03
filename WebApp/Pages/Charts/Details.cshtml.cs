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

namespace WebApp.Pages.Charts
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;
        public ChartDetailsViewModel ChartDetailsViewModel { get; set; } = default!;

        public DetailsModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Chart == null)
            {
                return NotFound();
            }

            Chart chart = (await _context.Chart
                .Include(chart=>chart.Songs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id))!;
            if (chart == null)
            {
                return NotFound();
            }
            else
            {
                ChartDetailsViewModel = new()
                {
                    Id = chart.Id,
                    Title = chart.Title,
                    Description = chart.Description,
                    Year = chart.Year,
                    Month = chart.Month,
                    Week = chart.Week,
                    CreatedBy = chart.CreatedBy,
                    DateCreated = chart.DateCreated,
                    DisplayImageUrl = chart.DisplayImageUrl,
                    Songs = chart.Songs,
                };
            }
            return Page();
        }
    }
}
