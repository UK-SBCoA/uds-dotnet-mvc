using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UDS.Net.Data;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;

namespace COA.UDS.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UdsContext _context;

        public DashboardController(UdsContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<ActionResult> Index()
        {

            var inProgressVisits = await _context.Visits
                .AsNoTracking()
                .Where(v => v.IsSubmittedToNACC == false && v.Status == VisitStatus.InProgress)
                .OrderBy(v => v.FriendlyId)
                .ToListAsync();

            var visits = new CurrentVisits
            {
                InProgress = inProgressVisits
            };

            return View(visits);
        }

    }
}