using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UDS.Net.Web.ViewModels;
using UDS.Net.Data.Dtos;

namespace COA.UDS.Web.Views.Shared.Components.VisitHeader
{
    public class VisitHeaderViewComponent: ViewComponent
    {
        private readonly IConfiguration _config;

        public VisitHeaderViewComponent(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync(int visitId, int participationId, ParticipantDto participantProfile)
        {
            var participantPortalUrl = _config.GetValue<string>("ParticipantPortal:Url");

            var visitParticipant = new VisitParticipant
            {
                ParticipantPortalBaseUrl = participantPortalUrl,
                ParticipationId = participationId,
                ParticipantProfile = participantProfile,
                CurrentVisitId = visitId
            };

            return View("Default", visitParticipant);
        }

    }
}
