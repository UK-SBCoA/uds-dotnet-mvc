using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;

namespace COA.UDS.Web.Views.Shared.Components.VisitDatesDropdown
{
    public class VisitDatesDropdownViewComponent: ViewComponent
    {
        private readonly UdsContext _context;

        public VisitDatesDropdownViewComponent(UdsContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int currentVisitId, int participationId)
        {
            var visits = await _context.Visits
                .AsNoTracking()
                .Where(v => v.Participant.Id == participationId)
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();

            ViewData["currentVisitId"] = currentVisitId;

            return View(visits);
        }
    }
}
