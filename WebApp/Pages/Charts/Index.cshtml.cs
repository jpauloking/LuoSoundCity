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
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.WebAppContext _context;

        public IList<ChartListViewModel> ChartListViewModel { get;set; } = default!;

        public IndexModel(WebApp.Data.WebAppContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (_context.Chart != null)
            {
                List<Chart> charts = await _context.Chart.ToListAsync();
                ChartListViewModel = new List<ChartListViewModel>();
                foreach (Chart chart in charts)
                {
                    ChartListViewModel.Add(new()
                    {
                        Id = chart.Id,
                        Title = chart.Title,
                        Description = chart.Description,
                        Year = chart.Year,
                        Month = chart.Month,
                        Week = chart.Week,
                        CreatedBy = chart.CreatedBy,
                        DateCreated = chart.DateCreated,
                        DisplayImageUrl = chart.DisplayImageUrl
                    });
                }
            }
        }
    }
}
