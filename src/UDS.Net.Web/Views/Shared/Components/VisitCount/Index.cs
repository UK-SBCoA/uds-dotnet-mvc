using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Enums;

namespace COA.UDS.Web.Views.Shared.Components.FormStatusCount
{
    public class VisitCountViewComponent : ViewComponent
    {
        private readonly UdsContext _context;

        public VisitCountViewComponent(UdsContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string visitStatus)
        {
            var count = await GetCountAsync(visitStatus);
            return View(count);
        }

        private async Task<int> GetCountAsync(string visitStatus)
        {
            if (visitStatus == "InProgress")
            {
                return await _context.Visits
                    .AsNoTracking()
                    .Where(v => v.IsSubmittedToNACC == false && v.Status == VisitStatus.InProgress)
                    .CountAsync();
            }
            return 0;
        }
    }
}
