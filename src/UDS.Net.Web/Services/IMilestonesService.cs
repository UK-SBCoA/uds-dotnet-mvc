using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Services
{
    public interface IMilestonesService
    {
        Task<IEnumerable<Milestone>> GetMilestonesForParticipant(int friendlyId);

        Task<Milestone> GetMilestone(int id);

        Task<bool> IsParticipantDeceased(int friendlyId);

        Task<Milestone> CreateMilestone(Milestone milestone);

        Task<Milestone> UpdateMilestone(int id, Milestone milestone);
    }
}
