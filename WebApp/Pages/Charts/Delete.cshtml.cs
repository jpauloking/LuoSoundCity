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
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        [BindProperty]
        public ChartDeleteViewModel ChartDeleteViewModel { get; set; } = default!;

        public DeleteModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Chart == null)
            {
                return NotFound();
            }

            Chart chart = (await _context.Chart.FirstOrDefaultAsync(m => m.Id == id))!;

            if (chart == null)
            {
                return NotFound();
            }
            else
            {
                ChartDeleteViewModel = new()
                {
                    Id = chart.Id,
                    Title = chart.Title,
                    Year = chart.Year,
                    Month = chart.Month,
                    Week = chart.Week,
                    CreatedBy = chart.CreatedBy
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Chart == null)
            {
                return NotFound();
            }
            Chart chart = (await _context.Chart.FindAsync(id))!;

            if (chart != null)
            {
                _context.Chart.Remove(chart);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
