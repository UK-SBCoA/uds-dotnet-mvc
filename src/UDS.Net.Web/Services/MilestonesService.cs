using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UDS.Net.Data;
using UDS.Net.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace UDS.Net.Web.Services
{
    public class MilestonesService : IMilestonesService
    {
        private readonly UdsContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public MilestonesService(UdsContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<Milestone> CreateMilestone(Milestone milestone)
        {
            try
            {
                _context.Milestones.Add(milestone);
                await _context.SaveChangesAsync(_httpContext.HttpContext.User.Identity.Name);
                return milestone;
            }
            catch (Exception ex)
            {
                throw;
            };
        }

        public async Task<Milestone> UpdateMilestone(int id, Milestone milestone)
        {
            if (id != milestone.Id)
            {
                return null;
            }

            try
            {
                _context.Update(milestone);
                await _context.SaveChangesAsync(_httpContext.HttpContext.User.Identity.Name);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return milestone;
        }

        public async Task<Milestone> GetMilestone(int id)
        {
            return await _context.Milestones.FindAsync(id);
        }

        public async Task<IEnumerable<Milestone>> GetMilestonesForParticipant(int friendlyId)
        {
            return await _context.Milestones
                .Where(m => m.FriendlyId == friendlyId)
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public async Task<bool> IsParticipantDeceased(int friendlyId)
        {
            var milestone = await _context.Milestones
                .Where(m => m.FriendlyId == friendlyId && m.ParticipantIsDeceased.HasValue && m.ParticipantIsDeceased == true)
                .FirstOrDefaultAsync();

            if (milestone != null)
                return true;
            else
                return false;
        }
    }
}
