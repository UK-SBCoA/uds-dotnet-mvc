using System;
using System.Threading.Tasks;
using UDS.Net.Data.Entities;
using UDS.Net.Data.Enums;
using UDS.Net.Web.ViewModels;

namespace UDS.Net.Web.Services
{
    public interface IVisitService
    {
        Task<string> GetVisitFormsInStatusAsync(int id, VisitType visitType, FormStatus formStatus);

        Task<Visit> GetVisitWithForms(int id);

        Task<Visit> GetVisitWithParticipantAndForms(int id);

        Task<VisitOverviewViewModel> GetVisitWithParticipantAndFormBases(int id);

        Task<Visit> UpdateVisit(Visit updatedVisit, bool currentUserIsAdmin = false, bool currentUserIsSuperUser = false);

    }
}
